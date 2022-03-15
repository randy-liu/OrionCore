using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Orion.Api.Extensions;

namespace Orion.Mvc.Html
{

    /// <summary></summary>
    public class OrionHtmlHelper<TModel> : IHtmlHelper<TModel>
    {
        private readonly IHtmlHelper _org;


        /// <summary></summary>
        public OrionHtmlHelper(IHtmlHelper org, TModel model)
        {
            _org = org;

            ViewData = new ViewDataDictionary<TModel>(org.ViewData, model);
            ViewContext = new ViewContext(org.ViewContext, org.ViewContext.View, ViewData, org.ViewContext.Writer);
        }



        /// <summary></summary>
        public ViewContext ViewContext { get; private set; }
        /// <summary></summary>
        public ViewDataDictionary<TModel> ViewData { get; private set; }



        /// <summary></summary>
        public Html5DateRenderingMode Html5DateRenderingMode
        {
            get => _org.Html5DateRenderingMode;
            set => _org.Html5DateRenderingMode = value;
        }

        /// <summary></summary>
        public string IdAttributeDotReplacement => _org.IdAttributeDotReplacement;

        /// <summary></summary>
        public IModelMetadataProvider MetadataProvider => _org.MetadataProvider;

        /// <summary></summary>
        public dynamic ViewBag => _org.ViewBag;

        /// <summary></summary>
        public ITempDataDictionary TempData => _org.TempData;

        /// <summary></summary>
        public UrlEncoder UrlEncoder => _org.UrlEncoder;

        /// <summary></summary>
        ViewDataDictionary IHtmlHelper.ViewData => _org.ViewData;

        /// <summary></summary>
        public IHtmlContent ActionLink(string linkText, string actionName, string controllerName, string protocol, string hostname, string fragment, object routeValues, object htmlAttributes)
        {
            return _org.ActionLink(linkText, actionName, controllerName, protocol, hostname, fragment, routeValues, htmlAttributes);
        }

        /// <summary></summary>
        public IHtmlContent AntiForgeryToken()
        {
            return _org.AntiForgeryToken();
        }

        /// <summary></summary>
        public MvcForm BeginForm(string actionName, string controllerName, object routeValues, FormMethod method, bool? antiforgery, object htmlAttributes)
        {
            return _org.BeginForm(actionName, controllerName, routeValues, method, antiforgery, htmlAttributes);
        }

        /// <summary></summary>
        public MvcForm BeginRouteForm(string routeName, object routeValues, FormMethod method, bool? antiforgery, object htmlAttributes)
        {
            return _org.BeginRouteForm(routeName, routeValues, method, antiforgery, htmlAttributes);
        }

        /// <summary></summary>
        public IHtmlContent CheckBox(string expression, bool? isChecked, object htmlAttributes)
        {
            return _org.CheckBox(expression, isChecked, htmlAttributes);
        }

        /// <summary></summary>
        public IHtmlContent CheckBoxFor(Expression<Func<TModel, bool>> expression, object htmlAttributes)
        {
            throw new NotImplementedException();
        }


        /// <summary></summary>
        public IHtmlContent Display(string expression, string templateName, string htmlFieldName, object additionalViewData)
        {
            return _org.Display(expression, templateName, htmlFieldName, additionalViewData);
        }

        /// <summary></summary>
        public IHtmlContent DisplayFor<TResult>(Expression<Func<TModel, TResult>> expression, string templateName, string htmlFieldName, object additionalViewData)
        {
            throw new NotImplementedException();
        }

        /// <summary></summary>
        public string DisplayName(string expression)
        {
            return _org.DisplayName(expression);
        }

        /// <summary></summary>
        public string DisplayNameFor<TResult>(Expression<Func<TModel, TResult>> expression)
        {
            PropertyInfo prop = expression.GetProperty();
            return prop.GetDisplayName();
        }

        /// <summary></summary>
        public string DisplayNameForInnerType<TModelItem, TResult>(Expression<Func<TModelItem, TResult>> expression)
        {
            throw new NotImplementedException();
        }

        /// <summary></summary>
        public string DisplayText(string expression)
        {
            return _org.DisplayText(expression);
        }

        /// <summary></summary>
        public string DisplayTextFor<TResult>(Expression<Func<TModel, TResult>> expression)
        {
            throw new NotImplementedException();
        }

        /// <summary></summary>
        public IHtmlContent DropDownList(string expression, IEnumerable<SelectListItem> selectList, string optionLabel, object htmlAttributes)
        {
            return _org.DropDownList(expression, selectList, optionLabel, htmlAttributes);
        }

        /// <summary></summary>
        public IHtmlContent DropDownListFor<TResult>(Expression<Func<TModel, TResult>> expression, IEnumerable<SelectListItem> selectList, string optionLabel, object htmlAttributes)
        {
            throw new NotImplementedException();
        }

        /// <summary></summary>
        public IHtmlContent Editor(string expression, string templateName, string htmlFieldName, object additionalViewData)
        {
            return _org.Editor(expression, templateName, htmlFieldName, additionalViewData);
        }

        /// <summary></summary>
        public IHtmlContent EditorFor<TResult>(Expression<Func<TModel, TResult>> expression, string templateName, string htmlFieldName, object additionalViewData)
        {
            throw new NotImplementedException();
        }

        /// <summary></summary>
        public string Encode(object value)
        {
            return _org.Encode(value);
        }

        /// <summary></summary>
        public string Encode(string value)
        {
            return _org.Encode(value);
        }

        /// <summary></summary>
        public void EndForm()
        {
            _org.EndForm();
        }

        /// <summary></summary>
        public string FormatValue(object value, string format)
        {
            return _org.FormatValue(value, format);
        }

        /// <summary></summary>
        public string GenerateIdFromName(string fullName)
        {
            return _org.GenerateIdFromName(fullName);
        }

