using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;

namespace Orion.Mvc
{
    /// <summary></summary>
    public class RazorViewCaller
    {
        private readonly IRazorViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IServiceProvider _serviceProvider;


        /// <summary></summary>
        public RazorViewCaller(
            IRazorViewEngine viewEngine,
            ITempDataProvider tempDataProvider,
            IServiceProvider serviceProvider
        ) {
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
        }



        private async Task<string> renderAsync<TModel>(string viewPath, TModel model)
        {
            var viewEngineResult = _viewEngine.GetView(viewPath, viewPath, false);

            if (!viewEngineResult.Success)
            { throw new InvalidOperationException(string.Format("Can not find view '{0}'", viewPath)); }


            var httpContext = new DefaultHttpContext { RequestServices = _serviceProvider };
            var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
            var tempData = new TempDataDictionary(httpContext, _tempDataProvider);
            var viewData = new ViewDataDictionary<TModel>(new EmptyModelMetadataProvider(), new ModelStateDictionary())
            {
                Model = model,
            };


            using (var writer = new StringWriter())
            {
                var view = viewEngineResult.View;
                var viewContext = new ViewContext(actionContext, view, viewData, tempData, writer, new HtmlHelperOptions());
                await view.RenderAsync(viewContext);

                string result = writer.ToString();
                return result;
            }
        }



        /// <summary></summary>
        public string Render<TModel>(string viewPath, TModel model)
        {
            Task<string> task = renderAsync(viewPath, model);
            task.Wait();
            return task.Result;
        }

    }
}
