using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Orion.Api.Extensions;
using Orion.Mvc.Attributes;

namespace Orion.Mvc.Filters
{
	/// <summary></summary>
	public class UseViewPageActionFilter : ActionFilterAttribute
	{
		/// <summary></summary>
		public override void OnActionExecuted(ActionExecutedContext context)
		{
			base.OnActionExecuted(context);
			var result = context.Result as ViewResult;
			if (result == null) { return; }

			var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
			if(actionDescriptor == null) { return; }

			var attr = actionDescriptor.MethodInfo.GetCustomAttribute<UseViewPageAttribute>();
			if (attr == null) { return; }

			if (attr.Title.HasText()) { result.ViewData["Title"] = attr.Title; }

			if (result.ViewName.NoText()) { result.ViewName = attr.ViewName; }
		}

	}
}