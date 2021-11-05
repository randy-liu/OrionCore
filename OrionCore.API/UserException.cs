using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;

namespace OrionCore.API
{
	/// <summary>Custom Exception</summary>
	[Serializable]
	[ClassInterface(ClassInterfaceType.None)]
	[ComVisible(true)]
	public class UserException : Exception
	{
		/// <summary>Custom Exception That Just One Parm</summary>
		public UserException(string message) : base(message) { }

		/// <summary>Custom Exception That Just Two Parms</summary>
		public UserException(string message, Exception innerException) : base(message, innerException) { }

		/// <summary>Custom Exception 須遵守嚴格的安全性稽核，以確保其可在安全的執行環境中使用</summary>
		[SecuritySafeCritical]
		protected UserException(SerializationInfo info, StreamingContext context) : base(info, context) { }

	}


	/// <summary>Custom No Data Exception</summary>
	[Serializable]
	[ClassInterface(ClassInterfaceType.None)]
	[ComVisible(true)]
	public class UserNoDataException : UserException
	{
		/// <summary>Custom No Data Exception That Just One Parm</summary>
		public UserNoDataException(string message) : base(message) { }

		/// <summary>Custom No Data Exception That Just Two Parms</summary>
		public UserNoDataException(string message, Exception innerException) : base(message, innerException) { }

		/// <summary>Custom No Data Exception 須遵守嚴格的安全性稽核，以確保其可在安全的執行環境中使用</summary>
		[SecuritySafeCritical]
		protected UserNoDataException(SerializationInfo info, StreamingContext context) : base(info, context) { }

	}


}
