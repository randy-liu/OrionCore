using System;

namespace Orion.Api
{
    /// <summary>使用者自訂例外基底型別。</summary>
    [Serializable]
    public class UserException : Exception
    {
        /// <summary>使用訊息初始化例外。</summary>
        /// <param name="message">例外訊息。</param>
        public UserException(string message) : base(message) { }

        /// <summary>使用訊息與內部例外初始化例外。</summary>
        /// <param name="message">例外訊息。</param>
        /// <param name="innerException">內部例外。</param>
        public UserException(string message, Exception innerException) : base(message, innerException) { }
    }


    /// <summary>表示查無資料的使用者自訂例外。</summary>
    [Serializable]
    public class UserNoDataException : UserException
    {
        /// <summary>使用訊息初始化例外。</summary>
        /// <param name="message">例外訊息。</param>
        public UserNoDataException(string message) : base(message) { }

        /// <summary>使用訊息與內部例外初始化例外。</summary>
        /// <param name="message">例外訊息。</param>
        /// <param name="innerException">內部例外。</param>
        public UserNoDataException(string message, Exception innerException) : base(message, innerException) { }
    }


}
