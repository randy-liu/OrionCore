using System;


namespace Orion.Api
{ 

	/// <summary>以 NLog 實作的記錄器。</summary>
	public class OrionNLogLogger : IOrionLogger
	{
		private NLog.Logger _log;

		/// <summary>建立使用預設名稱 <c>Default</c> 的 NLog 記錄器。</summary>
		public OrionNLogLogger() : this("Default") { }

		/// <summary>依型別完整名稱建立 NLog 記錄器。</summary>
		/// <param name="type">要用來產生 Logger 名稱的型別（使用 `type.FullName`）。</param>
		public OrionNLogLogger(Type type) : this(type.FullName) { }

		/// <summary>依指定名稱建立 NLog 記錄器。</summary>
		/// <param name="name">要建立的 NLog Logger 名稱。</param>
		public OrionNLogLogger(string name)
		{
			_log = NLog.LogManager.GetLogger(name);
		}


		/// <summary>nlog Trace</summary>
		/// <param name="message">要寫入 Trace 等級的訊息內容。</param>
		/// <param name="exception">要一併記錄的例外物件（可為 `null`）。</param>
		public void Trace(string message, Exception exception = null)
		{
			_log.Trace(exception, message); 
		}

		/// <summary>nlog Debug</summary>
		/// <param name="message">要寫入 Debug 等級的訊息內容。</param>
		/// <param name="exception">要一併記錄的例外物件（可為 `null`）。</param>
		public void Debug(string message, Exception exception = null)
		{
			_log.Debug(exception, message); 
		}

		/// <summary>nlog Info</summary>
		/// <param name="message">要寫入 Info 等級的訊息內容。</param>
		/// <param name="exception">要一併記錄的例外物件（可為 `null`）。</param>
		public void Info(string message, Exception exception = null)
		{
			_log.Info(exception, message); 
		}

		/// <summary>nlog Warn</summary>
		/// <param name="message">要寫入 Warn 等級的訊息內容。</param>
		/// <param name="exception">要一併記錄的例外物件（可為 `null`）。</param>
		public void Warn(string message, Exception exception = null)
		{
			_log.Warn(exception, message); 
		}

		/// <summary>nlog Error</summary>
		/// <param name="message">要寫入 Error 等級的訊息內容。</param>
		/// <param name="exception">要一併記錄的例外物件（可為 `null`）。</param>
		public void Error(string message, Exception exception = null)
		{
			_log.Error(exception, message); 
		}

		/// <summary>nlog Fatal</summary>
		/// <param name="message">要寫入 Fatal 等級的訊息內容。</param>
		/// <param name="exception">要一併記錄的例外物件（可為 `null`）。</param>
		public void Fatal(string message, Exception exception = null)
		{
			_log.Fatal(exception, message); 
		}

	}
	
}
