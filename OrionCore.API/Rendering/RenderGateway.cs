using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Orion.Api.Rendering.Contracts;
using Orion.Api.Rendering.Requests;

namespace Orion.Api.Rendering
{
    /// <summary>根據格式分派渲染請求的核心閘道。</summary>
    public sealed class RenderGateway : IRenderGateway
    {
        private readonly IReadOnlyDictionary<RenderFormat, IRenderer> _rendererMap;

        /// <summary>建立渲染閘道並載入可用渲染器。</summary>
        /// <param name="renderers">渲染器集合。</param>
        /// <exception cref="ArgumentNullException">`renderers` 為 `null` 時拋出。</exception>
        public RenderGateway(IEnumerable<IRenderer> renderers)
        {
            if (renderers == null) { throw new ArgumentNullException(nameof(renderers)); }
            _rendererMap = renderers.ToDictionary(renderer => renderer.Format);
        }

        /// <summary>依請求格式執行渲染流程。</summary>
        /// <param name="request">渲染請求內容。</param>
        /// <param name="cancellationToken">取消權杖。</param>
        /// <returns>渲染輸出結果。</returns>
        /// <exception cref="ArgumentNullException">`request` 為 `null` 時拋出。</exception>
        /// <exception cref="NotSupportedException">查無對應 `request.Format` 的渲染器時拋出。</exception>
        public Task<RenderResult> RenderAsync(RenderRequest request, CancellationToken cancellationToken = default)
        {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (!_rendererMap.TryGetValue(request.Format, out IRenderer renderer))
            {
                throw new NotSupportedException($"不支援的渲染格式：{request.Format}。");
            }

            return renderer.RenderAsync(request, cancellationToken);
        }

        /// <summary>執行 PDF 渲染流程。</summary>
        /// <param name="request">PDF 渲染請求。</param>
        /// <param name="cancellationToken">取消權杖。</param>
        /// <returns>PDF 渲染輸出結果。</returns>
        /// <exception cref="ArgumentNullException">`request` 為 `null` 時拋出。</exception>
        /// <exception cref="NotSupportedException">未註冊 PDF 渲染器時拋出。</exception>
        public Task<RenderResult> RenderPdfAsync(PdfRenderRequest request, CancellationToken cancellationToken = default)
        {
            return RenderAsync(request, cancellationToken);
        }

        /// <summary>執行 PNG 渲染流程。</summary>
        /// <param name="request">PNG 渲染請求。</param>
        /// <param name="cancellationToken">取消權杖。</param>
        /// <returns>PNG 渲染輸出結果。</returns>
        /// <exception cref="ArgumentNullException">`request` 為 `null` 時拋出。</exception>
        /// <exception cref="NotSupportedException">未註冊 PNG 渲染器時拋出。</exception>
        public Task<RenderResult> RenderPngAsync(PngRenderRequest request, CancellationToken cancellationToken = default)
        {
            return RenderAsync(request, cancellationToken);
        }
    }
}
