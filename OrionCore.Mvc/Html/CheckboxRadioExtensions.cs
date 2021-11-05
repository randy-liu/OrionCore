using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using OrionCore.API;

namespace OrionCore.Mvc.Html
{

	/// <summary></summary>
	public static class CheckboxRadioExtensions 
	{


		/// <summary></summary>
		public static IHtmlContent BoolRadioList(this IHtmlHelper htmlHelper, string name, object htmlAttributes = null)
		{
			return RadioList(htmlHelper, name, HelperUtils.GetBoolSelectListItem(), htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent BoolRadioList(this IHtmlHelper htmlHelper, string name, string trueLabel, string falseLabel, object htmlAttributes = null)
		{
			return RadioList(htmlHelper, name, HelperUtils.GetBoolSelectListItem(trueLabel, falseLabel), htmlAttributes);
		}

		/// <summary></summary>
		public static IHtmlContent RadioList(this IHtmlHelper htmlHelper, string name, IEnumerable<int> selectList, object htmlAttributes = null)
		{
			return RadioList(htmlHelper, name, HelperUtils.ToSelectListItem(selectList), htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent RadioList(this IHtmlHelper htmlHelper, string name, IEnumerable<string> selectList, object htmlAttributes = null)
		{
			return RadioList(htmlHelper, name, HelperUtils.ToSelectListItem(selectList), htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent RadioList<K, V>(this IHtmlHelper htmlHelper, string name, IDictionary<K, V> selectList, object htmlAttributes = null)
		{
			return RadioList(htmlHelper, name, HelperUtils.ToSelectListItem(selectList), htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent RadioList(this IHtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList, object htmlAttributes = null)
		{
			return RadioList(htmlHelper, name, selectList, ((IDictionary<string, object>)HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes)));
		}



		/// <summary></summary>
		public static IHtmlContent BoolRadioList(this IHtmlHelper htmlHelper, string name, IDictionary<string, object> htmlAttributes)
		{
			return RadioList(htmlHelper, name, HelperUtils.GetBoolSelectListItem(), htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent BoolRadioList(this IHtmlHelper htmlHelper, string name, string trueLabel, string falseLabel, IDictionary<string, object> htmlAttributes)
		{
			return RadioList(htmlHelper, name, HelperUtils.GetBoolSelectListItem(trueLabel, falseLabel), htmlAttributes);
		}

		/// <summary></summary>
		public static IHtmlContent RadioList(this IHtmlHelper htmlHelper, string name, IEnumerable<int> selectList, IDictionary<string, object> htmlAttributes)
		{
			return RadioList(htmlHelper, name, HelperUtils.ToSelectListItem(selectList), htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent RadioList(this IHtmlHelper htmlHelper, string name, IEnumerable<string> selectList, IDictionary<string, object> htmlAttributes)
		{
			return RadioList(htmlHelper, name, HelperUtils.ToSelectListItem(selectList), htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent RadioList<K, V>(this IHtmlHelper htmlHelper, string name, IDictionary<K, V> selectList, IDictionary<string, object> htmlAttributes)
		{
			return RadioList(htmlHelper, name, HelperUtils.ToSelectListItem(selectList), htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent RadioList(this IHtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList, IDictionary<string, object> htmlAttributes)
		{
			var srcTag = htmlHelper.DropDownList(name, selectList, null, null) as TagBuilder;
			return itemInternal(htmlHelper, srcTag, "radio", htmlAttributes);
		}



		/// <summary></summary>
		public static IHtmlContent BoolRadioListFor<TModel, TBool>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TBool>> expression, object htmlAttributes = null)
		{
			return RadioListFor(htmlHelper, expression, HelperUtils.GetBoolSelectListItem(), htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent BoolRadioListFor<TModel, TBool>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TBool>> expression, string trueLabel, string falseLabel, object htmlAttributes = null)
		{
			return RadioListFor(htmlHelper, expression, HelperUtils.GetBoolSelectListItem(trueLabel, falseLabel), htmlAttributes);
		}

		/// <summary></summary>
		public static IHtmlContent EnumRadioListFor<TModel, TEnum>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, object htmlAttributes = null)
		{
			IDictionary<string, string> selectList = OrionUtils.EnumToDictionary<TEnum>();
			return RadioListFor(htmlHelper, expression, HelperUtils.ToSelectListItem(selectList), htmlAttributes);
		}

		/// <summary></summary>
		public static IHtmlContent RadioListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<int> selectList, object htmlAttributes = null)
		{
			return RadioListFor(htmlHelper, expression, HelperUtils.ToSelectListItem(selectList), htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent RadioListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<string> selectList, object htmlAttributes = null)
		{
			return RadioListFor(htmlHelper, expression, HelperUtils.ToSelectListItem(selectList), htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent RadioListFor<TModel, TProperty, K, V>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<K, V> selectList, object htmlAttributes = null)
		{
			return RadioListFor(htmlHelper, expression, HelperUtils.ToSelectListItem(selectList), htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent RadioListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, object htmlAttributes = null)
		{
			return RadioListFor(htmlHelper, expression, selectList, ((IDictionary<string, object>)HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes)));
		}



		/// <summary></summary>
		public static IHtmlContent BoolRadioListFor<TModel, TBool>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TBool>> expression, IDictionary<string, object> htmlAttributes)
		{
			return RadioListFor(htmlHelper, expression, HelperUtils.GetBoolSelectListItem(), htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent BoolRadioListFor<TModel, TBool>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TBool>> expression, string trueLabel, string falseLabel, IDictionary<string, object> htmlAttributes)
		{
			return RadioListFor(htmlHelper, expression, HelperUtils.GetBoolSelectListItem(trueLabel, falseLabel), htmlAttributes);
		}

		/// <summary></summary>
		public static IHtmlContent EnumRadioListFor<TModel, TEnum>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, IDictionary<string, object> htmlAttributes)
		{
			IDictionary<string, string> selectList = OrionUtils.EnumToDictionary<TEnum>();
			return RadioListFor(htmlHelper, expression, HelperUtils.ToSelectListItem(selectList), htmlAttributes);
		}

		/// <summary></summary>
		public static IHtmlContent RadioListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<int> selectList, IDictionary<string, object> htmlAttributes)
		{
			return RadioListFor(htmlHelper, expression, HelperUtils.ToSelectListItem(selectList), htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent RadioListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<string> selectList, IDictionary<string, object> htmlAttributes)
		{
			return RadioListFor(htmlHelper, expression, HelperUtils.ToSelectListItem(selectList), htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent RadioListFor<TModel, TProperty, K, V>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<K, V> selectList, IDictionary<string, object> htmlAttributes)
		{
			return RadioListFor(htmlHelper, expression, HelperUtils.ToSelectListItem(selectList), htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent RadioListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, IDictionary<string, object> htmlAttributes)
		{
			var srcTag = htmlHelper.DropDownListFor(expression, selectList, null, null) as TagBuilder;
			return itemInternal(htmlHelper, srcTag, "radio", htmlAttributes);
		}
		 






		/// <summary></summary>
		public static IHtmlContent CheckboxList(this IHtmlHelper htmlHelper, string name, IEnumerable<int> selectList, object htmlAttributes = null)
		{
			return CheckboxList(htmlHelper, name, HelperUtils.ToSelectListItem(selectList), htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent CheckboxList(this IHtmlHelper htmlHelper, string name, IEnumerable<string> selectList, object htmlAttributes = null)
		{
			return CheckboxList(htmlHelper, name, HelperUtils.ToSelectListItem(selectList), htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent CheckboxList<K, V>(this IHtmlHelper htmlHelper, string name, IDictionary<K, V> selectList, object htmlAttributes = null)
		{
			return CheckboxList(htmlHelper, name, HelperUtils.ToSelectListItem(selectList), htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent CheckboxList(this IHtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList, object htmlAttributes = null)
		{
			return CheckboxList(htmlHelper, name, selectList, ((IDictionary<string, object>)HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes)));
		}


		/// <summary></summary>
		public static IHtmlContent CheckboxList(this IHtmlHelper htmlHelper, string name, IEnumerable<int> selectList, IDictionary<string, object> htmlAttributes)
		{
			return CheckboxList(htmlHelper, name, HelperUtils.ToSelectListItem(selectList), htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent CheckboxList(this IHtmlHelper htmlHelper, string name, IEnumerable<string> selectList, IDictionary<string, object> htmlAttributes)
		{
			return CheckboxList(htmlHelper, name, HelperUtils.ToSelectListItem(selectList), htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent CheckboxList<K, V>(this IHtmlHelper htmlHelper, string name, IDictionary<K, V> selectList, IDictionary<string, object> htmlAttributes)
		{
			return CheckboxList(htmlHelper, name, HelperUtils.ToSelectListItem(selectList), htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent CheckboxList(this IHtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList, IDictionary<string, object> htmlAttributes)
		{
			var srcTag = htmlHelper.ListBox(name, selectList, null) as TagBuilder;
			return itemInternal(htmlHelper, srcTag, "checkbox", htmlAttributes);			
		}


		/// <summary></summary>
		public static IHtmlContent EnumCheckboxListFor<TModel, TEnum>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, object htmlAttributes = null)
		{
			IDictionary<string, string> selectList = OrionUtils.EnumToDictionary<TEnum>();
			return CheckboxListFor(htmlHelper, expression, HelperUtils.ToSelectListItem(selectList), htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent CheckboxListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<int> selectList, object htmlAttributes = null)
		{
			return CheckboxListFor(htmlHelper, expression, HelperUtils.ToSelectListItem(selectList), htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent CheckboxListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<string> selectList, object htmlAttributes = null)
		{
			return CheckboxListFor(htmlHelper, expression, HelperUtils.ToSelectListItem(selectList), htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent CheckboxListFor<TModel, TProperty, K, V>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<K, V> selectList, object htmlAttributes = null)
		{
			return CheckboxListFor(htmlHelper, expression, HelperUtils.ToSelectListItem(selectList), htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent CheckboxListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, object htmlAttributes = null)
		{
			return CheckboxListFor(htmlHelper, expression, selectList, ((IDictionary<string, object>)HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes)));
		}


		/// <summary></summary>
		public static IHtmlContent EnumCheckboxListFor<TModel, TEnum>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, IDictionary<string, object> htmlAttributes)
		{
			IDictionary<string, string> selectList = OrionUtils.EnumToDictionary<TEnum>();
			return CheckboxListFor(htmlHelper, expression, HelperUtils.ToSelectListItem(selectList), htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent CheckboxListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<int> selectList, IDictionary<string, object> htmlAttributes)
		{
			return CheckboxListFor(htmlHelper, expression, HelperUtils.ToSelectListItem(selectList), htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent CheckboxListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<string> selectList, IDictionary<string, object> htmlAttributes)
		{
			return CheckboxListFor(htmlHelper, expression, HelperUtils.ToSelectListItem(selectList), htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent CheckboxListFor<TModel, TProperty, K, V>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<K, V> selectList, IDictionary<string, object> htmlAttributes)
		{
			return CheckboxListFor(htmlHelper, expression, HelperUtils.ToSelectListItem(selectList), htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent CheckboxListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, IDictionary<string, object> htmlAttributes)
		{
			TagBuilder srcTag;

			Type propType = typeof(TProperty);
			if (propType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(propType))
			{ srcTag = htmlHelper.ListBoxFor(expression, selectList, null) as TagBuilder; }
			else
			{ srcTag = htmlHelper.DropDownListFor(expression, selectList, null, null) as TagBuilder; }

			return itemInternal(htmlHelper, srcTag, "checkbox", htmlAttributes);
		}



		private static IHtmlContent itemInternal(IHtmlHelper htmlHelper, TagBuilder srcTag, string type, IDictionary<string, object> htmlAttributes)
		{
			string fullName = srcTag.Attributes["name"];

			var validationAttributes = srcTag.Attributes
				.Where(x => x.Key.StartsWith("data-"))
				.ToDictionary(x => x.Key, x => x.Value);

			var list = HelperUtils.ParseSelectList(srcTag);

			var cb = new HtmlContentBuilder();
			foreach (SelectListItem item in list)
			{
				var input = new TagBuilder("input");
				input.Attributes["type"] = type;
				input.Attributes["name"] = fullName;
				input.Attributes["value"] = item.Value; 

				if (item.Selected) { input.Attributes["checked"] = "checked"; }
				if (item.Disabled) { input.Attributes["disabled"] = "disabled"; }

				var label = new TagBuilder("label");
				label.InnerHtml.AppendHtml(input.RenderSelfClosingTag());
				label.InnerHtml.Append(item.Text);
				label.MergeAttributes(validationAttributes);
				label.MergeAttributes(htmlAttributes);

				cb.AppendHtml(label);
			}

			return cb;
		}
		
		
		
	}

}