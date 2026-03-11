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

	/// <summary>HtmlHelper 常用輸出與格式化擴充方法。</summary>
	public static class HelperExtensions
	{
		/// <summary>過濾 QueryString 中不要的變數</summary>
		/// <param name="helper">HTML Helper。</param>
		/// <param name="filterNames">要排除的 QueryString 參數名稱。</param>
		/// <returns>處理後字串結果。</returns>
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
		/// <param name="helper">HTML Helper。</param>
		/// <param name="contentPath">內容檔案路徑。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent Content(this IHtmlHelper helper, string contentPath)
		{
			return new HtmlString(readFileContent(helper, contentPath));
		}


		/// <summary>將 Content 的內容輸出到畫面</summary>
		/// <param name="helper">HTML Helper。</param>
		/// <param name="contentPath">內容檔案路徑。</param>
		public static void RenderContent(this IHtmlHelper helper, string contentPath)
		{
			helper.ViewContext.Writer.Write(readFileContent(helper, contentPath));
		}








		/*#############################################################*/

		/// <summary>以指定模型建立型別化 HtmlHelper。</summary>
		/// <param name="helper">HTML Helper。</param>
		/// <param name="model">目標模型。</param>
		/// <returns>型別化 HtmlHelper。</returns>
		public static IHtmlHelper<TModel> ModelHelper<TModel>(this IHtmlHelper helper, TModel model)
		{
			return new OrionHtmlHelper<TModel>(helper, model);
		}


		/// <summary>建立屬性用途的型別化 HtmlHelper。</summary>
		/// <param name="helper">HTML Helper。</param>
		/// <returns>型別化 HtmlHelper。</returns>
		public static IHtmlHelper<TModel> PropertyHelper<TModel>(this IHtmlHelper helper)
		{
			return ModelHelper(helper, default(TModel));
		}
		/// <summary>由清單型別建立屬性用途的型別化 HtmlHelper。</summary>
		/// <param name="helper">HTML Helper。</param>
		/// <param name="list">模型清單。</param>
		/// <returns>型別化 HtmlHelper。</returns>
		public static IHtmlHelper<TModel> PropertyHelper<TModel>(this IHtmlHelper helper, List<TModel> list)
		{
			return ModelHelper(helper, default(TModel));
		}
		/// <summary>由分頁型別建立屬性用途的型別化 HtmlHelper。</summary>
		/// <param name="helper">HTML Helper。</param>
		/// <param name="pagination">分頁資料。</param>
		/// <returns>型別化 HtmlHelper。</returns>
		public static IHtmlHelper<TModel> PropertyHelper<TModel>(this IHtmlHelper helper, Pagination<TModel> pagination)
		{
			return ModelHelper(helper, default(TModel));
		}




		/*#############################################################*/

		private static IHtmlContent buildShowItem(Enum enumValue) 
		{
			string text = enumValue.GetDisplayName();
			if (text.NoText()) { text = enumValue.ToString(); }

			var tb = new TagBuilder("span");
			tb.AddCssClass("item-" + enumValue);
			tb.InnerHtml.Append(text);

			return tb;
		}

		/// <summary>將列舉集合轉為顯示標籤內容。</summary>
		/// <param name="enumValues">列舉值集合。</param>
		/// <returns>產生的 HTML 內容。</returns>
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
		/// <summary>將列舉值轉為顯示標籤內容。</summary>
		/// <param name="enumValue">列舉值。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent ShowItem(this Enum enumValue) 
		{
			if (enumValue == null) { return HtmlString.Empty; }
			return buildShowItem(enumValue);
		}

 






		/*##############################################################################*/


		/// <summary>依字典對照將值集合轉為顯示標籤內容。</summary>
		/// <param name="values">值集合。</param>
		/// <param name="selectList">值與顯示文字對照。</param>
		/// <returns>產生的 HTML 內容。</returns>
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


		/// <summary>依字典對照將可空值轉為顯示標籤內容。</summary>
		/// <param name="value">可空值。</param>
		/// <param name="selectList">值與顯示文字對照。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent ShowItem<K, V>(this K? value, IDictionary<K, V> selectList) where K : struct
		{
			if (!value.HasValue) { return HtmlString.Empty; }
			return buildShowItem(value.Value, selectList);
		}



		/// <summary>依字典對照將單一值轉為顯示標籤內容。</summary>
		/// <param name="value">值。</param>
		/// <param name="selectList">值與顯示文字對照。</param>
		/// <returns>產生的 HTML 內容。</returns>
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




		/// <summary>依字串字典對照將物件值轉為顯示標籤內容。</summary>
		/// <param name="obj">來源物件。</param>
		/// <param name="selectList">值與顯示文字對照。</param>
		/// <returns>產生的 HTML 內容。</returns>
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


		/// <summary>顯示項目並包含鍵值文字。</summary>
		/// <param name="obj">來源物件。</param>
		/// <param name="selectList">值與顯示文字對照。</param>
		/// <returns>產生的 HTML 內容。</returns>
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




		/*#############################################################*/

		/// <summary>將位元組數格式化為可讀大小字串。</summary>
		/// <param name="size">位元組數。</param>
		/// <returns>處理後字串結果。</returns>
		public static string ShowBytes(this long size)
		{
			var unit = new string[] { " Bytes", " KB", " MB", " GB", " TB", " PB", " EB", " ZB", " YB" };
			if (size == 0) { return "n/a"; }

			var i = (int)Math.Floor(Math.Log(size, 1024));
			return Math.Round(size / Math.Pow(1024, i), 2) + unit[i];
		}

		/// <summary>將 `int` 位元組數格式化為可讀大小字串。</summary>
		/// <param name="size">位元組數。</param>
		/// <returns>處理後字串結果。</returns>
		public static string ShowBytes(this int size) { return ShowBytes((long)size); }

		/// <summary>將 `double` 位元組數格式化為可讀大小字串。</summary>
		/// <param name="size">位元組數。</param>
		/// <returns>處理後字串結果。</returns>
		public static string ShowBytes(this double size) { return ShowBytes((long)size); }

		/// <summary>將 `decimal` 位元組數格式化為可讀大小字串。</summary>
		/// <param name="size">位元組數。</param>
		/// <returns>處理後字串結果。</returns>
		public static string ShowBytes(this decimal size) { return ShowBytes((long)size); }




		/*##############################################################################*/

		/// <summary>將 object 轉換為 JsonRaw</summary>
		/// <param name="obj">來源物件。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent ToJsonRaw(this object obj)
		{
			return new HtmlString(ObjectExtensions.ToJson(obj));
		}



		/// <summary>將 object 轉換為縮排格式化的 JsonRaw</summary>
		/// <param name="obj">來源物件。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent ToFormatJsonRaw(this object obj)
		{
			return new HtmlString(ObjectExtensions.ToFormatJson(obj));
		}






		/*##############################################################################*/

		/// <summary>輸出含千分位格式的數值包裝 HTML。</summary>
		/// <param name="value">數值。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent CommaWrap<T>(this T value) where T : struct
		{
			var formated = ObjectExtensions.Comma(value);
			return new HtmlString($"<span raw=\"{value}\">{formated}</span>");
		}

		/// <summary>輸出含千分位格式的可空數值包裝 HTML。</summary>
		/// <param name="value">可空數值。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent CommaWrap<T>(this T? value) where T : struct
		{
			if(value == null) { return HtmlString.Empty; }
			return CommaWrap(value.Value);
		}


		/// <summary>輸出指定小數位千分位格式的數值包裝 HTML。</summary>
		/// <param name="value">數值。</param>
		/// <param name="digits">小數位數。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent CommaWrap<T>(this T value, int digits) where T : struct
		{
			var formated = ObjectExtensions.Comma(value, digits);
			return new HtmlString($"<span raw=\"{value}\">{formated}</span>");
		}

		/// <summary>輸出指定小數位千分位格式的可空數值包裝 HTML。</summary>
		/// <param name="value">可空數值。</param>
		/// <param name="digits">小數位數。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent CommaWrap<T>(this T? value, int digits) where T : struct
		{
			if (value == null) { return HtmlString.Empty; }
			return CommaWrap(value.Value, digits);
		}





		/*#############################################################*/

		/// <summary>輸出指定列舉型別的 JSON 字串。</summary>
		/// <param name="helper">HTML Helper。</param>
		/// <returns>處理後字串結果。</returns>
		public static string EnumJson<TEnum>(this IHtmlHelper helper)
		{
			return OrionUtils.EnumToDictionary<TEnum>().ToJson();
		}

		/// <summary>輸出指定型別列舉的 JSON 字串。</summary>
		/// <param name="helper">HTML Helper。</param>
		/// <param name="type">列舉型別。</param>
		/// <returns>處理後字串結果。</returns>
		public static string EnumJson(this IHtmlHelper helper, Type type)
		{
			return OrionUtils.EnumToDictionary(type).ToJson();
		}

		/// <summary>輸出指定列舉型別的 JSON Raw 內容。</summary>
		/// <param name="helper">HTML Helper。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent EnumJsonRaw<TEnum>(this IHtmlHelper helper)
		{
			return OrionUtils.EnumToDictionary<TEnum>().ToJsonRaw();
		}

		/// <summary>輸出指定型別列舉的 JSON Raw 內容。</summary>
		/// <param name="helper">HTML Helper。</param>
		/// <param name="type">列舉型別。</param>
		/// <returns>產生的 HTML 內容。</returns>
		public static IHtmlContent EnumJsonRaw(this IHtmlHelper helper, Type type)
		{
			return OrionUtils.EnumToDictionary(type).ToJsonRaw();
		}





		/*#############################################################*/

		/// <summary>取得返回上一頁網址，並支援 Cookie 記錄回跳位置。</summary>
		/// <param name="helper">HTML Helper。</param>
		/// <param name="defaultUrl">預設返回網址。</param>
		/// <returns>處理後字串結果。</returns>
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
