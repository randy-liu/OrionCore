using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrionCore.API
{

    /// <summary></summary>
    public class ExceptionCatcher
    {
        /// <summary></summary>
        public static ExceptionCatcher Create()
        {
            return new ExceptionCatcher();
        }


        /*========================================*/


        /// <summary></summary>
        public List<Exception> Errors { get; private set; } = new List<Exception>();

        /// <summary></summary>
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

        /// <summary></summary>
        public ExceptionCatcher Try(Func<Task> action)
        {
            return Try(() => action().Wait());
        }


        /// <summary></summary>
        public void Throw<T>(string message) where T : Exception
        {
            if (Errors.Count == 0) { return; }

            var ex = Activator.CreateInstance(typeof(T), message, new AggregateException(Errors));
            throw (T)ex;
        }


        /// <summary></summary>
        public void Throw<T>(Func<List<Exception>, string> messageFunc) where T : Exception
        {
            string message = messageFunc(Errors);
            Throw<T>(message);
        }


    }

}
