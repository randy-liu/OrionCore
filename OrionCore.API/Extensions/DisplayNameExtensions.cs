using System;
using System.Reflection;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Resources;

namespace Orion.Api.Extensions
{
	/// <summary>用於取得 Display、Description Attribute 設定名稱或描述 </summary>
	public static class DisplayNameExtensions
	{

		/// <summary>取得成員上的顯示名稱或描述文字。</summary>
		/// <param name="info">方法、屬性或欄位的成員資訊。</param>
		/// <returns>優先取 `DisplayName`、`Display`、`Description` 的字串；皆無則回傳 `null`。</returns>
		public static string GetDisplayName(this MemberInfo info)
		{
			if (info == null) { return null; }

			var disNameAttr = info.GetCustomAttribute<DisplayNameAttribute>();
			if (disNameAttr != null) { return disNameAttr.DisplayName; }

			var disAttr = info.GetCustomAttribute<DisplayAttribute>();
			if (disAttr != null)
			{
				if (disAttr.ResourceType == null) { return disAttr.Name; }
				return new ResourceManager(disAttr.ResourceType).GetString(disAttr.Name) ?? disAttr.Name;
			}

			var descAttr = info.GetCustomAttribute<DescriptionAttribute>();
			return descAttr?.Description; 
		}



		/// <summary>取得列舉值對應成員上的顯示名稱或描述文字。</summary>
		/// <param name="enumValue">列舉值。</param>
		/// <returns>列舉成員的顯示名稱或描述；找不到時回傳 `null`。</returns>
		public static string GetDisplayName(this Enum enumValue) 
		{
			if (enumValue == null) { return null; }

			var fi = enumValue.GetType().GetField(enumValue.ToString());
			if (fi == null) { return null; }

			return GetDisplayName(fi);
		}




	}

}
