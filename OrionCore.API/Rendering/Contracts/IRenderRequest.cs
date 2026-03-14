namespace Orion.Api.Rendering.Contracts
{
    /// <summary>定義渲染請求的共用欄位。</summary>
    public interface IRenderRequest
    {
        /// <summary>取得目標輸出格式。</summary>
        RenderFormat Format { get; }
    }
}
