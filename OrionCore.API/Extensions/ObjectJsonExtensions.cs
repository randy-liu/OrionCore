using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Web;

namespace OrionCore.API.Extensions
{
	/// <summary>定義 Object 的 Extension</summary>
	public static class ObjectJsonExtensions
	{
		private static readonly List<JsonConverter> _defaultConverters = new List<JsonConverter>()
		{
			new StringEnumConverter(), 
			new IsoDateTimeConverter(),
			//new StringEnumConverter() { CamelCaseText = true },
		};


		/// <summary>將 object 轉換為 Json</summary>
		public static string ToJson(this object obj) 
		{
			var settings = new JsonSerializerSettings()
			{
				Converters = _defaultConverters,
				//ContractResolver = new CamelCasePropertyNamesContractResolver(),
			};

			return JsonConvert.SerializeObject(obj, (Type)null, settings);
		}



		/// <summary>將 object 轉換為縮排格式化的 Json</summary>
		public static string ToFormatJson(this object obj)
		{
			var settings = new JsonSerializerSettings()
			{
				Formatting = Formatting.Indented,
				Converters = _defaultConverters,
				//ContractResolver = new CamelCasePropertyNamesContractResolver(),
			};

			return JsonConvert.SerializeObject(obj, (Type)null, settings);
		}



		/// <summary>將 json string 轉換為 Object</summary>
		public static TObject JsonToObject<TObject>(this String jsonStr)
		{
			if (jsonStr == null) { return default(TObject); }
			return JsonConvert.DeserializeObject<TObject>(jsonStr);
		}



	}
}