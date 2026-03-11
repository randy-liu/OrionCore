using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;

namespace Orion.Api
{
    /// <summary>使用者自訂例外基底型別。</summary>
    [Serializable]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    public class UserException : Exception
    {
        /// <summary>使用訊息初始化例外。</summary>
        /// <param name="message">例外訊息。</param>
        public UserException(string message) : base(message) { }

        /// <summary>使用訊息與內部例外初始化例外。</summary>
        /// <param name="message">例外訊息。</param>
        /// <param name="innerException">內部例外。</param>
        public UserException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>供序列化機制還原例外內容的受保護建構式。</summary>
        [SecuritySafeCritical]
        protected UserException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    }


    /// <summary>表示查無資料的使用者自訂例外。</summary>
    [Serializable]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    public class UserNoDataException : UserException
    {
        /// <summary>使用訊息初始化例外。</summary>
        /// <param name="message">例外訊息。</param>
        public UserNoDataException(string message) : base(message) { }

        /// <summary>使用訊息與內部例外初始化例外。</summary>
        /// <param name="message">例外訊息。</param>
        /// <param name="innerException">內部例外。</param>
        public UserNoDataException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>供序列化機制還原例外內容的受保護建構式。</summary>
        [SecuritySafeCritical]
        protected UserNoDataException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    }


}
