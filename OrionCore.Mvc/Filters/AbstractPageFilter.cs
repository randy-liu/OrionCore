using Microsoft.AspNetCore.Mvc.Filters;

namespace Orion.Mvc.Filters
{
    /// <summary>`IPageFilter` 抽象基底，提供可覆寫的空實作。</summary>
    public abstract class AbstractPageFilter : IPageFilter
    {
        /// <summary>在選取處理常式後呼叫。</summary>
        /// <param name="context">Page Handler 選取內容。</param>
        public virtual void OnPageHandlerSelected(PageHandlerSelectedContext context) { }
        /// <summary>在執行處理常式前呼叫。</summary>
        /// <param name="context">Page Handler 執行前內容。</param>
        public virtual void OnPageHandlerExecuting(PageHandlerExecutingContext context) { }
        /// <summary>在執行處理常式後呼叫。</summary>
        /// <param name="context">Page Handler 執行後內容。</param>
        public virtual void OnPageHandlerExecuted(PageHandlerExecutedContext context) { }
    }
}
