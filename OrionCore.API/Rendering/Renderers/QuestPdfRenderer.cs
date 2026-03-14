using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Orion.Api.Rendering.Contracts;
using Orion.Api.Rendering.Requests;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace Orion.Api.Rendering.Renderers
{
    /// <summary>以 QuestPDF 實作的 PDF 渲染器。</summary>
    public sealed class QuestPdfRenderer : IRenderer
    {
        /// <summary>取得此渲染器負責的輸出格式。</summary>
        public RenderFormat Format => RenderFormat.Pdf;

        /// <summary>執行 PDF 渲染並輸出位元組內容。</summary>
        /// <param name="request">渲染請求內容。</param>
        /// <param name="cancellationToken">取消權杖。</param>
        /// <returns>PDF 渲染輸出結果。</returns>
        /// <exception cref="OperationCanceledException">`cancellationToken` 已取消時拋出。</exception>
        /// <exception cref="ArgumentException">`request` 不是 `PdfRenderRequest` 時拋出。</exception>
        public Task<RenderResult> RenderAsync(RenderRequest request, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (request is not PdfRenderRequest pdfRequest)
            {
                throw new ArgumentException("QuestPdfRenderer 僅接受 PdfRenderRequest。", nameof(request));
            }

            QuestPDF.Settings.License = LicenseType.Community;

            using var outputStream = new MemoryStream();
            pdfRequest.Document.GeneratePdf(outputStream);

            var result = new RenderResult(
                RenderFormat.Pdf,
                outputStream.ToArray(),
                "application/pdf",
                ".pdf");

            return Task.FromResult(result);
        }
    }
}
