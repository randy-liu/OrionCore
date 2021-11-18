using System;

namespace Orion.Api
{

	/// <summary>通知 Attribute</summary>
	[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
	public abstract class NotifiAttribute : Attribute
	{
		/// <summary>非同步執行</summary>
		public bool Async { get; set; }

		/// <summary>唯一執行</summary>
		public bool OnlyOne { get; set; }


		/// <summary>通知名稱</summary>
		public string GetName()
		{
			return GetType().Name.Replace("Attribute", (Async ? " Async" : ""));
		}

		/// <summary>參數數量</summary>
		public abstract int GetParamLimit();
	}






	/*########################################################*/

	/// <summary>初始</summary>
	[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
	public class OnInitAttribute : NotifiAttribute
    {
		/// <summary>參數數量 0</summary>
		public override int GetParamLimit() { return 0; }
	}


	/// <summary>關閉</summary>
	[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
	public class OnCloseAttribute : NotifiAttribute
    {
		/// <summary>參數數量 0</summary>
		public override int GetParamLimit() { return 0; }
	}


	/// <summary>週期</summary>
	[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
	public class OnCycleAttribute : NotifiAttribute
    {
		/// <summary>參數數量 0</summary>
		public override int GetParamLimit() { return 0; }
	}

	/// <summary>改變</summary>
	[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
	public class OnChangeAttribute : NotifiAttribute
    {
		/// <summary>參數數量 1</summary>
		public override int GetParamLimit() { return 1; }
	}


	/// <summary>逾時</summary>
	[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
	public class OnTimeoutAttribute : NotifiAttribute
    {
		/// <summary>參數數量 1</summary>
		public override int GetParamLimit() { return 1; }
	}

	/// <summary>失敗</summary>
	[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
	public class OnFailureAttribute : NotifiAttribute
    {
		/// <summary>參數數量 1</summary>
		public override int GetParamLimit() { return 1; }
	}

	/// <summary>完成</summary>
	[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
	public class OnCompleteAttribute : NotifiAttribute
    {
		/// <summary>參數數量 1</summary>
		public override int GetParamLimit() { return 1; }
	}


}
