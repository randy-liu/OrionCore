using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace OrionCore.API
{
	/// <summary>Utility Tools</summary>
	public static class OrionElmahUtils
	{

		private static readonly ConcurrentDictionary<string, bool> _flag = new ConcurrentDictionary<string, bool>();



		/// <summary>
		///	清除 Elmah XML 的舊紀錄，
		///	會根據 &lt;errorLog size=&quot;100&quot; logPath=&quot;~/App_Data/Elmah.Errors&quot; /&gt; 這兩個參數進行清除
		/// </summary>
		public static bool ClearOldXmlLog() 
		{
			return ClearOldXmlLog("elmah/errorLog", 22);
		}

		 



		/// <summary>
		///	清除 Elmah XML 的舊紀錄
		/// </summary>
		public static bool ClearOldXmlLog(string logPath, int size) 
		{
			if(_flag.GetOrAdd(logPath, false)) { return false; }
			_flag[logPath] = true;

			try
			{
				//TODO 清除 Elmah 
				string realpath = ""; // HttpContext.Current.Server.MapPath(logPath);


				IEnumerable<FileInfo> files = new DirectoryInfo(realpath)
					.EnumerateFiles("*.xml")
					.OrderByDescending(file => {
						try { return file.LastWriteTime; }
						catch (Exception) { return DateTime.MaxValue; }
					})
					.Skip(size);

				foreach (var file in files)
				{
					try { file.Delete(); } catch (Exception) { }
				}
			}
			catch (Exception ex)
			{
				NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
				log.Warn(ex, "Elmah clear error");
			}

			_flag[logPath] = false;
			return true;
		}





	}
}