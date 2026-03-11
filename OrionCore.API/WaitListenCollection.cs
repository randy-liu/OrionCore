using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Orion.Api.Extensions;

namespace Orion.Api
{

    /// <summary>等待通知的監聽集合，支援條件與逾時機制。</summary>
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


        /// <summary>新增等待監聽並回傳可等待的工作。</summary>
        /// <typeparam name="T">等待的資料型別。</typeparam>
        /// <param name="timeoutSec">逾時秒數。</param>
        /// <param name="condition">符合條件時才完成等待。</param>
        /// <returns>等待結果工作。</returns>
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


        /// <summary>觸發指定型別通知，喚醒符合條件的等待監聽。</summary>
        /// <param name="type">通知資料型別。</param>
        /// <param name="model">通知資料內容。</param>
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


            /// <summary>建立等待監聽項目。</summary>
            /// <param name="timeoutSec">等待逾時秒數。</param>
            /// <param name="condition">結果比對條件。</param>
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


            /// <summary>啟動等待流程。</summary>
            /// <returns>等待結果的非同步工作。</returns>
            public Task<TResult> Start()
            {
                _task = resultHandle();
                return _task;
            }

            /// <summary>判斷事件是否符合此等待項目。</summary>
            /// <param name="type">事件型別。</param>
            /// <param name="model">事件資料。</param>
            /// <returns>符合型別且符合條件時回傳 true。</returns>
            public bool IsMatch(Type type, object model)
            {
                if (!_target.IsAssignableFrom(type)) { return false; }
                return (bool)_condition.DynamicInvoke(model);
            }

            /// <summary>接收事件資料並寫入等待結果。</summary>
            /// <param name="model">事件資料。</param>
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
