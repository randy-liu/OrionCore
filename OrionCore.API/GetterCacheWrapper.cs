using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace OrionCore.API
{

    /// <summary></summary>
    public class GetterCacheWrapper : DispatchProxy
    {
        /// <summary></summary>
        public static T Wrap<T>(T target)
        {
            object proxy = Create<T, GetterCacheWrapper>();
            ((GetterCacheWrapper)proxy)._target = target;
            return (T)proxy;
        }



        /*==========================================================*/

        private readonly ConcurrentDictionary<string, object> _cache = new ConcurrentDictionary<string, object>();

        private object _target;


        /// <summary></summary>
        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            string cacheKey = targetMethod.Name;
            object target = _target; /* 避免 delegate 記憶體洩漏 */

            object result;
            if (cacheKey.StartsWith("get_"))
            { result = _cache.GetOrAdd(cacheKey, _ => targetMethod.Invoke(target, args)); }
            else
            { result = targetMethod.Invoke(target, args); }

            return result;
        }

    }



}
