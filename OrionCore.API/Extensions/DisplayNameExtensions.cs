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

		/// <summary>取得 Method | Property | Field  DisplayAttribute、DescriptionAttribute 中的字串</summary>
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



		/// <summary>取得 Method | Property | Field  DisplayAttribute 中的字串</summary>
		public static string GetDisplayName(this Enum enumValue) 
		{
			if (enumValue == null) { return null; }

			var fi = enumValue.GetType().GetField(enumValue.ToString());
			if (fi == null) { return null; }

			return GetDisplayName(fi);
		}




	}

}