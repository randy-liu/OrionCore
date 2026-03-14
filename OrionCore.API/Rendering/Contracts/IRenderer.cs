using System.Threading;
using System.Threading.Tasks;
using Orion.Api.Rendering.Requests;

namespace Orion.Api.Rendering.Contracts
{
    /// <summary>定義單一輸出格式的渲染器。</summary>
    public interface IRenderer
    {
        /// <summary>取得此渲染器負責的輸出格式。</summary>
        RenderFormat Format { get; }

        /// <summary>執行渲染並回傳結果。</summary>
        /// <param name="request">渲染請求內容。</param>
        /// <param name="cancellationToken">取消權杖。</param>
        /// <returns>渲染輸出結果。</returns>
        /// <exception cref="System.OperationCanceledException">`cancellationToken` 已取消時拋出。</exception>
        /// <exception cref="System.ArgumentException">`request` 型別或內容不符合渲染器需求時拋出。</exception>
        Task<RenderResult> RenderAsync(RenderRequest request, CancellationToken cancellationToken = default);
    }
}
