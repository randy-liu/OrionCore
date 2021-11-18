using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Orion.Mvc.Html
{

	/// <summary></summary>
	public static class BsSelectDictionaryExtensions
	{

		/// <summary></summary>
		public static IHtmlContent BsDropDownList(this IHtmlHelper htmlHelper, string name, IEnumerable<int> selectList)
		{
			return BsDropDownList(htmlHelper, name, selectList, null, null);
		}
		/// <summary></summary>
		public static IHtmlContent BsDropDownList(this IHtmlHelper htmlHelper, string name, IEnumerable<string> selectList)
		{
			return BsDropDownList(htmlHelper, name, selectList, null, null);
		}
		/// <summary></summary>
		public static IHtmlContent BsDropDownList<K, V>(this IHtmlHelper htmlHelper, string name, IDictionary<K, V> selectList)
		{
			return BsDropDownList(htmlHelper, name, selectList, null, null);
		}
		/// <summary></summary>
		public static IHtmlContent BsDropDownList(this IHtmlHelper htmlHelper, string name, IEnumerable<int> selectList, object htmlAttributes)
		{
			return BsDropDownList(htmlHelper, name, selectList, null, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}
		/// <summary></summary>
		public static IHtmlContent BsDropDownList(this IHtmlHelper htmlHelper, string name, IEnumerable<string> selectList, object htmlAttributes)
		{
			return BsDropDownList(htmlHelper, name, selectList, null, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}
		/// <summary></summary>
		public static IHtmlContent BsDropDownList<K, V>(this IHtmlHelper htmlHelper, string name, IDictionary<K, V> selectList, object htmlAttributes)
		{
			return BsDropDownList(htmlHelper, name, selectList, null, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}


		/// <summary></summary>
		public static IHtmlContent BsDropDownList(this IHtmlHelper htmlHelper, string name, IEnumerable<int> selectList, IDictionary<string, object> htmlAttributes)
		{
			return BsDropDownList(htmlHelper, name, selectList, null, htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent BsDropDownList(this IHtmlHelper htmlHelper, string name, IEnumerable<string> selectList, IDictionary<string, object> htmlAttributes)
		{
			return BsDropDownList(htmlHelper, name, selectList, null, htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent BsDropDownList<K, V>(this IHtmlHelper htmlHelper, string name, IDictionary<K, V> selectList, IDictionary<string, object> htmlAttributes)
		{
			return BsDropDownList(htmlHelper, name, selectList, null, htmlAttributes);
		}



		/// <summary></summary>
		public static IHtmlContent BsDropDownList(this IHtmlHelper htmlHelper, string name, IEnumerable<int> selectList, string optionLabel)
		{
			return BsDropDownList(htmlHelper, name, selectList, optionLabel, null);
		}
		/// <summary></summary>
		public static IHtmlContent BsDropDownList(this IHtmlHelper htmlHelper, string name, IEnumerable<string> selectList, string optionLabel)
		{
			return BsDropDownList(htmlHelper, name, selectList, optionLabel, null);
		}
		/// <summary></summary>
		public static IHtmlContent BsDropDownList<K, V>(this IHtmlHelper htmlHelper, string name, IDictionary<K, V> selectList, string optionLabel)
		{
			return BsDropDownList(htmlHelper, name, selectList, optionLabel, null);
		}
		/// <summary></summary>
		public static IHtmlContent BsDropDownList(this IHtmlHelper htmlHelper, string name, IEnumerable<int> selectList, string optionLabel, object htmlAttributes)
		{
			return BsDropDownList(htmlHelper, name, selectList, optionLabel, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}
		/// <summary></summary>
		public static IHtmlContent BsDropDownList(this IHtmlHelper htmlHelper, string name, IEnumerable<string> selectList, string optionLabel, object htmlAttributes)
		{
			return BsDropDownList(htmlHelper, name, selectList, optionLabel, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}
		/// <summary></summary>
		public static IHtmlContent BsDropDownList<K, V>(this IHtmlHelper htmlHelper, string name, IDictionary<K, V> selectList, string optionLabel, object htmlAttributes)
		{
			return BsDropDownList(htmlHelper, name, selectList, optionLabel, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}


		/// <summary></summary>
		public static IHtmlContent BsDropDownList(this IHtmlHelper htmlHelper, string name, IEnumerable<int> selectList, string optionLabel, IDictionary<string, object> htmlAttributes)
		{
			return htmlHelper.BsDropDownList(name, HelperUtils.ToSelectListItem(selectList), optionLabel, htmlAttributes);
		}

		/// <summary></summary>
		public static IHtmlContent BsDropDownList(this IHtmlHelper htmlHelper, string name, IEnumerable<string> selectList, string optionLabel, IDictionary<string, object> htmlAttributes)
		{
			return htmlHelper.BsDropDownList(name, HelperUtils.ToSelectListItem(selectList), optionLabel, htmlAttributes);
		}

		/// <summary></summary>
		public static IHtmlContent BsDropDownList<K, V>(this IHtmlHelper htmlHelper, string name, IDictionary<K, V> selectList, string optionLabel, IDictionary<string, object> htmlAttributes)
		{
			return htmlHelper.BsDropDownList(name, HelperUtils.ToSelectListItem(selectList), optionLabel, htmlAttributes);
		}






		/*==================================================== */


		/// <summary></summary>
		public static IHtmlContent BsDropDownListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<int> selectList)
		{
			return BsDropDownListFor(htmlHelper, expression, selectList, null, null);
		}
		/// <summary></summary>
		public static IHtmlContent BsDropDownListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<string> selectList)
		{
			return BsDropDownListFor(htmlHelper, expression, selectList, null, null);
		}
		/// <summary></summary>
		public static IHtmlContent BsDropDownListFor<TModel, TProperty, K, V>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<K, V> selectList)
		{
			return BsDropDownListFor(htmlHelper, expression, selectList, null, null);
		}
		/// <summary></summary>
		public static IHtmlContent BsDropDownListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<int> selectList, object htmlAttributes)
		{
			return BsDropDownListFor(htmlHelper, expression, selectList, null, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}
		/// <summary></summary>
		public static IHtmlContent BsDropDownListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<string> selectList, object htmlAttributes)
		{
			return BsDropDownListFor(htmlHelper, expression, selectList, null, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}
		/// <summary></summary>
		public static IHtmlContent BsDropDownListFor<TModel, TProperty, K, V>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<K, V> selectList, object htmlAttributes)
		{
			return BsDropDownListFor(htmlHelper, expression, selectList, null, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}

		/// <summary></summary>
		public static IHtmlContent BsDropDownListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<int> selectList, IDictionary<string, object> htmlAttributes)
		{
			return BsDropDownListFor(htmlHelper, expression, selectList, null, htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent BsDropDownListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<string> selectList, IDictionary<string, object> htmlAttributes)
		{
			return BsDropDownListFor(htmlHelper, expression, selectList, null, htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent BsDropDownListFor<TModel, TProperty, K, V>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<K, V> selectList, IDictionary<string, object> htmlAttributes)
		{
			return BsDropDownListFor(htmlHelper, expression, selectList, null, htmlAttributes);
		}


		/// <summary></summary>
		public static IHtmlContent BsDropDownListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<int> selectList, string optionLabel)
		{
			return BsDropDownListFor(htmlHelper, expression, selectList, optionLabel, null);
		}
		/// <summary></summary>
		public static IHtmlContent BsDropDownListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<string> selectList, string optionLabel)
		{
			return BsDropDownListFor(htmlHelper, expression, selectList, optionLabel, null);
		}
		/// <summary></summary>
		public static IHtmlContent BsDropDownListFor<TModel, TProperty, K, V>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<K, V> selectList, string optionLabel)
		{
			return BsDropDownListFor(htmlHelper, expression, selectList, optionLabel, null);
		}
		/// <summary></summary>
		public static IHtmlContent BsDropDownListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<int> selectList, string optionLabel, object htmlAttributes)
		{
			return BsDropDownListFor(htmlHelper, expression, selectList, optionLabel, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}
		/// <summary></summary>
		public static IHtmlContent BsDropDownListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<string> selectList, string optionLabel, object htmlAttributes)
		{
			return BsDropDownListFor(htmlHelper, expression, selectList, optionLabel, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}
		/// <summary></summary>
		public static IHtmlContent BsDropDownListFor<TModel, TProperty, K, V>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<K, V> selectList, string optionLabel, object htmlAttributes)
		{
			return BsDropDownListFor(htmlHelper, expression, selectList, optionLabel, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}


		/// <summary></summary>
		public static IHtmlContent BsDropDownListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<int> selectList, string optionLabel, IDictionary<string, object> htmlAttributes)
		{
			return htmlHelper.BsDropDownListFor(expression, HelperUtils.ToSelectListItem(selectList), optionLabel, htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent BsDropDownListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<string> selectList, string optionLabel, IDictionary<string, object> htmlAttributes)
		{
			return htmlHelper.BsDropDownListFor(expression, HelperUtils.ToSelectListItem(selectList), optionLabel, htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent BsDropDownListFor<TModel, TProperty, K, V>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<K, V> selectList, string optionLabel, IDictionary<string, object> htmlAttributes)
		{
			return htmlHelper.BsDropDownListFor(expression, HelperUtils.ToSelectListItem(selectList), optionLabel, htmlAttributes);
		}
		 

	}
}