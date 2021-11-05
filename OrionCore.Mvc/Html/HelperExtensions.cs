using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using OrionCore.API;
using OrionCore.API.Extensions;
using OrionCore.API.Models;
using OrionCore.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using Microsoft.Extensions.DependencyInjection;


namespace OrionCore.Mvc.Html
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



		/// <summary></summary>
		public static string DisplayName(this IHtmlHelper html, Enum enumValue)
		{
			return enumValue.GetDisplayName();
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

		private static IHtmlContent getShowItem<TEnum>(TEnum enumValue, string tag = null)
		{
			if (enumValue == null) { return HtmlString.Empty; }
			if (tag.NoText()) { tag = "span"; }

			string text = enumValue.ToString();
			if (enumValue is Enum) { text = OrionUtils.GetEnumDisplayName(enumValue); }

			var tb = new TagBuilder(tag);
			tb.AddCssClass("item-" + enumValue);
			tb.InnerHtml.Append(text);

			return tb;
		}


		/// <summary></summary>
		public static IHtmlContent ShowItem<TEnum>(this IHtmlHelper helper, IEnumerable<TEnum> enumValues, string tag) where TEnum : struct
		{
			if (enumValues == null) { return HtmlString.Empty; }

			var cb = new HtmlContentBuilder();
			foreach (var enumValue in enumValues)
			{
				cb.AppendHtml(getShowItem(enumValue, tag));
				cb.AppendHtml(" ");
			}

			return cb;
		}
		/// <summary></summary>
		public static IHtmlContent ShowItem<TEnum>(this IHtmlHelper helper, IEnumerable<TEnum> enumValues) where TEnum : struct
		{
			return ShowItem(helper, enumValues, null);
		}


		/// <summary></summary>
		public static IHtmlContent ShowItem<TEnum>(this IHtmlHelper helper, TEnum? enumValue) where TEnum : struct
		{
			if (!enumValue.HasValue) { return HtmlString.Empty; }
			return getShowItem(enumValue.Value);
		}
		/// <summary></summary>
		public static IHtmlContent ShowItem<TEnum>(this IHtmlHelper helper, TEnum? enumValue, string tag) where TEnum : struct
		{
			if (!enumValue.HasValue) { return HtmlString.Empty; }
			return getShowItem(enumValue.Value, tag);
		}


		/// <summary></summary>
		public static IHtmlContent ShowItem(this IHtmlHelper helper, Enum enumValue)
		{
			return ShowItem(helper, enumValue, null);
		}
		/// <summary></summary>
		public static IHtmlContent ShowItem(this IHtmlHelper helper, Enum enumValue, string tag)
		{
			if (enumValue == null) { return HtmlString.Empty; }
			return getShowItem(enumValue, tag);
		}






		private static IHtmlContent getShowItem<K, V>(K value, IDictionary<K, V> selectList, string tag = null)
		{
			if (value == null) { return HtmlString.Empty; }
			if (tag.NoText()) { tag = "span"; }

			string text = selectList.ContainsKey(value) ? selectList[value].ToString() : value.ToString();

			var tb = new TagBuilder(tag);
			tb.AddCssClass("item-" + value);
			tb.InnerHtml.Append(text);

			return tb;
		}

		/// <summary></summary>
		public static IHtmlContent ShowItem<K, V>(this IHtmlHelper helper, IEnumerable<K> values, IDictionary<K, V> selectList)
		{
			return ShowItem(helper, values, selectList, null);
		}
		/// <summary></summary>
		public static IHtmlContent ShowItem<K, V>(this IHtmlHelper helper, IEnumerable<K> values, IDictionary<K, V> selectList, string tag)
		{
			if (values == null) { return HtmlString.Empty; }

			var cb = new HtmlContentBuilder();
			foreach (var value in values)
			{
				cb.AppendHtml(getShowItem(value, selectList, tag));
				cb.AppendHtml(" ");
			}

			return cb;
		}


		/// <summary></summary>
		public static IHtmlContent ShowItem<K, V>(this IHtmlHelper helper, K? value, IDictionary<K, V> selectList) where K : struct
		{
			if (!value.HasValue) { return HtmlString.Empty; }
			return getShowItem(value.Value, selectList);
		}
		/// <summary></summary>
		public static IHtmlContent ShowItem<K, V>(this IHtmlHelper helper, K? value, IDictionary<K, V> selectList, string tag) where K : struct
		{
			if (!value.HasValue) { return HtmlString.Empty; }
			return getShowItem(value.Value, selectList, tag);
		}


		/// <summary></summary>
		public static IHtmlContent ShowItem<K, V>(this IHtmlHelper helper, K value, IDictionary<K, V> selectList)
		{
			if (value == null) { return HtmlString.Empty; }
			return getShowItem(value, selectList);
		}
		/// <summary></summary>
		public static IHtmlContent ShowItem<K, V>(this IHtmlHelper helper, K value, IDictionary<K, V> selectList, string tag)
		{
			if (value == null) { return HtmlString.Empty; }
			return getShowItem(value, selectList, tag);
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


		private static string getLiveTime(TimeSpan diffTime, int diffYear)
		{
			if (diffTime.TotalSeconds < 60) { return $"{diffTime.TotalSeconds:0} 秒前"; }
			if (diffTime.TotalMinutes < 60) { return $"{diffTime.TotalMinutes:0} 分鐘前"; }
			if (diffTime.TotalHours < 24) { return $"{diffTime.TotalHours:0} 小時前"; }
			if (diffTime.TotalDays < 30) { return $"{diffTime.TotalDays:0} 天前"; }
			if (diffTime.TotalDays < 360) { return $"{(diffTime.TotalDays / 30):0} 個月前"; }

			return diffYear + " 年前";
		}


		/// <summary>顯示日期</summary>
		public static string ShowDate(this IHtmlHelper helper, DateTime? data)
		{
			return data?.ToString("d");
		}



		/// <summary>顯示時間</summary>
		public static string ShowTime(this IHtmlHelper helper, DateTime? data)
		{
			return data?.ToString("t");
		}




		/// <summary>顯示活動時間</summary>
		public static string ShowLiveTime(this IHtmlHelper helper, DateTime? datatime)
		{
			if (datatime == null) { return null; }

			string liveTime = getLiveTime(
				DateTime.Now - datatime.Value,
				DateTime.Now.Year - datatime.Value.Year
			);
			return liveTime;
		}





		/*#############################################################*/

		/// <summary>顯示日期</summary>
		public static string ShowDateTime(this IHtmlHelper helper, DateTimeOffset? data)
		{
			return ThreadTimeZone.ConvertZone(data)?.ToString("f");
		}


		/// <summary>顯示日期</summary>
		public static string ShowDate(this IHtmlHelper helper, DateTimeOffset? data)
		{
			return ThreadTimeZone.ConvertZone(data)?.ToString("d");
		}

		
		/// <summary>顯示時間</summary>
		public static string ShowTime(this IHtmlHelper helper, DateTimeOffset? data)
		{
			return ThreadTimeZone.ConvertZone(data)?.ToString("t");
		}

			   
		/// <summary>顯示活動時間</summary>
		public static string ShowLiveTime(this IHtmlHelper helper, DateTimeOffset? datatime)
		{
			if (datatime == null) { return null; }

			string liveTime = getLiveTime(
				DateTimeOffset.Now - datatime.Value,
				DateTimeOffset.Now.Year - datatime.Value.Year
			);
			return liveTime;
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