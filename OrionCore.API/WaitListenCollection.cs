using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Orion.Api.Extensions;

namespace Orion.Api
{

    /// <summary></summary>
    public class WaitListenCollection
    {
        private readonly object _listLock = new object();
        private readonly List<IWaitListen> _listenList = new List<IWaitListen>();
        private readonly List<Type> _waitTypes = new List<Type>();


        private bool containsType(Func<Type, bool> match)
        {
            /* 使用一般的 for 去規避線程安全 */
            for (int i = 0; i < _waitTypes.Count; i++)
            {
                if (match(_waitTypes[i])) { return true; }
            }
            return false;
        }


        /// <summary></summary>
        public Task<T> Add<T>(int timeoutSec, Func<T, bool> condition)
        {
            var listen = new WaitListen<T>(timeoutSec, condition);
            Task<T> task = listen.Start();

            lock (_listLock)
            {
                _listenList.RemoveAll(x => x.IsCompleted);
                _listenList.Add(listen);
            }

            Type type = typeof(T);
            if (!containsType(t => t == type)) { _waitTypes.Add(type); }

            return task;
        }


        /// <summary></summary>
        public void Trigger(Type type, object model)
        {
            if (_listenList.Count == 0) { return; }

            /* 檢查是否有等待的類型，降低 lock 時間  */
            if (!containsType(t => t.IsAssignableFrom(type))) { return; }


            List<IWaitListen> list;

            lock (_listLock)
            {
                /* 複製一份，降低 lock 時間 */
                list = _listenList.ToList();
            }

            list.Where(x => !x.IsCompleted)
                .Where(x => x.IsMatch(type, model))
                .ForEach(x => x.Invoke(model));
        }




        /*======================================================*/

        internal interface IWaitListen
        {
            bool IsCompleted { get; }
            bool IsMatch(Type type, object model);
            void Invoke(object model);
        }


        internal class WaitListen<TResult> : IWaitListen
        {
            private Type _target;
            private DateTime _timeoutLimit;
            private Func<TResult, bool> _condition;

            private bool _hasResult = false;
            private TResult _result;
            private Task<TResult> _task;


            public bool IsCompleted { get { return _task.IsCompleted; } }


            public WaitListen(int timeoutSec, Func<TResult, bool> condition)
            {
                _target = typeof(TResult);
                _timeoutLimit = DateTime.Now.AddSeconds(timeoutSec);
                _condition = condition ?? (x => true);
            }


            private async Task<TResult> resultHandle()
            {
                while (_timeoutLimit >= DateTime.Now)
                {
                    await Task.Delay(240);
                    if (_hasResult) { return _result; }
                }

                throw new TimeoutException($"{typeof(TResult).Name} 等待逾時");
            }


            public Task<TResult> Start()
            {
                _task = resultHandle();
                return _task;
            }

            public bool IsMatch(Type type, object model)
            {
                if (!_target.IsAssignableFrom(type)) { return false; }
                return (bool)_condition.DynamicInvoke(model);
            }

            public void Invoke(object model)
            {
                if (model is TResult)
                {
                    _result = (TResult)model;
                    _hasResult = true;
                }
            }
        }


    }

}
