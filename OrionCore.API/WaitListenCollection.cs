using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OrionCore.API.Extensions;

namespace OrionCore.API
{

    /// <summary></summary>
    public class WaitListenCollection
    {

        private readonly object _lockFlag = new object();
        private readonly List<TriggerInfo> _triggerList = new List<TriggerInfo>();
        private readonly List<IWaitListen> _listenList = new List<IWaitListen>();

        private bool _isCheckTimeout = false;


        private DateTime getEffectTime()
        {
            return DateTime.Now.AddMilliseconds(-600);
        }


        /// <summary></summary>
        public Task<T> Add<T>(int timeoutSec, Func<T, bool> condition)
        {
            var listen = new WaitListen<T>(timeoutSec, condition);

            lock (_lockFlag)
            {
                DateTime effect = getEffectTime();
                TriggerInfo info = _triggerList
                    .TakeWhile(x => x.Time >= effect)
                    .Where(x => listen.IsMatch(x.Type, x.Model))
                    .FirstOrDefault();

                if(info != null)
                {
                    listen.Invoke(info.Model);
                    return listen.Task;
                }


                _listenList.Add(listen);

                if (!_isCheckTimeout)
                {
                    _isCheckTimeout = true;

                    var thread = new Thread(checkTimeout);
                    thread.SetApartmentState(ApartmentState.MTA);
                    thread.IsBackground = true;
                    thread.Start();
                }
            }
            return listen.Task;
        }

 


        /// <summary></summary>
        public void Trigger(Type type, object model)
        {
            List<IWaitListen> list;

            lock (_lockFlag)
            {
                DateTime effect = getEffectTime();
                _triggerList.RemoveAll(x => x.Time < effect);
                _triggerList.Insert(0, new TriggerInfo(type, model));

                list = _listenList
                    .Where(x => !x.IsCompleted)
                    .Where(x => x.IsMatch(type, model))
                    .ToList();
            }

            list.ForEach(x => x.Invoke(model));
        }


        private void checkTimeout()
        {
            while (true)
            {
                Thread.Sleep(883);

                IWaitListen[] list;

                lock (_lockFlag)
                {
                    if (_listenList.Count == 0)
                    {
                        _isCheckTimeout = false;
                        return;
                    }

                    _listenList.RemoveAll(x => x.IsCompleted);
                    if (_listenList.Count == 0) { continue; }

                    list = _listenList.ToArray();
                }

                list.ForEach(x => x.CheckTimeout());
            }
        }


        class TriggerInfo
        {
            public DateTime Time;
            public Type Type;
            public object Model;

            public TriggerInfo(Type type, object model)
            {
                Time = DateTime.Now;
                Type = type;
                Model = model;
            }
        }
    }


    internal interface IWaitListen
    {
        bool IsTimeout { get; }
        bool IsCompleted { get; }
        bool IsMatch(Type type, object model);
        void Invoke(object model);
        void CheckTimeout();
    }


    internal class WaitListen<TResult> : IWaitListen
    {
        private object _invokeLock = new object();

        private TResult _result;
        private DateTime _timeoutLimit;
        private Func<TResult, bool> _condition;

        public bool IsTimeout { get; private set; }
        public bool IsCompleted { get { return Task.IsCompleted; } }
        public Task<TResult> Task { get; private set; }


        public WaitListen(int timeoutSec, Func<TResult, bool> condition)
        {
            _timeoutLimit = DateTime.Now.AddSeconds(timeoutSec);
            _condition = condition ?? (x => true);
            Task = new Task<TResult>(resultHandle);
        }


        private TResult resultHandle()
        {
            if (!IsTimeout) { return _result; }

            throw new TimeoutException($"{typeof(TResult).Name} 等待逾時");
        }


        public bool IsMatch(Type type, object model)
        {
            if (!typeof(TResult).IsAssignableFrom(type)) { return false; }
            return (bool)_condition.DynamicInvoke(model);
        }


        public void Invoke(object model)
        {
            if (model is TResult) { _result = (TResult)model; }

            lock (_invokeLock)
            {
                if (!Task.IsCompleted) { Task.RunSynchronously(); }
            }
        }


        public void CheckTimeout()
        {
            if (Task.IsCompleted) { return; }
            if (_timeoutLimit > DateTime.Now) { return; }

            lock (_invokeLock)
            {
                if (Task.IsCompleted) { return; }
                IsTimeout = true;
                Task.RunSynchronously();
            }
        }

    }


}
