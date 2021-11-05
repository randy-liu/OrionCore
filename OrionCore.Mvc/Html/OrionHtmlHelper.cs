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
using OrionCore.API.Extensions;

namespace OrionCore.Mvc.Html
{
    
    public class OrionHtmlHelper<TModel> : IHtmlHelper<TModel>
    {
        private readonly IHtmlHelper _org;


        public OrionHtmlHelper(IHtmlHelper org, TModel model)
        {
            _org = org;

            ViewData = new ViewDataDictionary<TModel>(org.ViewData, model);
            ViewContext = new ViewContext(org.ViewContext, org.ViewContext.View, ViewData, org.ViewContext.Writer);
        }



        public ViewContext ViewContext { get; private set; }
        public ViewDataDictionary<TModel> ViewData { get; private set; }



        public Html5DateRenderingMode Html5DateRenderingMode
        {
            get => _org.Html5DateRenderingMode;
            set => _org.Html5DateRenderingMode = value;
        }

        public string IdAttributeDotReplacement => _org.IdAttributeDotReplacement;

        public IModelMetadataProvider MetadataProvider => _org.MetadataProvider;

        public dynamic ViewBag => _org.ViewBag;

        public ITempDataDictionary TempData => _org.TempData;

        public UrlEncoder UrlEncoder => _org.UrlEncoder;

        ViewDataDictionary IHtmlHelper.ViewData => _org.ViewData;

        public IHtmlContent ActionLink(string linkText, string actionName, string controllerName, string protocol, string hostname, string fragment, object routeValues, object htmlAttributes)
        {
            return _org.ActionLink(linkText, actionName, controllerName, protocol, hostname, fragment, routeValues, htmlAttributes);
        }

        public IHtmlContent AntiForgeryToken()
        {
            return _org.AntiForgeryToken();
        }

        public MvcForm BeginForm(string actionName, string controllerName, object routeValues, FormMethod method, bool? antiforgery, object htmlAttributes)
        {
            return _org.BeginForm(actionName, controllerName, routeValues, method, antiforgery, htmlAttributes);
        }

        public MvcForm BeginRouteForm(string routeName, object routeValues, FormMethod method, bool? antiforgery, object htmlAttributes)
        {
            return _org.BeginRouteForm(routeName, routeValues, method, antiforgery, htmlAttributes);
        }

        public IHtmlContent CheckBox(string expression, bool? isChecked, object htmlAttributes)
        {
            return _org.CheckBox(expression, isChecked, htmlAttributes);
        }

        public IHtmlContent CheckBoxFor(Expression<Func<TModel, bool>> expression, object htmlAttributes)
        {
            throw new NotImplementedException();
        }


        public IHtmlContent Display(string expression, string templateName, string htmlFieldName, object additionalViewData)
        {
            return _org.Display(expression, templateName, htmlFieldName, additionalViewData);
        }

        public IHtmlContent DisplayFor<TResult>(Expression<Func<TModel, TResult>> expression, string templateName, string htmlFieldName, object additionalViewData)
        {
            throw new NotImplementedException();
        }

        public string DisplayName(string expression)
        {
            return _org.DisplayName(expression);
        }

        public string DisplayNameFor<TResult>(Expression<Func<TModel, TResult>> expression)
        {
            PropertyInfo prop = expression.GetProperty();
            return prop.GetDisplayName();
        }

        public string DisplayNameForInnerType<TModelItem, TResult>(Expression<Func<TModelItem, TResult>> expression)
        {
            throw new NotImplementedException();
        }

        public string DisplayText(string expression)
        {
            return _org.DisplayText(expression);
        }

        public string DisplayTextFor<TResult>(Expression<Func<TModel, TResult>> expression)
        {
            throw new NotImplementedException();
        }

        public IHtmlContent DropDownList(string expression, IEnumerable<SelectListItem> selectList, string optionLabel, object htmlAttributes)
        {
            return _org.DropDownList(expression, selectList, optionLabel, htmlAttributes);
        }

        public IHtmlContent DropDownListFor<TResult>(Expression<Func<TModel, TResult>> expression, IEnumerable<SelectListItem> selectList, string optionLabel, object htmlAttributes)
        {
            throw new NotImplementedException();
        }

        public IHtmlContent Editor(string expression, string templateName, string htmlFieldName, object additionalViewData)
        {
            return _org.Editor(expression, templateName, htmlFieldName, additionalViewData);
        }

        public IHtmlContent EditorFor<TResult>(Expression<Func<TModel, TResult>> expression, string templateName, string htmlFieldName, object additionalViewData)
        {
            throw new NotImplementedException();
        }

        public string Encode(object value)
        {
            return _org.Encode(value);
        }

        public string Encode(string value)
        {
            return _org.Encode(value);
        }

        public void EndForm()
        {
            _org.EndForm();
        }

        public string FormatValue(object value, string format)
        {
            return _org.FormatValue(value, format);
        }

