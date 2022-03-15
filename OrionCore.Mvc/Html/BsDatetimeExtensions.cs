using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Orion.Mvc.Html
{
    /// <summary></summary>
    public static class BsDatetimeExtensions
    {


        /// <summary></summary>
        public static IHtmlContent BsDateBox(this IHtmlHelper htmlHelper, string name, object value = null)
        {
            return htmlHelper.BsDateBox(name, value, null);
        }
        /// <summary></summary>
        public static IHtmlContent BsDateBox(this IHtmlHelper htmlHelper, string name, object value, object htmlAttributes = null)
        {
            return htmlHelper.BsDateBox(name, value, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }
        /// <summary></summary>
        public static IHtmlContent BsDateBox(this IHtmlHelper htmlHelper, string name, object value, IDictionary<string, object> htmlAttributes)
        {
            if (htmlAttributes == null) { htmlAttributes = new Dictionary<string, object>(); }
            htmlAttributes["ext-picker"] = HelperUtils.IsTaiwanCalendar ? "tw-date" : "date";

            return htmlHelper.BsTextBox(name, value, "{0:d}", htmlAttributes);
        }



        /// <summary></summary>
        public static IHtmlContent BsDateBoxFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            return htmlHelper.BsDateBoxFor(expression, null);
        }
        /// <summary></summary>
        public static IHtmlContent BsDateBoxFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
        {
            return htmlHelper.BsDateBoxFor(expression, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }
        /// <summary></summary>
        public static IHtmlContent BsDateBoxFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes)
        {
            if (htmlAttributes == null) { htmlAttributes = new Dictionary<string, object>(); }
            htmlAttributes["ext-picker"] = HelperUtils.IsTaiwanCalendar ? "tw-date" : "date";
            return htmlHelper.BsTextBoxFor(expression, "{0:d}", htmlAttributes);
        }




        /*==================================================== */

        /// <summary></summary>
        public static IHtmlContent MBsDateBox(this IHtmlHelper htmlHelper, string name, object value = null)
        {
            return htmlHelper.MBsDateBox(name, value, null);
        }
        /// <summary></summary>
        public static IHtmlContent MBsDateBox(this IHtmlHelper htmlHelper, string name, object value, object htmlAttributes = null)
        {
            return htmlHelper.MBsDateBox(name, value, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }
        /// <summary></summary>
        public static IHtmlContent MBsDateBox(this IHtmlHelper htmlHelper, string name, object value, IDictionary<string, object> htmlAttributes)
        {
            var input = htmlHelper.BsTextBox(name, value, "{0:d}", new Dictionary<string, object>
            {
                ["ext-picker"] = "date",
                ["readonly"] = "readonly",
                ["style"] = "background: #fff;",
            });
            return warpDateBox(input, htmlAttributes);
        }



        /// <summary></summary>
        public static IHtmlContent MBsDateBoxFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            return htmlHelper.MBsDateBoxFor(expression, null);
        }
        /// <summary></summary>
        public static IHtmlContent MBsDateBoxFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
        {
            return htmlHelper.MBsDateBoxFor(expression, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }
        /// <summary></summary>
        public static IHtmlContent MBsDateBoxFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes)
        {
            var input = htmlHelper.BsTextBoxFor(expression, "{0:d}", new Dictionary<string, object>
            {
                ["ext-picker"] = "date",
                ["readonly"] = "readonly",
                ["style"] = "background: #fff;",
            });
            return warpDateBox(input, htmlAttributes);
        }


        /// <summary></summary>
        private static IHtmlContent warpDateBox(IHtmlContent input, IDictionary<string, object> htmlAttributes)
        {
            /*
				<div class="input-group">
					<input readonly="" type="text" class="form-control" />
					<span class="input-group-btn" onclick="$(this).prev().val('')"><b class="btn btn-default"><i class="fa fa-times"></i></b></span>
				</div> 
			*/

            var i = new TagBuilder("i");
            i.AddCssClass("fa fa-times");

            var b = new TagBuilder("b");
            b.AddCssClass("btn btn-default");
            b.InnerHtml.AppendHtml(i);

            var span = new TagBuilder("span");
            span.AddCssClass("input-group-btn");
            span.Attributes["onclick"] = "$(this).prev().val('').trigger('change')";
            span.InnerHtml.AppendHtml(b);

            var div = new TagBuilder("div");
            div.MergeAttributes(htmlAttributes);
            div.AddCssClass("input-group date-group");
            div.InnerHtml.AppendHtml(input);
            div.InnerHtml.AppendHtml(span);

            return div;
        }




        /*==================================================== */






        /// <summary></summary>
        public static IHtmlContent BsDateTimeBox(this IHtmlHelper htmlHelper, string name, object value = null)
        {
            return htmlHelper.BsDateTimeBox(name, value, null);
        }
        /// <summary></summary>
        public static IHtmlContent BsDateTimeBox(this IHtmlHelper htmlHelper, string name, object value, object htmlAttributes = null)
        {
            return htmlHelper.BsDateTimeBox(name, value, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }
        /// <summary></summary>
        public static IHtmlContent BsDateTimeBox(this IHtmlHelper htmlHelper, string name, object value, IDictionary<string, object> htmlAttributes)
        {
            if (htmlAttributes == null) { htmlAttributes = new Dictionary<string, object>(); }
            htmlAttributes["ext-picker"] = HelperUtils.IsTaiwanCalendar ? "tw-datetime" : "datetime";
            return htmlHelper.BsTextBox(name, value, "{0:f}", htmlAttributes);
        }



        /// <summary></summary>
        public static IHtmlContent BsDateTimeBoxFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            return htmlHelper.BsDateTimeBoxFor(expression, null);
        }
        /// <summary></summary>
        public static IHtmlContent BsDateTimeBoxFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
        {
            return htmlHelper.BsDateTimeBoxFor(expression, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }
        /// <summary></summary>
        public static IHtmlContent BsDateTimeBoxFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes)
        {
            if (htmlAttributes == null) { htmlAttributes = new Dictionary<string, object>(); }
            htmlAttributes["ext-picker"] = HelperUtils.IsTaiwanCalendar ? "tw-datetime" : "datetime";
            return htmlHelper.BsTextBoxFor(expression, "{0:f}", htmlAttributes);
        }



        /*==================================================== */

        /// <summary></summary>
        public static IHtmlContent BsTimeBox(this IHtmlHelper htmlHelper, string name, object value = null)
        {
            return htmlHelper.BsTimeBox(name, value, null);
        }
        /// <summary></summary>
        public static IHtmlContent BsTimeBox(this IHtmlHelper htmlHelper, string name, object value, object htmlAttributes = null)
        {
            return htmlHelper.BsTimeBox(name, value, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }
        /// <summary></summary>
        public static IHtmlContent BsTimeBox(this IHtmlHelper htmlHelper, string name, object value, IDictionary<string, object> htmlAttributes)
        {
            if (htmlAttributes == null) { htmlAttributes = new Dictionary<string, object>(); }
            htmlAttributes["ext-picker"] = "time";
            return htmlHelper.BsTextBox(name, value, null, htmlAttributes);
        }



        /// <summary></summary>
        public static IHtmlContent BsTimeBoxFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            return htmlHelper.BsTimeBoxFor(expression, null);
        }
        /// <summary></summary>
        public static IHtmlContent BsTimeBoxFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
        {
            return htmlHelper.BsTimeBoxFor(expression, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }
        /// <summary></summary>
        public static IHtmlContent BsTimeBoxFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes)
        {
            var type = typeof(TProperty);
            var nType = Nullable.GetUnderlyingType(type);
            if (nType != null) { type = nType; }

            if (htmlAttributes == null) { htmlAttributes = new Dictionary<string, object>(); }
            htmlAttributes["ext-picker"] = "time";

            string format = (type == typeof(DateTime) || type == typeof(DateTimeOffset) ? "{0:t}" : null);
            return htmlHelper.BsTextBoxFor(expression, format, htmlAttributes);
        }






    }
}
