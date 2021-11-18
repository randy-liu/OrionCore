using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Orion.Api;
using Orion.Api.Extensions;

namespace Orion.Mvc.Extensions
{
    /// <summary>定義 ModelState 的 Extension</summary>
    public static class ModelStateExtensions
    {

        /// <summary></summary>
        public static void ThrowIfNotValid(this ModelStateDictionary modelState)
        {
            if (modelState.IsValid) { return; }

            string msg = modelState.Values
                .Where(x => x.ValidationState == ModelValidationState.Invalid)
                .SelectMany(x => x.Errors)
                .Select(x => x.ErrorMessage)
                .JoinBy(", ");

            throw new UserException(msg);
        }

    }
}
