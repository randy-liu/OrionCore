using System;
using System.IO;
using System.Linq;
using System.Reflection;
using OrionCore.API.Models;

namespace OrionCore.API.Extensions
{

	/// <summary>定義 Assembly 的 Extension</summary>
	public static class AssemblyExtensions
	{

		/// <summary></summary>
		public static AssemblyMeta GetMeta(this Assembly asm)
		{
			return AssemblyUtils.GetMeta(asm);
		}


		/// <summary></summary>
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
