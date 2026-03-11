using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Orion.Api.Models;

namespace Orion.Api.Extensions
{

	/// <summary>定義 Assembly 的 Extension</summary>
	public static class AssemblyExtensions
	{

		/// <summary>取得組件中繼資料。</summary>
		/// <param name="asm">目標組件。</param>
		/// <returns>組件中繼資料。</returns>
		public static AssemblyMeta GetMeta(this Assembly asm)
		{
			return AssemblyUtils.GetMeta(asm);
		}


		/// <summary>依資源路徑取得內嵌資源串流。</summary>
		/// <param name="assembly">目標組件。</param>
		/// <param name="filePath">資源路徑。</param>
		/// <returns>找到時回傳資源串流，否則回傳 `null`。</returns>
		public static Stream GetManifestResourceStreamByPath(this Assembly assembly, string filePath)
		{
			filePath = filePath.Replace("/", ".");

			string embeddedName = assembly.GetManifestResourceNames()
				.Where(x => x.EndsWith(filePath, StringComparison.OrdinalIgnoreCase))
				.OrderBy(x => x.Length)
				.FirstOrDefault();

			if (embeddedName == null) { return null; }

			Stream stream = assembly.GetManifestResourceStream(embeddedName);
			return stream;
		}


	}
}
