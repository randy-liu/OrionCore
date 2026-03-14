using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;
using Orion.Api.Rendering;
using Orion.Api.Rendering.Contracts;
using Orion.Api.Rendering.Requests;

namespace Orion.Api.Extensions
{
    /// <summary>提供 `PrintDocument` 舊版邊界與渲染核心串接的擴充方法。</summary>
    public static class PrintDocumentExtensions
    {
        private const string LegacyPrintDocumentMigrationMessage =
            "PrintDocument 為僅支援 Windows 的 legacy 路徑。請改用 Orion.Api.Rendering 核心：PDF 使用 IRenderGateway.RenderPdfAsync/PdfRenderRequest 或 ToPdfStream；PNG 使用 IRenderGateway.RenderPngAsync/PngRenderRequest 或 IRenderGateway 的 ToPngStream。";

        /// <summary>將 `PrintDocument` 以舊版路徑渲染為 `Bitmap`。</summary>
        /// <param name="doc">列印文件。</param>
        /// <returns>渲染後影像。</returns>
        /// <exception cref="ArgumentNullException">`doc` 為 `null` 時拋出。</exception>
        /// <exception cref="TargetInvocationException">反射呼叫列印流程時，若內部拋出例外則包裝後拋出。</exception>
        /// <exception cref="PlatformNotSupportedException">非 Windows 平台呼叫時拋出，並提供遷移指引。</exception>
        [SupportedOSPlatform("windows")]
        public static Bitmap ToBitmap(this PrintDocument doc)
        {
            if (doc == null) { throw new ArgumentNullException(nameof(doc)); }

            EnsureLegacyPrintDocumentSupported();

            PageSettings settings = doc.DefaultPageSettings;
            var bitmap = new Bitmap(settings.PaperSize.Width, settings.PaperSize.Height);

            using var graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.White);

            var args = new PrintPageEventArgs(graphics, settings.Bounds, settings.Bounds, settings);

            MethodInfo method = doc.GetType()
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .First(x => x.Name == "OnPrintPage");

            method.Invoke(doc, new object[] { args });

            return bitmap;
        }

        /// <summary>將 `PrintDocument` 以舊版路徑轉為指定格式影像串流。</summary>
        /// <param name="doc">列印文件。</param>
        /// <param name="format">影像格式。</param>
        /// <returns>影像串流。</returns>
        /// <exception cref="ArgumentNullException">`doc` 或 `format` 為 `null` 時拋出。</exception>
        /// <exception cref="PlatformNotSupportedException">非 Windows 平台呼叫時拋出，並提供遷移指引。</exception>
        [SupportedOSPlatform("windows")]
        public static Stream ToImageStream(this PrintDocument doc, ImageFormat format)
        {
            if (doc == null) { throw new ArgumentNullException(nameof(doc)); }
            if (format == null) { throw new ArgumentNullException(nameof(format)); }

            EnsureLegacyPrintDocumentSupported();

            using Bitmap bitmap = ToBitmap(doc);

            var stream = new MemoryStream();
            bitmap.Save(stream, format);
            stream.Seek(0, SeekOrigin.Begin);

            return stream;
        }

        /// <summary>將 `PrintDocument` 以舊版路徑轉為 JPEG 影像串流。</summary>
        /// <param name="doc">列印文件。</param>
        /// <returns>JPEG 影像串流。</returns>
        /// <remarks>此方法僅支援 Windows。跨平台情境請改用渲染核心。</remarks>
        /// <exception cref="ArgumentNullException">`doc` 為 `null` 時拋出。</exception>
        /// <exception cref="PlatformNotSupportedException">非 Windows 平台呼叫時拋出，並提供遷移指引。</exception>
        [SupportedOSPlatform("windows")]
        public static Stream ToJpegStream(this PrintDocument doc)
        {
            return ToImageStream(doc, ImageFormat.Jpeg);
        }

        /// <summary>將 `PrintDocument` 以舊版路徑轉為 PNG 影像串流。</summary>
        /// <param name="doc">列印文件。</param>
        /// <returns>PNG 影像串流。</returns>
        /// <remarks>此方法僅支援 Windows。跨平台情境請改用渲染核心。</remarks>
        /// <exception cref="ArgumentNullException">`doc` 為 `null` 時拋出。</exception>
        /// <exception cref="PlatformNotSupportedException">非 Windows 平台呼叫時拋出，並提供遷移指引。</exception>
        [SupportedOSPlatform("windows")]
        public static Stream ToPngStream(this PrintDocument doc)
        {
            return ToImageStream(doc, ImageFormat.Png);
        }

