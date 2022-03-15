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
		private static readonly Type _noneType = typeof(Notifier);

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

				bool useAsync = attrs.Any(x => x.Async);
				bool onlyOne = attrs.Any(x => x.OnlyOne);

				IListen listen = makeListen(handle, method, useAsync, onlyOne);
				_monitorList.Add(listen.Monitor);


				var initAttr = method.GetCustomAttribute<OnInitAttribute>();
				if (initAttr != null) { add(_initListen, initAttr, method, listen); }

				var closeAttr = method.GetCustomAttribute<OnCloseAttribute>();
				if (closeAttr != null) { add(_closeListen, closeAttr, method, listen); }

				var cycleAttr = method.GetCustomAttribute<OnCycleAttribute>();
				if (cycleAttr != null) { add(_cycleListen, cycleAttr, method, listen); }

				var changeAttr = method.GetCustomAttribute<OnChangeAttribute>();
				if (changeAttr != null) { add(_changeListen, changeAttr, method, listen); }

				var timeoutAttr = method.GetCustomAttribute<OnTimeoutAttribute>();
				if (timeoutAttr != null) { add(_timeoutListen, timeoutAttr, method, listen); }

				var failureAttr = method.GetCustomAttribute<OnFailureAttribute>();
				if (failureAttr != null) { add(_failureListen, failureAttr, method, listen); }

				var completeAttr = method.GetCustomAttribute<OnCompleteAttribute>();
				if (completeAttr != null) { add(_completeListen, completeAttr, method, listen); }
			}

		}





		private IListen makeListen(object handle, MethodInfo method, bool useAsync, bool onlyOne)
		{
			bool asyncMethod = method.GetCustomAttributes<AsyncStateMachineAttribute>().Any();

			if (asyncMethod && !typeof(Task).IsAssignableFrom(method.ReturnType))
			{
				var fullName = method.DeclaringType.FullName + "." + method.Name;
				throw new ArgumentException(fullName, "async 的 return type 必須是 Task");
			}


			IOrionLogger handleLog = _logFactory.Create(handle.GetType().Name);

			IListen listen;

			if (asyncMethod && onlyOne)
			{ listen = new AsyncOnlyOneListen(handle, method, handleLog); }
			else if (asyncMethod)
			{ listen = new AsyncListen(handle, method, handleLog); }
			else if (useAsync && onlyOne)
			{ listen = new NormalOnlyOneListen(handle, method, handleLog); }
			else if (useAsync)
			{ listen = new NormalAsyncListen(handle, method, handleLog); }
			else
			{ listen = new NormalListen(handle, method, handleLog); }

			return listen;
		}


		private void add(List<IListen> listenList, NotifiAttribute attr, MethodInfo method, IListen listen)
		{
			var fullName = method.DeclaringType.FullName + "." + method.Name;
			int paramLimit = attr.GetParamLimit();

			_log.Info("Register " + attr.GetName() + " " + fullName);

			if (method.GetParameters().Length != paramLimit)
			{ throw new ArgumentOutOfRangeException(fullName, " 參數只能有" + paramLimit + " 個"); }

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





		/*##########################################################################*/

		internal interface IListen
		{
			NotifiMonitor Monitor { get; }

			bool IsMatch(Type type);
			void Invoke(object model);
		}



		/// <summary>針對普通 method 的調用</summary>
		internal class NormalListen : IListen
		{
			protected readonly object Handle;
			protected readonly MethodInfo Method;
			protected readonly IOrionLogger Log;

			protected readonly string ExecName;
			protected readonly bool HasParam;

			private readonly Type _target;


			public NotifiMonitor Monitor { get; private set; }

			public NormalListen(object handle, MethodInfo method, IOrionLogger log)
			{
				Handle = handle;
				Method = method;
				Log = log;

				Monitor = new NotifiMonitor(handle, method);

				ParameterInfo[] parames = method.GetParameters();
				_target = parames.Select(x => x.ParameterType).DefaultIfEmpty(_noneType).First();
				ExecName = method.DeclaringType.FullName + "." + method.Name;
				HasParam = parames.Length > 0;
			}


			public bool IsMatch(Type type)
			{
				return _target.IsAssignableFrom(type);
			}


			public virtual void Invoke(object model)
			{
				var beginTime = DateTime.Now;

				object[] parameters = HasParam ? new object[] { model } : new object[] { };
				try
				{
					/* 執行 Method */
					Method.Invoke(Handle, parameters);
				}
				catch (AggregateException ex)
				{
					string msg = string.Format("Error {0} params: {1}", ExecName, parameters.ToJson());
					foreach (var inner in ex.InnerExceptions) { Log.Error(msg, inner); }
				}
				catch (Exception ex)
				{
					string msg = string.Format("Error {0} params: {1}", ExecName, parameters.ToJson());
					Log.Error(msg, ex);
				}

				Monitor.Add(beginTime, DateTime.Now);
			}


		}


		/// <summary>針對普通 method 非同步的調用</summary>
		internal class NormalAsyncListen : NormalListen
		{
			public NormalAsyncListen(object handle, MethodInfo method, IOrionLogger log) : base(handle, method, log) { }


			public override void Invoke(object model)
			{
				Task.Run(() => { base.Invoke(model); });
			}
		}


		/// <summary>針對普通 method 非同步單一執行的調用</summary>
		internal class NormalOnlyOneListen : NormalListen
		{
			public NormalOnlyOneListen(object handle, MethodInfo method, IOrionLogger log) : base(handle, method, log) { }


			private bool _runFlag = false;

			public override void Invoke(object model)
			{
				if (_runFlag) { return; }
				_runFlag = true;

				Task.Run(() =>
				{
					base.Invoke(model);
					_runFlag = false;
				});
			}
		}



		/*======================================================*/

		/// <summary>針對 async method 的呼叫</summary>
		internal class AsyncListen : NormalListen
		{
			public AsyncListen(object handle, MethodInfo method, IOrionLogger log) : base(handle, method, log) { }


			public override void Invoke(object model)
			{
				_ = InvokeAsync(model);
			}

			protected virtual async Task InvokeAsync(object model)
			{
				var beginTime = DateTime.Now;

				object[] parameters = HasParam ? new object[] { model } : new object[] { };
				try
				{
					/* 執行 Method */
					await (Task)Method.Invoke(Handle, parameters);
				}
				catch (AggregateException ex)
				{
					string msg = string.Format("Error {0} params: {1}", ExecName, parameters.ToJson());
					foreach (var inner in ex.InnerExceptions) { Log.Error(msg, inner); }
				}
				catch (Exception ex)
				{
					string msg = string.Format("Error {0} params: {1}", ExecName, parameters.ToJson());
					Log.Error(msg, ex);
				}

				Monitor.Add(beginTime, DateTime.Now);
			}
		}


		/// <summary>針對普通 method 非同步單一執行的調用</summary>
		internal class AsyncOnlyOneListen : AsyncListen
		{
			public AsyncOnlyOneListen(object handle, MethodInfo method, IOrionLogger log) : base(handle, method, log) { }


			private bool _runFlag = false;


			public override void Invoke(object model)
			{
				if (_runFlag) { return; }
				_runFlag = true;
				_ = InvokeAsync(model);
			}

			protected override async Task InvokeAsync(object model)
			{
				await base.InvokeAsync(model);
				_runFlag = false;
			}

		}




	}



}
