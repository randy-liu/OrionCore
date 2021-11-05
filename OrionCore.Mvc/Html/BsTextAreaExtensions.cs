using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace OrionCore.Mvc.Html
{

	/// <summary></summary>
	public static class BsTextAreaExtensions
	{


		/// <summary></summary>
		public static IHtmlContent BsTextArea(this IHtmlHelper htmlHelper, string name)
		{
			return BsTextArea(htmlHelper, name, null, null);
		}
		/// <summary></summary>
		public static IHtmlContent BsTextArea(this IHtmlHelper htmlHelper, string name, object htmlAttributes)
		{
			return BsTextArea(htmlHelper, name, null, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}
		/// <summary></summary>
		public static IHtmlContent BsTextArea(this IHtmlHelper htmlHelper, string name, IDictionary<string, object> htmlAttributes)
		{
			return BsTextArea(htmlHelper, name, null, htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent BsTextArea(this IHtmlHelper htmlHelper, string name, string value)
		{
			return BsTextArea(htmlHelper, name, value, null);
		}
		/// <summary></summary>
		public static IHtmlContent BsTextArea(this IHtmlHelper htmlHelper, string name, string value, object htmlAttributes)
		{
			return BsTextArea(htmlHelper, name, value, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}
		/// <summary></summary>
		public static IHtmlContent BsTextArea(this IHtmlHelper htmlHelper, string name, string value, IDictionary<string, object> htmlAttributes)
		{
			return BsTextArea(htmlHelper, name, value, 2, 20, htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent BsTextArea(this IHtmlHelper htmlHelper, string name, string value, int rows, int columns, object htmlAttributes)
		{
			return BsTextArea(htmlHelper, name, value, rows, columns, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}
		/// <summary></summary>
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

		/// <summary></summary>
		public static IHtmlContent BsTextAreaFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
		{
			return BsTextAreaFor(htmlHelper, expression, null);
		}
		/// <summary></summary>
		public static IHtmlContent BsTextAreaFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
		{
			return BsTextAreaFor(htmlHelper, expression, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}
		/// <summary></summary>
		public static IHtmlContent BsTextAreaFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes)
		{
			return BsTextAreaFor(htmlHelper, expression, 2, 20, htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent BsTextAreaFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, int rows, int columns, object htmlAttributes)
		{
			return BsTextAreaFor(htmlHelper, expression, rows, columns, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}
		/// <summary></summary>
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

