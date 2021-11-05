using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;
using OrionCore.API;
using OrionCore.API.Extensions;
using OrionCore.API.Models;
using OrionCore.Mvc.Extensions;


namespace OrionCore.Mvc.ModelBinder
{
    /// <summary></summary>
    public class WhereParamsModelBinderProvider : IModelBinderProvider
    {
        private readonly Type _type = typeof(IWhereParams);
        private readonly WhereParamsModelBinder _binder = new WhereParamsModelBinder();


        /// <summary></summary>
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            return _type.IsAssignableFrom(context.Metadata.ModelType) ? _binder : null;
        }
    }





    /*###################################################################*/

    /// <summary></summary>
    public class WhereParamsModelBinder : IModelBinder
    {

        /// <summary></summary>
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null) { throw new ArgumentNullException(nameof(bindingContext)); }

            var requestParams = new NameValueCollection();

            if (bindingContext.HttpContext.Request.IsPostMethod())
            { requestParams.Add(toNameValueCollection(bindingContext.HttpContext.Request.Form)); }

            requestParams.Add(toNameValueCollection(bindingContext.HttpContext.Request.Query));

            Type type = bindingContext.ModelType.GetGenericArguments()[0];
            object model = CreateWhereParams(type, requestParams, bindingContext.ModelState);
            bindingContext.Result = ModelBindingResult.Success(model);

            return Task.CompletedTask;
        }


        private NameValueCollection toNameValueCollection(IEnumerable<KeyValuePair<string, StringValues>> source)
        {
            var result = new NameValueCollection();

            foreach (var pair in source)
                foreach (var value in pair.Value)
                {
                    result.Add(pair.Key, value);
                }
            return result;
        }


        /// <summary></summary>
        public object CreateWhereParams(Type type, NameValueCollection data, ModelStateDictionary modelState)
        {
            Type makeme = typeof(WhereParams<>).MakeGenericType(type);
            var param = (IWhereParams)Activator.CreateInstance(makeme);

            foreach (var prop in type.GetProperties())
            {
                string strValue = data.GetValues(prop.Name)?.FirstOrDefault();
                if (string.IsNullOrWhiteSpace(strValue)) { continue; }

                Type propType = prop.PropertyType;
                if (propType.IsArray)
                {
                    propType = propType.GetElementType();
                }
                else if (propType.IsGenericType && typeof(IEnumerable).IsAssignableFrom(propType))
                {
                    propType = propType.GenericTypeArguments.First();
                }


                try
                {
                    var res = parseStringValue(propType, strValue.Trim());
                    if (res.Values.Length == 0) { continue; }

                    param.SetValues(prop.Name, res.Operator, res.Values);
                }
                catch (InvalidCastException ex)
                {
                    modelState.AddModelError(nameof(WhereParams) + "." + prop.Name, ex.Message);
                }
            }


            return param;
        }




        private ParseResult parseStringValue(Type propType, string strValue)
        {
            var result = new ParseResult
            {
                Operator = WhereOperator.Undefined
            };



            /* Between */
            if (strValue.Contains(".."))
            {
                var split = strValue.Split(new string[] { ".." }, StringSplitOptions.None);

                if (string.IsNullOrWhiteSpace(split[0]))
                {
                    result.Operator = WhereOperator.LessEquals;
                    result.Values = convertTo(propType, split[1]);
                }
                else if (string.IsNullOrWhiteSpace(split[1]))
                {
                    result.Operator = WhereOperator.GreaterEquals;
                    result.Values = convertTo(propType, split[0]);
                }
                else
                {
                    result.Operator = WhereOperator.Between;
                    result.Values = convertTo(propType, split);
                }

                return result;
            }



            /* IN */
            if (strValue.Contains("|"))
            {
                if (strValue.StartsWith("!"))
                {
                    result.Operator = WhereOperator.NotIn;
                    strValue = strValue.Substring(1);
                }
                else
                {
                    result.Operator = WhereOperator.In;
                }

                result.Values = convertTo(propType, strValue.Split('|'));
                return result;

            }



            /* 三字運算符 */
            if (strValue.Length >= 3)
            {
                switch (strValue.Substring(0, 3))
                {
                    case "^!=": result.Operator = WhereOperator.NotStartsWith; break;
                    case "$!=": result.Operator = WhereOperator.NotEndsWith; break;
                    case "*!=": result.Operator = WhereOperator.NotContains; break;
                }

                if (result.Operator != WhereOperator.Undefined)
                {
                    result.Values = convertTo(propType, strValue.Substring(3));
                    return result;
                }
            }



            /* 雙字運算符 */
            if (strValue.Length >= 2)
            {
                switch (strValue.Substring(0, 2))
                {
                    case "<=": result.Operator = WhereOperator.LessEquals; break;
                    case ">=": result.Operator = WhereOperator.GreaterEquals; break;
                    case "!=": result.Operator = WhereOperator.NotEquals; break;
                    case "^=": result.Operator = WhereOperator.StartsWith; break;
                    case "$=": result.Operator = WhereOperator.EndsWith; break;
                    case "*=": result.Operator = WhereOperator.Contains; break;
                }

                if (result.Operator != WhereOperator.Undefined)
                {
                    result.Values = convertTo(propType, strValue.Substring(2));
                    return result;
                }
            }



            /* 單字運算符 */
            switch (strValue.Substring(0, 1))
            {
                case "=": result.Operator = WhereOperator.Equals; break;
                case "<": result.Operator = WhereOperator.LessThan; break;
                case ">": result.Operator = WhereOperator.GreaterThan; break;
            }

            if (result.Operator != WhereOperator.Undefined)
            {
                result.Values = convertTo(propType, strValue.Substring(1));
                return result;
            }



            result.Operator = WhereOperator.Equals;
            result.Values = convertTo(propType, strValue);
            return result;
        }




        private object[] convertTo(Type propType, params string[] strValues)
        {
            return strValues
                .Where(x => x.HasText())
                .Select(x =>
                {
                    object result = OrionUtils.ConvertType(x, propType);
                    if (result == null) { throw new InvalidCastException(x + " 不是有效的查詢條件"); }
                    return result;
                })
                .ToArray();
        }



        class ParseResult
        {
            public WhereOperator Operator { get; set; }
            public object[] Values { get; set; }
        }



    }
}
