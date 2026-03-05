using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Orion.Api.Extensions;

namespace Orion.Api
{


	/// <summary></summary>
	public enum NotifierLog
	{
		/// <summary></summary>
		Trace,
		/// <summary></summary>
		Debug,
		/// <summary></summary>
		Info,
		/// <summary></summary>
		Warn,
		/// <summary></summary>
		Error,
		/// <summary></summary>
		Fatal,
	}


	/// <summary>事件通知者</summary>
	public interface INotifier
	{
		/// <summary>監看清單</summary>
		IEnumerable<NotifiMonitor> Monitors { get; }


		/// <summary>註冊監聽，建立 async method 時回傳型態請用 Task，這樣 Notifier 才有辦法攔截 Exception</summary>
		void RegisterListen(object handle);


		/// <summary>觸發初始</summary>
		void TriggerInit(NotifierLog level = NotifierLog.Trace);

		/// <summary>觸發關閉</summary>
		void TriggerClose(NotifierLog level = NotifierLog.Trace);

		/// <summary>觸發週期</summary>
		void TriggerCycle(NotifierLog level = NotifierLog.Trace);


		/// <summary>觸發改變</summary>
		void TriggerChange<T>(T value, NotifierLog level = NotifierLog.Info);

		/// <summary>觸發逾時</summary>
		void TriggerTimeout<T>(T value, NotifierLog level = NotifierLog.Warn);

		/// <summary>觸發失敗</summary>
		void TriggerFailure<T>(T value, NotifierLog level = NotifierLog.Error);

		/// <summary>觸發完成</summary>
		void TriggerComplete<T>(T value, NotifierLog level = NotifierLog.Info);


		/// <summary>等待通知</summary>
		Task<T> Wait<T>(Func<T, bool> condition = null);
		/// <summary>等待通知</summary>
		Task<T> Wait<T>(int timeoutSec, Func<T, bool> condition = null);
	}









	/*########################################################*/

	/// <summary>事件通知者</summary>
	public class Notifier : INotifier
	{
		private readonly List<NotifiMonitor> _monitorList = new List<NotifiMonitor>();
		private readonly HashSet<object> _registeredListen = new HashSet<object>();


		private readonly List<INotifiListen> _initListen = new List<INotifiListen>();
		private readonly List<INotifiListen> _closeListen = new List<INotifiListen>();
		private readonly List<INotifiListen> _cycleListen = new List<INotifiListen>();
		private readonly List<INotifiListen> _changeListen = new List<INotifiListen>();
		private readonly List<INotifiListen> _timeoutListen = new List<INotifiListen>();
		private readonly List<INotifiListen> _failureListen = new List<INotifiListen>();
		private readonly List<INotifiListen> _completeListen = new List<INotifiListen>();
		private readonly WaitListenCollection _waitListen = new WaitListenCollection();

		private readonly IOrionLoggerFactory _logFactory;
		private readonly IOrionLogger _log;

		private bool _beginCycle = false;


		/// <summary>等待逾時秒數</summary>
		public int WaitTimeout { get; set; } = 20;

		/// <summary>監看清單</summary>
		public IEnumerable<NotifiMonitor> Monitors { get { return _monitorList; } }



		/// <summary>事件通知者</summary>
		public Notifier(IOrionLoggerFactory logFactory)
		{
			_logFactory = logFactory;
			_log = logFactory.Create(this.GetType());
		}



		/// <summary>註冊監聽，建立 async method 時回傳型態請用 Task，這樣 Notifier 才有辦法攔截 Exception</summary>
		public void RegisterListen(object handle)
		{
			if (_registeredListen.Contains(handle)) { return; }
			_registeredListen.Add(handle);


			foreach (MethodInfo method in handle.GetType().GetMethods())
			{
				var attrs = method.GetCustomAttributes<NotifiAttribute>();
				if (!attrs.Any()) { continue; }

				IOrionLogger log = _logFactory.Create(handle.GetType().Name);
				var meta = new MethodMeta(handle, method, log);
				_monitorList.Add(meta.Monitor);

				var initAttr = method.GetCustomAttribute<OnInitAttribute>();
				if (initAttr != null) { add(_initListen, initAttr, meta); }

				var closeAttr = method.GetCustomAttribute<OnCloseAttribute>();
				if (closeAttr != null) { add(_closeListen, closeAttr, meta); }

				var cycleAttr = method.GetCustomAttribute<OnCycleAttribute>();
				if (cycleAttr != null) { add(_cycleListen, cycleAttr, meta); }

				var changeAttr = method.GetCustomAttribute<OnChangeAttribute>();
				if (changeAttr != null) { add(_changeListen, changeAttr, meta); }

				var timeoutAttr = method.GetCustomAttribute<OnTimeoutAttribute>();
				if (timeoutAttr != null) { add(_timeoutListen, timeoutAttr, meta); }

				var failureAttr = method.GetCustomAttribute<OnFailureAttribute>();
				if (failureAttr != null) { add(_failureListen, failureAttr, meta); }

				var completeAttr = method.GetCustomAttribute<OnCompleteAttribute>();
				if (completeAttr != null) { add(_completeListen, completeAttr, meta); }
			}
		}



