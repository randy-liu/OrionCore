using System.Collections.Specialized;
using System.Web;
using Microsoft.AspNetCore.Http;

namespace Orion.Mvc.Extensions
{
    /// <summary></summary>
    public static class QueryStringExtensions
    {

        /// <summary></summary>
        public static QueryString Set(this QueryString qs, string name, string value)
        {
            NameValueCollection values = HttpUtility.ParseQueryString(qs.ToString());
            values.Set(name, value);

            return new QueryString("?" + values.ToString());
        }

        /// <summary></summary>
        public static QueryString Remove(this QueryString qs, string name)
        {
            NameValueCollection values = HttpUtility.ParseQueryString(qs.ToString());
            values.Remove(name);

            return new QueryString("?" + values.ToString());
        }


    }
}
