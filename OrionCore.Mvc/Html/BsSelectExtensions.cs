using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using OrionCore.API;

namespace OrionCore.Mvc.Html
{
	/// <summary></summary>
	public static class BsSelectExtensions
	{


		/// <summary></summary>
		public static IHtmlContent BsDropDownList(this IHtmlHelper htmlHelper, string name)
		{
			return BsDropDownList(htmlHelper, name, null, null, null);
		}
		/// <summary></summary>
		public static IHtmlContent BsDropDownList(this IHtmlHelper htmlHelper, string name, string optionLabel)
		{
			return BsDropDownList(htmlHelper, name, null, optionLabel, null);
		}
		/// <summary></summary>
		public static IHtmlContent BsDropDownList(this IHtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList)
		{
			return BsDropDownList(htmlHelper, name, selectList, null, null);
		}
		/// <summary></summary>
		public static IHtmlContent BsDropDownList(this IHtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList, object htmlAttributes)
		{
			return BsDropDownList(htmlHelper, name, selectList, null, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}
		/// <summary></summary>
		public static IHtmlContent BsDropDownList(this IHtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList, IDictionary<string, object> htmlAttributes)
		{
			return BsDropDownList(htmlHelper, name, selectList, null, htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent BsDropDownList(this IHtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList, string optionLabel)
		{
			return BsDropDownList(htmlHelper, name, selectList, optionLabel, null);
		}
		/// <summary></summary>
		public static IHtmlContent BsDropDownList(this IHtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList, string optionLabel, object htmlAttributes)
		{
			return BsDropDownList(htmlHelper, name, selectList, optionLabel, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}
		/// <summary></summary>
		public static IHtmlContent BsDropDownList(this IHtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList, string optionLabel, IDictionary<string, object> htmlAttributes)
		{
			if (htmlAttributes == null) { htmlAttributes = new Dictionary<string, object>(); }

			bool isEdit = HelperUtils.IsEditable(htmlAttributes);
			HelperUtils.ClearBoolAttribute(htmlAttributes);
			HelperUtils.StandardAttribute(htmlAttributes);

			if (isEdit)
			{ 
				HelperUtils.AddCssClass(htmlAttributes, "form-control");
				return htmlHelper.DropDownList(name, selectList, optionLabel, htmlAttributes);
			}

			var srcTag = htmlHelper.DropDownList(name, selectList, optionLabel, htmlAttributes) as TagBuilder;
			return staticControlHelper(htmlHelper, srcTag, htmlAttributes);
		}


		/*==================================================== */

		/// <summary></summary>
		public static IHtmlContent BsDropDownListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList)
		{
			return BsDropDownListFor(htmlHelper, expression, selectList, null, null);
		}
		/// <summary></summary>
		public static IHtmlContent BsDropDownListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, object htmlAttributes)
		{
			return BsDropDownListFor(htmlHelper, expression, selectList, null, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}
		/// <summary></summary>
		public static IHtmlContent BsDropDownListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, IDictionary<string, object> htmlAttributes)
		{
			return BsDropDownListFor(htmlHelper, expression, selectList, null, htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent BsDropDownListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, string optionLabel)
		{
			return BsDropDownListFor(htmlHelper, expression, selectList, optionLabel, null);
		}
		/// <summary></summary>
		public static IHtmlContent BsDropDownListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, string optionLabel, object htmlAttributes)
		{
			return BsDropDownListFor(htmlHelper, expression, selectList, optionLabel, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}
		/// <summary></summary>
		public static IHtmlContent BsDropDownListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, string optionLabel, IDictionary<string, object> htmlAttributes)
		{
			if (htmlAttributes == null) { htmlAttributes = new Dictionary<string, object>(); }

			bool isEdit = HelperUtils.IsEditable(htmlAttributes);
			HelperUtils.ClearBoolAttribute(htmlAttributes);
			HelperUtils.StandardAttribute(htmlAttributes);

			if (isEdit)
			{ 
				HelperUtils.AddCssClass(htmlAttributes, "form-control");
				return htmlHelper.DropDownListFor(expression, selectList, optionLabel, htmlAttributes);
			}

			var srcTag = htmlHelper.DropDownListFor(expression, selectList, optionLabel, htmlAttributes) as TagBuilder;
			return staticControlHelper(htmlHelper, srcTag, htmlAttributes); 
		}




		/*==================================================== */

		/// <summary></summary>
		public static IHtmlContent BsEnumDropDownListFor<TModel, TEnum>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression)
		{
			return BsEnumDropDownListFor(htmlHelper, expression, null, null);
		}
		/// <summary></summary>
		public static IHtmlContent BsEnumDropDownListFor<TModel, TEnum>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, object htmlAttributes)
		{
			return BsEnumDropDownListFor(htmlHelper, expression, null, htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent BsEnumDropDownListFor<TModel, TEnum>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, IDictionary<string, object> htmlAttributes)
		{
			return BsEnumDropDownListFor(htmlHelper, expression, null, htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent BsEnumDropDownListFor<TModel, TEnum>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, string optionLabel)
		{
			return BsEnumDropDownListFor(htmlHelper, expression, optionLabel, null);
		}
		/// <summary></summary>
		public static IHtmlContent BsEnumDropDownListFor<TModel, TEnum>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, string optionLabel, object htmlAttributes)
		{
			return BsEnumDropDownListFor(htmlHelper, expression, optionLabel, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}

		/// <summary></summary>
		public static IHtmlContent BsEnumDropDownListFor<TModel, TEnum>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, string optionLabel, IDictionary<string, object> htmlAttributes)
		{
			IDictionary<string, string> selectList = OrionUtils.EnumToDictionary<TEnum>();
			return BsDropDownListFor(htmlHelper, expression, HelperUtils.ToSelectListItem(selectList), optionLabel, htmlAttributes);
		}



		/*==================================================== */

			 

		private static IHtmlContent staticControlHelper(IHtmlHelper htmlHelper, TagBuilder srcTag, IDictionary<string, object> htmlAttributes)
		{
			string fullName = srcTag.Attributes["name"];
			var list = HelperUtils.ParseSelectList(srcTag);

			foreach (SelectListItem item in list)
			{
				if (!item.Selected) { continue; }
				return htmlHelper.BsStaticControl(fullName, item.Value, item.Text, htmlAttributes);
			}

			return htmlHelper.BsStaticControl(fullName, null, null, htmlAttributes);
		}
		


	}
}
