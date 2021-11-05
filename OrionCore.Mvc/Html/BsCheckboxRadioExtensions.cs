using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using OrionCore.API;

namespace OrionCore.Mvc.Html
{

	/// <summary></summary>
	public static class BsCheckboxRadioExtensions  
	{


		/// <summary></summary>
		public static IHtmlContent BsBoolRadioList(this IHtmlHelper htmlHelper, string name)
		{
			return BsRadioList(htmlHelper, name, HelperUtils.GetBoolSelectListItem(), null);
		}
		/// <summary></summary>
		public static IHtmlContent BsBoolRadioList(this IHtmlHelper htmlHelper, string name, object htmlAttributes)
		{
			return BsRadioList(htmlHelper, name, HelperUtils.GetBoolSelectListItem(), htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent BsBoolRadioList(this IHtmlHelper htmlHelper, string name, string trueLabel, string falseLabel)
		{
			return BsRadioList(htmlHelper, name, HelperUtils.GetBoolSelectListItem(trueLabel, falseLabel), null);
		}
		/// <summary></summary>
		public static IHtmlContent BsBoolRadioList(this IHtmlHelper htmlHelper, string name, string trueLabel, string falseLabel, object htmlAttributes)
		{
			return BsRadioList(htmlHelper, name, HelperUtils.GetBoolSelectListItem(trueLabel, falseLabel), htmlAttributes);
		}

		/// <summary></summary>
		public static IHtmlContent BsRadioList(this IHtmlHelper htmlHelper, string name, IEnumerable<int> selectList)
		{
			return BsRadioList(htmlHelper, name, HelperUtils.ToSelectListItem(selectList), null);
		}
		/// <summary></summary>
		public static IHtmlContent BsRadioList(this IHtmlHelper htmlHelper, string name, IEnumerable<int> selectList, object htmlAttributes)
		{
			return BsRadioList(htmlHelper, name, HelperUtils.ToSelectListItem(selectList), htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent BsRadioList(this IHtmlHelper htmlHelper, string name, IEnumerable<string> selectList)
		{
			return BsRadioList(htmlHelper, name, HelperUtils.ToSelectListItem(selectList), null);
		}
		/// <summary></summary>
		public static IHtmlContent BsRadioList(this IHtmlHelper htmlHelper, string name, IEnumerable<string> selectList, object htmlAttributes)
		{
			return BsRadioList(htmlHelper, name, HelperUtils.ToSelectListItem(selectList), htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent BsRadioList<K, V>(this IHtmlHelper htmlHelper, string name, IDictionary<K, V> selectList)
		{
			return BsRadioList(htmlHelper, name, HelperUtils.ToSelectListItem(selectList), null);
		}
		/// <summary></summary>
		public static IHtmlContent BsRadioList<K, V>(this IHtmlHelper htmlHelper, string name, IDictionary<K, V> selectList, object htmlAttributes)
		{
			return BsRadioList(htmlHelper, name, HelperUtils.ToSelectListItem(selectList), htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent BsRadioList(this IHtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList)
		{
			return BsRadioList(htmlHelper, name, selectList, null);
		}
		/// <summary></summary>
		public static IHtmlContent BsRadioList(this IHtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList, object htmlAttributes)
		{
			return BsRadioList(htmlHelper, name, selectList, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}


		
		/// <summary></summary>
		public static IHtmlContent BsBoolRadioList(this IHtmlHelper htmlHelper, string name, IDictionary<string, object> htmlAttributes)
		{
			return BsRadioList(htmlHelper, name, HelperUtils.GetBoolSelectListItem(), htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent BsBoolRadioList(this IHtmlHelper htmlHelper, string name, string trueLabel, string falseLabel, IDictionary<string, object> htmlAttributes)
		{
			return BsRadioList(htmlHelper, name, HelperUtils.GetBoolSelectListItem(trueLabel, falseLabel), htmlAttributes);
		}

		/// <summary></summary>
		public static IHtmlContent BsRadioList(this IHtmlHelper htmlHelper, string name, IEnumerable<int> selectList, IDictionary<string, object> htmlAttributes)
		{
			return BsRadioList(htmlHelper, name, HelperUtils.ToSelectListItem(selectList), htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent BsRadioList(this IHtmlHelper htmlHelper, string name, IEnumerable<string> selectList, IDictionary<string, object> htmlAttributes)
		{
			return BsRadioList(htmlHelper, name, HelperUtils.ToSelectListItem(selectList), htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent BsRadioList<K, V>(this IHtmlHelper htmlHelper, string name, IDictionary<K, V> selectList, IDictionary<string, object> htmlAttributes)
		{
			return BsRadioList(htmlHelper, name, HelperUtils.ToSelectListItem(selectList), htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent BsRadioList(this IHtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList, IDictionary<string, object> htmlAttributes)
		{
			if (htmlAttributes == null) { htmlAttributes = new Dictionary<string, object>(); }

			bool isEdit = HelperUtils.IsEditable(htmlAttributes);
			HelperUtils.ClearBoolAttribute(htmlAttributes);
			HelperUtils.StandardAttribute(htmlAttributes);

			IHtmlContent itemHtml;
			var itemAttributes = new Dictionary<string, object> { { "class", "radio-inline" } };

			var srcTag = htmlHelper.DropDownList(name, selectList, null, null) as TagBuilder;

			if (!isEdit)
			{ itemHtml = staticControlHelper(htmlHelper, srcTag, itemAttributes); }
			else
			{ itemHtml = htmlHelper.RadioList(name, selectList, itemAttributes); }

			return generateWidget(name + "RadioList", htmlAttributes, itemHtml);
		}





		/*==================================================== */

		/// <summary></summary>
		public static IHtmlContent BsBoolRadioListFor<TModel, TBool>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TBool>> expression)
		{
			return BsRadioListFor(htmlHelper, expression, HelperUtils.GetBoolSelectListItem(), null);
		}
		/// <summary></summary>
		public static IHtmlContent BsBoolRadioListFor<TModel, TBool>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TBool>> expression, object htmlAttributes)
		{
			return BsRadioListFor(htmlHelper, expression, HelperUtils.GetBoolSelectListItem(), htmlAttributes);
		}

		/// <summary></summary>
		public static IHtmlContent BsBoolRadioListFor<TModel, TBool>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TBool>> expression, string trueLabel, string falseLabel)
		{
			return BsRadioListFor(htmlHelper, expression, HelperUtils.GetBoolSelectListItem(trueLabel, falseLabel), null);
		}
		/// <summary></summary>
		public static IHtmlContent BsBoolRadioListFor<TModel, TBool>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TBool>> expression, string trueLabel, string falseLabel, object htmlAttributes)
		{
			return BsRadioListFor(htmlHelper, expression, HelperUtils.GetBoolSelectListItem(trueLabel, falseLabel), htmlAttributes);
		}

		/// <summary></summary>
		public static IHtmlContent BsEnumRadioListFor<TModel, TEnum>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression)
		{
			IDictionary<string, string> selectList = OrionUtils.EnumToDictionary<TEnum>();
			return BsRadioListFor(htmlHelper, expression, HelperUtils.ToSelectListItem(selectList), null);
		}
		/// <summary></summary>
		public static IHtmlContent BsEnumRadioListFor<TModel, TEnum>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, object htmlAttributes)
		{
			IDictionary<string, string> selectList = OrionUtils.EnumToDictionary<TEnum>();
			return BsRadioListFor(htmlHelper, expression, HelperUtils.ToSelectListItem(selectList), htmlAttributes);
		}

		/// <summary></summary>
		public static IHtmlContent BsRadioListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<int> selectList)
		{
			return BsRadioListFor(htmlHelper, expression, HelperUtils.ToSelectListItem(selectList), null);
		}
		/// <summary></summary>
		public static IHtmlContent BsRadioListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<int> selectList, object htmlAttributes)
		{
			return BsRadioListFor(htmlHelper, expression, HelperUtils.ToSelectListItem(selectList), htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent BsRadioListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<string> selectList)
		{
			return BsRadioListFor(htmlHelper, expression, HelperUtils.ToSelectListItem(selectList), null);
		}
		/// <summary></summary>
		public static IHtmlContent BsRadioListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<string> selectList, object htmlAttributes)
		{
			return BsRadioListFor(htmlHelper, expression, HelperUtils.ToSelectListItem(selectList), htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent BsRadioListFor<TModel, TProperty, K, V>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<K, V> selectList)
		{
			return BsRadioListFor(htmlHelper, expression, HelperUtils.ToSelectListItem(selectList), null);
		}
		/// <summary></summary>
		public static IHtmlContent BsRadioListFor<TModel, TProperty, K, V>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<K, V> selectList, object htmlAttributes)
		{
			return BsRadioListFor(htmlHelper, expression, HelperUtils.ToSelectListItem(selectList), htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent BsRadioListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList)
		{
			return BsRadioListFor(htmlHelper, expression, selectList, HtmlHelper.AnonymousObjectToHtmlAttributes(null));
		}
		/// <summary></summary>
		public static IHtmlContent BsRadioListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, object htmlAttributes)
		{
			return BsRadioListFor(htmlHelper, expression, selectList, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}




		/// <summary></summary>
		public static IHtmlContent BsBoolRadioListFor<TModel, TBool>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TBool>> expression, IDictionary<string, object> htmlAttributes)
		{
			return BsRadioListFor(htmlHelper, expression, HelperUtils.GetBoolSelectListItem(), htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent BsBoolRadioListFor<TModel, TBool>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TBool>> expression, string trueLabel, string falseLabel, IDictionary<string, object> htmlAttributes)
		{
			return BsRadioListFor(htmlHelper, expression, HelperUtils.GetBoolSelectListItem(trueLabel, falseLabel), htmlAttributes);
		}

		/// <summary></summary>
		public static IHtmlContent BsEnumRadioListFor<TModel, TEnum>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, IDictionary<string, object> htmlAttributes)
		{
			IDictionary<string, string> selectList = OrionUtils.EnumToDictionary<TEnum>();
			return BsRadioListFor(htmlHelper, expression, HelperUtils.ToSelectListItem(selectList), htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent BsRadioListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<int> selectList, IDictionary<string, object> htmlAttributes)
		{
			return BsRadioListFor(htmlHelper, expression, HelperUtils.ToSelectListItem(selectList), htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent BsRadioListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<string> selectList, IDictionary<string, object> htmlAttributes)
		{
			return BsRadioListFor(htmlHelper, expression, HelperUtils.ToSelectListItem(selectList), htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent BsRadioListFor<TModel, TProperty, K, V>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<K, V> selectList, IDictionary<string, object> htmlAttributes)
		{
			return BsRadioListFor(htmlHelper, expression, HelperUtils.ToSelectListItem(selectList), htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent BsRadioListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, IDictionary<string, object> htmlAttributes)
		{
			if (htmlAttributes == null) { htmlAttributes = new Dictionary<string, object>(); }

			bool isEdit = HelperUtils.IsEditable(htmlAttributes);
			HelperUtils.ClearBoolAttribute(htmlAttributes);
			HelperUtils.StandardAttribute(htmlAttributes);


			IHtmlContent itemHtml;
			var itemAttributes = new Dictionary<string, object> { { "class", "radio-inline" } };

			var srcTag = htmlHelper.DropDownListFor(expression, selectList, null, null) as TagBuilder;

			if (!isEdit)
			{ itemHtml = staticControlHelper(htmlHelper, srcTag, itemAttributes); }
			else
			{ itemHtml = htmlHelper.RadioListFor(expression, selectList, itemAttributes); }

			string name = srcTag.Attributes["name"];
			return generateWidget(name + "RadioList", htmlAttributes, itemHtml);
		}










		/*==================================================== */




		/// <summary></summary>
		public static IHtmlContent BsCheckboxList(this IHtmlHelper htmlHelper, string name, IEnumerable<int> selectList)
		{
			return BsCheckboxList(htmlHelper, name, HelperUtils.ToSelectListItem(selectList), null);
		}
		/// <summary></summary>
		public static IHtmlContent BsCheckboxList(this IHtmlHelper htmlHelper, string name, IEnumerable<int> selectList, object htmlAttributes)
		{
			return BsCheckboxList(htmlHelper, name, HelperUtils.ToSelectListItem(selectList), htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent BsCheckboxList(this IHtmlHelper htmlHelper, string name, IEnumerable<string> selectList)
		{
			return BsCheckboxList(htmlHelper, name, HelperUtils.ToSelectListItem(selectList), null);
		}
		/// <summary></summary>
		public static IHtmlContent BsCheckboxList(this IHtmlHelper htmlHelper, string name, IEnumerable<string> selectList, object htmlAttributes)
		{
			return BsCheckboxList(htmlHelper, name, HelperUtils.ToSelectListItem(selectList), htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent BsCheckboxList<K, V>(this IHtmlHelper htmlHelper, string name, IDictionary<K, V> selectList)
		{
			return BsCheckboxList(htmlHelper, name, HelperUtils.ToSelectListItem(selectList), null);
		}
		/// <summary></summary>
		public static IHtmlContent BsCheckboxList<K, V>(this IHtmlHelper htmlHelper, string name, IDictionary<K, V> selectList, object htmlAttributes)
		{
			return BsCheckboxList(htmlHelper, name, HelperUtils.ToSelectListItem(selectList), htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent BsCheckboxList(this IHtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList)
		{
			return BsCheckboxList(htmlHelper, name, selectList, null);
		}
		/// <summary></summary>
		public static IHtmlContent BsCheckboxList(this IHtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList, object htmlAttributes)
		{
			return BsCheckboxList(htmlHelper, name, selectList, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}


		/// <summary></summary>
		public static IHtmlContent BsCheckboxList(this IHtmlHelper htmlHelper, string name, IEnumerable<int> selectList, IDictionary<string, object> htmlAttributes)
		{
			return BsCheckboxList(htmlHelper, name, HelperUtils.ToSelectListItem(selectList), htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent BsCheckboxList(this IHtmlHelper htmlHelper, string name, IEnumerable<string> selectList, IDictionary<string, object> htmlAttributes)
		{
			return BsCheckboxList(htmlHelper, name, HelperUtils.ToSelectListItem(selectList), htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent BsCheckboxList<K, V>(this IHtmlHelper htmlHelper, string name, IDictionary<K, V> selectList, IDictionary<string, object> htmlAttributes)
		{
			return BsCheckboxList(htmlHelper, name, HelperUtils.ToSelectListItem(selectList), htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent BsCheckboxList(this IHtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList, IDictionary<string, object> htmlAttributes)
		{
			if (htmlAttributes == null) { htmlAttributes = new Dictionary<string, object>(); }

			bool isEdit = HelperUtils.IsEditable(htmlAttributes);
			HelperUtils.ClearBoolAttribute(htmlAttributes);
			HelperUtils.StandardAttribute(htmlAttributes);


			IHtmlContent itemHtml;
			var itemAttributes = new Dictionary<string, object> { { "class", "checkbox-inline" } };

			var srcTag = htmlHelper.ListBox(name, selectList, null) as TagBuilder;

			if (!isEdit)
			{ itemHtml = staticControlHelper(htmlHelper, srcTag, itemAttributes); }
			else
			{ itemHtml = htmlHelper.CheckboxList(name, selectList, itemAttributes); }

			return generateWidget(name + "CheckboxList", htmlAttributes, itemHtml);
		}







		/*==================================================== */

		/// <summary></summary>
		public static IHtmlContent BsEnumCheckboxListFor<TModel, TEnum>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, IEnumerable<TEnum>>> expression)
		{
			IDictionary<string, string> selectList = OrionUtils.EnumToDictionary<TEnum>();
			return BsCheckboxListFor(htmlHelper, expression, HelperUtils.ToSelectListItem(selectList), null);
		}
		/// <summary></summary>
		public static IHtmlContent BsEnumCheckboxListFor<TModel, TEnum>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, IEnumerable<TEnum>>> expression, object htmlAttributes)
		{
			IDictionary<string, string> selectList = OrionUtils.EnumToDictionary<TEnum>();
			return BsCheckboxListFor(htmlHelper, expression, HelperUtils.ToSelectListItem(selectList), htmlAttributes);
		}

		/// <summary></summary>
		public static IHtmlContent BsCheckboxListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<int> selectList)
		{
			return BsCheckboxListFor(htmlHelper, expression, HelperUtils.ToSelectListItem(selectList), null);
		}
		/// <summary></summary>
		public static IHtmlContent BsCheckboxListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<int> selectList, object htmlAttributes)
		{
			return BsCheckboxListFor(htmlHelper, expression, HelperUtils.ToSelectListItem(selectList), htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent BsCheckboxListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<string> selectList)
		{
			return BsCheckboxListFor(htmlHelper, expression, HelperUtils.ToSelectListItem(selectList), null);
		}
		/// <summary></summary>
		public static IHtmlContent BsCheckboxListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<string> selectList, object htmlAttributes)
		{
			return BsCheckboxListFor(htmlHelper, expression, HelperUtils.ToSelectListItem(selectList), htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent BsCheckboxListFor<TModel, TProperty, K, V>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<K, V> selectList)
		{
			return BsCheckboxListFor(htmlHelper, expression, HelperUtils.ToSelectListItem(selectList), null);
		}
		/// <summary></summary>
		public static IHtmlContent BsCheckboxListFor<TModel, TProperty, K, V>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<K, V> selectList, object htmlAttributes)
		{
			return BsCheckboxListFor(htmlHelper, expression, HelperUtils.ToSelectListItem(selectList), htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent BsCheckboxListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList)
		{
			return BsCheckboxListFor(htmlHelper, expression, selectList, null);
		}
		/// <summary></summary>
		public static IHtmlContent BsCheckboxListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, object htmlAttributes)
		{
			return BsCheckboxListFor(htmlHelper, expression, selectList, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}


		/// <summary></summary>
		public static IHtmlContent BsEnumCheckboxListFor<TModel, TEnum>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, IEnumerable<TEnum>>> expression, IDictionary<string, object> htmlAttributes)
		{
			IDictionary<string, string> selectList = OrionUtils.EnumToDictionary<TEnum>();
			return BsCheckboxListFor(htmlHelper, expression, HelperUtils.ToSelectListItem(selectList), htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent BsCheckboxListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<int> selectList, IDictionary<string, object> htmlAttributes)
		{
			return BsCheckboxListFor(htmlHelper, expression, HelperUtils.ToSelectListItem(selectList), htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent BsCheckboxListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<string> selectList, IDictionary<string, object> htmlAttributes)
		{
			return BsCheckboxListFor(htmlHelper, expression, HelperUtils.ToSelectListItem(selectList), htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent BsCheckboxListFor<TModel, TProperty, K, V>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<K, V> selectList, IDictionary<string, object> htmlAttributes)
		{
			return BsCheckboxListFor(htmlHelper, expression, HelperUtils.ToSelectListItem(selectList), htmlAttributes);
		}
		/// <summary></summary>
		public static IHtmlContent BsCheckboxListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, IDictionary<string, object> htmlAttributes)
		{
			if (htmlAttributes == null) { htmlAttributes = new Dictionary<string, object>(); }

			bool isEdit = HelperUtils.IsEditable(htmlAttributes);
			HelperUtils.ClearBoolAttribute(htmlAttributes);
			HelperUtils.StandardAttribute(htmlAttributes);


			IHtmlContent itemHtml;
			var itemAttributes = new Dictionary<string, object> { { "class", "checkbox-inline" } };

			TagBuilder srcTag;

			Type propType = typeof(TProperty);
			if (propType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(propType))
			{ srcTag = htmlHelper.ListBoxFor(expression, selectList, null) as TagBuilder; }
			else
			{ srcTag = htmlHelper.DropDownListFor(expression, selectList, null, null) as TagBuilder; }
			
			if (!isEdit)
			{ itemHtml = staticControlHelper(htmlHelper, srcTag, itemAttributes); }
			else
			{ itemHtml = htmlHelper.CheckboxListFor(expression, selectList, itemAttributes); }

			string name = srcTag.Attributes["name"];
			return generateWidget(name + "CheckboxList", htmlAttributes, itemHtml);
		}

		



		/*==================================================== */

		private static IHtmlContent generateWidget(string id, IDictionary<string, object> htmlAttributes, IHtmlContent itemHtml)
		{
			var div = new TagBuilder("div");
			div.MergeAttributes(htmlAttributes);
			div.Attributes["id"] = id;
			div.AddCssClass("inline-options");
			div.InnerHtml.AppendHtml(itemHtml);

			return div;
		}



		private static IHtmlContent staticControlHelper(IHtmlHelper htmlHelper, TagBuilder srcTag, Dictionary<string, object> itemAttributes)
		{
			string fullName = srcTag.Attributes["name"];
			var list = HelperUtils.ParseSelectList(srcTag);

			var cb = new HtmlContentBuilder();
			foreach (SelectListItem item in list)
			{
				if (!item.Selected) { continue; }

				var input = new TagBuilder("input");
				input.Attributes["type"] = "hidden";
				input.Attributes["name"] = fullName;
				input.Attributes["value"] = item.Value;

				var label = new TagBuilder("label");
				label.InnerHtml.AppendHtml(input.RenderSelfClosingTag());
				label.InnerHtml.Append(item.Text);
				label.MergeAttributes(itemAttributes);
				label.AddCssClass("editable-fix");

				cb.AppendHtml(label);
			}

			return cb;
		}
		 
		
	}

}