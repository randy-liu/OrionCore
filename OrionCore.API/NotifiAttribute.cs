using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Orion.Api
{


	/// <summary>通知 Attribute</summary>
	[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
	public abstract class NotifiAttribute : Attribute
	{
		/// <summary>非同步執行</summary>
		public bool Async { get; set; }


		/// <summary>通知名稱</summary>
		public string GetName()
		{
			return GetType().Name.Replace("Attribute", (Async ? " Async" : ""));
		}

		/// <summary>參數數量</summary>
		public abstract int GetParamLimit();


		internal virtual INotifiListen MakeListen(MethodMeta meta)
		{
			if (meta.ParamLength != GetParamLimit())
			{ throw new ArgumentOutOfRangeException(meta.FullName, " 參數只能有" + GetParamLimit() + " 個"); }


			bool asyncMethod = meta.Method.GetCustomAttributes<AsyncStateMachineAttribute>().Any();

			if (asyncMethod && !typeof(Task).IsAssignableFrom(meta.Method.ReturnType))
			{ throw new ArgumentException(meta.FullName, "async 的 return type 必須是 Task"); }


			INotifiListen listen;

			if (asyncMethod || Async)
			{ listen = new AsyncListen(meta); }
			else
			{ listen = new NormalListen(meta); }

			return listen;
		}
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



	/// <summary>週期</summary>
	[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
	public class OnCycleAttribute : NotifiAttribute
	{
		/// <summary>唯一執行</summary>
		public bool OnlyOne { get; set; }

		/// <summary>間隔秒數</summary>
		public int IntervalSecs { get; set; }

		/// <summary>參數數量 0</summary>
		public override int GetParamLimit() { return 0; }


		internal override INotifiListen MakeListen(MethodMeta meta)
		{
			INotifiListen listen = base.MakeListen(meta);

			if (IntervalSecs > 0) { listen = new IntervalListenWrapper(listen, IntervalSecs); }
			if (OnlyOne) { listen = new OnlyOneListenWrapper(listen); }

			return listen;
		}

	}




}
