using Microsoft.AspNetCore.Mvc.Filters;

namespace Orion.Mvc.Filters
{
    /// <summary></summary>
    public abstract class AbstractPageFilter : IPageFilter
    {
        /// <summary></summary>
        public virtual void OnPageHandlerSelected(PageHandlerSelectedContext context) { }
        /// <summary></summary>
        public virtual void OnPageHandlerExecuting(PageHandlerExecutingContext context) { }
        /// <summary></summary>
        public virtual void OnPageHandlerExecuted(PageHandlerExecutedContext context) { }
    }
}