        /// <summary></summary>
        public IEnumerable<SelectListItem> GetEnumSelectList<TEnum>() where TEnum : struct
        {
            return _org.GetEnumSelectList<TEnum>();
        }

        /// <summary></summary>
        public IEnumerable<SelectListItem> GetEnumSelectList(Type enumType)
        {
            return _org.GetEnumSelectList(enumType);
        }

        /// <summary></summary>
        public IHtmlContent Hidden(string expression, object value, object htmlAttributes)
        {
            return _org.Hidden(expression, value, htmlAttributes);
        }

        /// <summary></summary>
        public IHtmlContent HiddenFor<TResult>(Expression<Func<TModel, TResult>> expression, object htmlAttributes)
        {
            throw new NotImplementedException();
        }

        /// <summary></summary>
        public string Id(string expression)
        {
            return _org.Id(expression);
        }

        /// <summary></summary>
        public string IdFor<TResult>(Expression<Func<TModel, TResult>> expression)
        {
            throw new NotImplementedException();
        }

        /// <summary></summary>
        public IHtmlContent Label(string expression, string labelText, object htmlAttributes)
        {
            return _org.Label(expression, labelText, htmlAttributes);
        }

        /// <summary></summary>
        public IHtmlContent LabelFor<TResult>(Expression<Func<TModel, TResult>> expression, string labelText, object htmlAttributes)
        {
            throw new NotImplementedException();
        }

        /// <summary></summary>
        public IHtmlContent ListBox(string expression, IEnumerable<SelectListItem> selectList, object htmlAttributes)
        {
            return _org.ListBox(expression, selectList, htmlAttributes);
        }

        /// <summary></summary>
        public IHtmlContent ListBoxFor<TResult>(Expression<Func<TModel, TResult>> expression, IEnumerable<SelectListItem> selectList, object htmlAttributes)
        {
            throw new NotImplementedException();
        }

        /// <summary></summary>
        public string Name(string expression)
        {
            return _org.Name(expression);
        }

        /// <summary></summary>
        public string NameFor<TResult>(Expression<Func<TModel, TResult>> expression)
        {
            throw new NotImplementedException();
        }

        /// <summary></summary>
        public Task<IHtmlContent> PartialAsync(string partialViewName, object model, ViewDataDictionary viewData)
        {
            return _org.PartialAsync(partialViewName, model, viewData);
        }

        /// <summary></summary>
        public IHtmlContent Password(string expression, object value, object htmlAttributes)
        {
            return _org.Password(expression, value, htmlAttributes);
        }

        /// <summary></summary>
        public IHtmlContent PasswordFor<TResult>(Expression<Func<TModel, TResult>> expression, object htmlAttributes)
        {
            throw new NotImplementedException();
        }

        /// <summary></summary>
        public IHtmlContent RadioButton(string expression, object value, bool? isChecked, object htmlAttributes)
        {
            return _org.RadioButton(expression, value, isChecked, htmlAttributes);
        }

        /// <summary></summary>
        public IHtmlContent RadioButtonFor<TResult>(Expression<Func<TModel, TResult>> expression, object value, object htmlAttributes)
        {
            throw new NotImplementedException();
        }

        /// <summary></summary>
        public IHtmlContent Raw(object value)
        {
            return _org.Raw(value);
        }

        /// <summary></summary>
        public IHtmlContent Raw(string value)
        {
            return _org.Raw(value);
        }

        /// <summary></summary>
        public Task RenderPartialAsync(string partialViewName, object model, ViewDataDictionary viewData)
        {
            return _org.RenderPartialAsync(partialViewName, model, viewData);
        }

        /// <summary></summary>
        public IHtmlContent RouteLink(string linkText, string routeName, string protocol, string hostName, string fragment, object routeValues, object htmlAttributes)
        {
            return _org.RouteLink(linkText, routeName, protocol, hostName, fragment, routeValues, htmlAttributes);
        }

        /// <summary></summary>
        public IHtmlContent TextArea(string expression, string value, int rows, int columns, object htmlAttributes)
        {
            return _org.TextArea(expression, value, rows, columns, htmlAttributes);
        }

        /// <summary></summary>
        public IHtmlContent TextAreaFor<TResult>(Expression<Func<TModel, TResult>> expression, int rows, int columns, object htmlAttributes)
        {
            throw new NotImplementedException();
        }

        /// <summary></summary>
        public IHtmlContent TextBox(string expression, object value, string format, object htmlAttributes)
        {
            return _org.TextBox(expression, value, format, htmlAttributes);
        }

        /// <summary></summary>
        public IHtmlContent TextBoxFor<TResult>(Expression<Func<TModel, TResult>> expression, string format, object htmlAttributes)
        {
            throw new NotImplementedException();
        }

        /// <summary></summary>
        public IHtmlContent ValidationMessage(string expression, string message, object htmlAttributes, string tag)
        {
            return _org.ValidationMessage(expression, message, htmlAttributes, tag);
        }

        /// <summary></summary>
        public IHtmlContent ValidationMessageFor<TResult>(Expression<Func<TModel, TResult>> expression, string message, object htmlAttributes, string tag)
        {
            throw new NotImplementedException();
        }

        /// <summary></summary>
        public IHtmlContent ValidationSummary(bool excludePropertyErrors, string message, object htmlAttributes, string tag)
        {
            return _org.ValidationSummary(excludePropertyErrors, message, htmlAttributes, tag);
        }

        /// <summary></summary>
        public string Value(string expression, string format)
        {
            return _org.Value(expression, format);
        }

        /// <summary></summary>
        public string ValueFor<TResult>(Expression<Func<TModel, TResult>> expression, string format)
        {
            throw new NotImplementedException();
        }
    }
}
