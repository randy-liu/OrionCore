using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Orion.Mvc.Html
{

	/// <summary>提供 Dictionary/Enumerable 來源的 Bootstrap 下拉選單擴充方法。</summary>
	public static class BsSelectDictionaryExtensions
	{

		/// <summary>建立 Bootstrap 下拉選單（由清單來源）。</summary>
		/// <param name="htmlHelper">HTML Helper。</param>
		/// <param name="name">欄位名稱。</param>
		/// <param name="selectList">選項來源。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent BsDropDownList(this IHtmlHelper htmlHelper, string name, IEnumerable<int> selectList)
		{
			return BsDropDownList(htmlHelper, name, selectList, null, null);
		}
		/// <summary>建立 Bootstrap 下拉選單（由清單來源）。</summary>
		/// <param name="htmlHelper">HTML Helper。</param>
		/// <param name="name">欄位名稱。</param>
		/// <param name="selectList">選項來源。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent BsDropDownList(this IHtmlHelper htmlHelper, string name, IEnumerable<string> selectList)
		{
			return BsDropDownList(htmlHelper, name, selectList, null, null);
		}
		/// <summary>建立 Bootstrap 下拉選單（由清單來源）。</summary>
		/// <param name="htmlHelper">HTML Helper。</param>
		/// <param name="name">欄位名稱。</param>
		/// <param name="selectList">選項來源。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent BsDropDownList<K, V>(this IHtmlHelper htmlHelper, string name, IDictionary<K, V> selectList)
		{
			return BsDropDownList(htmlHelper, name, selectList, null, null);
		}
		/// <summary>建立 Bootstrap 下拉選單（由清單來源）。</summary>
		/// <param name="htmlHelper">HTML Helper。</param>
		/// <param name="name">欄位名稱。</param>
		/// <param name="selectList">選項來源。</param>
		/// <param name="htmlAttributes">HTML 屬性。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent BsDropDownList(this IHtmlHelper htmlHelper, string name, IEnumerable<int> selectList, object htmlAttributes)
		{
			return BsDropDownList(htmlHelper, name, selectList, null, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}
		/// <summary>建立 Bootstrap 下拉選單（由清單來源）。</summary>
		/// <param name="htmlHelper">HTML Helper。</param>
		/// <param name="name">欄位名稱。</param>
		/// <param name="selectList">選項來源。</param>
		/// <param name="htmlAttributes">HTML 屬性。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent BsDropDownList(this IHtmlHelper htmlHelper, string name, IEnumerable<string> selectList, object htmlAttributes)
		{
			return BsDropDownList(htmlHelper, name, selectList, null, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}
		/// <summary>建立 Bootstrap 下拉選單（由清單來源）。</summary>
		/// <param name="htmlHelper">HTML Helper。</param>
		/// <param name="name">欄位名稱。</param>
		/// <param name="selectList">選項來源。</param>
		/// <param name="htmlAttributes">HTML 屬性。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent BsDropDownList<K, V>(this IHtmlHelper htmlHelper, string name, IDictionary<K, V> selectList, object htmlAttributes)
		{
			return BsDropDownList(htmlHelper, name, selectList, null, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}


		/// <summary>建立 Bootstrap 下拉選單（由清單來源）。</summary>
		/// <param name="htmlHelper">HTML Helper。</param>
		/// <param name="name">欄位名稱。</param>
		/// <param name="selectList">選項來源。</param>
		/// <param name="htmlAttributes">HTML 屬性。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent BsDropDownList(this IHtmlHelper htmlHelper, string name, IEnumerable<int> selectList, IDictionary<string, object> htmlAttributes)
		{
			return BsDropDownList(htmlHelper, name, selectList, null, htmlAttributes);
		}
		/// <summary>建立 Bootstrap 下拉選單（由清單來源）。</summary>
		/// <param name="htmlHelper">HTML Helper。</param>
		/// <param name="name">欄位名稱。</param>
		/// <param name="selectList">選項來源。</param>
		/// <param name="htmlAttributes">HTML 屬性。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent BsDropDownList(this IHtmlHelper htmlHelper, string name, IEnumerable<string> selectList, IDictionary<string, object> htmlAttributes)
		{
			return BsDropDownList(htmlHelper, name, selectList, null, htmlAttributes);
		}
		/// <summary>建立 Bootstrap 下拉選單（由清單來源）。</summary>
		/// <param name="htmlHelper">HTML Helper。</param>
		/// <param name="name">欄位名稱。</param>
		/// <param name="selectList">選項來源。</param>
		/// <param name="htmlAttributes">HTML 屬性。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent BsDropDownList<K, V>(this IHtmlHelper htmlHelper, string name, IDictionary<K, V> selectList, IDictionary<string, object> htmlAttributes)
		{
			return BsDropDownList(htmlHelper, name, selectList, null, htmlAttributes);
		}



		/// <summary>建立 Bootstrap 下拉選單（由清單來源）。</summary>
		/// <param name="htmlHelper">HTML Helper。</param>
		/// <param name="name">欄位名稱。</param>
		/// <param name="selectList">選項來源。</param>
		/// <param name="optionLabel">預設提示文字。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent BsDropDownList(this IHtmlHelper htmlHelper, string name, IEnumerable<int> selectList, string optionLabel)
		{
			return BsDropDownList(htmlHelper, name, selectList, optionLabel, null);
		}
		/// <summary>建立 Bootstrap 下拉選單（由清單來源）。</summary>
		/// <param name="htmlHelper">HTML Helper。</param>
		/// <param name="name">欄位名稱。</param>
		/// <param name="selectList">選項來源。</param>
		/// <param name="optionLabel">預設提示文字。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent BsDropDownList(this IHtmlHelper htmlHelper, string name, IEnumerable<string> selectList, string optionLabel)
		{
			return BsDropDownList(htmlHelper, name, selectList, optionLabel, null);
		}
		/// <summary>建立 Bootstrap 下拉選單（由清單來源）。</summary>
		/// <param name="htmlHelper">HTML Helper。</param>
		/// <param name="name">欄位名稱。</param>
		/// <param name="selectList">選項來源。</param>
		/// <param name="optionLabel">預設提示文字。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent BsDropDownList<K, V>(this IHtmlHelper htmlHelper, string name, IDictionary<K, V> selectList, string optionLabel)
		{
			return BsDropDownList(htmlHelper, name, selectList, optionLabel, null);
		}
		/// <summary>建立 Bootstrap 下拉選單（由清單來源）。</summary>
		/// <param name="htmlHelper">HTML Helper。</param>
		/// <param name="name">欄位名稱。</param>
		/// <param name="selectList">選項來源。</param>
		/// <param name="optionLabel">預設提示文字。</param>
		/// <param name="htmlAttributes">HTML 屬性。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent BsDropDownList(this IHtmlHelper htmlHelper, string name, IEnumerable<int> selectList, string optionLabel, object htmlAttributes)
		{
			return BsDropDownList(htmlHelper, name, selectList, optionLabel, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}
		/// <summary>建立 Bootstrap 下拉選單（由清單來源）。</summary>
		/// <param name="htmlHelper">HTML Helper。</param>
		/// <param name="name">欄位名稱。</param>
		/// <param name="selectList">選項來源。</param>
		/// <param name="optionLabel">預設提示文字。</param>
		/// <param name="htmlAttributes">HTML 屬性。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent BsDropDownList(this IHtmlHelper htmlHelper, string name, IEnumerable<string> selectList, string optionLabel, object htmlAttributes)
		{
			return BsDropDownList(htmlHelper, name, selectList, optionLabel, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}
		/// <summary>建立 Bootstrap 下拉選單（由清單來源）。</summary>
		/// <param name="htmlHelper">HTML Helper。</param>
		/// <param name="name">欄位名稱。</param>
		/// <param name="selectList">選項來源。</param>
		/// <param name="optionLabel">預設提示文字。</param>
		/// <param name="htmlAttributes">HTML 屬性。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent BsDropDownList<K, V>(this IHtmlHelper htmlHelper, string name, IDictionary<K, V> selectList, string optionLabel, object htmlAttributes)
		{
			return BsDropDownList(htmlHelper, name, selectList, optionLabel, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}


		/// <summary>建立 Bootstrap 下拉選單（由清單來源）。</summary>
		/// <param name="htmlHelper">HTML Helper。</param>
		/// <param name="name">欄位名稱。</param>
		/// <param name="selectList">選項來源。</param>
		/// <param name="optionLabel">預設提示文字。</param>
		/// <param name="htmlAttributes">HTML 屬性。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent BsDropDownList(this IHtmlHelper htmlHelper, string name, IEnumerable<int> selectList, string optionLabel, IDictionary<string, object> htmlAttributes)
		{
			return htmlHelper.BsDropDownList(name, HelperUtils.ToSelectListItem(selectList), optionLabel, htmlAttributes);
		}

		/// <summary>建立 Bootstrap 下拉選單（由清單來源）。</summary>
		/// <param name="htmlHelper">HTML Helper。</param>
		/// <param name="name">欄位名稱。</param>
		/// <param name="selectList">選項來源。</param>
		/// <param name="optionLabel">預設提示文字。</param>
		/// <param name="htmlAttributes">HTML 屬性。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent BsDropDownList(this IHtmlHelper htmlHelper, string name, IEnumerable<string> selectList, string optionLabel, IDictionary<string, object> htmlAttributes)
		{
			return htmlHelper.BsDropDownList(name, HelperUtils.ToSelectListItem(selectList), optionLabel, htmlAttributes);
		}

		/// <summary>建立 Bootstrap 下拉選單（由清單來源）。</summary>
		/// <param name="htmlHelper">HTML Helper。</param>
		/// <param name="name">欄位名稱。</param>
		/// <param name="selectList">選項來源。</param>
		/// <param name="optionLabel">預設提示文字。</param>
		/// <param name="htmlAttributes">HTML 屬性。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent BsDropDownList<K, V>(this IHtmlHelper htmlHelper, string name, IDictionary<K, V> selectList, string optionLabel, IDictionary<string, object> htmlAttributes)
		{
			return htmlHelper.BsDropDownList(name, HelperUtils.ToSelectListItem(selectList), optionLabel, htmlAttributes);
		}






		/*==================================================== */


		/// <summary>依模型屬性建立 Bootstrap 下拉選單（由清單來源）。</summary>
		/// <param name="htmlHelper">HTML Helper。</param>
		/// <param name="expression">模型屬性運算式。</param>
		/// <param name="selectList">選項來源。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent BsDropDownListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<int> selectList)
		{
			return BsDropDownListFor(htmlHelper, expression, selectList, null, null);
		}
		/// <summary>依模型屬性建立 Bootstrap 下拉選單（由清單來源）。</summary>
		/// <param name="htmlHelper">HTML Helper。</param>
		/// <param name="expression">模型屬性運算式。</param>
		/// <param name="selectList">選項來源。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent BsDropDownListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<string> selectList)
		{
			return BsDropDownListFor(htmlHelper, expression, selectList, null, null);
		}
		/// <summary>依模型屬性建立 Bootstrap 下拉選單（由清單來源）。</summary>
		/// <param name="htmlHelper">HTML Helper。</param>
		/// <param name="expression">模型屬性運算式。</param>
		/// <param name="selectList">選項來源。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent BsDropDownListFor<TModel, TProperty, K, V>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<K, V> selectList)
		{
			return BsDropDownListFor(htmlHelper, expression, selectList, null, null);
		}
		/// <summary>依模型屬性建立 Bootstrap 下拉選單（由清單來源）。</summary>
		/// <param name="htmlHelper">HTML Helper。</param>
		/// <param name="expression">模型屬性運算式。</param>
		/// <param name="selectList">選項來源。</param>
		/// <param name="htmlAttributes">HTML 屬性。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent BsDropDownListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<int> selectList, object htmlAttributes)
		{
			return BsDropDownListFor(htmlHelper, expression, selectList, null, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}
		/// <summary>依模型屬性建立 Bootstrap 下拉選單（由清單來源）。</summary>
		/// <param name="htmlHelper">HTML Helper。</param>
		/// <param name="expression">模型屬性運算式。</param>
		/// <param name="selectList">選項來源。</param>
		/// <param name="htmlAttributes">HTML 屬性。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent BsDropDownListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<string> selectList, object htmlAttributes)
		{
			return BsDropDownListFor(htmlHelper, expression, selectList, null, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}
		/// <summary>依模型屬性建立 Bootstrap 下拉選單（由清單來源）。</summary>
		/// <param name="htmlHelper">HTML Helper。</param>
		/// <param name="expression">模型屬性運算式。</param>
		/// <param name="selectList">選項來源。</param>
		/// <param name="htmlAttributes">HTML 屬性。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent BsDropDownListFor<TModel, TProperty, K, V>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<K, V> selectList, object htmlAttributes)
		{
			return BsDropDownListFor(htmlHelper, expression, selectList, null, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}

		/// <summary>依模型屬性建立 Bootstrap 下拉選單（由清單來源）。</summary>
		/// <param name="htmlHelper">HTML Helper。</param>
		/// <param name="expression">模型屬性運算式。</param>
		/// <param name="selectList">選項來源。</param>
		/// <param name="htmlAttributes">HTML 屬性。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent BsDropDownListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<int> selectList, IDictionary<string, object> htmlAttributes)
		{
			return BsDropDownListFor(htmlHelper, expression, selectList, null, htmlAttributes);
		}
		/// <summary>依模型屬性建立 Bootstrap 下拉選單（由清單來源）。</summary>
		/// <param name="htmlHelper">HTML Helper。</param>
		/// <param name="expression">模型屬性運算式。</param>
		/// <param name="selectList">選項來源。</param>
		/// <param name="htmlAttributes">HTML 屬性。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent BsDropDownListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<string> selectList, IDictionary<string, object> htmlAttributes)
		{
			return BsDropDownListFor(htmlHelper, expression, selectList, null, htmlAttributes);
		}
		/// <summary>依模型屬性建立 Bootstrap 下拉選單（由清單來源）。</summary>
		/// <param name="htmlHelper">HTML Helper。</param>
		/// <param name="expression">模型屬性運算式。</param>
		/// <param name="selectList">選項來源。</param>
		/// <param name="htmlAttributes">HTML 屬性。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent BsDropDownListFor<TModel, TProperty, K, V>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<K, V> selectList, IDictionary<string, object> htmlAttributes)
		{
			return BsDropDownListFor(htmlHelper, expression, selectList, null, htmlAttributes);
		}


		/// <summary>依模型屬性建立 Bootstrap 下拉選單（由清單來源）。</summary>
		/// <param name="htmlHelper">HTML Helper。</param>
		/// <param name="expression">模型屬性運算式。</param>
		/// <param name="selectList">選項來源。</param>
		/// <param name="optionLabel">預設提示文字。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent BsDropDownListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<int> selectList, string optionLabel)
		{
			return BsDropDownListFor(htmlHelper, expression, selectList, optionLabel, null);
		}
		/// <summary>依模型屬性建立 Bootstrap 下拉選單（由清單來源）。</summary>
		/// <param name="htmlHelper">HTML Helper。</param>
		/// <param name="expression">模型屬性運算式。</param>
		/// <param name="selectList">選項來源。</param>
		/// <param name="optionLabel">預設提示文字。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent BsDropDownListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<string> selectList, string optionLabel)
		{
			return BsDropDownListFor(htmlHelper, expression, selectList, optionLabel, null);
		}
		/// <summary>依模型屬性建立 Bootstrap 下拉選單（由清單來源）。</summary>
		/// <param name="htmlHelper">HTML Helper。</param>
		/// <param name="expression">模型屬性運算式。</param>
		/// <param name="selectList">選項來源。</param>
		/// <param name="optionLabel">預設提示文字。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent BsDropDownListFor<TModel, TProperty, K, V>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<K, V> selectList, string optionLabel)
		{
			return BsDropDownListFor(htmlHelper, expression, selectList, optionLabel, null);
		}
		/// <summary>依模型屬性建立 Bootstrap 下拉選單（由清單來源）。</summary>
		/// <param name="htmlHelper">HTML Helper。</param>
		/// <param name="expression">模型屬性運算式。</param>
		/// <param name="selectList">選項來源。</param>
		/// <param name="optionLabel">預設提示文字。</param>
		/// <param name="htmlAttributes">HTML 屬性。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent BsDropDownListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<int> selectList, string optionLabel, object htmlAttributes)
		{
			return BsDropDownListFor(htmlHelper, expression, selectList, optionLabel, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}
		/// <summary>依模型屬性建立 Bootstrap 下拉選單（由清單來源）。</summary>
		/// <param name="htmlHelper">HTML Helper。</param>
		/// <param name="expression">模型屬性運算式。</param>
		/// <param name="selectList">選項來源。</param>
		/// <param name="optionLabel">預設提示文字。</param>
		/// <param name="htmlAttributes">HTML 屬性。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent BsDropDownListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<string> selectList, string optionLabel, object htmlAttributes)
		{
			return BsDropDownListFor(htmlHelper, expression, selectList, optionLabel, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}
		/// <summary>依模型屬性建立 Bootstrap 下拉選單（由清單來源）。</summary>
		/// <param name="htmlHelper">HTML Helper。</param>
		/// <param name="expression">模型屬性運算式。</param>
		/// <param name="selectList">選項來源。</param>
		/// <param name="optionLabel">預設提示文字。</param>
		/// <param name="htmlAttributes">HTML 屬性。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent BsDropDownListFor<TModel, TProperty, K, V>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<K, V> selectList, string optionLabel, object htmlAttributes)
		{
			return BsDropDownListFor(htmlHelper, expression, selectList, optionLabel, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}


		/// <summary>依模型屬性建立 Bootstrap 下拉選單（由清單來源）。</summary>
		/// <param name="htmlHelper">HTML Helper。</param>
		/// <param name="expression">模型屬性運算式。</param>
		/// <param name="selectList">選項來源。</param>
		/// <param name="optionLabel">預設提示文字。</param>
		/// <param name="htmlAttributes">HTML 屬性。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent BsDropDownListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<int> selectList, string optionLabel, IDictionary<string, object> htmlAttributes)
		{
			return htmlHelper.BsDropDownListFor(expression, HelperUtils.ToSelectListItem(selectList), optionLabel, htmlAttributes);
		}
		/// <summary>依模型屬性建立 Bootstrap 下拉選單（由清單來源）。</summary>
		/// <param name="htmlHelper">HTML Helper。</param>
		/// <param name="expression">模型屬性運算式。</param>
		/// <param name="selectList">選項來源。</param>
		/// <param name="optionLabel">預設提示文字。</param>
		/// <param name="htmlAttributes">HTML 屬性。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent BsDropDownListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<string> selectList, string optionLabel, IDictionary<string, object> htmlAttributes)
		{
			return htmlHelper.BsDropDownListFor(expression, HelperUtils.ToSelectListItem(selectList), optionLabel, htmlAttributes);
		}
		/// <summary>依模型屬性建立 Bootstrap 下拉選單（由清單來源）。</summary>
		/// <param name="htmlHelper">HTML Helper。</param>
		/// <param name="expression">模型屬性運算式。</param>
		/// <param name="selectList">選項來源。</param>
		/// <param name="optionLabel">預設提示文字。</param>
		/// <param name="htmlAttributes">HTML 屬性。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent BsDropDownListFor<TModel, TProperty, K, V>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<K, V> selectList, string optionLabel, IDictionary<string, object> htmlAttributes)
		{
			return htmlHelper.BsDropDownListFor(expression, HelperUtils.ToSelectListItem(selectList), optionLabel, htmlAttributes);
		}
		 

	}
}
