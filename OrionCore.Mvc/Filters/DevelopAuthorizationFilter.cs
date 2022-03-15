using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Filters;
using Orion.Api;
using Orion.Api.Extensions;
using Orion.Mvc.Extensions;

namespace Orion.Mvc.Filters
{
    /// <summary></summary>
    public class DevelopAuthorizationFilter : IAuthorizationFilter
    {
        private readonly List<Claim> _claims;

        /// <summary></summary>
        public DevelopAuthorizationFilter(List<Claim> claims)
        {
            _claims = claims;
        }

        /// <summary></summary>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated) { return; }

            var claimsIdentity = new ClaimsIdentity(_claims, CookieAuthenticationDefaults.AuthenticationScheme);
            context.HttpContext.SignInAsync(new ClaimsPrincipal(claimsIdentity)).Wait();
        }
    }




    /// <summary></summary>
    public class DevelopAuthorizationFilter<TActEnum> : IAuthorizationFilter
    {
        private bool _runOneFlag = false;


        /// <summary></summary>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (_runOneFlag) { return; }
            _runOneFlag = true;

            if (context.HttpContext.User.Identity.IsAuthenticated) { return; }

            List<Claim> claims = OrionUtils.EnumToDictionary<TActEnum>()
                .Keys.ToList(x => new Claim(ClaimTypes.Role, x));

            claims.Add(new Claim(ClaimTypes.Role, "DevelopAdmin"));

            claims.Add(new Claim(OrionUser.UserId, "11"));
            claims.Add(new Claim(OrionUser.UserName, "Admin"));
            claims.Add(new Claim(OrionUser.Account, "admin"));

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            context.HttpContext.SignInAsync(new ClaimsPrincipal(claimsIdentity)).Wait();
            context.HttpContext.Response.Redirect("/");
        }
    }
}