		private void add(List<INotifiListen> listenList, NotifiAttribute attr, MethodMeta meta)
		{
			_log.Info("Register " + attr.GetName() + " " + meta.FullName);

			INotifiListen listen = attr.MakeListen(meta);
			listenList.Add(listen);
		}





		/*=================================================================*/

		private Type getType<T>(T value)
		{
			return value != null ? value.GetType() : typeof(T);
		}


		private void setEventType(object model, NotifiStatus type)
		{
			var setable = model as INotifiStatusable;
			if (setable != null) { setable.SetNotifiStatus(type); }
		}



		private void addLog(NotifierLog level, string message)
		{
			switch (level)
			{
				default:
				case NotifierLog.Trace: _log.Trace(message); break;
				case NotifierLog.Debug: _log.Debug(message); break;
				case NotifierLog.Info: _log.Info(message); break;
				case NotifierLog.Warn: _log.Warn(message); break;
				case NotifierLog.Error: _log.Error(message); break;
				case NotifierLog.Fatal: _log.Fatal(message); break;
			}
		}

		private void trigger(List<INotifiListen> listenList, NotifierLog level, [CallerMemberName] string memberName = "")
		{
			addLog(level, memberName);

			var parameters = new object[] { };
			listenList.ForEach(l => l.Invoke(parameters));
		}


		private void trigger<T>(List<INotifiListen> listenList, NotifierLog level, T value, [CallerMemberName] string memberName = "")
		{
			Type type = getType(value);
			addLog(level, memberName + " " + type.Name + " " + value.ToJson());

			var parameters = new object[] { value };
			listenList.Where(l => l.IsMatch(type)).ForEach(l => l.Invoke(parameters));

			_waitListen.Trigger(type, value);
		}





		/// <summary>觸發初始</summary>
		public void TriggerInit(NotifierLog level = NotifierLog.Trace)
		{
			trigger(_initListen, level);
		}

		/// <summary>觸發關閉</summary>
		public void TriggerClose(NotifierLog level = NotifierLog.Trace)
		{
			trigger(_closeListen, level);
		}

		/// <summary>觸發週期</summary>
		public void TriggerCycle(NotifierLog level = NotifierLog.Trace)
		{
			trigger(_cycleListen, level);
		}



		/// <summary>觸發改變</summary>
		public void TriggerChange<T>(T value, NotifierLog level = NotifierLog.Info)
		{
			setEventType(value, NotifiStatus.Change);
			trigger(_changeListen, level, value);
		}

		/// <summary>觸發逾時</summary>
		public void TriggerTimeout<T>(T value, NotifierLog level = NotifierLog.Warn)
		{
			setEventType(value, NotifiStatus.Timeout);
			trigger(_timeoutListen, level, value);
		}

		/// <summary>觸發失敗</summary>
		public void TriggerFailure<T>(T value, NotifierLog level = NotifierLog.Error)
		{
			setEventType(value, NotifiStatus.Failure);
			trigger(_failureListen, level, value);
		}

		/// <summary>觸發完成</summary>
		public void TriggerComplete<T>(T value, NotifierLog level = NotifierLog.Info)
		{
			setEventType(value, NotifiStatus.Complete);
			trigger(_completeListen, level, value);
		}







		/*=====================================================*/

		/// <summary>啟動週期</summary>
		public void StartCycle(int cycleMilliseconds)
		{
			if (_beginCycle) { return; }
			_beginCycle = true;

			var thread = new Thread(() =>
			{
				while (_beginCycle)
				{
					var nextBegin = DateTime.Now.AddMilliseconds(cycleMilliseconds);

					TriggerCycle();

					/* Sleep 直到下一次開始 */
					while (DateTime.Now < nextBegin)
					{
						if (!_beginCycle) { return; }
						Thread.Sleep(100); /* 分段 Sleep 可以讓程式比較快關閉 */
					}
				}
			});
			thread.SetApartmentState(ApartmentState.MTA);
			thread.Priority = ThreadPriority.Highest;
			thread.Start();
		}


		/// <summary>停止週期</summary>
		public void StopCycle()
		{
			_beginCycle = false;
		}



		/*=====================================================*/

		/// <summary>等待通知，逾時會 throw System.TimeoutException</summary>
		public Task<T> Wait<T>(Func<T, bool> condition = null)
		{
			return Wait<T>(WaitTimeout, condition);
		}

		/// <summary>等待通知，逾時會 throw System.TimeoutException</summary>
		public Task<T> Wait<T>(int timeoutSec, Func<T, bool> condition = null)
		{
			return _waitListen.Add(timeoutSec, condition);
		}

	}