        /// <summary>透過渲染核心將 PDF 請求轉為串流。</summary>
        /// <param name="renderGateway">渲染核心入口。</param>
        /// <param name="request">PDF 渲染請求。</param>
        /// <param name="cancellationToken">取消權杖。</param>
        /// <returns>唯讀 PDF 串流。</returns>
        /// <remarks>此為同步封裝，會封鎖目前執行緒直到渲染完成。</remarks>
        /// <exception cref="ArgumentNullException">`renderGateway` 或 `request` 為 `null` 時拋出。</exception>
        public static Stream ToPdfStream(this IRenderGateway renderGateway, PdfRenderRequest request, CancellationToken cancellationToken = default)
        {
            if (renderGateway == null) { throw new ArgumentNullException(nameof(renderGateway)); }
            if (request == null) { throw new ArgumentNullException(nameof(request)); }

            RenderResult renderResult = renderGateway.RenderPdfAsync(request, cancellationToken)
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();

            return ToReadOnlyStream(renderResult);
        }

        /// <summary>透過渲染核心將 PNG 請求轉為串流。</summary>
        /// <param name="renderGateway">渲染核心入口。</param>
        /// <param name="request">PNG 渲染請求。</param>
        /// <param name="cancellationToken">取消權杖。</param>
        /// <returns>唯讀 PNG 串流。</returns>
        /// <remarks>此為同步封裝，會封鎖目前執行緒直到渲染完成。</remarks>
        /// <exception cref="ArgumentNullException">`renderGateway` 或 `request` 為 `null` 時拋出。</exception>
        public static Stream ToPngStream(this IRenderGateway renderGateway, PngRenderRequest request, CancellationToken cancellationToken = default)
        {
            if (renderGateway == null) { throw new ArgumentNullException(nameof(renderGateway)); }
            if (request == null) { throw new ArgumentNullException(nameof(request)); }

            RenderResult renderResult = renderGateway.RenderPngAsync(request, cancellationToken)
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();

            return ToReadOnlyStream(renderResult);
        }

        /// <summary>透過渲染核心非同步將 PDF 請求轉為串流。</summary>
        /// <param name="renderGateway">渲染核心入口。</param>
        /// <param name="request">PDF 渲染請求。</param>
        /// <param name="cancellationToken">取消權杖。</param>
        /// <returns>唯讀 PDF 串流。</returns>
        /// <exception cref="ArgumentNullException">`renderGateway` 或 `request` 為 `null` 時拋出。</exception>
        public static async Task<Stream> ToPdfStreamAsync(this IRenderGateway renderGateway, PdfRenderRequest request, CancellationToken cancellationToken = default)
        {
            if (renderGateway == null) { throw new ArgumentNullException(nameof(renderGateway)); }
            if (request == null) { throw new ArgumentNullException(nameof(request)); }

            RenderResult renderResult = await renderGateway.RenderPdfAsync(request, cancellationToken).ConfigureAwait(false);
            return ToReadOnlyStream(renderResult);
        }

        /// <summary>透過渲染核心非同步將 PNG 請求轉為串流。</summary>
        /// <param name="renderGateway">渲染核心入口。</param>
        /// <param name="request">PNG 渲染請求。</param>
        /// <param name="cancellationToken">取消權杖。</param>
        /// <returns>唯讀 PNG 串流。</returns>
        /// <exception cref="ArgumentNullException">`renderGateway` 或 `request` 為 `null` 時拋出。</exception>
        public static async Task<Stream> ToPngStreamAsync(this IRenderGateway renderGateway, PngRenderRequest request, CancellationToken cancellationToken = default)
        {
            if (renderGateway == null) { throw new ArgumentNullException(nameof(renderGateway)); }
            if (request == null) { throw new ArgumentNullException(nameof(request)); }

            RenderResult renderResult = await renderGateway.RenderPngAsync(request, cancellationToken).ConfigureAwait(false);
            return ToReadOnlyStream(renderResult);
        }

        /// <summary>確認舊版 `PrintDocument` 路徑可在目前平台執行。</summary>
        /// <exception cref="PlatformNotSupportedException">目前平台非 Windows 時拋出，並提供遷移指引。</exception>
        private static void EnsureLegacyPrintDocumentSupported()
        {
            if (!OperatingSystem.IsWindows())
            {
                throw new PlatformNotSupportedException(LegacyPrintDocumentMigrationMessage);
            }
        }

        /// <summary>將渲染結果內容包裝為唯讀記憶體串流。</summary>
        /// <param name="renderResult">渲染結果。</param>
        /// <returns>可從開頭讀取的唯讀記憶體串流。</returns>
        /// <exception cref="ArgumentNullException">`renderResult` 為 `null` 時拋出。</exception>
        private static Stream ToReadOnlyStream(RenderResult renderResult)
        {
            if (renderResult == null) { throw new ArgumentNullException(nameof(renderResult)); }

            return new MemoryStream(renderResult.Content ?? Array.Empty<byte>(), writable: false);
        }
    }
}
