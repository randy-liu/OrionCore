using Orion.Api.Rendering.Contracts;

namespace Orion.Api.Rendering.Requests
{
    /// <summary>表示渲染請求的共用基底。</summary>
    public abstract class RenderRequest : IRenderRequest
    {
        /// <summary>建立渲染請求基底。</summary>
        /// <param name="format">目標輸出格式。</param>
        protected RenderRequest(RenderFormat format)
        {
            Format = format;
        }

        /// <summary>取得目標輸出格式。</summary>
        public RenderFormat Format { get; }
    }
}
