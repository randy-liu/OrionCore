using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;
using Orion.Api;
using Orion.Api.Extensions;
using Orion.Api.Models;
using Orion.Mvc.Extensions;

namespace Orion.Mvc.Html
{

	/// <summary></summary>
	public static class HelperExtensions
	{
		/// <summary>過濾 QueryString 中不要的變數</summary>
		public static string FilterQueryString(this IHtmlHelper helper, params string[] filterNames)
		{
			string queryString = helper.ViewContext.HttpContext.Request.QueryString.ToString();
			NameValueCollection values = HttpUtility.ParseQueryString(queryString);

			foreach (string key in filterNames)
			{
				values.Remove(key);
			}

			return values.ToString();
		}





		private static string readFileContent(IHtmlHelper helper, string contentPath)
		{
			var env = helper.ViewContext.HttpContext.RequestServices.GetService<IWebHostEnvironment>();

			string filePath = env.MapPath(contentPath.TrimStart('~'));
			return File.ReadAllText(filePath);
		}

		/// <summary>將 Content 的內容輸出到畫面</summary>
		public static IHtmlContent Content(this IHtmlHelper helper, string contentPath)
		{
			return new HtmlString(readFileContent(helper, contentPath));
		}


		/// <summary>將 Content 的內容輸出到畫面</summary>
		public static void RenderContent(this IHtmlHelper helper, string contentPath)
		{
			helper.ViewContext.Writer.Write(readFileContent(helper, contentPath));
		}








		/*#############################################################*/

		/// <summary></summary>
		public static IHtmlHelper<TModel> ModelHelper<TModel>(this IHtmlHelper helper, TModel model)
		{
			return new OrionHtmlHelper<TModel>(helper, model);
		}


		/// <summary></summary>
		public static IHtmlHelper<TModel> PropertyHelper<TModel>(this IHtmlHelper helper)
		{
			return ModelHelper(helper, default(TModel));
		}
		/// <summary></summary>
		public static IHtmlHelper<TModel> PropertyHelper<TModel>(this IHtmlHelper helper, List<TModel> list)
		{
			return ModelHelper(helper, default(TModel));
		}
		/// <summary></summary>
		public static IHtmlHelper<TModel> PropertyHelper<TModel>(this IHtmlHelper helper, Pagination<TModel> pagination)
		{
			return ModelHelper(helper, default(TModel));
		}




		/*#############################################################*/

		private static IHtmlContent buildShowItem(Enum enumValue) 
		{
			string text = enumValue.GetDisplayName();

			var tb = new TagBuilder("span");
			tb.AddCssClass("item-" + enumValue);
			tb.InnerHtml.Append(text);

			return tb;
		}

		/// <summary></summary>
		public static IHtmlContent ShowItem<TEnum>(this IEnumerable<TEnum> enumValues) where TEnum : struct, Enum
		{
			if (enumValues == null) { return HtmlString.Empty; }

			var cb = new HtmlContentBuilder();
			foreach (var enumValue in enumValues)
			{
				cb.AppendHtml(buildShowItem(enumValue));
				cb.AppendHtml(" ");
			}

			return cb;
		}
		/// <summary></summary>
		public static IHtmlContent ShowItem(this Enum enumValue) 
		{
			if (enumValue == null) { return HtmlString.Empty; }
			return buildShowItem(enumValue);
		}

 






		/*##############################################################################*/


		/// <summary></summary>
		public static IHtmlContent ShowItem<K, V>(this IEnumerable<K> values, IDictionary<K, V> selectList)
		{
			if (values == null) { return HtmlString.Empty; }

			var cb = new HtmlContentBuilder();
			foreach (var value in values)
			{
				cb.AppendHtml(buildShowItem(value, selectList));
				cb.AppendHtml(" ");
			}

			return cb;
		}


		/// <summary></summary>
		public static IHtmlContent ShowItem<K, V>(this K? value, IDictionary<K, V> selectList) where K : struct
		{
			if (!value.HasValue) { return HtmlString.Empty; }
			return buildShowItem(value.Value, selectList);
		}



		/// <summary></summary>
		public static IHtmlContent ShowItem<K, V>(this K value, IDictionary<K, V> selectList)
		{
			if (value == null) { return HtmlString.Empty; }
			return buildShowItem(value, selectList);
		}



		private static IHtmlContent buildShowItem<K, V>(K value, IDictionary<K, V> selectList)
		{
			if (value == null) { return HtmlString.Empty; }

			string text = selectList.ContainsKey(value) ? selectList[value].ToString() : value.ToString();

			var tb = new TagBuilder("span");
			tb.AddCssClass("item-" + value);
			tb.InnerHtml.Append(text);

			return tb;
		}




		/// <summary></summary>
		public static IHtmlContent ShowItem(this object obj, IDictionary<string, string> selectList)
		{
			return buildShowItem(obj, selectList);
		}