        public string GenerateIdFromName(string fullName)
        {
            return _org.GenerateIdFromName(fullName);
        }

        public IEnumerable<SelectListItem> GetEnumSelectList<TEnum>() where TEnum : struct
        {
            return _org.GetEnumSelectList<TEnum>();
        }

        public IEnumerable<SelectListItem> GetEnumSelectList(Type enumType)
        {
            return _org.GetEnumSelectList(enumType);
        }

        public IHtmlContent Hidden(string expression, object value, object htmlAttributes)
        {
            return _org.Hidden(expression, value, htmlAttributes);
        }

        public IHtmlContent HiddenFor<TResult>(Expression<Func<TModel, TResult>> expression, object htmlAttributes)
        {
            throw new NotImplementedException();
        }

        public string Id(string expression)
        {
            return _org.Id(expression);
        }

        public string IdFor<TResult>(Expression<Func<TModel, TResult>> expression)
        {
            throw new NotImplementedException();
        }

        public IHtmlContent Label(string expression, string labelText, object htmlAttributes)
        {
            return _org.Label(expression, labelText, htmlAttributes);
        }

        public IHtmlContent LabelFor<TResult>(Expression<Func<TModel, TResult>> expression, string labelText, object htmlAttributes)
        {
            throw new NotImplementedException();
        }

        public IHtmlContent ListBox(string expression, IEnumerable<SelectListItem> selectList, object htmlAttributes)
        {
            return _org.ListBox(expression, selectList, htmlAttributes);
        }

        public IHtmlContent ListBoxFor<TResult>(Expression<Func<TModel, TResult>> expression, IEnumerable<SelectListItem> selectList, object htmlAttributes)
        {
            throw new NotImplementedException();
        }

        public string Name(string expression)
        {
            return _org.Name(expression);
        }

        public string NameFor<TResult>(Expression<Func<TModel, TResult>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<IHtmlContent> PartialAsync(string partialViewName, object model, ViewDataDictionary viewData)
        {
            return _org.PartialAsync(partialViewName, model, viewData);
        }

        public IHtmlContent Password(string expression, object value, object htmlAttributes)
        {
            return _org.Password(expression, value, htmlAttributes);
        }

        public IHtmlContent PasswordFor<TResult>(Expression<Func<TModel, TResult>> expression, object htmlAttributes)
        {
            throw new NotImplementedException();
        }

        public IHtmlContent RadioButton(string expression, object value, bool? isChecked, object htmlAttributes)
        {
            return _org.RadioButton(expression, value, isChecked, htmlAttributes);
        }

        public IHtmlContent RadioButtonFor<TResult>(Expression<Func<TModel, TResult>> expression, object value, object htmlAttributes)
        {
            throw new NotImplementedException();
        }

        public IHtmlContent Raw(object value)
        {
            return _org.Raw(value);
        }

        public IHtmlContent Raw(string value)
        {
            return _org.Raw(value);
        }

        public Task RenderPartialAsync(string partialViewName, object model, ViewDataDictionary viewData)
        {
            return _org.RenderPartialAsync(partialViewName, model, viewData);
        }

        public IHtmlContent RouteLink(string linkText, string routeName, string protocol, string hostName, string fragment, object routeValues, object htmlAttributes)
        {
            return _org.RouteLink(linkText, routeName, protocol, hostName, fragment, routeValues, htmlAttributes);
        }

        public IHtmlContent TextArea(string expression, string value, int rows, int columns, object htmlAttributes)
        {
            return _org.TextArea(expression, value, rows, columns, htmlAttributes);
        }

        public IHtmlContent TextAreaFor<TResult>(Expression<Func<TModel, TResult>> expression, int rows, int columns, object htmlAttributes)
        {
            throw new NotImplementedException();
        }

        public IHtmlContent TextBox(string expression, object value, string format, object htmlAttributes)
        {
            return _org.TextBox(expression, value, format, htmlAttributes);
        }

        public IHtmlContent TextBoxFor<TResult>(Expression<Func<TModel, TResult>> expression, string format, object htmlAttributes)
        {
            throw new NotImplementedException();
        }

        public IHtmlContent ValidationMessage(string expression, string message, object htmlAttributes, string tag)
        {
            return _org.ValidationMessage(expression, message, htmlAttributes, tag);
        }

        public IHtmlContent ValidationMessageFor<TResult>(Expression<Func<TModel, TResult>> expression, string message, object htmlAttributes, string tag)
        {
            throw new NotImplementedException();
        }

        public IHtmlContent ValidationSummary(bool excludePropertyErrors, string message, object htmlAttributes, string tag)
        {
            return _org.ValidationSummary(excludePropertyErrors, message, htmlAttributes, tag);
        }

        public string Value(string expression, string format)
        {
            return _org.Value(expression, format);
        }

        public string ValueFor<TResult>(Expression<Func<TModel, TResult>> expression, string format)
        {
            throw new NotImplementedException();
        }
    }
}
