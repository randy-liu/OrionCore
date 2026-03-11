using System;

namespace Orion.Api
{

	/// <summary>以事件方式對外發送記錄訊息的 Logger。</summary>
	public class OrionEventLogger : IOrionLogger
	{
		/// <summary>記錄事件，包含層級、訊息與例外資訊。</summary>
		public event Action<OrionLogLevel, string, Exception> Event;

		/// <summary>送出 Trace 層級記錄事件。</summary>
		/// <param name="message">記錄訊息。</param>
		/// <param name="exception">附帶例外；可為 <c>null</c>。</param>
		public void Trace(string message, Exception exception = null)
		{
			Event?.Invoke(OrionLogLevel.Trace, message, exception);
		}

		/// <summary>送出 Debug 層級記錄事件。</summary>
		/// <param name="message">記錄訊息。</param>
		/// <param name="exception">附帶例外；可為 <c>null</c>。</param>
		public void Debug(string message, Exception exception = null)
		{
			Event?.Invoke(OrionLogLevel.Debug, message, exception);
		}

		/// <summary>送出 Info 層級記錄事件。</summary>
		/// <param name="message">記錄訊息。</param>
		/// <param name="exception">附帶例外；可為 <c>null</c>。</param>
		public void Info(string message, Exception exception = null)
		{
			Event?.Invoke(OrionLogLevel.Info, message, exception);
		}

		/// <summary>送出 Warn 層級記錄事件。</summary>
		/// <param name="message">記錄訊息。</param>
		/// <param name="exception">附帶例外；可為 <c>null</c>。</param>
		public void Warn(string message, Exception exception = null)
		{
			Event?.Invoke(OrionLogLevel.Warn, message, exception);
		}

		/// <summary>送出 Error 層級記錄事件。</summary>
		/// <param name="message">記錄訊息。</param>
		/// <param name="exception">附帶例外；可為 <c>null</c>。</param>
		public void Error(string message, Exception exception = null)
		{
			Event?.Invoke(OrionLogLevel.Error, message, exception);
		}

		/// <summary>送出 Fatal 層級記錄事件。</summary>
		/// <param name="message">記錄訊息。</param>
		/// <param name="exception">附帶例外；可為 <c>null</c>。</param>
		public void Fatal(string message, Exception exception = null)
		{
			Event?.Invoke(OrionLogLevel.Fatal, message, exception);
		}

	}
	
}
