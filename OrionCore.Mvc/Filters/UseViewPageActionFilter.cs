using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Orion.Api.Extensions;
using Orion.Mvc.Attributes;

namespace Orion.Mvc.Filters
{
	/// <summary>依 `UseViewPageAttribute` 套用 View 名稱與標題的 Action 過濾器。</summary>
	public class UseViewPageActionFilter : ActionFilterAttribute
	{
		/// <summary>在 Action 執行後調整 `ViewResult` 的 ViewName 與 Title。</summary>
		/// <param name="context">目前 Action 執行結果內容。</param>
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
