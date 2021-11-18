using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Web;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Orion.Api.Extensions;

namespace Orion.Mvc.Html
{

    /// <summary></summary>
    public static class HtmlExcelExportExtensions
	{
		/// <summary></summary>
		public static HtmlExcelExport<T> ExcelExport<T>(this IHtmlHelper helper, IEnumerable<T> dataSource) where T : class
		{
			return new HtmlExcelExport<T>(helper.ViewContext, dataSource);
		}
		
	}


	/// <summary></summary>
	public class HtmlExcelExport<T> where T : class
	{

		private readonly TextWriter _writer;
		private IEnumerable<T> _dataSource;
		private List<string> _columnOrder = new List<string>();
		private List<string> _settingOrder = new List<string>();
		private Dictionary<string, string> _columnHeaders = new Dictionary<string, string>();
		private Dictionary<string, Delegate> _columnGetters = new Dictionary<string, Delegate>();

		private string _headerStyle = "color: #fff; background: #2d6da3;";

		public ViewContext ViewContext { get; private set; }
		public HttpContext HttpContext { get; private set; }

		/// <summary></summary>
		public HtmlExcelExport(ViewContext context, IEnumerable<T> dataSource)
		{
			ViewContext = context;
			HttpContext = context.HttpContext;
			_writer = context.Writer;
			_dataSource = dataSource;

			foreach (PropertyInfo prop in typeof(T).GetProperties().Where(x => x.CanRead))
			{
				_columnHeaders[prop.Name] = prop.GetDisplayName() ?? prop.Name;
				_columnGetters[prop.Name] = (Func<T, object>)(x => prop.GetValue(x));
			}
		}


		/// <summary></summary>
		public HtmlExcelExport<T> HeaderStyle(string cssStyle)
		{
			_headerStyle = cssStyle;
			return this;
		}


		/// <summary></summary>
		public HtmlExcelExport<T> ColumnOrder(IEnumerable<string> columnList)
		{
			_columnOrder = columnList.ToList();
			return this;
		}
		


		/// <summary></summary>
		public HtmlExcelExport<T> Column<TProp>(Expression<Func<T, TProp>> expression, string headerLabel = null)
		{
			string name = getPropertyName(expression);
			Column(name, headerLabel, expression.Compile());
			return this;
		}


		/// <summary></summary>
		public HtmlExcelExport<T> Column<TProp>(string columnName, string headerLabel, Func<T, TProp> func)
		{
			_settingOrder.Add(columnName);

			_columnGetters[columnName] = func;
			if (headerLabel != null) { _columnHeaders[columnName] = headerLabel; }
			return this;
		}



		/// <summary>取得欄位顯示順序</summary>
		private List<string> getColumnNames()
		{
			if (_columnOrder.Any()) { return _columnOrder; }
			if (_settingOrder.Any()) { return _settingOrder; }

			return _columnGetters.Keys.ToList(); 
		}


		private string getColumnTypeMark(string column)
		{
			Type type = typeof(T).GetProperty(column)?.PropertyType;
			if(type == null) { return ""; }

			string mark = "";
			if (type == typeof(string)) { mark = "text"; }

			return mark;
		}



		/// <summary></summary>
		public void Render()
		{
			List<string> columns = getColumnNames();

			Action<T> writeRow = (x => { });

			foreach (string column in columns)
			{
				if (!_columnGetters.ContainsKey(column)) { continue; }

				string mark = getColumnTypeMark(column);
				Delegate columnGetter = _columnGetters[column];

				writeRow += (x => {
					writeHtml($"<td class=\"{mark}\">");
					writeSecure(columnGetter.DynamicInvoke(x));
					writeHtml("</td>");
				});
			}
			

			writeHtml("<table>");
			writeHtml("<tr class=\"head\">");

			foreach (var column in columns)
			{
				if (!_columnGetters.ContainsKey(column)) { continue; }

				writeHtml($"<td style=\"{_headerStyle}\">");
				writeSecure(_columnHeaders[column]);
				writeHtml("</td>");
			}

			writeHtml("</tr>\n");


			foreach (var row in _dataSource)
			{
				writeHtml("<tr>");
				writeRow(row);
				writeHtml("</tr>\n");
			}

			writeHtml("</table>");
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




		/*===============================================================*/

		private string getPropertyName(LambdaExpression lambdaExpr)
		{
			PropertyInfo prop = lambdaExpr.GetProperty();
			if (prop != null) { return prop.Name; }

			throw new ArgumentException("無法取得 " + lambdaExpr + " 的 Property 名稱"); 
		}

		private void writeHtml(string html)
		{
			_writer.Write(html);
		}

		private void writeSecure(object obj)
		{
			if (obj == null) { return; }

			var html = obj as IHtmlContent;
			if (html != null) 
			{ html.WriteTo(_writer, HtmlEncoder.Default); }
			else
			{ _writer.Write(HttpUtility.HtmlEncode(obj.ToString())); }

		}


	}


}
