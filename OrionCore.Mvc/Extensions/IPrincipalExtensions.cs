using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using Orion.Api;
using Orion.Api.Extensions;

namespace Orion.Mvc.Extensions
{
    /// <summary>提供 `IPrincipal` 權限與 Claim 讀取擴充方法。</summary>
    public static class IPrincipalExtensions
    {

        /// <summary>檢查使用者是否具備任一指定角色（或動作）。</summary>
        /// <param name="user">目前使用者主體。</param>
        /// <param name="actLits">要檢查的角色列舉集合。</param>
        /// <returns>集合為空時回傳是否已登入；否則符合任一角色時回傳 `true`。</returns>
        public static bool AnyAct(this IPrincipal user, params Enum[] actLits)
        {
            if (actLits.Length == 0) { return user.Identity.IsAuthenticated; }
            return actLits.Any(x => user.IsInRole(x.ToString()));
        }

        /// <summary>檢查使用者是否具備全部指定角色（或動作）。</summary>
        /// <param name="user">目前使用者主體。</param>
        /// <param name="actLits">要檢查的角色列舉集合。</param>
        /// <returns>集合為空時回傳是否已登入；否則符合全部角色時回傳 `true`。</returns>
        public static bool AllAct(this IPrincipal user, params Enum[] actLits)
        {
            if (actLits.Length == 0) { return user.Identity.IsAuthenticated; }
            return actLits.All(x => user.IsInRole(x.ToString()));
        }





        /// <summary>依 Claim 名稱取得對應 Claim。</summary>
        /// <param name="user">目前使用者主體。</param>
        /// <param name="claimName">Claim 名稱。</param>
        /// <returns>找到時回傳 Claim，否則回傳 `null`。</returns>
        public static Claim GetClaim(this IPrincipal user, string claimName)
        {
            var identity = user.Identity as ClaimsIdentity;
            if (identity == null) { return null; }

            Claim claim = identity.Claims.FirstOrDefault(x => x.Type == claimName);
            return claim;
        }


        /// <summary>取得使用者識別碼。</summary>
        /// <param name="user">目前使用者主體。</param>
        /// <returns>使用者識別碼；未提供或轉型失敗時回傳 `0`。</returns>
        public static int GetUserId(this IPrincipal user)
        {
            Claim claim = GetClaim(user, OrionUser.UserId);
            return (claim?.Value).ConvertTo<int>();
        }

        /// <summary>取得使用者名稱。</summary>
        /// <param name="user">目前使用者主體。</param>
        /// <returns>使用者名稱；不存在時回傳 `null`。</returns>
        public static string GetUserName(this IPrincipal user)
        {
            Claim claim = GetClaim(user, OrionUser.UserName);
            return claim?.Value;
        }

        /// <summary>取得使用者類型。</summary>
        /// <param name="user">目前使用者主體。</param>
        /// <returns>使用者類型；不存在時回傳 `null`。</returns>
        public static string GetUserType(this IPrincipal user)
        {
            Claim claim = GetClaim(user, OrionUser.UserType);
            return claim?.Value;
        }

        /// <summary>取得帳號。</summary>
        /// <param name="user">目前使用者主體。</param>
        /// <returns>帳號字串；不存在時回傳 `null`。</returns>
        public static string GetAccount(this IPrincipal user)
        {
            Claim claim = GetClaim(user, OrionUser.Account);
            return claim?.Value;
        }





    }
}
