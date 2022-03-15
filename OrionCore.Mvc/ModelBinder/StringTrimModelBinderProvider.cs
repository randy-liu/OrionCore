using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Orion.Mvc.ModelBinder
{
    /// <summary></summary>
    public class StringTrimModelBinderProvider : IModelBinderProvider
    {
        /// <summary></summary>
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null) { throw new ArgumentNullException(nameof(context)); }

            if (context.Metadata.IsComplexType) { return null; }
            if (context.Metadata.ModelType != typeof(string)) { return null; }

            var loggerFactory = context.Services.GetService<ILoggerFactory>();

            return new StringTrimModelBinder(new SimpleTypeModelBinder(typeof(string), loggerFactory));
        }
    }



    /// <summary></summary>
    public class StringTrimModelBinder : IModelBinder
    {
        private readonly IModelBinder _fallbackBinder;

        /// <summary></summary>
        public StringTrimModelBinder(IModelBinder fallbackBinder)
        {
            _fallbackBinder = fallbackBinder;
        }


        /// <summary></summary>
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null) { throw new ArgumentNullException(nameof(bindingContext)); }

            ValueProviderResult result = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            string value = result.FirstValue as string;

            if (!string.IsNullOrEmpty(value))
            {
                bindingContext.Result = ModelBindingResult.Success(value?.Trim());
                return Task.CompletedTask;
            }
            return _fallbackBinder.BindModelAsync(bindingContext);
        }

    }

}