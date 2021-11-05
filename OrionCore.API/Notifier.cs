using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using OrionCore.API.Extensions;

namespace OrionCore.API
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
		private readonly Type _anyType = typeof(Notifier);

		private readonly ConcurrentDictionary<string, bool> _runFlag = new ConcurrentDictionary<string, bool>();
		private readonly List<NotifiMonitor> _monitorList = new List<NotifiMonitor>();
		private readonly HashSet<object> _registeredListen = new HashSet<object>();


		private readonly ListenCollection _initListen = new ListenCollection();
		private readonly ListenCollection _closeListen = new ListenCollection();
		private readonly ListenCollection _cycleListen = new ListenCollection();
		private readonly ListenCollection _changeListen = new ListenCollection();
		private readonly ListenCollection _timeoutListen = new ListenCollection();
		private readonly ListenCollection _failureListen = new ListenCollection();
		private readonly ListenCollection _completeListen = new ListenCollection();
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
				if (method.GetCustomAttributes<AsyncStateMachineAttribute>().Any() && !typeof(Task).IsAssignableFrom(method.ReturnType))
				{
					var fullName = method.DeclaringType.FullName + "." + method.Name;
					throw new ArgumentException(fullName, "async 的 return type 必須是 Task");
				}

				var attrs = method.GetCustomAttributes<NotifiAttribute>();
				if (!attrs.Any()) { continue; }

				var monitor = new NotifiMonitor(handle, method);
				_monitorList.Add(monitor);

				var paramType = method.GetParameters().Select(x => x.ParameterType).FirstOrDefault();

				var initAttr = method.GetCustomAttribute<OnInitAttribute>();
				if (initAttr != null) { add(_initListen, _anyType, initAttr, handle, method, monitor); }

				var closeAttr = method.GetCustomAttribute<OnCloseAttribute>();
				if (closeAttr != null) { add(_closeListen, _anyType, closeAttr, handle, method, monitor); }

				var cycleAttr = method.GetCustomAttribute<OnCycleAttribute>();
				if (cycleAttr != null) { add(_cycleListen, _anyType, cycleAttr, handle, method, monitor); }

				var changeAttr = method.GetCustomAttribute<OnChangeAttribute>();
				if (changeAttr != null) { add(_changeListen, paramType, changeAttr, handle, method, monitor); }

				var timeoutAttr = method.GetCustomAttribute<OnTimeoutAttribute>();
				if (timeoutAttr != null) { add(_timeoutListen, paramType, timeoutAttr, handle, method, monitor); }

				var failureAttr = method.GetCustomAttribute<OnFailureAttribute>();
				if (failureAttr != null) { add(_failureListen, paramType, failureAttr, handle, method, monitor); }

				var completeAttr = method.GetCustomAttribute<OnCompleteAttribute>();
				if (completeAttr != null) { add(_completeListen, paramType, completeAttr, handle, method, monitor); }

			}

		}





		private void add(ListenCollection listenCollection, Type type, NotifiAttribute attr, object handle, MethodInfo method, NotifiMonitor monitor)
		{
			var fullName = method.DeclaringType.FullName + "." + method.Name;
			int paramLimit = attr.GetParamLimit();

			_log.Info("Register " + attr.GetName() + " " + fullName);

			if (method.GetParameters().Length != paramLimit)
			{
				throw new ArgumentOutOfRangeException(fullName, " 參數只能有" + paramLimit + " 個");
			}

			Action<object> listen = makeListen(attr, handle, method, monitor);
			listenCollection.Add(type, listen, attr.Async);
		}




		private Action<object> makeListen(NotifiAttribute attr, object handle, MethodInfo method, NotifiMonitor monitor)
		{
			IOrionLogger handleLog = _logFactory.Create(handle.GetType().Name);
			string execName = attr.GetName() + " " + method.DeclaringType.FullName + "." + method.Name;
			bool hasParam = (attr.GetParamLimit() == 1);
			
			Action<object> listen = (x =>
			{
				var beginTime = DateTime.Now;

				object[] parameters = null;
				try
				{
					/* 執行 Method */
					parameters = hasParam ? new object[] { x } : new object[] { };

					Task result = method.Invoke(handle, parameters) as Task;
					if (result != null){ result.Wait(); }
				}
				catch (AggregateException ex)
				{
					string msg = string.Format("Error {0} params: {1}", execName, parameters.ToJson());
					foreach (var inner in ex.InnerExceptions) { handleLog.Error(msg, inner); }
				}
				catch (Exception ex)
				{
					string msg = string.Format("Error {0} params: {1}", execName, parameters.ToJson());
					handleLog.Error(msg, ex);
				}
				finally
				{
					monitor.Add(beginTime, DateTime.Now);
				}
			});

			if (!attr.OnlyOne) { return listen; }
			

			string runKey = handle.GetHashCode() + "_" + method.Name;
			_runFlag[runKey] = false;

			return x =>
			{
				if (_runFlag[runKey]) { return; }

				_runFlag[runKey] = true;
				listen(x);
				_runFlag[runKey] = false; 
			};
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

		

		/// <summary>觸發初始</summary>
		public void TriggerInit()
		{
			TriggerInit(NotifierLog.Trace);
		}
		/// <summary>觸發初始</summary>
		public void TriggerInit(NotifierLog level)
		{
			addLog(level, nameof(TriggerInit));
			_initListen.Trigger(_anyType, null);
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
			_closeListen.Trigger(_anyType, null);
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
			_cycleListen.Trigger(_anyType, null);
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
			_changeListen.Trigger(type, value);
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
			_timeoutListen.Trigger(type, value);
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
			_failureListen.Trigger(type, value);
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
			_completeListen.Trigger(type, value);
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
					TriggerCycle();
					Thread.Sleep(cycleMilliseconds);
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




	/*########################################################*/

	internal class ListenCollection
	{

		private Dictionary<Type, Action<object>> _listenMap = new Dictionary<Type, Action<object>>();
		private Dictionary<Type, Action<object>> _listenMapAsync = new Dictionary<Type, Action<object>>();


		public void Add(Type type, Action<object> callback, bool isAsync)
		{
			var target = isAsync ? _listenMapAsync : _listenMap;

			if (target.ContainsKey(type))
			{ target[type] += callback; }
			else
			{ target[type] = callback; }
		}


		public void Trigger(Type type, object model)
		{
			foreach (var pair in _listenMapAsync)
			{
				if (!pair.Key.IsAssignableFrom(type)) { continue; }

				foreach (Delegate listen in pair.Value.GetInvocationList())
				{
					ThreadPool.QueueUserWorkItem(m => { listen.DynamicInvoke(m); }, model);
				}
			}

			foreach (var pair in _listenMap)
			{
				if (!pair.Key.IsAssignableFrom(type)) { continue; }

				pair.Value.DynamicInvoke(model);
			}
		}
	}


	 


}
