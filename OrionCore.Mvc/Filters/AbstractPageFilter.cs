using Microsoft.AspNetCore.Mvc.Filters;

namespace Orion.Mvc.Filters
{
    public abstract class AbstractPageFilter : IPageFilter
    {
        public virtual void OnPageHandlerSelected(PageHandlerSelectedContext context) { }
        public virtual void OnPageHandlerExecuting(PageHandlerExecutingContext context) { }
        public virtual void OnPageHandlerExecuted(PageHandlerExecutedContext context) { }
    }
}
