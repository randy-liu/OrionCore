using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;

namespace Orion.Mvc.Filters
{
    /// <summary></summary>
    public class PageModelInjectFilter : AbstractPageFilter
    {
        /// <summary>注入器的快取</summary>
        private readonly ConcurrentDictionary<Type, Action<PageModel, IServiceProvider>> _injecterCache = new ConcurrentDictionary<Type, Action<PageModel, IServiceProvider>>();


        /// <summary>建構注入器</summary>
        private Action<PageModel, IServiceProvider> buildInjecter(Type type) 
        {
            Action<PageModel, IServiceProvider> action = (page, provider) => { };

            /* 取得 Properties 且是可以寫入，並具有 [Inject] Attribute */
            var props = type.GetProperties()
                .Where(p => p.CanWrite)
                .Where(p => p.IsDefined(typeof(InjectAttribute)));

            foreach (var prop in props)
            {
                action += (page, provider) => 
                { 
                    /* 如果 Property 是已經有 Value 的就不要進行注入 */
                    if(prop.GetValue(page) != null) { return; }

                    /* 從 provider 取得依賴的物件 */
                    object value = provider.GetRequiredService(prop.PropertyType);
                    prop.SetValue(page, value);
                };
            }

            return action;
        }


        /// <summary>在選取處理常式方法之後，但在進行模型系結之前呼叫。</summary>
        public override void OnPageHandlerSelected(PageHandlerSelectedContext context)
        {
            var page = context.HandlerInstance as PageModel;
            if (page == null) { return; }

            Action<PageModel, IServiceProvider> injecter = _injecterCache.GetOrAdd(page.GetType(), buildInjecter);
            injecter(page, context.HttpContext.RequestServices);
        }

    }
}
