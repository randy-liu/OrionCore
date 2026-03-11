using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Orion.Mvc.Html
{

	/// <summary>提供 Bootstrap 文字區塊 (`textarea`) 的 HtmlHelper 擴充方法。</summary>
	public static class BsTextAreaExtensions
	{


		/// <summary>建立 Bootstrap 文字區塊欄位。</summary>
		/// <param name="htmlHelper">HTML Helper。</param>
		/// <param name="name">欄位名稱。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent BsTextArea(this IHtmlHelper htmlHelper, string name)
		{
			return BsTextArea(htmlHelper, name, null, null);
		}
		/// <summary>建立 Bootstrap 文字區塊欄位，使用匿名 HTML 屬性。</summary>
		/// <param name="htmlHelper">HTML Helper。</param>
		/// <param name="name">欄位名稱。</param>
		/// <param name="htmlAttributes">HTML 屬性。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent BsTextArea(this IHtmlHelper htmlHelper, string name, object htmlAttributes)
		{
			return BsTextArea(htmlHelper, name, null, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}
		/// <summary>建立 Bootstrap 文字區塊欄位，使用 HTML 屬性集合。</summary>
		/// <param name="htmlHelper">HTML Helper。</param>
		/// <param name="name">欄位名稱。</param>
		/// <param name="htmlAttributes">HTML 屬性。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent BsTextArea(this IHtmlHelper htmlHelper, string name, IDictionary<string, object> htmlAttributes)
		{
			return BsTextArea(htmlHelper, name, null, htmlAttributes);
		}
		/// <summary>建立 Bootstrap 文字區塊欄位並指定值。</summary>
		/// <param name="htmlHelper">HTML Helper。</param>
		/// <param name="name">欄位名稱。</param>
		/// <param name="value">欄位值。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent BsTextArea(this IHtmlHelper htmlHelper, string name, string value)
		{
			return BsTextArea(htmlHelper, name, value, null);
		}
		/// <summary>建立 Bootstrap 文字區塊欄位並指定值與匿名 HTML 屬性。</summary>
		/// <param name="htmlHelper">HTML Helper。</param>
		/// <param name="name">欄位名稱。</param>
		/// <param name="value">欄位值。</param>
		/// <param name="htmlAttributes">HTML 屬性。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent BsTextArea(this IHtmlHelper htmlHelper, string name, string value, object htmlAttributes)
		{
			return BsTextArea(htmlHelper, name, value, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}
		/// <summary>建立 Bootstrap 文字區塊欄位並指定值與 HTML 屬性集合。</summary>
		/// <param name="htmlHelper">HTML Helper。</param>
		/// <param name="name">欄位名稱。</param>
		/// <param name="value">欄位值。</param>
		/// <param name="htmlAttributes">HTML 屬性。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent BsTextArea(this IHtmlHelper htmlHelper, string name, string value, IDictionary<string, object> htmlAttributes)
		{
			return BsTextArea(htmlHelper, name, value, 2, 20, htmlAttributes);
		}
		/// <summary>建立 Bootstrap 文字區塊欄位並指定列欄數與匿名 HTML 屬性。</summary>
		/// <param name="htmlHelper">HTML Helper。</param>
		/// <param name="name">欄位名稱。</param>
		/// <param name="value">欄位值。</param>
		/// <param name="rows">列數。</param>
		/// <param name="columns">欄數。</param>
		/// <param name="htmlAttributes">HTML 屬性。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent BsTextArea(this IHtmlHelper htmlHelper, string name, string value, int rows, int columns, object htmlAttributes)
		{
			return BsTextArea(htmlHelper, name, value, rows, columns, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}
		/// <summary>建立 Bootstrap 文字區塊欄位並指定列欄數與 HTML 屬性集合。</summary>
		/// <param name="htmlHelper">HTML Helper。</param>
		/// <param name="name">欄位名稱。</param>
		/// <param name="value">欄位值。</param>
		/// <param name="rows">列數。</param>
		/// <param name="columns">欄數。</param>
		/// <param name="htmlAttributes">HTML 屬性。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent BsTextArea(this IHtmlHelper htmlHelper, string name, string value, int rows, int columns, IDictionary<string, object> htmlAttributes)
		{
			if (htmlAttributes == null) { htmlAttributes = new Dictionary<string, object>(); }

			bool isEdit = HelperUtils.IsEditable(htmlAttributes);
			HelperUtils.ClearBoolAttribute(htmlAttributes);
			HelperUtils.StandardAttribute(htmlAttributes);

			if (isEdit)
			{
				HelperUtils.AddCssClass(htmlAttributes, "form-control");
				return htmlHelper.TextArea(name, value, rows, columns, htmlAttributes);
			}


			var input = htmlHelper.Hidden(name, value);

			var pre = new TagBuilder("pre");
			pre.InnerHtml.Append(value);
			pre.MergeAttributes(htmlAttributes);

			var cb = new HtmlContentBuilder();
			cb.AppendHtml(input);
			cb.AppendHtml(pre);

			return cb;
		}


		/*==================================================== */

		/// <summary>依模型屬性建立 Bootstrap 文字區塊欄位。</summary>
		/// <param name="htmlHelper">型別化 HTML Helper。</param>
		/// <param name="expression">模型屬性運算式。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent BsTextAreaFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
		{
			return BsTextAreaFor(htmlHelper, expression, null);
		}
		/// <summary>依模型屬性建立 Bootstrap 文字區塊欄位，使用匿名 HTML 屬性。</summary>
		/// <param name="htmlHelper">型別化 HTML Helper。</param>
		/// <param name="expression">模型屬性運算式。</param>
		/// <param name="htmlAttributes">HTML 屬性。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent BsTextAreaFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
		{
			return BsTextAreaFor(htmlHelper, expression, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}
		/// <summary>依模型屬性建立 Bootstrap 文字區塊欄位，使用 HTML 屬性集合。</summary>
		/// <param name="htmlHelper">型別化 HTML Helper。</param>
		/// <param name="expression">模型屬性運算式。</param>
		/// <param name="htmlAttributes">HTML 屬性。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent BsTextAreaFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes)
		{
			return BsTextAreaFor(htmlHelper, expression, 2, 20, htmlAttributes);
		}
		/// <summary>依模型屬性建立 Bootstrap 文字區塊欄位並指定列欄數與匿名 HTML 屬性。</summary>
		/// <param name="htmlHelper">型別化 HTML Helper。</param>
		/// <param name="expression">模型屬性運算式。</param>
		/// <param name="rows">列數。</param>
		/// <param name="columns">欄數。</param>
		/// <param name="htmlAttributes">HTML 屬性。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent BsTextAreaFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, int rows, int columns, object htmlAttributes)
		{
			return BsTextAreaFor(htmlHelper, expression, rows, columns, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}
		/// <summary>依模型屬性建立 Bootstrap 文字區塊欄位並指定列欄數與 HTML 屬性集合。</summary>
		/// <param name="htmlHelper">型別化 HTML Helper。</param>
		/// <param name="expression">模型屬性運算式。</param>
		/// <param name="rows">列數。</param>
		/// <param name="columns">欄數。</param>
		/// <param name="htmlAttributes">HTML 屬性。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent BsTextAreaFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, int rows, int columns, IDictionary<string, object> htmlAttributes)
		{
			if (htmlAttributes == null) { htmlAttributes = new Dictionary<string, object>(); }

			bool isEdit = HelperUtils.IsEditable(htmlAttributes);
			HelperUtils.ClearBoolAttribute(htmlAttributes);
			HelperUtils.StandardAttribute(htmlAttributes);

			if (isEdit)
			{
				HelperUtils.AddCssClass(htmlAttributes, "form-control");
				return htmlHelper.TextAreaFor(expression, rows, columns, htmlAttributes);
			}

			var input = htmlHelper.HiddenFor(expression) as TagBuilder;

			var pre = new TagBuilder("pre");
			pre.InnerHtml.Append(input.Attributes["value"]);
			pre.MergeAttributes(htmlAttributes);

			var cb = new HtmlContentBuilder();
			cb.AppendHtml(input);
			cb.AppendHtml(pre);

			return cb;
		}

		 

	}
}

