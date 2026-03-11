using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Orion.Mvc.Html
{

    /// <summary>提供 Dictionary/Enumerable 來源的 Select/ListBox 擴充方法。</summary>
    public static class SelectDictionaryExtensions
    {

        /// <summary>建立下拉選單（由清單來源）。</summary>
        /// <param name="htmlHelper">HTML Helper。</param>
        /// <param name="name">欄位名稱。</param>
        /// <param name="selectList">選項來源。</param>
        /// <param name="htmlAttributes">HTML 屬性。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent DropDownList(this IHtmlHelper htmlHelper, string name, IEnumerable<int> selectList, IDictionary<string, object> htmlAttributes)
        {
            return DropDownList(htmlHelper, name, selectList, null, htmlAttributes);
        }
        /// <summary>建立下拉選單（由清單來源）。</summary>
        /// <param name="htmlHelper">HTML Helper。</param>
        /// <param name="name">欄位名稱。</param>
        /// <param name="selectList">選項來源。</param>
        /// <param name="htmlAttributes">HTML 屬性。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent DropDownList(this IHtmlHelper htmlHelper, string name, IEnumerable<string> selectList, IDictionary<string, object> htmlAttributes)
        {
            return DropDownList(htmlHelper, name, selectList, null, htmlAttributes);
        }
        /// <summary>建立下拉選單（由清單來源）。</summary>
        /// <param name="htmlHelper">HTML Helper。</param>
        /// <param name="name">欄位名稱。</param>
        /// <param name="selectList">選項來源。</param>
        /// <param name="htmlAttributes">HTML 屬性。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent DropDownList<K, V>(this IHtmlHelper htmlHelper, string name, IDictionary<K, V> selectList, IDictionary<string, object> htmlAttributes)
        {
            return DropDownList(htmlHelper, name, selectList, null, htmlAttributes);
        }


        /// <summary>建立下拉選單（由清單來源）。</summary>
        /// <param name="htmlHelper">HTML Helper。</param>
        /// <param name="name">欄位名稱。</param>
        /// <param name="selectList">選項來源。</param>
        /// <param name="htmlAttributes">HTML 屬性。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent DropDownList(this IHtmlHelper htmlHelper, string name, IEnumerable<int> selectList, object htmlAttributes = null)
        {
            return DropDownList(htmlHelper, name, selectList, null, htmlAttributes);
        }
        /// <summary>建立下拉選單（由清單來源）。</summary>
        /// <param name="htmlHelper">HTML Helper。</param>
        /// <param name="name">欄位名稱。</param>
        /// <param name="selectList">選項來源。</param>
        /// <param name="htmlAttributes">HTML 屬性。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent DropDownList(this IHtmlHelper htmlHelper, string name, IEnumerable<string> selectList, object htmlAttributes = null)
        {
            return DropDownList(htmlHelper, name, selectList, null, htmlAttributes);
        }
        /// <summary>建立下拉選單（由清單來源）。</summary>
        /// <param name="htmlHelper">HTML Helper。</param>
        /// <param name="name">欄位名稱。</param>
        /// <param name="selectList">選項來源。</param>
        /// <param name="htmlAttributes">HTML 屬性。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent DropDownList<K, V>(this IHtmlHelper htmlHelper, string name, IDictionary<K, V> selectList, object htmlAttributes = null)
        {
            return DropDownList(htmlHelper, name, selectList, null, htmlAttributes);
        }


        /// <summary>建立下拉選單（由清單來源）。</summary>
        /// <param name="htmlHelper">HTML Helper。</param>
        /// <param name="name">欄位名稱。</param>
        /// <param name="selectList">選項來源。</param>
        /// <param name="optionLabel">預設提示文字。</param>
        /// <param name="htmlAttributes">HTML 屬性。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent DropDownList(this IHtmlHelper htmlHelper, string name, IEnumerable<int> selectList, string optionLabel, object htmlAttributes = null)
        {
            return DropDownList(htmlHelper, name, selectList, optionLabel, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }
        /// <summary>建立下拉選單（由清單來源）。</summary>
        /// <param name="htmlHelper">HTML Helper。</param>
        /// <param name="name">欄位名稱。</param>
        /// <param name="selectList">選項來源。</param>
        /// <param name="optionLabel">預設提示文字。</param>
        /// <param name="htmlAttributes">HTML 屬性。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent DropDownList(this IHtmlHelper htmlHelper, string name, IEnumerable<string> selectList, string optionLabel, object htmlAttributes = null)
        {
            return DropDownList(htmlHelper, name, selectList, optionLabel, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }
        /// <summary>建立下拉選單（由清單來源）。</summary>
        /// <param name="htmlHelper">HTML Helper。</param>
        /// <param name="name">欄位名稱。</param>
        /// <param name="selectList">選項來源。</param>
        /// <param name="optionLabel">預設提示文字。</param>
        /// <param name="htmlAttributes">HTML 屬性。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent DropDownList<K, V>(this IHtmlHelper htmlHelper, string name, IDictionary<K, V> selectList, string optionLabel, object htmlAttributes = null)
        {
            return DropDownList(htmlHelper, name, selectList, optionLabel, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }


        /// <summary>建立下拉選單（由清單來源）。</summary>
        /// <param name="htmlHelper">HTML Helper。</param>
        /// <param name="name">欄位名稱。</param>
        /// <param name="selectList">選項來源。</param>
        /// <param name="optionLabel">預設提示文字。</param>
        /// <param name="htmlAttributes">HTML 屬性。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent DropDownList(this IHtmlHelper htmlHelper, string name, IEnumerable<int> selectList, string optionLabel, IDictionary<string, object> htmlAttributes)
        {
            return dropDownList(htmlHelper, name, HelperUtils.ToSelectListItem(selectList), optionLabel, htmlAttributes);
        }
        /// <summary>建立下拉選單（由清單來源）。</summary>
        /// <param name="htmlHelper">HTML Helper。</param>
        /// <param name="name">欄位名稱。</param>
        /// <param name="selectList">選項來源。</param>
        /// <param name="optionLabel">預設提示文字。</param>
        /// <param name="htmlAttributes">HTML 屬性。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent DropDownList(this IHtmlHelper htmlHelper, string name, IEnumerable<string> selectList, string optionLabel, IDictionary<string, object> htmlAttributes)
        {
            return dropDownList(htmlHelper, name, HelperUtils.ToSelectListItem(selectList), optionLabel, htmlAttributes);
        }

        /// <summary>建立下拉選單（由清單來源）。</summary>
        /// <param name="htmlHelper">HTML Helper。</param>
        /// <param name="name">欄位名稱。</param>
        /// <param name="selectList">選項來源。</param>
        /// <param name="optionLabel">預設提示文字。</param>
        /// <param name="htmlAttributes">HTML 屬性。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent DropDownList<K, V>(this IHtmlHelper htmlHelper, string name, IDictionary<K, V> selectList, string optionLabel, IDictionary<string, object> htmlAttributes)
        {
            return dropDownList(htmlHelper, name, HelperUtils.ToSelectListItem(selectList), optionLabel, htmlAttributes);
        }

        private static IHtmlContent dropDownList(IHtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList, string optionLabel, IDictionary<string, object> htmlAttributes)
        {
            return htmlHelper.DropDownList(name, selectList, optionLabel, htmlAttributes);
        }







        /// <summary>依模型屬性建立下拉選單（由清單來源）。</summary>
        /// <param name="htmlHelper">HTML Helper。</param>
        /// <param name="expression">模型屬性運算式。</param>
        /// <param name="selectList">選項來源。</param>
        /// <param name="htmlAttributes">HTML 屬性。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent DropDownListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<int> selectList, IDictionary<string, object> htmlAttributes)
        {
            return DropDownListFor(htmlHelper, expression, selectList, null, htmlAttributes);
        }
        /// <summary>依模型屬性建立下拉選單（由清單來源）。</summary>
        /// <param name="htmlHelper">HTML Helper。</param>
        /// <param name="expression">模型屬性運算式。</param>
        /// <param name="selectList">選項來源。</param>
        /// <param name="htmlAttributes">HTML 屬性。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent DropDownListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<string> selectList, IDictionary<string, object> htmlAttributes)
        {
            return DropDownListFor(htmlHelper, expression, selectList, null, htmlAttributes);
        }
        /// <summary>依模型屬性建立下拉選單（由清單來源）。</summary>
        /// <param name="htmlHelper">HTML Helper。</param>
        /// <param name="expression">模型屬性運算式。</param>
        /// <param name="selectList">選項來源。</param>
        /// <param name="htmlAttributes">HTML 屬性。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent DropDownListFor<TModel, TProperty, K, V>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<K, V> selectList, IDictionary<string, object> htmlAttributes)
        {
            return DropDownListFor(htmlHelper, expression, selectList, null, htmlAttributes);
        }

        /// <summary>依模型屬性建立下拉選單（由清單來源）。</summary>
        /// <param name="htmlHelper">HTML Helper。</param>
        /// <param name="expression">模型屬性運算式。</param>
        /// <param name="selectList">選項來源。</param>
        /// <param name="htmlAttributes">HTML 屬性。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent DropDownListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<int> selectList, object htmlAttributes = null)
        {
            return DropDownListFor(htmlHelper, expression, selectList, null, htmlAttributes);
        }
        /// <summary>依模型屬性建立下拉選單（由清單來源）。</summary>
        /// <param name="htmlHelper">HTML Helper。</param>
        /// <param name="expression">模型屬性運算式。</param>
        /// <param name="selectList">選項來源。</param>
        /// <param name="htmlAttributes">HTML 屬性。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent DropDownListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<string> selectList, object htmlAttributes = null)
        {
            return DropDownListFor(htmlHelper, expression, selectList, null, htmlAttributes);
        }
        /// <summary>依模型屬性建立下拉選單（由清單來源）。</summary>
        /// <param name="htmlHelper">HTML Helper。</param>
        /// <param name="expression">模型屬性運算式。</param>
        /// <param name="selectList">選項來源。</param>
        /// <param name="htmlAttributes">HTML 屬性。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent DropDownListFor<TModel, TProperty, K, V>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<K, V> selectList, object htmlAttributes = null)
        {
            return DropDownListFor(htmlHelper, expression, selectList, null, htmlAttributes);
        }


        /// <summary>依模型屬性建立下拉選單（由清單來源）。</summary>
        /// <param name="htmlHelper">HTML Helper。</param>
        /// <param name="expression">模型屬性運算式。</param>
        /// <param name="selectList">選項來源。</param>
        /// <param name="optionLabel">預設提示文字。</param>
        /// <param name="htmlAttributes">HTML 屬性。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent DropDownListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<int> selectList, string optionLabel, object htmlAttributes = null)
        {
            return DropDownListFor(htmlHelper, expression, selectList, optionLabel, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }
        /// <summary>依模型屬性建立下拉選單（由清單來源）。</summary>
        /// <param name="htmlHelper">HTML Helper。</param>
        /// <param name="expression">模型屬性運算式。</param>
        /// <param name="selectList">選項來源。</param>
        /// <param name="optionLabel">預設提示文字。</param>
        /// <param name="htmlAttributes">HTML 屬性。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent DropDownListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<string> selectList, string optionLabel, object htmlAttributes = null)
        {
            return DropDownListFor(htmlHelper, expression, selectList, optionLabel, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }
        /// <summary>依模型屬性建立下拉選單（由清單來源）。</summary>
        /// <param name="htmlHelper">HTML Helper。</param>
        /// <param name="expression">模型屬性運算式。</param>
        /// <param name="selectList">選項來源。</param>
        /// <param name="optionLabel">預設提示文字。</param>
        /// <param name="htmlAttributes">HTML 屬性。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent DropDownListFor<TModel, TProperty, K, V>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<K, V> selectList, string optionLabel, object htmlAttributes = null)
        {
            return DropDownListFor(htmlHelper, expression, selectList, optionLabel, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }


        /// <summary>依模型屬性建立下拉選單（由清單來源）。</summary>
        /// <param name="htmlHelper">HTML Helper。</param>
        /// <param name="expression">模型屬性運算式。</param>
        /// <param name="selectList">選項來源。</param>
        /// <param name="optionLabel">預設提示文字。</param>
        /// <param name="htmlAttributes">HTML 屬性。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent DropDownListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<int> selectList, string optionLabel, IDictionary<string, object> htmlAttributes)
        {
            return dropDownListFor(htmlHelper, expression, HelperUtils.ToSelectListItem(selectList), optionLabel, htmlAttributes);
        }
        /// <summary>依模型屬性建立下拉選單（由清單來源）。</summary>
        /// <param name="htmlHelper">HTML Helper。</param>
        /// <param name="expression">模型屬性運算式。</param>
        /// <param name="selectList">選項來源。</param>
        /// <param name="optionLabel">預設提示文字。</param>
        /// <param name="htmlAttributes">HTML 屬性。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent DropDownListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<string> selectList, string optionLabel, IDictionary<string, object> htmlAttributes)
        {
            return dropDownListFor(htmlHelper, expression, HelperUtils.ToSelectListItem(selectList), optionLabel, htmlAttributes);
        }
        /// <summary>依模型屬性建立下拉選單（由清單來源）。</summary>
        /// <param name="htmlHelper">HTML Helper。</param>
        /// <param name="expression">模型屬性運算式。</param>
        /// <param name="selectList">選項來源。</param>
        /// <param name="optionLabel">預設提示文字。</param>
        /// <param name="htmlAttributes">HTML 屬性。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent DropDownListFor<TModel, TProperty, K, V>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<K, V> selectList, string optionLabel, IDictionary<string, object> htmlAttributes)
        {
            return dropDownListFor(htmlHelper, expression, HelperUtils.ToSelectListItem(selectList), optionLabel, htmlAttributes);
        }

        private static IHtmlContent dropDownListFor<TModel, TProperty>(IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, string optionLabel, IDictionary<string, object> htmlAttributes)
        {
            return htmlHelper.DropDownListFor(expression, selectList, optionLabel, htmlAttributes);
        }





        /// <summary>建立多選清單（由清單來源）。</summary>
        /// <param name="htmlHelper">HTML Helper。</param>
        /// <param name="name">欄位名稱。</param>
        /// <param name="selectList">選項來源。</param>
        /// <param name="htmlAttributes">HTML 屬性。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent ListBox(this IHtmlHelper htmlHelper, string name, IEnumerable<int> selectList, object htmlAttributes = null)
        {
            return ListBox(htmlHelper, name, selectList, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }
        /// <summary>建立多選清單（由清單來源）。</summary>
        /// <param name="htmlHelper">HTML Helper。</param>
        /// <param name="name">欄位名稱。</param>
        /// <param name="selectList">選項來源。</param>
        /// <param name="htmlAttributes">HTML 屬性。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent ListBox(this IHtmlHelper htmlHelper, string name, IEnumerable<string> selectList, object htmlAttributes = null)
        {
            return ListBox(htmlHelper, name, selectList, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }
        /// <summary>建立多選清單（由清單來源）。</summary>
        /// <param name="htmlHelper">HTML Helper。</param>
        /// <param name="name">欄位名稱。</param>
        /// <param name="selectList">選項來源。</param>
        /// <param name="htmlAttributes">HTML 屬性。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent ListBox<K, V>(this IHtmlHelper htmlHelper, string name, IDictionary<K, V> selectList, object htmlAttributes = null)
        {
            return ListBox(htmlHelper, name, selectList, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }


        /// <summary>建立多選清單（由清單來源）。</summary>
        /// <param name="htmlHelper">HTML Helper。</param>
        /// <param name="name">欄位名稱。</param>
        /// <param name="selectList">選項來源。</param>
        /// <param name="htmlAttributes">HTML 屬性。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent ListBox(this IHtmlHelper htmlHelper, string name, IEnumerable<int> selectList, IDictionary<string, object> htmlAttributes)
        {
            return listBox(htmlHelper, name, HelperUtils.ToSelectListItem(selectList), htmlAttributes);
        }
        /// <summary>建立多選清單（由清單來源）。</summary>
        /// <param name="htmlHelper">HTML Helper。</param>
        /// <param name="name">欄位名稱。</param>
        /// <param name="selectList">選項來源。</param>
        /// <param name="htmlAttributes">HTML 屬性。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent ListBox(this IHtmlHelper htmlHelper, string name, IEnumerable<string> selectList, IDictionary<string, object> htmlAttributes)
        {
            return listBox(htmlHelper, name, HelperUtils.ToSelectListItem(selectList), htmlAttributes);
        }
        /// <summary>建立多選清單（由清單來源）。</summary>
        /// <param name="htmlHelper">HTML Helper。</param>
        /// <param name="name">欄位名稱。</param>
        /// <param name="selectList">選項來源。</param>
        /// <param name="htmlAttributes">HTML 屬性。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent ListBox<K, V>(this IHtmlHelper htmlHelper, string name, IDictionary<K, V> selectList, IDictionary<string, object> htmlAttributes)
        {
            return listBox(htmlHelper, name, HelperUtils.ToSelectListItem(selectList), htmlAttributes);
        }


        private static IHtmlContent listBox(IHtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList, IDictionary<string, object> htmlAttributes)
        {
            return htmlHelper.ListBox(name, selectList, htmlAttributes);
        }



        /// <summary>依模型屬性建立多選清單（由清單來源）。</summary>
        /// <param name="htmlHelper">HTML Helper。</param>
        /// <param name="expression">模型屬性運算式。</param>
        /// <param name="selectList">選項來源。</param>
        /// <param name="htmlAttributes">HTML 屬性。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent ListBoxFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<int> selectList, object htmlAttributes = null)
        {
            return ListBoxFor(htmlHelper, expression, selectList, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }
        /// <summary>依模型屬性建立多選清單（由清單來源）。</summary>
        /// <param name="htmlHelper">HTML Helper。</param>
        /// <param name="expression">模型屬性運算式。</param>
        /// <param name="selectList">選項來源。</param>
        /// <param name="htmlAttributes">HTML 屬性。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent ListBoxFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<string> selectList, object htmlAttributes = null)
        {
            return ListBoxFor(htmlHelper, expression, selectList, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }
        /// <summary>依模型屬性建立多選清單（由清單來源）。</summary>
        /// <param name="htmlHelper">HTML Helper。</param>
        /// <param name="expression">模型屬性運算式。</param>
        /// <param name="selectList">選項來源。</param>
        /// <param name="htmlAttributes">HTML 屬性。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent ListBoxFor<TModel, TProperty, K, V>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<K, V> selectList, object htmlAttributes = null)
        {
            return ListBoxFor(htmlHelper, expression, selectList, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }


        /// <summary>依模型屬性建立多選清單（由清單來源）。</summary>
        /// <param name="htmlHelper">HTML Helper。</param>
        /// <param name="expression">模型屬性運算式。</param>
        /// <param name="selectList">選項來源。</param>
        /// <param name="htmlAttributes">HTML 屬性。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent ListBoxFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<int> selectList, IDictionary<string, object> htmlAttributes)
        {
            return listBoxFor(htmlHelper, expression, HelperUtils.ToSelectListItem(selectList), htmlAttributes);
        }
        /// <summary>依模型屬性建立多選清單（由清單來源）。</summary>
        /// <param name="htmlHelper">HTML Helper。</param>
        /// <param name="expression">模型屬性運算式。</param>
        /// <param name="selectList">選項來源。</param>
        /// <param name="htmlAttributes">HTML 屬性。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent ListBoxFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<string> selectList, IDictionary<string, object> htmlAttributes)
        {
            return listBoxFor(htmlHelper, expression, HelperUtils.ToSelectListItem(selectList), htmlAttributes);
        }
        /// <summary>依模型屬性建立多選清單（由清單來源）。</summary>
        /// <param name="htmlHelper">HTML Helper。</param>
        /// <param name="expression">模型屬性運算式。</param>
        /// <param name="selectList">選項來源。</param>
        /// <param name="htmlAttributes">HTML 屬性。</param>
        /// <returns>產生的 HTML 內容。</returns>
        public static IHtmlContent ListBoxFor<TModel, TProperty, K, V>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<K, V> selectList, IDictionary<string, object> htmlAttributes)
        {
            return listBoxFor(htmlHelper, expression, HelperUtils.ToSelectListItem(selectList), htmlAttributes);

        }

        private static IHtmlContent listBoxFor<TModel, TProperty>(IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, IDictionary<string, object> htmlAttributes)
        {
            return htmlHelper.ListBoxFor(expression, selectList, htmlAttributes);
        }


    }
}
