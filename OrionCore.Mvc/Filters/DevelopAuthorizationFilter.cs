using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Filters;
using OrionCore.API;
using OrionCore.API.Extensions;
using OrionCore.Mvc.Extensions;

namespace OrionCore.Mvc.Filters
{
    public class DevelopAuthorizationFilter<TActEnum> : IAuthorizationFilter
    {
        private bool _runOneFlag = false;


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