	/*##########################################################################*/

	internal class MethodMeta
	{
		public readonly object Handle;
		public readonly MethodInfo Method;
		public readonly IOrionLogger Log;

		public readonly NotifiMonitor Monitor;
		public readonly Type Target;

		public readonly int ParamLength;
		public readonly string FullName;


		public MethodMeta(object handle, MethodInfo method, IOrionLogger log)
		{
			Handle = handle;
			Method = method;
			Log = log;

			ParameterInfo[] parames = method.GetParameters();

			Monitor = new NotifiMonitor(handle, method);
			Target = parames.Select(x => x.ParameterType).FirstOrDefault();
			ParamLength = parames.Length;
			FullName = method.DeclaringType.FullName + "." + method.Name;
		}
	}



	/*##########################################################################*/

	internal interface INotifiListen
	{
		bool IsRun { get; }

		bool IsMatch(Type type);
		void Invoke(object[] parameters);
	}



	/// <summary>針對普通 method 的調用</summary>
	internal class NormalListen : INotifiListen
	{
		public bool IsRun { get; private set; }


		protected readonly MethodMeta Meta;

		public NormalListen(MethodMeta meta)
		{
			Meta = meta;
		}


		public bool IsMatch(Type type)
		{
			return Meta.Target.IsAssignableFrom(type);
		}


		public void Invoke(object[] parameters)
		{
			IsRun = true;
			var beginTime = DateTime.Now;

			try
			{
				/* 執行 Method */
				Meta.Method.Invoke(Meta.Handle, parameters);
			}
			catch (AggregateException ex)
			{
				string msg = string.Format("Error {0} params: {1}", Meta.FullName, parameters.ToJson());
				foreach (var inner in ex.InnerExceptions) { Meta.Log.Error(msg, inner); }
			}
			catch (Exception ex)
			{
				string msg = string.Format("Error {0} params: {1}", Meta.FullName, parameters.ToJson());
				Meta.Log.Error(msg, ex);
			}
			finally
			{
				IsRun = false;
				Meta.Monitor.Add(beginTime, DateTime.Now);
			}
		}
	}



	/// <summary>針對 async method 的呼叫</summary>
	internal class AsyncListen : INotifiListen
	{
		public bool IsRun { get; private set; }


		protected readonly MethodMeta Meta;

		public AsyncListen(MethodMeta meta)
		{
			Meta = meta;
		}


		public bool IsMatch(Type type)
		{
			return Meta.Target.IsAssignableFrom(type);
		}


		public void Invoke(object[] parameters)
		{
			IsRun = true;
			invokeAsync(parameters);
		}

		private async void invokeAsync(object[] parameters)
		{
			await Task.Yield();
			var beginTime = DateTime.Now;

			try
			{
				/* 執行 Method */
				Task task = Meta.Method.Invoke(Meta.Handle, parameters) as Task;
				if (task != null) { await task; }
			}
			catch (AggregateException ex)
			{
				string msg = string.Format("Error {0} params: {1}", Meta.FullName, parameters.ToJson());
				foreach (var inner in ex.InnerExceptions) { Meta.Log.Error(msg, inner); }
			}
			catch (Exception ex)
			{
				string msg = string.Format("Error {0} params: {1}", Meta.FullName, parameters.ToJson());
				Meta.Log.Error(msg, ex);
			}
			finally
			{
				IsRun = false;
				Meta.Monitor.Add(beginTime, DateTime.Now);
			}
		}
	}



	/*======================================================*/


	/// <summary>針對單一執行的調用</summary>
	internal class OnlyOneListenWrapper : INotifiListen
	{
		public bool IsRun { get { return _listen.IsRun; } }
		public bool IsMatch(Type type) { return _listen.IsMatch(type); }


		private readonly INotifiListen _listen;

		public OnlyOneListenWrapper(INotifiListen listen)
		{
			_listen = listen;
		}

		public void Invoke(object[] parameters)
		{
			if (_listen.IsRun) { return; }

			_listen.Invoke(parameters);
		}
	}



	/// <summary>針對間隔執行的調用</summary>
	internal class IntervalListenWrapper : INotifiListen
	{
		public bool IsRun { get { return _listen.IsRun; } }
		public bool IsMatch(Type type) { return _listen.IsMatch(type); }


		private readonly INotifiListen _listen;
		private readonly int _intervalSecs;

		public IntervalListenWrapper(INotifiListen listen, int intervalSecs)
		{
			_listen = listen;
			_intervalSecs = intervalSecs;
		}


		/// <summary>下次的執行時間</summary>
		private DateTime _nextTime;

		public void Invoke(object[] parameters)
		{
			if (_nextTime > DateTime.Now) { return; }
			_nextTime = DateTime.Now.AddSeconds(_intervalSecs);

			_listen.Invoke(parameters);
		}
	}




}
