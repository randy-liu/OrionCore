using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Orion.Mvc.Filters
{
	public class HandlerAuthorizeFilter : AbstractPageFilter
	{
		private ConcurrentDictionary<MethodInfo, AuthorizeAttribute> _cache = new ConcurrentDictionary<MethodInfo, AuthorizeAttribute>();

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
