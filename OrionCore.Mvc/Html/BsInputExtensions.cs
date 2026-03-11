using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Orion.Mvc.Html
{
    /// <summary>提供 Bootstrap Input 與 StaticControl 的 HtmlHelper 擴充方法。</summary>
    public static class BsInputExtensions
    {

        /// <summary>建立唯讀靜態顯示欄位。</summary>
        /// <param name="htmlHelper">HTML Helper。</param>
        /// <param name="value">顯示值。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent BsStaticControl(this IHtmlHelper htmlHelper, string value)
        {
            return BsStaticControl(htmlHelper, value, null);
        }
        /// <summary>建立唯讀靜態顯示欄位。</summary>
        /// <param name="htmlHelper">HTML Helper。</param>
        /// <param name="value">顯示值。</param>
        /// <param name="htmlAttributes">HTML 屬性。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent BsStaticControl(this IHtmlHelper htmlHelper, string value, object htmlAttributes)
        {
            return BsStaticControl(htmlHelper, value, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }
        /// <summary>建立唯讀靜態顯示欄位。</summary>
        /// <param name="htmlHelper">HTML Helper。</param>
        /// <param name="value">顯示值。</param>
        /// <param name="htmlAttributes">HTML 屬性。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent BsStaticControl(this IHtmlHelper htmlHelper, string value, IDictionary<string, object> htmlAttributes)
        {
            var span = new TagBuilder("span");
            span.InnerHtml.Append(value);
            span.Attributes["class"] = "value-text";

            var div = new TagBuilder("div");
            div.InnerHtml.AppendHtml(span);
            div.MergeAttributes(htmlAttributes);
            div.AddCssClass("form-control-static");

            return div;
        }


        /// <summary>建立唯讀靜態顯示欄位。</summary>
        /// <param name="htmlHelper">HTML Helper。</param>
        /// <param name="name">欄位名稱。</param>
        /// <param name="value">欄位值。</param>
        /// <param name="htmlAttributes">HTML 屬性。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent BsStaticControl(this IHtmlHelper htmlHelper, string name, object value, object htmlAttributes)
        {
            return BsStaticControl(htmlHelper, name, value, value?.ToString(), htmlAttributes);
        }

        /// <summary>建立唯讀靜態顯示欄位。</summary>
        /// <param name="htmlHelper">HTML Helper。</param>
        /// <param name="name">欄位名稱。</param>
        /// <param name="value">欄位值。</param>
        /// <param name="text">顯示文字。</param>
        /// <param name="htmlAttributes">HTML 屬性。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent BsStaticControl(this IHtmlHelper htmlHelper, string name, object value, string text, object htmlAttributes)
        {
            return BsStaticControl(htmlHelper, name, value, text, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        /// <summary>建立唯讀靜態顯示欄位。</summary>
        /// <param name="htmlHelper">HTML Helper。</param>
        /// <param name="name">欄位名稱。</param>
        /// <param name="value">欄位值。</param>
        /// <param name="text">顯示文字。</param>
        /// <param name="htmlAttributes">HTML 屬性。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent BsStaticControl(this IHtmlHelper htmlHelper, string name, object value, string text, IDictionary<string, object> htmlAttributes)
        {
            var input = htmlHelper.Hidden(name, value);

            var span = new TagBuilder("span");
            span.InnerHtml.Append(text);
            span.Attributes["class"] = "value-text";

            var div = new TagBuilder("div");
            div.InnerHtml.AppendHtml(input);
            div.InnerHtml.AppendHtml(span);
            div.MergeAttributes(htmlAttributes);
            div.AddCssClass("form-control-static");

            return div;
        }


        /// <summary>依模型屬性建立唯讀靜態顯示欄位。</summary>
        /// <param name="htmlHelper">型別化 HTML Helper。</param>
        /// <param name="expression">模型屬性運算式。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent BsStaticControlFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            return BsStaticControlFor(htmlHelper, expression, null);
        }
        /// <summary>依模型屬性建立唯讀靜態顯示欄位。</summary>
        /// <param name="htmlHelper">型別化 HTML Helper。</param>
        /// <param name="expression">模型屬性運算式。</param>
        /// <param name="htmlAttributes">HTML 屬性。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent BsStaticControlFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            return BsStaticControlFor(htmlHelper, expression, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }
        /// <summary>依模型屬性建立唯讀靜態顯示欄位，並指定格式/HTML 屬性。</summary>
        /// <param name="htmlHelper">型別化 HTML Helper。</param>
        /// <param name="expression">模型屬性運算式。</param>
        /// <param name="format">格式字串。</param>
        /// <param name="htmlAttributes">HTML 屬性。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent BsStaticControlFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string format, object htmlAttributes)
        {
            return BsStaticControlFor(htmlHelper, expression, format, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }
        /// <summary>依模型屬性建立唯讀靜態顯示欄位。</summary>
        /// <param name="htmlHelper">型別化 HTML Helper。</param>
        /// <param name="expression">模型屬性運算式。</param>
        /// <param name="htmlAttributes">HTML 屬性。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent BsStaticControlFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes)
        {
            return BsStaticControlFor(htmlHelper, expression, null, htmlAttributes);
        }
        /// <summary>依模型屬性建立唯讀靜態顯示欄位，並指定格式/HTML 屬性。</summary>
        /// <param name="htmlHelper">型別化 HTML Helper。</param>
        /// <param name="expression">模型屬性運算式。</param>
        /// <param name="format">格式字串。</param>
        /// <param name="htmlAttributes">HTML 屬性。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent BsStaticControlFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string format, IDictionary<string, object> htmlAttributes)
        {
            var srcTag = htmlHelper.TextBoxFor(expression, format, null) as TagBuilder;
            string value = srcTag.Attributes["value"];

            var input = htmlHelper.HiddenFor(expression) as TagBuilder;
            input.Attributes["value"] = value;

            var span = new TagBuilder("span");
            span.InnerHtml.Append(value);
            span.Attributes["class"] = "value-text";

            var div = new TagBuilder("div");
            div.InnerHtml.AppendHtml(input);
            div.InnerHtml.AppendHtml(span);
            div.MergeAttributes(htmlAttributes);
            div.AddCssClass("form-control-static");

            return div;

        }







        /*==================================================== */

        /// <summary>建立 Bootstrap 文字輸入欄位。</summary>
        /// <param name="htmlHelper">HTML Helper。</param>
        /// <param name="name">欄位名稱。</param>
        /// <param name="value">欄位值。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent BsTextBox(this IHtmlHelper htmlHelper, string name, object value = null)
        {
            return BsTextBox(htmlHelper, name, value, null);
        }
        /// <summary>建立 Bootstrap 文字輸入欄位。</summary>
        /// <param name="htmlHelper">HTML Helper。</param>
        /// <param name="name">欄位名稱。</param>
        /// <param name="value">欄位值。</param>
        /// <param name="htmlAttributes">HTML 屬性。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent BsTextBox(this IHtmlHelper htmlHelper, string name, object value, object htmlAttributes)
        {
            return BsTextBox(htmlHelper, name, value, null, htmlAttributes);
        }
        /// <summary>建立 Bootstrap 文字輸入欄位，並指定格式/HTML 屬性。</summary>
        /// <param name="htmlHelper">HTML Helper。</param>
        /// <param name="name">欄位名稱。</param>
        /// <param name="value">欄位值。</param>
        /// <param name="format">格式字串。</param>
        /// <param name="htmlAttributes">HTML 屬性。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent BsTextBox(this IHtmlHelper htmlHelper, string name, object value, string format, object htmlAttributes = null)
        {
            return BsTextBox(htmlHelper, name, value, format, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }
        /// <summary>建立 Bootstrap 文字輸入欄位。</summary>
        /// <param name="htmlHelper">HTML Helper。</param>
        /// <param name="name">欄位名稱。</param>
        /// <param name="value">欄位值。</param>
        /// <param name="htmlAttributes">HTML 屬性。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent BsTextBox(this IHtmlHelper htmlHelper, string name, object value, IDictionary<string, object> htmlAttributes)
        {
            return BsTextBox(htmlHelper, name, value, null, htmlAttributes);
        }
        /// <summary>建立 Bootstrap 文字輸入欄位，並指定格式/HTML 屬性。</summary>
        /// <param name="htmlHelper">HTML Helper。</param>
        /// <param name="name">欄位名稱。</param>
        /// <param name="value">欄位值。</param>
        /// <param name="format">格式字串。</param>
        /// <param name="htmlAttributes">HTML 屬性。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent BsTextBox(this IHtmlHelper htmlHelper, string name, object value, string format, IDictionary<string, object> htmlAttributes)
        {
            if (htmlAttributes == null) { htmlAttributes = new Dictionary<string, object>(); }

            bool isEdit = HelperUtils.IsEditable(htmlAttributes);
            HelperUtils.ClearBoolAttribute(htmlAttributes);
            HelperUtils.StandardAttribute(htmlAttributes);

            if (!isEdit)
            {
                return BsStaticControl(htmlHelper, name, value, format, htmlAttributes);
            }

            HelperUtils.AddCssClass(htmlAttributes, "form-control");
            return htmlHelper.TextBox(name, value, format, htmlAttributes);
        }


        /*==================================================== */

        /// <summary>依模型屬性建立 Bootstrap 文字輸入欄位。</summary>
        /// <param name="htmlHelper">型別化 HTML Helper。</param>
        /// <param name="expression">模型屬性運算式。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent BsTextBoxFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            return BsTextBoxFor(htmlHelper, expression, null);
        }
        /// <summary>依模型屬性建立 Bootstrap 文字輸入欄位。</summary>
        /// <param name="htmlHelper">型別化 HTML Helper。</param>
        /// <param name="expression">模型屬性運算式。</param>
        /// <param name="htmlAttributes">HTML 屬性。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent BsTextBoxFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            return BsTextBoxFor(htmlHelper, expression, null, htmlAttributes);
        }
        /// <summary>依模型屬性建立 Bootstrap 文字輸入欄位，並指定格式/HTML 屬性。</summary>
        /// <param name="htmlHelper">型別化 HTML Helper。</param>
        /// <param name="expression">模型屬性運算式。</param>
        /// <param name="format">格式字串。</param>
        /// <param name="htmlAttributes">HTML 屬性。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent BsTextBoxFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string format, object htmlAttributes = null)
        {
            return BsTextBoxFor(htmlHelper, expression, format, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }
        /// <summary>依模型屬性建立 Bootstrap 文字輸入欄位。</summary>
        /// <param name="htmlHelper">型別化 HTML Helper。</param>
        /// <param name="expression">模型屬性運算式。</param>
        /// <param name="htmlAttributes">HTML 屬性。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent BsTextBoxFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes)
        {
            return BsTextBoxFor(htmlHelper, expression, null, htmlAttributes);
        }
        /// <summary>依模型屬性建立 Bootstrap 文字輸入欄位，並指定格式/HTML 屬性。</summary>
        /// <param name="htmlHelper">型別化 HTML Helper。</param>
        /// <param name="expression">模型屬性運算式。</param>
        /// <param name="format">格式字串。</param>
        /// <param name="htmlAttributes">HTML 屬性。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent BsTextBoxFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string format, IDictionary<string, object> htmlAttributes)
        {
            if (htmlAttributes == null) { htmlAttributes = new Dictionary<string, object>(); }

            bool isEdit = HelperUtils.IsEditable(htmlAttributes);
            HelperUtils.ClearBoolAttribute(htmlAttributes);
            HelperUtils.StandardAttribute(htmlAttributes);

            if (!isEdit)
            {
                return BsStaticControlFor(htmlHelper, expression, format, htmlAttributes);
            }

            HelperUtils.AddCssClass(htmlAttributes, "form-control");
            return htmlHelper.TextBoxFor(expression, format, htmlAttributes);
        }


    }
}
