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
		Debug,
		/// <summary></summary>
		Error,
		/// <summary></summary>
		Fatal,
		/// <summary></summary>
		Info,
		/// <summary></summary>
		Trace,
		/// <summary></summary>
		Warn,
	}


	/// <summary>事件通知者</summary>
	public interface INotifier
	{
		/// <summary>監看清單</summary>
		IEnumerable<NotifiMonitor> Monitors { get; }


		/// <summary>註冊監聽，建立 async method 時回傳型態請用 Task，這樣 Notifier 才有辦法攔截 Exception</summary>
		void RegisterListen(object handle);


		/// <summary>觸發初始</summary>
		void TriggerInit();
		/// <summary>觸發初始</summary>
		void TriggerInit(NotifierLog level);



		/// <summary>觸發關閉</summary>
		void TriggerClose();
		/// <summary>觸發關閉</summary>
		void TriggerClose(NotifierLog level);



		/// <summary>觸發週期</summary>
		void TriggerCycle();
		/// <summary>觸發週期</summary>
		void TriggerCycle(NotifierLog level);



		/// <summary>觸發改變</summary>
		void TriggerChange<T>(T value);
		/// <summary>觸發改變</summary>
		void TriggerChange<T>(T value, NotifierLog level);



		/// <summary>觸發逾時</summary>
		void TriggerTimeout<T>(T value);
		/// <summary>觸發逾時</summary>
		void TriggerTimeout<T>(T value, NotifierLog level);



		/// <summary>觸發失敗</summary>
		void TriggerFailure<T>(T value);
		/// <summary>觸發失敗</summary>
		void TriggerFailure<T>(T value, NotifierLog level);



		/// <summary>觸發完成</summary>
		void TriggerComplete<T>(T value);
		/// <summary>觸發完成</summary>
		void TriggerComplete<T>(T value, NotifierLog level);



		/// <summary>等待通知</summary>
		Task<T> Wait<T>(Func<T, bool> condition = null);
		/// <summary>等待通知</summary>
		Task<T> Wait<T>(int timeoutSec, Func<T, bool> condition = null);
	}









	/*########################################################*/

	/// <summary>事件通知者</summary>
	public class Notifier : INotifier
	{
		internal static readonly Type _noneType = typeof(Notifier);

		private readonly List<NotifiMonitor> _monitorList = new List<NotifiMonitor>();
		private readonly HashSet<object> _registeredListen = new HashSet<object>();


		private readonly List<IListen> _initListen = new List<IListen>();
		private readonly List<IListen> _closeListen = new List<IListen>();
		private readonly List<IListen> _cycleListen = new List<IListen>();
		private readonly List<IListen> _changeListen = new List<IListen>();
		private readonly List<IListen> _timeoutListen = new List<IListen>();
		private readonly List<IListen> _failureListen = new List<IListen>();
		private readonly List<IListen> _completeListen = new List<IListen>();
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



		private void add(List<IListen> listenList, NotifiAttribute attr, MethodMeta meta)
		{
			_log.Info("Register " + attr.GetName() + " " + meta.FullName);

			IListen listen = attr.MakeListen(meta);
			listenList.Add(listen);
		}





		/*=====================================================*/

		private Type getType<T>(T value)
		{
			return value != null ? value.GetType() : typeof(T);
		}


		private void setEventType(object model, NotifiStatus type)
		{
			var setable = model as INotifiStatusable;
			if (setable != null) { setable.SetNotifiStatus(type); }
		}



		private void logTrigger<T>(string triggerName, T value)
		{
			_log.Info(string.Format("{0} {1} {2}", triggerName, getType(value).Name, value.ToJson()));
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


		private void trigger(List<IListen> listenList, Type type, object model)
		{
			IEnumerable<IListen> list = listenList;
			if (type != _noneType) { list = list.Where(l => l.IsMatch(type)); }

			list.ForEach(l => l.Invoke(model));
		}






		/// <summary>觸發初始</summary>
		public void TriggerInit()
		{
			TriggerInit(NotifierLog.Trace);
		}
		/// <summary>觸發初始</summary>
		public void TriggerInit(NotifierLog level)
		{
			addLog(level, nameof(TriggerInit));
			trigger(_initListen, _noneType, null);
		}




		/// <summary>觸發關閉</summary>
		public void TriggerClose()
		{
			TriggerClose(NotifierLog.Trace);
		}
		/// <summary>觸發關閉</summary>
		public void TriggerClose(NotifierLog level)
		{
			addLog(level, nameof(TriggerClose));
			trigger(_closeListen, _noneType, null);
		}




		/// <summary>觸發週期</summary>
		public void TriggerCycle()
		{
			TriggerCycle(NotifierLog.Trace);
		}
		/// <summary>觸發週期</summary>
		public void TriggerCycle(NotifierLog level)
		{
			addLog(level, nameof(TriggerCycle));
			trigger(_cycleListen, _noneType, null);
		}



		/// <summary>觸發改變</summary>
		public void TriggerChange<T>(T value)
		{
			TriggerChange(value, NotifierLog.Info);
		}
		/// <summary>觸發改變</summary>
		public void TriggerChange<T>(T value, NotifierLog level)
		{
			Type type = getType(value);

			setEventType(value, NotifiStatus.Change);
			addLog(level, nameof(TriggerChange) + " " + type.Name + " " + value.ToJson());
			trigger(_changeListen, type, value);
			_waitListen.Trigger(type, value);
		}




		/// <summary>觸發逾時</summary>
		public void TriggerTimeout<T>(T value)
		{
			TriggerTimeout(value, NotifierLog.Warn);
		}
		/// <summary>觸發逾時</summary>
		public void TriggerTimeout<T>(T value, NotifierLog level)
		{
			Type type = getType(value);

			setEventType(value, NotifiStatus.Timeout);
			addLog(level, nameof(TriggerTimeout) + " " + type.Name + " " + value.ToJson());
			trigger(_timeoutListen, type, value);
			_waitListen.Trigger(type, value);
		}




		/// <summary>觸發失敗</summary>
		public void TriggerFailure<T>(T value)
		{
			TriggerFailure(value, NotifierLog.Error);
		}
		/// <summary>觸發失敗</summary>
		public void TriggerFailure<T>(T value, NotifierLog level)
		{
			Type type = getType(value);

			setEventType(value, NotifiStatus.Failure);
			addLog(level, nameof(TriggerFailure) + " " + type.Name + " " + value.ToJson());
			trigger(_failureListen, type, value);
			_waitListen.Trigger(type, value);
		}




		/// <summary>觸發完成</summary>
		public void TriggerComplete<T>(T value)
		{
			TriggerComplete(value, NotifierLog.Info);
		}
		/// <summary>觸發完成</summary>
		public void TriggerComplete<T>(T value, NotifierLog level)
		{
			Type type = getType(value);

			setEventType(value, NotifiStatus.Complete);
			addLog(level, nameof(TriggerComplete) + " " + type.Name + " " + value.ToJson());
			trigger(_completeListen, type, value);
			_waitListen.Trigger(type, value);
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
		public readonly bool HasParam;
		public readonly string FullName;


		public MethodMeta(object handle, MethodInfo method, IOrionLogger log)
		{
			Handle = handle;
			Method = method;
			Log = log;

			ParameterInfo[] parames = method.GetParameters();

			Monitor = new NotifiMonitor(handle, method);
			Target = parames.Select(x => x.ParameterType).DefaultIfEmpty(Notifier._noneType).First();
			ParamLength = parames.Length;
			HasParam = parames.Length > 0;
			FullName = method.DeclaringType.FullName + "." + method.Name;
		}
	}



	/*##########################################################################*/
	
	internal interface IListen
	{
		bool IsRun { get; }

		bool IsMatch(Type type);
		void Invoke(object model);
	}



	/// <summary>針對普通 method 的調用</summary>
	internal class NormalListen : IListen
	{
		public bool IsRun { get; set; }


		protected readonly MethodMeta Meta;

		public NormalListen(MethodMeta meta)
		{
			Meta = meta;
		}


		public bool IsMatch(Type type)
		{
			return Meta.Target.IsAssignableFrom(type);
		}


		public void Invoke(object model)
		{
			IsRun = true;
			var beginTime = DateTime.Now;

			object[] parameters = Meta.HasParam ? new object[] { model } : new object[] { };
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
	internal class AsyncListen : IListen
	{
		public bool IsRun { get; set; }


		protected readonly MethodMeta Meta;

		public AsyncListen(MethodMeta meta)
		{
			Meta = meta;
		}


		public bool IsMatch(Type type)
		{
			return Meta.Target.IsAssignableFrom(type);
		}


		public void Invoke(object model)
		{
			IsRun = true;
			invokeAsync(model);
		}

		private async void invokeAsync(object model)
		{
			await Task.Yield();
			var beginTime = DateTime.Now;

			object[] parameters = Meta.HasParam ? new object[] { model } : new object[] { };
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
	internal class OnlyOneListenWrapper : IListen
	{
		public bool IsRun { get { return _listen.IsRun; } }
		public bool IsMatch(Type type) { return _listen.IsMatch(type); }


		private readonly IListen _listen;

		public OnlyOneListenWrapper(IListen listen)
		{
			_listen = listen;
		}

		public void Invoke(object model)
		{
			if (_listen.IsRun) { return; }

			_listen.Invoke(model);
		}
	}



	/// <summary>針對間隔執行的調用</summary>
	internal class IntervalListenWrapper : IListen
	{
		public bool IsRun { get { return _listen.IsRun; } }
		public bool IsMatch(Type type) { return _listen.IsMatch(type); }


		private readonly IListen _listen;
		private readonly int _intervalSecs;

		public IntervalListenWrapper(IListen listen, int intervalSecs)
		{
			_listen = listen;
			_intervalSecs = intervalSecs;
		}


		/// <summary>下次的執行時間</summary>
		private DateTime _nextTime;

		public void Invoke(object model)
		{
			if (_nextTime > DateTime.Now) { return; }
			_nextTime = DateTime.Now.AddSeconds(_intervalSecs);

			_listen.Invoke(model);
		}
	}




}