		private static IHtmlContent buildShowItem(object obj, IDictionary<string, string> selectList)
		{
			if (obj == null) { return HtmlString.Empty; }

			if (obj is IEnumerable && !(obj is string))
			{
				var cb = new HtmlContentBuilder();

				foreach (var item in (obj as IEnumerable))
				{ cb.AppendLine(buildShowItem(item, selectList)); }

				return cb;
			}
			else
			{
				string value = "" + obj;
				string text = selectList.ContainsKey(value) ? selectList[value] : value;

				var tb = new TagBuilder("span");
				tb.AddCssClass("item-" + obj);
				tb.InnerHtml.Append(text);

				return tb;
			}
		}





		/*##############################################################################*/


		/// <summary></summary>
		public static IHtmlContent ShowItemAndKey(this object obj, IDictionary<string, string> selectList)
		{
			return buildShowItemAndKey(obj, selectList);
		}

		private static IHtmlContent buildShowItemAndKey(object obj, IDictionary<string, string> selectList)
		{
			if (obj == null) { return HtmlString.Empty; }


			if (obj is IEnumerable && !(obj is string))
			{
				var cb = new HtmlContentBuilder();

				foreach (var item in (obj as IEnumerable))
				{ cb.AppendLine(buildShowItemAndKey(item, selectList)); }

				return cb;
			}
			else
			{
				string text = "" + obj;
				if (selectList.ContainsKey(text)) { text += " " + selectList[text]; }

				var tb = new TagBuilder("span");
				tb.AddCssClass("item-" + obj);
				tb.InnerHtml.Append(text);

				return tb;
			}
		}






		/*##############################################################################*/

		/// <summary>將 object 轉換為 JsonRaw</summary>
		public static IHtmlContent ToJsonRaw(this object obj)
		{
			return new HtmlString(ObjectExtensions.ToJson(obj));
		}



		/// <summary>將 object 轉換為縮排格式化的 JsonRaw</summary>
		public static IHtmlContent ToFormatJsonRaw(this object obj)
		{
			return new HtmlString(ObjectExtensions.ToFormatJson(obj));
		}






		/*##############################################################################*/

		/// <summary></summary>
		public static IHtmlContent CommaWrap<T>(this T value) where T : struct
		{
			var formated = ObjectExtensions.Comma(value);
			return new HtmlString($"<span raw=\"{value}\">{formated}</span>");
		}

		/// <summary></summary>
		public static IHtmlContent CommaWrap<T>(this T? value) where T : struct
		{
			if(value == null) { return HtmlString.Empty; }
			return CommaWrap(value.Value);
		}


		/// <summary></summary>
		public static IHtmlContent CommaWrap<T>(this T value, int digits) where T : struct
		{
			var formated = ObjectExtensions.Comma(value, digits);
			return new HtmlString($"<span raw=\"{value}\">{formated}</span>");
		}

		/// <summary></summary>
		public static IHtmlContent CommaWrap<T>(this T? value, int digits) where T : struct
		{
			if (value == null) { return HtmlString.Empty; }
			return CommaWrap(value.Value, digits);
		}





		/*#############################################################*/

		/// <summary></summary>
		public static string EnumJson<TEnum>(this IHtmlHelper helper)
		{
			return OrionUtils.EnumToDictionary<TEnum>().ToJson();
		}

		/// <summary></summary>
		public static string EnumJson(this IHtmlHelper helper, Type type)
		{
			return OrionUtils.EnumToDictionary(type).ToJson();
		}

		/// <summary></summary>
		public static IHtmlContent EnumJsonRaw<TEnum>(this IHtmlHelper helper)
		{
			return OrionUtils.EnumToDictionary<TEnum>().ToJsonRaw();
		}

		/// <summary></summary>
		public static IHtmlContent EnumJsonRaw(this IHtmlHelper helper, Type type)
		{
			return OrionUtils.EnumToDictionary(type).ToJsonRaw();
		}





		/*#############################################################*/

		/// <summary></summary>
		public static string BackUrl(this IHtmlHelper helper, string defaultUrl = "JavaScript:history.back();void(0);")
		{
			string backUrl = helper.ViewBag.JwHelperBackUrl as string;
			if (backUrl != null) { return backUrl; }

			HttpRequest request = helper.ViewContext.HttpContext.Request;
			string requestPath = request.Path;
			string cookieName = requestPath.Replace('/', '-').Trim('-');
			backUrl = request.Headers["Referer"].ToString();

			/* 如果當前頁面與前一頁不相同，則儲存網址*/
			if (backUrl.HasText() && !backUrl.Contains(requestPath))
			{
				var response = helper.ViewContext.HttpContext.Response;
				response.Cookies.Append(cookieName, backUrl, new CookieOptions { Path = requestPath });
			}
			else
			{
				/* 從 Cookie 取得上次的記錄 */
				backUrl = request.Cookies[cookieName] ?? defaultUrl;
			}

			/* 紀錄返回網址*/
			helper.ViewBag.JwHelperBackUrl = backUrl;

			return backUrl;
		}




	}
}