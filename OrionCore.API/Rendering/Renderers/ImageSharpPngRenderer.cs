using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Orion.Api.Rendering.Contracts;
using Orion.Api.Rendering.Requests;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Orion.Api.Rendering.Renderers
{
    /// <summary>以 ImageSharp 實作的 PNG 渲染器。</summary>
    public sealed class ImageSharpPngRenderer : IRenderer
    {
        /// <summary>取得此渲染器負責的輸出格式。</summary>
        public RenderFormat Format => RenderFormat.Png;

        /// <summary>執行 PNG 渲染並輸出位元組內容。</summary>
        /// <param name="request">渲染請求內容。</param>
        /// <param name="cancellationToken">取消權杖。</param>
        /// <returns>PNG 渲染輸出結果。</returns>
        /// <exception cref="OperationCanceledException">`cancellationToken` 已取消時拋出。</exception>
        /// <exception cref="ArgumentException">`request` 不是 `PngRenderRequest`，或像素資料長度與寬高不符時拋出。</exception>
        public Task<RenderResult> RenderAsync(RenderRequest request, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (request is not PngRenderRequest pngRequest)
            {
                throw new ArgumentException("ImageSharpPngRenderer 僅接受 PngRenderRequest。", nameof(request));
            }

            int expectedBytes = checked(pngRequest.Width * pngRequest.Height * 4);
            if (pngRequest.RgbaPixels.Length != expectedBytes)
            {
                throw new ArgumentException("RGBA 像素資料長度與寬高不符。", nameof(request));
            }

            using Image<Rgba32> image = Image.LoadPixelData<Rgba32>(pngRequest.RgbaPixels, pngRequest.Width, pngRequest.Height);
            pngRequest.Pipeline?.Invoke(image);

            using var outputStream = new MemoryStream();
            image.SaveAsPng(outputStream);

            var result = new RenderResult(
                RenderFormat.Png,
                outputStream.ToArray(),
                "image/png",
                ".png");

            return Task.FromResult(result);
        }
    }
}
