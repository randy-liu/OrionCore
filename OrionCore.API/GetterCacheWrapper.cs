using System.Collections.Concurrent;
using System.Reflection;


namespace Orion.Api
{

    /// <summary>以 `DispatchProxy` 包裝物件，快取 getter 方法結果。</summary>
    public class GetterCacheWrapper : DispatchProxy
    {
        /// <summary>建立帶有 getter 快取能力的代理實例。</summary>
        /// <typeparam name="T">目標介面或型別。</typeparam>
        /// <param name="target">要包裝的目標物件。</param>
        /// <returns>代理後物件。</returns>
        public static T Wrap<T>(T target)
        {
            object proxy = Create<T, GetterCacheWrapper>();
            ((GetterCacheWrapper)proxy)._target = target;
            return (T)proxy;
        }



        /*==========================================================*/

        private readonly ConcurrentDictionary<string, object> _cache = new ConcurrentDictionary<string, object>();

        private object _target;


        /// <summary>攔截方法呼叫，對 getter 進行快取。</summary>
        /// <param name="targetMethod">被呼叫的方法。</param>
        /// <param name="args">方法參數。</param>
        /// <returns>方法執行結果。</returns>
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
