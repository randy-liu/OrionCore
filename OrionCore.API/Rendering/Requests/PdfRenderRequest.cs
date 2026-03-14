using System;
using QuestPDF.Infrastructure;

namespace Orion.Api.Rendering.Requests
{
    /// <summary>描述 PDF 渲染所需資訊。</summary>
    public sealed class PdfRenderRequest : RenderRequest
    {
        /// <summary>建立 PDF 渲染請求。</summary>
        /// <param name="document">QuestPDF 文件模型。</param>
        /// <exception cref="ArgumentNullException">`document` 為 `null` 時拋出。</exception>
        public PdfRenderRequest(IDocument document) : base(RenderFormat.Pdf)
        {
            Document = document ?? throw new ArgumentNullException(nameof(document));
        }

        /// <summary>取得 QuestPDF 文件模型。</summary>
        public IDocument Document { get; }
    }
}
