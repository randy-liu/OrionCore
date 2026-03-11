using System;

namespace Orion.Mvc.Attributes
{
    /// <summary>指定 Action 要使用的 View 與頁面標題。</summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class UseViewPageAttribute : Attribute
    {
        /// <summary>View 名稱。</summary>
        public string ViewName { get; private set; }

        /// <summary>頁面標題。</summary>
        public string Title { get; private set; }


        /// <summary>建立 View 設定屬性。</summary>
        /// <param name="viewName">View 名稱。</param>
        public UseViewPageAttribute(string viewName)
        {
            ViewName = viewName;
        }

        /// <summary>建立 View 設定屬性並指定標題。</summary>
        /// <param name="viewName">View 名稱。</param>
        /// <param name="title">頁面標題。</param>
        public UseViewPageAttribute(string viewName, string title)
        {
            ViewName = viewName;
            Title = title;
        }

    }
}