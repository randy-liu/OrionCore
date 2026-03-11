using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Orion.Api.Extensions;

namespace Orion.Mvc.Html
{

    /// <summary>提供 Bootstrap 樣式 `label` 的 HtmlHelper 擴充方法。</summary>
    public static class BsLabelExtensions
    {


        /// <summary>產生 Bootstrap 樣式 `label`。</summary>
        /// <param name="htmlHelper">HTML Helper。</param>
        /// <param name="expression">模型屬性運算式。</param>
        /// <param name="labelText">要顯示的 Label 文字。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent BsLabel(this IHtmlHelper htmlHelper, string expression, string labelText = null)
        {
            return BsLabel(htmlHelper, expression, labelText, null);
        }
        /// <summary>產生 Bootstrap 樣式 `label`，並套用匿名 HTML 屬性。</summary>
        /// <param name="htmlHelper">HTML Helper。</param>
        /// <param name="expression">模型屬性運算式。</param>
        /// <param name="htmlAttributes">HTML 屬性。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent BsLabel(this IHtmlHelper htmlHelper, string expression, object htmlAttributes)
        {
            return BsLabel(htmlHelper, expression, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }
        /// <summary>產生 Bootstrap 樣式 `label`，並套用 HTML 屬性集合。</summary>
        /// <param name="htmlHelper">HTML Helper。</param>
        /// <param name="expression">模型屬性運算式。</param>
        /// <param name="htmlAttributes">HTML 屬性。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent BsLabel(this IHtmlHelper htmlHelper, string expression, IDictionary<string, object> htmlAttributes)
        {
            return BsLabel(htmlHelper, expression, null, htmlAttributes);
        }
        /// <summary>產生 Bootstrap 樣式 `label`，指定文字與匿名 HTML 屬性。</summary>
        /// <param name="htmlHelper">HTML Helper。</param>
        /// <param name="expression">模型屬性運算式。</param>
        /// <param name="labelText">要顯示的 Label 文字。</param>
        /// <param name="htmlAttributes">HTML 屬性。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent BsLabel(this IHtmlHelper htmlHelper, string expression, string labelText, object htmlAttributes)
        {
            return BsLabel(htmlHelper, expression, labelText, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }
        /// <summary>產生 Bootstrap 樣式 `label`，指定文字與 HTML 屬性集合。</summary>
        /// <param name="htmlHelper">HTML Helper。</param>
        /// <param name="expression">模型屬性運算式。</param>
        /// <param name="labelText">參數說明。</param>
        /// <param name="htmlAttributes">HTML 屬性。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent BsLabel(this IHtmlHelper htmlHelper, string expression, string labelText, IDictionary<string, object> htmlAttributes)
        {
            if (htmlAttributes == null) { htmlAttributes = new Dictionary<string, object>(); }

            HelperUtils.AddCssClass(htmlAttributes, "control-label");
            return htmlHelper.Label(expression, labelText, htmlAttributes);
        }



        /*==================================================== */

        /// <summary>依列舉值產生 Bootstrap 樣式 `label`。</summary>
        /// <param name="htmlHelper">HTML Helper。</param>
        /// <param name="enumValue">參數說明。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent BsLabel(this IHtmlHelper htmlHelper, Enum enumValue)
        {
            return BsLabel(htmlHelper, enumValue, null);
        }
        /// <summary>依列舉值產生 Bootstrap 樣式 `label`，並套用匿名 HTML 屬性。</summary>
        /// <param name="htmlHelper">HTML Helper。</param>
        /// <param name="enumValue">參數說明。</param>
        /// <param name="htmlAttributes">HTML 屬性。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent BsLabel(this IHtmlHelper htmlHelper, Enum enumValue, object htmlAttributes)
        {
            return BsLabel(htmlHelper, enumValue, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }
        /// <summary>依列舉值產生 Bootstrap 樣式 `label`，並套用 HTML 屬性集合。</summary>
        /// <param name="htmlHelper">HTML Helper。</param>
        /// <param name="enumValue">參數說明。</param>
        /// <param name="htmlAttributes">HTML 屬性。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent BsLabel(this IHtmlHelper htmlHelper, Enum enumValue, IDictionary<string, object> htmlAttributes) 
        {
            if (htmlAttributes == null) { htmlAttributes = new Dictionary<string, object>(); }

            HelperUtils.AddCssClass(htmlAttributes, "control-label");
            string labelText = enumValue.GetDisplayName();
            return htmlHelper.Label(enumValue.ToString(), labelText, htmlAttributes);
        }




        /*==================================================== */

        /// <summary>依模型屬性產生 Bootstrap 樣式 `label`。</summary>
        /// <param name="htmlHelper">HTML Helper。</param>
        /// <param name="expression">模型屬性運算式。</param>
        /// <param name="labelText">參數說明。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent BsLabelFor<TModel, TValue>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, string labelText = null)
        {
            return BsLabelFor(htmlHelper, expression, labelText, null);
        }
        /// <summary>依模型屬性產生 Bootstrap 樣式 `label`，並套用匿名 HTML 屬性。</summary>
        /// <param name="htmlHelper">HTML Helper。</param>
        /// <param name="expression">模型屬性運算式。</param>
        /// <param name="htmlAttributes">HTML 屬性。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent BsLabelFor<TModel, TValue>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, object htmlAttributes)
        {
            return BsLabelFor(htmlHelper, expression, null, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }
        /// <summary>依模型屬性產生 Bootstrap 樣式 `label`，並套用 HTML 屬性集合。</summary>
        /// <param name="htmlHelper">HTML Helper。</param>
        /// <param name="expression">模型屬性運算式。</param>
        /// <param name="htmlAttributes">HTML 屬性。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent BsLabelFor<TModel, TValue>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, IDictionary<string, object> htmlAttributes)
        {
            return BsLabelFor(htmlHelper, expression, null, htmlAttributes);
        }
        /// <summary>依模型屬性產生 Bootstrap 樣式 `label`，指定文字與匿名 HTML 屬性。</summary>
        /// <param name="htmlHelper">HTML Helper。</param>
        /// <param name="expression">模型屬性運算式。</param>
        /// <param name="labelText">參數說明。</param>
        /// <param name="htmlAttributes">HTML 屬性。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent BsLabelFor<TModel, TValue>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, string labelText, object htmlAttributes)
        {
            return BsLabelFor(htmlHelper, expression, labelText, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }
        /// <summary>依模型屬性產生 Bootstrap 樣式 `label`，指定文字與 HTML 屬性集合。</summary>
        /// <param name="htmlHelper">HTML Helper。</param>
        /// <param name="expression">模型屬性運算式。</param>
        /// <param name="labelText">參數說明。</param>
        /// <param name="htmlAttributes">HTML 屬性。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent BsLabelFor<TModel, TValue>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, string labelText, IDictionary<string, object> htmlAttributes)
        {
            if (htmlAttributes == null) { htmlAttributes = new Dictionary<string, object>(); }

            HelperUtils.AddCssClass(htmlAttributes, "control-label");
            return htmlHelper.LabelFor(expression, labelText, htmlAttributes);
        }



        /*==================================================== */

        /// <summary>依目前模型產生 Bootstrap 樣式 `label`。</summary>
        /// <param name="htmlHelper">HTML Helper。</param>
        /// <param name="labelText">參數說明。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent BsLabelForModel(this IHtmlHelper htmlHelper, string labelText = null)
        {
            return BsLabelForModel(htmlHelper, labelText, null);
        }
        /// <summary>依目前模型產生 Bootstrap 樣式 `label`，並套用匿名 HTML 屬性。</summary>
        /// <param name="htmlHelper">HTML Helper。</param>
        /// <param name="htmlAttributes">HTML 屬性。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent BsLabelForModel(this IHtmlHelper htmlHelper, object htmlAttributes)
        {
            return BsLabelForModel(htmlHelper, null, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }
        /// <summary>依目前模型產生 Bootstrap 樣式 `label`，並套用 HTML 屬性集合。</summary>
        /// <param name="htmlHelper">HTML Helper。</param>
        /// <param name="htmlAttributes">HTML 屬性。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent BsLabelForModel(this IHtmlHelper htmlHelper, IDictionary<string, object> htmlAttributes)
        {
            return BsLabelForModel(htmlHelper, null, htmlAttributes);
        }
        /// <summary>依目前模型產生 Bootstrap 樣式 `label`，指定文字與匿名 HTML 屬性。</summary>
        /// <param name="htmlHelper">HTML Helper。</param>
        /// <param name="labelText">參數說明。</param>
        /// <param name="htmlAttributes">HTML 屬性。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent BsLabelForModel(this IHtmlHelper htmlHelper, string labelText, object htmlAttributes)
        {
            return BsLabelForModel(htmlHelper, labelText, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }
        /// <summary>依目前模型產生 Bootstrap 樣式 `label`，指定文字與 HTML 屬性集合。</summary>
        /// <param name="htmlHelper">HTML Helper。</param>
        /// <param name="labelText">參數說明。</param>
        /// <param name="htmlAttributes">HTML 屬性。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent BsLabelForModel(this IHtmlHelper htmlHelper, string labelText, IDictionary<string, object> htmlAttributes)
        {
            if (htmlAttributes == null) { htmlAttributes = new Dictionary<string, object>(); }

            HelperUtils.AddCssClass(htmlAttributes, "control-label");
            return htmlHelper.LabelForModel(labelText, htmlAttributes);
        }

    }

}
