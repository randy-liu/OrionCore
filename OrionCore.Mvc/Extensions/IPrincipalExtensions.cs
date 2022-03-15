using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using Orion.Api;
using Orion.Api.Extensions;

namespace Orion.Mvc.Extensions
{
    /// <summary></summary>
    public static class IPrincipalExtensions
    {

        /// <summary></summary>
        public static bool AnyAct(this IPrincipal user, params Enum[] actLits)
        {
            if (actLits.Length == 0) { return user.Identity.IsAuthenticated; }
            return actLits.Any(x => user.IsInRole(x.ToString()));
        }

        /// <summary></summary>
        public static bool AllAct(this IPrincipal user, params Enum[] actLits)
        {
            if (actLits.Length == 0) { return user.Identity.IsAuthenticated; }
            return actLits.All(x => user.IsInRole(x.ToString()));
        }





        /// <summary></summary>
        public static Claim GetClaim(this IPrincipal user, string claimName)
        {
            var identity = user.Identity as ClaimsIdentity;
            if (identity == null) { return null; }

            Claim claim = identity.Claims.FirstOrDefault(x => x.Type == claimName);
            return claim;
        }


        /// <summary></summary>
        public static int GetUserId(this IPrincipal user)
        {
            Claim claim = GetClaim(user, OrionUser.UserId);
            return (claim?.Value).ConvertTo<int>();
        }

        /// <summary></summary>
        public static string GetUserName(this IPrincipal user)
        {
            Claim claim = GetClaim(user, OrionUser.UserName);
            return claim?.Value;
        }

        /// <summary></summary>
        public static string GetUserType(this IPrincipal user)
        {
            Claim claim = GetClaim(user, OrionUser.UserType);
            return claim?.Value;
        }

        /// <summary></summary>
        public static string GetAccount(this IPrincipal user)
        {
            Claim claim = GetClaim(user, OrionUser.Account);
            return claim?.Value;
        }





    }
}