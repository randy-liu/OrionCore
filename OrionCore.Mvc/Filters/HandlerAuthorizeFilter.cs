using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Orion.Mvc.Filters
{
	/// <summary>依 Razor Page Handler 上的 `AuthorizeAttribute` 進行角色授權檢查。</summary>
	public class HandlerAuthorizeFilter : AbstractPageFilter
	{
		private ConcurrentDictionary<MethodInfo, AuthorizeAttribute> _cache = new ConcurrentDictionary<MethodInfo, AuthorizeAttribute>();

		/// <summary>在 Handler 執行前檢查使用者是否符合角色要求。</summary>
		/// <param name="context">Page Handler 執行前內容。</param>
		public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
		{
			if(context.HandlerMethod == null) { return; }

			var attr = _cache.GetOrAdd(context.HandlerMethod.MethodInfo, m => m.GetCustomAttribute<AuthorizeAttribute>());
			if (attr == null) { return; }

			ClaimsPrincipal user = context.HttpContext.User;

			bool isAuth = attr.Roles.Split(',').Any(user.IsInRole);
			if (!isAuth) { context.Result = new ForbidResult(); }
		}

	}
}
