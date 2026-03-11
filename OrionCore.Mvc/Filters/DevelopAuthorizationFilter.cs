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
    /// <summary>開發環境用授權過濾器，於未登入時以指定 Claims 自動登入。</summary>
    public class DevelopAuthorizationFilter : IAuthorizationFilter
    {
        private readonly List<Claim> _claims;

        /// <summary>建立開發授權過濾器。</summary>
        /// <param name="claims">未登入時要建立的使用者 Claims。</param>
        public DevelopAuthorizationFilter(List<Claim> claims)
        {
            _claims = claims;
        }

        /// <summary>授權檢查時，若目前使用者未驗證則以 Cookie 方案執行登入。</summary>
        /// <param name="context">授權過濾器內容。</param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated) { return; }

            var claimsIdentity = new ClaimsIdentity(_claims, CookieAuthenticationDefaults.AuthenticationScheme);
            context.HttpContext.SignInAsync(new ClaimsPrincipal(claimsIdentity)).Wait();
        }
    }




    /// <summary>開發環境用泛型授權過濾器，首次執行時建立含角色的測試登入身分。</summary>
    public class DevelopAuthorizationFilter<TActEnum> : IAuthorizationFilter
    {
        private bool _runOneFlag = false;


        /// <summary>首次授權檢查時，若尚未登入則建立角色與基本使用者 Claims 後登入並導向首頁。</summary>
        /// <param name="context">授權過濾器內容。</param>
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
