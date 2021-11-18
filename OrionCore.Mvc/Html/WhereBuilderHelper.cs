using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Orion.Api;
using Orion.Api.Extensions;
using Orion.Api.Models;

namespace Orion.Mvc.Html
{



    /// <summary></summary>
    public static class WhereBuilderHelperExtensions
    {
        /// <summary></summary>
        public static WhereBuilderHelper<TDomain> WhereBuilder<TDomain>(this IHtmlHelper helper)
        {
            return new WhereBuilderHelper<TDomain>(helper.ViewContext.Writer);
        }

        /// <summary></summary>
        public static WhereBuilderHelper<TDomain> WhereBuilder<TDomain>(this IHtmlHelper helper, List<TDomain> pagination)
        {
            return new WhereBuilderHelper<TDomain>(helper.ViewContext.Writer);
        }

        /// <summary></summary>
        public static WhereBuilderHelper<TDomain> WhereBuilder<TDomain>(this IHtmlHelper helper, Pagination<TDomain> pagination)
        {
            return new WhereBuilderHelper<TDomain>(helper.ViewContext.Writer);
        }


    }




    /// <summary></summary>
    public class WhereBuilderHelper<TDomain>
    {

        private readonly TextWriter _writer;
        private readonly TagBuilder _selectTag;

        private int _dividerSeq = 1;




        /// <summary></summary>
        public WhereBuilderHelper(TextWriter writer)
        {
            _writer = writer;

            _selectTag = new TagBuilder("select");
            _selectTag.Attributes["class"] = "where-builder hidden";
            _selectTag.InnerHtml.AppendHtml("\n");

        }




        /// <summary></summary>
        public WhereBuilderHelper<TDomain> Divider()
        {
            return Column($"divider{_dividerSeq++}", "divider", "");
        }



        /// <summary></summary>
        public WhereBuilderHelper<TDomain> Column(string column, string type, string label)
        {
            var option = new TagBuilder("option");
            option.Attributes["value"] = column;
            option.Attributes["type"] = type;
            option.InnerHtml.Append(label);

            _selectTag.InnerHtml.AppendHtml(option);
            _selectTag.InnerHtml.AppendHtml("\n");

            return this;
        }


        /// <summary></summary>
        public WhereBuilderHelper<TDomain> Column<TProp>(Expression<Func<TDomain, TProp>> expression, string type, string label)
        {
            PropertyInfo prop = expression.GetProperty();
            return Column(prop.Name, type, label ?? prop.GetDisplayName());
        }



        /// <summary></summary>
        public WhereBuilderHelper<TDomain> Column(Expression<Func<TDomain, bool?>> expression, string label = null)
        {
            return Column(expression, "bool", label);
        }
        /// <summary></summary>
        public WhereBuilderHelper<TDomain> Column(Expression<Func<TDomain, short?>> expression, string label = null)
        {
            return Column(expression, "short", label);
        }
        /// <summary></summary>
        public WhereBuilderHelper<TDomain> Column(Expression<Func<TDomain, int?>> expression, string label = null)
        {
            return Column(expression, "int", label);
        }
        /// <summary></summary>
        public WhereBuilderHelper<TDomain> Column(Expression<Func<TDomain, long?>> expression, string label = null)
        {
            return Column(expression, "long", label);
        }


        /// <summary></summary>
        public WhereBuilderHelper<TDomain> Column(Expression<Func<TDomain, float?>> expression, string label = null)
        {
            return Column(expression, "float", label);
        }
        /// <summary></summary>
        public WhereBuilderHelper<TDomain> Column(Expression<Func<TDomain, double?>> expression, string label = null)
        {
            return Column(expression, "double", label);
        }
        /// <summary></summary>
        public WhereBuilderHelper<TDomain> Column(Expression<Func<TDomain, decimal?>> expression, string label = null)
        {
            return Column(expression, "decimal", label);
        }


        /// <summary></summary>
        public WhereBuilderHelper<TDomain> Column(Expression<Func<TDomain, TimeSpan?>> expression, string label = null)
        {
            return Column(expression, "time", label);
        }


        /// <summary></summary>
        public WhereBuilderHelper<TDomain> Column(Expression<Func<TDomain, DateTime?>> expression, string label = null)
        {
            string expStr = expression.ToString();

            var type = "datetime";
            if (Regex.IsMatch(expStr, @"\.Date\b")) { type = "date"; }

            return Column(expression, type, label);
        }


        /// <summary></summary>
        public WhereBuilderHelper<TDomain> Column(Expression<Func<TDomain, DateTimeOffset?>> expression, string label = null)
        {
            string expStr = expression.ToString();

            var type = "datetime";
            if (Regex.IsMatch(expStr, @"\.Date\b")) { type = "date"; }

            return Column(expression, type, label);
        }


        /// <summary></summary>
        public WhereBuilderHelper<TDomain> Column(Expression<Func<TDomain, Enum>> expression, string label = null)
        {
            PropertyInfo prop = expression.GetProperty();
            Dictionary<string, string> items = OrionUtils.EnumToDictionary(prop.PropertyType);

            return Column(expression, items.ToJson(), label);
        }


        /// <summary></summary>
        public WhereBuilderHelper<TDomain> Column(Expression<Func<TDomain, string>> expression, string label = null)
        {
            string expStr = expression.ToString();

            var type = "string";
            if (expStr.Contains(".ToUpper(")) { type = "upper"; }

            return Column(expression, type, label);
        }



        /// <summary></summary>
        public WhereBuilderHelper<TDomain> Column<K, V>(Expression<Func<TDomain, K>> expression, IDictionary<K, V> selectList, string label = null)
        {
            return Column(expression, selectList.ToJson(), label);
        }
        /// <summary></summary>
        public WhereBuilderHelper<TDomain> Column<K, V>(Expression<Func<TDomain, IEnumerable<K>>> expression, IDictionary<K, V> selectList, string label = null)
        {
            return Column(expression, selectList.ToJson(), label);
        }
        /// <summary></summary>
        public WhereBuilderHelper<TDomain> Column<K, V>(Expression<Func<TDomain, K?>> expression, IDictionary<K, V> selectList, string label = null) where K : struct
        {
            return Column(expression, selectList.ToJson(), label);
        }

        /// <summary></summary>
        public WhereBuilderHelper<TDomain> Column<K>(Expression<Func<TDomain, K>> expression, IEnumerable<K> selectList, string label = null)
        {
            return Column(expression, selectList.ToJson(), label);
        }
        /// <summary></summary>
        public WhereBuilderHelper<TDomain> Column<K>(Expression<Func<TDomain, IEnumerable<K>>> expression, IEnumerable<K> selectList, string label = null)
        {
            return Column(expression, selectList.ToJson(), label);
        }
        /// <summary></summary>
        public WhereBuilderHelper<TDomain> Column<K>(Expression<Func<TDomain, K?>> expression, IEnumerable<K> selectList, string label = null) where K : struct
        {
            return Column(expression, selectList.ToJson(), label);
        }




        /// <summary></summary>
        public void Render()
        {
            _selectTag.WriteTo(_writer, HtmlEncoder.Default);
        }


        /// <summary>
        /// Renders to the TextWriter, and returns null. 
        /// This is by design so that it can be used with inline syntax in views.
        /// </summary>
        public override string ToString()
        {
            Render();
            return null;
        }

    }
}