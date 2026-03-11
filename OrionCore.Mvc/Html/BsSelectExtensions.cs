using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Orion.Api;

namespace Orion.Mvc.Html
{
	/// <summary>提供 Bootstrap Select/DropDownList 的 HtmlHelper 擴充方法。</summary>
	public static class BsSelectExtensions
	{


		/// <summary>建立 Bootstrap 下拉選單。</summary>
		/// <param name="htmlHelper">HTML Helper。</param>
		/// <param name="name">欄位名稱。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent BsDropDownList(this IHtmlHelper htmlHelper, string name)
		{
			return BsDropDownList(htmlHelper, name, null, null, null);
		}
		/// <summary>建立 Bootstrap 下拉選單。</summary>
		/// <param name="htmlHelper">HTML Helper。</param>
		/// <param name="name">欄位名稱。</param>
		/// <param name="optionLabel">預設提示文字。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent BsDropDownList(this IHtmlHelper htmlHelper, string name, string optionLabel)
		{
			return BsDropDownList(htmlHelper, name, null, optionLabel, null);
		}
		/// <summary>建立 Bootstrap 下拉選單。</summary>
		/// <param name="htmlHelper">HTML Helper。</param>
		/// <param name="name">欄位名稱。</param>
		/// <param name="selectList">下拉選項來源。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent BsDropDownList(this IHtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList)
		{
			return BsDropDownList(htmlHelper, name, selectList, null, null);
		}
		/// <summary>建立 Bootstrap 下拉選單。</summary>
		/// <param name="htmlHelper">HTML Helper。</param>
		/// <param name="name">欄位名稱。</param>
		/// <param name="selectList">下拉選項來源。</param>
		/// <param name="htmlAttributes">HTML 屬性。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent BsDropDownList(this IHtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList, object htmlAttributes)
		{
			return BsDropDownList(htmlHelper, name, selectList, null, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}
		/// <summary>建立 Bootstrap 下拉選單。</summary>
		/// <param name="htmlHelper">HTML Helper。</param>
		/// <param name="name">欄位名稱。</param>
		/// <param name="selectList">下拉選項來源。</param>
		/// <param name="htmlAttributes">HTML 屬性。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent BsDropDownList(this IHtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList, IDictionary<string, object> htmlAttributes)
		{
			return BsDropDownList(htmlHelper, name, selectList, null, htmlAttributes);
		}
		/// <summary>建立 Bootstrap 下拉選單。</summary>
		/// <param name="htmlHelper">HTML Helper。</param>
		/// <param name="name">欄位名稱。</param>
		/// <param name="selectList">下拉選項來源。</param>
		/// <param name="optionLabel">預設提示文字。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent BsDropDownList(this IHtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList, string optionLabel)
		{
			return BsDropDownList(htmlHelper, name, selectList, optionLabel, null);
		}
		/// <summary>建立 Bootstrap 下拉選單。</summary>
		/// <param name="htmlHelper">HTML Helper。</param>
		/// <param name="name">欄位名稱。</param>
		/// <param name="selectList">下拉選項來源。</param>
		/// <param name="optionLabel">預設提示文字。</param>
		/// <param name="htmlAttributes">HTML 屬性。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent BsDropDownList(this IHtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList, string optionLabel, object htmlAttributes)
		{
			return BsDropDownList(htmlHelper, name, selectList, optionLabel, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}
		/// <summary>建立 Bootstrap 下拉選單。</summary>
		/// <param name="htmlHelper">HTML Helper。</param>
		/// <param name="name">欄位名稱。</param>
		/// <param name="selectList">下拉選項來源。</param>
		/// <param name="optionLabel">預設提示文字。</param>
		/// <param name="htmlAttributes">HTML 屬性。</param>
		/// <returns>產生的 HTML 內容。</returns>
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

		/// <summary>依模型屬性建立 Bootstrap 下拉選單。</summary>
		/// <param name="htmlHelper">型別化 HTML Helper。</param>
		/// <param name="expression">模型屬性運算式。</param>
		/// <param name="selectList">下拉選項來源。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent BsDropDownListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList)
		{
			return BsDropDownListFor(htmlHelper, expression, selectList, null, null);
		}
		/// <summary>依模型屬性建立 Bootstrap 下拉選單。</summary>
		/// <param name="htmlHelper">型別化 HTML Helper。</param>
		/// <param name="expression">模型屬性運算式。</param>
		/// <param name="selectList">下拉選項來源。</param>
		/// <param name="htmlAttributes">HTML 屬性。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent BsDropDownListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, object htmlAttributes)
		{
			return BsDropDownListFor(htmlHelper, expression, selectList, null, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}
		/// <summary>依模型屬性建立 Bootstrap 下拉選單。</summary>
		/// <param name="htmlHelper">型別化 HTML Helper。</param>
		/// <param name="expression">模型屬性運算式。</param>
		/// <param name="selectList">下拉選項來源。</param>
		/// <param name="htmlAttributes">HTML 屬性。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent BsDropDownListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, IDictionary<string, object> htmlAttributes)
		{
			return BsDropDownListFor(htmlHelper, expression, selectList, null, htmlAttributes);
		}
		/// <summary>依模型屬性建立 Bootstrap 下拉選單。</summary>
		/// <param name="htmlHelper">型別化 HTML Helper。</param>
		/// <param name="expression">模型屬性運算式。</param>
		/// <param name="selectList">下拉選項來源。</param>
		/// <param name="optionLabel">預設提示文字。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent BsDropDownListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, string optionLabel)
		{
			return BsDropDownListFor(htmlHelper, expression, selectList, optionLabel, null);
		}
		/// <summary>依模型屬性建立 Bootstrap 下拉選單。</summary>
		/// <param name="htmlHelper">型別化 HTML Helper。</param>
		/// <param name="expression">模型屬性運算式。</param>
		/// <param name="selectList">下拉選項來源。</param>
		/// <param name="optionLabel">預設提示文字。</param>
		/// <param name="htmlAttributes">HTML 屬性。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent BsDropDownListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, string optionLabel, object htmlAttributes)
		{
			return BsDropDownListFor(htmlHelper, expression, selectList, optionLabel, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}
		/// <summary>依模型屬性建立 Bootstrap 下拉選單。</summary>
		/// <param name="htmlHelper">型別化 HTML Helper。</param>
		/// <param name="expression">模型屬性運算式。</param>
		/// <param name="selectList">下拉選項來源。</param>
		/// <param name="optionLabel">預設提示文字。</param>
		/// <param name="htmlAttributes">HTML 屬性。</param>
		/// <returns>產生的 HTML 內容。</returns>
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

		/// <summary>依列舉型別建立 Bootstrap 下拉選單。</summary>
		/// <param name="htmlHelper">型別化 HTML Helper。</param>
		/// <param name="expression">模型屬性運算式。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent BsEnumDropDownListFor<TModel, TEnum>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression)
		{
			return BsEnumDropDownListFor(htmlHelper, expression, null, null);
		}
		/// <summary>依列舉型別建立 Bootstrap 下拉選單。</summary>
		/// <param name="htmlHelper">型別化 HTML Helper。</param>
		/// <param name="expression">模型屬性運算式。</param>
		/// <param name="htmlAttributes">HTML 屬性。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent BsEnumDropDownListFor<TModel, TEnum>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, object htmlAttributes)
		{
			return BsEnumDropDownListFor(htmlHelper, expression, null, htmlAttributes);
		}
		/// <summary>依列舉型別建立 Bootstrap 下拉選單。</summary>
		/// <param name="htmlHelper">型別化 HTML Helper。</param>
		/// <param name="expression">模型屬性運算式。</param>
		/// <param name="htmlAttributes">HTML 屬性。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent BsEnumDropDownListFor<TModel, TEnum>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, IDictionary<string, object> htmlAttributes)
		{
			return BsEnumDropDownListFor(htmlHelper, expression, null, htmlAttributes);
		}
		/// <summary>依列舉型別建立 Bootstrap 下拉選單。</summary>
		/// <param name="htmlHelper">型別化 HTML Helper。</param>
		/// <param name="expression">模型屬性運算式。</param>
		/// <param name="optionLabel">預設提示文字。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent BsEnumDropDownListFor<TModel, TEnum>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, string optionLabel)
		{
			return BsEnumDropDownListFor(htmlHelper, expression, optionLabel, null);
		}
		/// <summary>依列舉型別建立 Bootstrap 下拉選單。</summary>
		/// <param name="htmlHelper">型別化 HTML Helper。</param>
		/// <param name="expression">模型屬性運算式。</param>
		/// <param name="optionLabel">預設提示文字。</param>
		/// <param name="htmlAttributes">HTML 屬性。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent BsEnumDropDownListFor<TModel, TEnum>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, string optionLabel, object htmlAttributes)
		{
			return BsEnumDropDownListFor(htmlHelper, expression, optionLabel, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}

		/// <summary>依列舉型別建立 Bootstrap 下拉選單。</summary>
		/// <param name="htmlHelper">型別化 HTML Helper。</param>
		/// <param name="expression">模型屬性運算式。</param>
		/// <param name="optionLabel">預設提示文字。</param>
		/// <param name="htmlAttributes">HTML 屬性。</param>
		/// <returns>產生的 HTML 內容。</returns>
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
