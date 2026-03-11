using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Orion.Api
{

    /// <summary>用於收集例外並於最後統一拋出的輔助工具。</summary>
    public class ExceptionCatcher
    {
        /// <summary>建立新的 <see cref="ExceptionCatcher"/> 執行個體。</summary>
        /// <returns>新的例外收集器。</returns>
        public static ExceptionCatcher Create()
        {
            return new ExceptionCatcher();
        }


        /*========================================*/


        /// <summary>目前累積的例外清單。</summary>
        public List<Exception> Errors { get; private set; } = new List<Exception>();

        /// <summary>執行同步動作並攔截發生的例外。</summary>
        /// <param name="action">要執行的同步動作。</param>
        /// <returns>目前收集器本身，供串接呼叫。</returns>
        public ExceptionCatcher Try(Action action)
        {
            try
            { action(); }
            catch (AggregateException e)
            { Errors.AddRange(e.InnerExceptions); }
            catch (Exception e)
            { Errors.Add(e); }

            return this;
        }

        /// <summary>執行非同步動作並攔截發生的例外。</summary>
        /// <param name="action">要執行的非同步動作。</param>
        /// <returns>目前收集器本身，供串接呼叫。</returns>
        public ExceptionCatcher Try(Func<Task> action)
        {
            return Try(() => action().Wait());
        }


        /// <summary>若有已收集例外，建立並拋出指定型別的例外。</summary>
        /// <typeparam name="T">要拋出的例外型別。</typeparam>
        /// <param name="message">例外訊息。</param>
        public void Throw<T>(string message) where T : Exception
        {
            if (Errors.Count == 0) { return; }

            var ex = Activator.CreateInstance(typeof(T), message, new AggregateException(Errors));
            throw (T)ex;
        }


        /// <summary>若有已收集例外，使用訊息產生器建立並拋出指定型別的例外。</summary>
        /// <typeparam name="T">要拋出的例外型別。</typeparam>
        /// <param name="messageFunc">依目前收集例外清單產生訊息的委派。</param>
        public void Throw<T>(Func<List<Exception>, string> messageFunc) where T : Exception
        {
            string message = messageFunc(Errors);
            Throw<T>(message);
        }


    }

}
