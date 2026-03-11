using System.Collections.Specialized;
using System.Web;
using Microsoft.AspNetCore.Http;

namespace Orion.Mvc.Extensions
{
    /// <summary>提供 `QueryString` 增修參數的擴充方法。</summary>
    public static class QueryStringExtensions
    {

        /// <summary>設定或覆寫指定 QueryString 參數。</summary>
        /// <param name="qs">原始 QueryString。</param>
        /// <param name="name">參數名稱。</param>
        /// <param name="value">參數值。</param>
        /// <returns>更新後的 QueryString。</returns>
        public static QueryString Set(this QueryString qs, string name, string value)
        {
            NameValueCollection values = HttpUtility.ParseQueryString(qs.ToString());
            values.Set(name, value);

            return new QueryString("?" + values.ToString());
        }

        /// <summary>移除指定 QueryString 參數。</summary>
        /// <param name="qs">原始 QueryString。</param>
        /// <param name="name">要移除的參數名稱。</param>
        /// <returns>更新後的 QueryString。</returns>
        public static QueryString Remove(this QueryString qs, string name)
        {
            NameValueCollection values = HttpUtility.ParseQueryString(qs.ToString());
            values.Remove(name);

            return new QueryString("?" + values.ToString());
        }


    }
}
