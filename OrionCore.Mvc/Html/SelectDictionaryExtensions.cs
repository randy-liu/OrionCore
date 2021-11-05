using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace OrionCore.Mvc.Html
{

	/// <summary></summary>
	public static class SelectDictionaryExtensions
	{

		/// <summary></summary>
		public static IHtmlContent DropDownList(this IHtmlHelper htmlHelper, string name, IEnumerable<int> selectList, IDictionary<string, object> htmlAttributes)
		{
			return DropDownList(htmlHelper, name, selectList, null, htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent DropDownList(this IHtmlHelper htmlHelper, string name, IEnumerable<string> selectList, IDictionary<string, object> htmlAttributes)
		{
			return DropDownList(htmlHelper, name, selectList, null, htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent DropDownList<K, V>(this IHtmlHelper htmlHelper, string name, IDictionary<K, V> selectList, IDictionary<string, object> htmlAttributes)
		{
			return DropDownList(htmlHelper, name, selectList, null, htmlAttributes);
		}


		/// <summary></summary>
		public static IHtmlContent DropDownList(this IHtmlHelper htmlHelper, string name, IEnumerable<int> selectList, object htmlAttributes = null)
		{
			return DropDownList(htmlHelper, name, selectList, null, htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent DropDownList(this IHtmlHelper htmlHelper, string name, IEnumerable<string> selectList, object htmlAttributes = null)
		{
			return DropDownList(htmlHelper, name, selectList, null, htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent DropDownList<K, V>(this IHtmlHelper htmlHelper, string name, IDictionary<K, V> selectList, object htmlAttributes = null)
		{
			return DropDownList(htmlHelper, name, selectList, null, htmlAttributes);
		}


		/// <summary></summary>
		public static IHtmlContent DropDownList(this IHtmlHelper htmlHelper, string name, IEnumerable<int> selectList, string optionLabel, object htmlAttributes = null)
		{
			return DropDownList(htmlHelper, name, selectList, optionLabel, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}
		/// <summary></summary>
		public static IHtmlContent DropDownList(this IHtmlHelper htmlHelper, string name, IEnumerable<string> selectList, string optionLabel, object htmlAttributes = null)
		{
			return DropDownList(htmlHelper, name, selectList, optionLabel, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}
		/// <summary></summary>
		public static IHtmlContent DropDownList<K, V>(this IHtmlHelper htmlHelper, string name, IDictionary<K, V> selectList, string optionLabel, object htmlAttributes = null)
		{
			return DropDownList(htmlHelper, name, selectList, optionLabel, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}


		/// <summary></summary>
		public static IHtmlContent DropDownList(this IHtmlHelper htmlHelper, string name, IEnumerable<int> selectList, string optionLabel, IDictionary<string, object> htmlAttributes)
		{
			return dropDownList(htmlHelper, name, HelperUtils.ToSelectListItem(selectList), optionLabel, htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent DropDownList(this IHtmlHelper htmlHelper, string name, IEnumerable<string> selectList, string optionLabel, IDictionary<string, object> htmlAttributes)
		{
			return dropDownList(htmlHelper, name, HelperUtils.ToSelectListItem(selectList), optionLabel, htmlAttributes);
		}

		/// <summary></summary>
		public static IHtmlContent DropDownList<K, V>(this IHtmlHelper htmlHelper, string name, IDictionary<K, V> selectList, string optionLabel, IDictionary<string, object> htmlAttributes)
		{
			return dropDownList(htmlHelper, name, HelperUtils.ToSelectListItem(selectList), optionLabel, htmlAttributes);
		}

		private static IHtmlContent dropDownList(IHtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList, string optionLabel, IDictionary<string, object> htmlAttributes)
		{
			//var list = HelperUtils.MarkSelectList(htmlHelper, null, name, selectList, false);
			return htmlHelper.DropDownList(name, selectList, optionLabel, htmlAttributes);
		}







		/// <summary></summary>
		public static IHtmlContent DropDownListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<int> selectList, IDictionary<string, object> htmlAttributes)
		{
			return DropDownListFor(htmlHelper, expression, selectList, null, htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent DropDownListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<string> selectList, IDictionary<string, object> htmlAttributes)
		{
			return DropDownListFor(htmlHelper, expression, selectList, null, htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent DropDownListFor<TModel, TProperty, K, V>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<K, V> selectList, IDictionary<string, object> htmlAttributes)
		{
			return DropDownListFor(htmlHelper, expression, selectList, null, htmlAttributes);
		}

		/// <summary></summary>
		public static IHtmlContent DropDownListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<int> selectList, object htmlAttributes = null)
		{
			return DropDownListFor(htmlHelper, expression, selectList, null, htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent DropDownListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<string> selectList, object htmlAttributes = null)
		{
			return DropDownListFor(htmlHelper, expression, selectList, null, htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent DropDownListFor<TModel, TProperty, K, V>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<K, V> selectList, object htmlAttributes = null)
		{
			return DropDownListFor(htmlHelper, expression, selectList, null, htmlAttributes);
		}


		/// <summary></summary>
		public static IHtmlContent DropDownListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<int> selectList, string optionLabel, object htmlAttributes = null)
		{
			return DropDownListFor(htmlHelper, expression, selectList, optionLabel, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}
		/// <summary></summary>
		public static IHtmlContent DropDownListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<string> selectList, string optionLabel, object htmlAttributes = null)
		{
			return DropDownListFor(htmlHelper, expression, selectList, optionLabel, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}
		/// <summary></summary>
		public static IHtmlContent DropDownListFor<TModel, TProperty, K, V>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<K, V> selectList, string optionLabel, object htmlAttributes = null)
		{
			return DropDownListFor(htmlHelper, expression, selectList, optionLabel, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}


		/// <summary></summary>
		public static IHtmlContent DropDownListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<int> selectList, string optionLabel, IDictionary<string, object> htmlAttributes)
		{
			return dropDownListFor(htmlHelper, expression, HelperUtils.ToSelectListItem(selectList), optionLabel, htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent DropDownListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<string> selectList, string optionLabel, IDictionary<string, object> htmlAttributes)
		{
			return dropDownListFor(htmlHelper, expression, HelperUtils.ToSelectListItem(selectList), optionLabel, htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent DropDownListFor<TModel, TProperty, K, V>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<K, V> selectList, string optionLabel, IDictionary<string, object> htmlAttributes)
		{
			return dropDownListFor(htmlHelper, expression, HelperUtils.ToSelectListItem(selectList), optionLabel, htmlAttributes);
		}

		private static IHtmlContent dropDownListFor<TModel, TProperty>(IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, string optionLabel, IDictionary<string, object> htmlAttributes)
		{
			//string name = ExpressionHelper.GetExpressionText(expression);
			//ModelExplorer metadata = ExpressionMetadataProvider.FromLambdaExpression(expression, htmlHelper.ViewData, htmlHelper.MetadataProvider);

			//var value = htmlHelper.ValueFor(expression);
			//var list = HelperUtils.MarkSelectList(htmlHelper, metadata, name, selectList, false);
			//return htmlHelper.DropDownListFor(expression, list, optionLabel, htmlAttributes);
			return htmlHelper.DropDownListFor(expression, selectList, optionLabel, htmlAttributes);
		}





		/// <summary></summary>
		public static IHtmlContent ListBox(this IHtmlHelper htmlHelper, string name, IEnumerable<int> selectList, object htmlAttributes = null)
		{
			return ListBox(htmlHelper, name, selectList, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}
		/// <summary></summary>
		public static IHtmlContent ListBox(this IHtmlHelper htmlHelper, string name, IEnumerable<string> selectList, object htmlAttributes = null)
		{
			return ListBox(htmlHelper, name, selectList, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}
		/// <summary></summary>
		public static IHtmlContent ListBox<K, V>(this IHtmlHelper htmlHelper, string name, IDictionary<K, V> selectList, object htmlAttributes = null)
		{
			return ListBox(htmlHelper, name, selectList, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}


		/// <summary></summary>
		public static IHtmlContent ListBox(this IHtmlHelper htmlHelper, string name, IEnumerable<int> selectList, IDictionary<string, object> htmlAttributes)
		{
			return listBox(htmlHelper, name, HelperUtils.ToSelectListItem(selectList), htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent ListBox(this IHtmlHelper htmlHelper, string name, IEnumerable<string> selectList, IDictionary<string, object> htmlAttributes)
		{
			return listBox(htmlHelper, name, HelperUtils.ToSelectListItem(selectList), htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent ListBox<K, V>(this IHtmlHelper htmlHelper, string name, IDictionary<K, V> selectList, IDictionary<string, object> htmlAttributes)
		{
			return listBox(htmlHelper, name, HelperUtils.ToSelectListItem(selectList), htmlAttributes);
		}


		private static IHtmlContent listBox(IHtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList, IDictionary<string, object> htmlAttributes)
		{
			//var list = HelperUtils.MarkSelectList(htmlHelper, null, name, selectList, true);
			return htmlHelper.ListBox(name, selectList, htmlAttributes);
		}



		/// <summary></summary>
		public static IHtmlContent ListBoxFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<int> selectList, object htmlAttributes = null)
		{
			return ListBoxFor(htmlHelper, expression, selectList, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}
		/// <summary></summary>
		public static IHtmlContent ListBoxFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<string> selectList, object htmlAttributes = null)
		{
			return ListBoxFor(htmlHelper, expression, selectList, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}
		/// <summary></summary>
		public static IHtmlContent ListBoxFor<TModel, TProperty, K, V>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<K, V> selectList, object htmlAttributes = null)
		{
			return ListBoxFor(htmlHelper, expression, selectList, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}


		/// <summary></summary>
		public static IHtmlContent ListBoxFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<int> selectList, IDictionary<string, object> htmlAttributes)
		{
			return listBoxFor(htmlHelper, expression, HelperUtils.ToSelectListItem(selectList), htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent ListBoxFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<string> selectList, IDictionary<string, object> htmlAttributes)
		{
			return listBoxFor(htmlHelper, expression, HelperUtils.ToSelectListItem(selectList), htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent ListBoxFor<TModel, TProperty, K, V>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<K, V> selectList, IDictionary<string, object> htmlAttributes)
		{
			return listBoxFor(htmlHelper, expression, HelperUtils.ToSelectListItem(selectList), htmlAttributes);

		}

		private static IHtmlContent listBoxFor<TModel, TProperty>(IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, IDictionary<string, object> htmlAttributes)
		{
			//string name = ExpressionHelper.GetExpressionText(expression);
			//ModelExplorer metadata = ExpressionMetadataProvider.FromLambdaExpression(expression, htmlHelper.ViewData, htmlHelper.MetadataProvider);

			//var list = HelperUtils.MarkSelectList(htmlHelper, metadata, name, selectList, true);
			//return htmlHelper.ListBoxFor(expression, list, htmlAttributes);
			return htmlHelper.ListBoxFor(expression, selectList, htmlAttributes);
		}


	}
}