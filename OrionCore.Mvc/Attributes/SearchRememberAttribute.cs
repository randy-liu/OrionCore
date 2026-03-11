using System;

namespace Orion.Mvc.Attributes
{
    /// <summary>標記方法需要記住搜尋條件（透過 QueryString/Cookie）。</summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class SearchRememberAttribute : Attribute
    {

		/// <summary>從 QueryString 取得要儲存在 cookie 的名稱 string storeName = qs[StoreKeyParam]</summary>
        public string StoreKeyParam { get; set; }

        /// <summary>只要儲存的 QueryString 參數</summary>
        public string[] StoreOnly { get; set; } = new string[0];

        /// <summary>要忽略的 QueryString 參數</summary>
        public string[] Ignore { get; set; } = new string[0];

    }


}
