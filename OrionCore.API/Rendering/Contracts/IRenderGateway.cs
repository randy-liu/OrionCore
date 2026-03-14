using System.Threading;
using System.Threading.Tasks;
using Orion.Api.Rendering.Requests;

namespace Orion.Api.Rendering.Contracts
{
    /// <summary>定義渲染核心入口。</summary>
    public interface IRenderGateway
    {
        /// <summary>依請求格式執行渲染流程。</summary>
        /// <param name="request">渲染請求內容。</param>
        /// <param name="cancellationToken">取消權杖。</param>
        /// <returns>渲染輸出結果。</returns>
        /// <exception cref="System.ArgumentNullException">`request` 為 `null` 時拋出。</exception>
        /// <exception cref="System.NotSupportedException">查無對應格式渲染器時拋出。</exception>
        Task<RenderResult> RenderAsync(RenderRequest request, CancellationToken cancellationToken = default);

        /// <summary>執行 PDF 渲染流程。</summary>
        /// <param name="request">PDF 渲染請求。</param>
        /// <param name="cancellationToken">取消權杖。</param>
        /// <returns>PDF 渲染輸出結果。</returns>
        /// <exception cref="System.ArgumentNullException">`request` 為 `null` 時拋出。</exception>
        /// <exception cref="System.NotSupportedException">未註冊 PDF 渲染器時拋出。</exception>
        Task<RenderResult> RenderPdfAsync(PdfRenderRequest request, CancellationToken cancellationToken = default);

        /// <summary>執行 PNG 渲染流程。</summary>
        /// <param name="request">PNG 渲染請求。</param>
        /// <param name="cancellationToken">取消權杖。</param>
        /// <returns>PNG 渲染輸出結果。</returns>
        /// <exception cref="System.ArgumentNullException">`request` 為 `null` 時拋出。</exception>
        /// <exception cref="System.NotSupportedException">未註冊 PNG 渲染器時拋出。</exception>
        Task<RenderResult> RenderPngAsync(PngRenderRequest request, CancellationToken cancellationToken = default);
    }
}
