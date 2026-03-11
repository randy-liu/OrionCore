using System;


namespace Orion.Api
{
    /// <summary>IJwLogger 介面</summary>
    public interface IOrionLogger
    {
        /// <summary>寫入除錯層級記錄。</summary>
        /// <param name="message">記錄訊息。</param>
        /// <param name="exception">例外資訊。</param>
        void Debug(string message, Exception exception = null);

        /// <summary>寫入錯誤層級記錄。</summary>
        /// <param name="message">記錄訊息。</param>
        /// <param name="exception">例外資訊。</param>
        void Error(string message, Exception exception = null);

        /// <summary>寫入嚴重錯誤層級記錄。</summary>
        /// <param name="message">記錄訊息。</param>
        /// <param name="exception">例外資訊。</param>
        void Fatal(string message, Exception exception = null);

        /// <summary>寫入資訊層級記錄。</summary>
        /// <param name="message">記錄訊息。</param>
        /// <param name="exception">例外資訊。</param>
        void Info(string message, Exception exception = null);

        /// <summary>寫入追蹤層級記錄。</summary>
        /// <param name="message">記錄訊息。</param>
        /// <param name="exception">例外資訊。</param>
        void Trace(string message, Exception exception = null);

        /// <summary>寫入警告層級記錄。</summary>
        /// <param name="message">記錄訊息。</param>
        /// <param name="exception">例外資訊。</param>
        void Warn(string message, Exception exception = null);
    }


    /// <summary>Orion 記錄層級。</summary>
    public enum OrionLogLevel
    {
        /// <summary>追蹤層級。</summary>
        Trace,

        /// <summary>除錯層級。</summary>
        Debug,

        /// <summary>資訊層級。</summary>
        Info,

        /// <summary>警告層級。</summary>
        Warn,

        /// <summary>錯誤層級。</summary>
        Error,

        /// <summary>嚴重錯誤層級。</summary>
        Fatal,

    }

}
