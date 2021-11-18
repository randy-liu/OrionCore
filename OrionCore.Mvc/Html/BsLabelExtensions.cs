using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Orion.Api.Extensions;

namespace Orion.Mvc.Html
{

    /// <summary></summary>
    public static class BsLabelExtensions
    {


        /// <summary></summary>
        public static IHtmlContent BsLabel(this IHtmlHelper htmlHelper, string expression, string labelText = null)
        {
            return BsLabel(htmlHelper, expression, labelText, null);
        }
        /// <summary></summary>
        public static IHtmlContent BsLabel(this IHtmlHelper htmlHelper, string expression, object htmlAttributes)
        {
            return BsLabel(htmlHelper, expression, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }
        /// <summary></summary>
        public static IHtmlContent BsLabel(this IHtmlHelper htmlHelper, string expression, IDictionary<string, object> htmlAttributes)
        {
            return BsLabel(htmlHelper, expression, null, htmlAttributes);
        }
        /// <summary></summary>
        public static IHtmlContent BsLabel(this IHtmlHelper htmlHelper, string expression, string labelText, object htmlAttributes)
        {
            return BsLabel(htmlHelper, expression, labelText, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }
        /// <summary></summary>
        public static IHtmlContent BsLabel(this IHtmlHelper htmlHelper, string expression, string labelText, IDictionary<string, object> htmlAttributes)
        {
            if (htmlAttributes == null) { htmlAttributes = new Dictionary<string, object>(); }

            HelperUtils.AddCssClass(htmlAttributes, "control-label");
            return htmlHelper.Label(expression, labelText, htmlAttributes);
        }



        /*==================================================== */

        /// <summary></summary>
        public static IHtmlContent BsLabel<TEnum>(this IHtmlHelper htmlHelper, TEnum expression) where TEnum : struct, Enum
        {
            return BsLabel(htmlHelper, expression, null);
        }
        /// <summary></summary>
        public static IHtmlContent BsLabel<TEnum>(this IHtmlHelper htmlHelper, TEnum expression, object htmlAttributes) where TEnum : struct, Enum
        {
            return BsLabel(htmlHelper, expression, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }
        /// <summary></summary>
        public static IHtmlContent BsLabel<TEnum>(this IHtmlHelper htmlHelper, TEnum expression, IDictionary<string, object> htmlAttributes) where TEnum : struct, Enum
        {
            if (htmlAttributes == null) { htmlAttributes = new Dictionary<string, object>(); }

            HelperUtils.AddCssClass(htmlAttributes, "control-label");
            string labelText = expression.GetDisplayName();
            return htmlHelper.Label(expression.ToString(), labelText, htmlAttributes);
        }




        /*==================================================== */

        /// <summary></summary>
        public static IHtmlContent BsLabelFor<TModel, TValue>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, string labelText = null)
        {
            return BsLabelFor(htmlHelper, expression, labelText, null);
        }
        /// <summary></summary>
        public static IHtmlContent BsLabelFor<TModel, TValue>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, object htmlAttributes)
        {
            return BsLabelFor(htmlHelper, expression, null, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }
        /// <summary></summary>
        public static IHtmlContent BsLabelFor<TModel, TValue>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, IDictionary<string, object> htmlAttributes)
        {
            return BsLabelFor(htmlHelper, expression, null, htmlAttributes);
        }
        /// <summary></summary>
        public static IHtmlContent BsLabelFor<TModel, TValue>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, string labelText, object htmlAttributes)
        {
            return BsLabelFor(htmlHelper, expression, labelText, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }
        /// <summary></summary>
        public static IHtmlContent BsLabelFor<TModel, TValue>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, string labelText, IDictionary<string, object> htmlAttributes)
        {
            if (htmlAttributes == null) { htmlAttributes = new Dictionary<string, object>(); }

            HelperUtils.AddCssClass(htmlAttributes, "control-label");
            return htmlHelper.LabelFor(expression, labelText, htmlAttributes);
        }



        /*==================================================== */

        /// <summary></summary>
        public static IHtmlContent BsLabelForModel(this IHtmlHelper htmlHelper, string labelText = null)
        {
            return BsLabelForModel(htmlHelper, labelText, null);
        }
        /// <summary></summary>
        public static IHtmlContent BsLabelForModel(this IHtmlHelper htmlHelper, object htmlAttributes)
        {
            return BsLabelForModel(htmlHelper, null, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }
        /// <summary></summary>
        public static IHtmlContent BsLabelForModel(this IHtmlHelper htmlHelper, IDictionary<string, object> htmlAttributes)
        {
            return BsLabelForModel(htmlHelper, null, htmlAttributes);
        }
        /// <summary></summary>
        public static IHtmlContent BsLabelForModel(this IHtmlHelper htmlHelper, string labelText, object htmlAttributes)
        {
            return BsLabelForModel(htmlHelper, labelText, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }
        /// <summary></summary>
        public static IHtmlContent BsLabelForModel(this IHtmlHelper htmlHelper, string labelText, IDictionary<string, object> htmlAttributes)
        {
            if (htmlAttributes == null) { htmlAttributes = new Dictionary<string, object>(); }

            HelperUtils.AddCssClass(htmlAttributes, "control-label");
            return htmlHelper.LabelForModel(labelText, htmlAttributes);
        }

    }

}
