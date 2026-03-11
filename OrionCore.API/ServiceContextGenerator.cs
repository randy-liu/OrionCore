using System;
using System.Collections.Concurrent;
using System.Reflection;
using Autofac;


namespace Orion.Api
{

    /// <summary>使用 <see cref="DispatchProxy"/> 依回傳型別從 DI 容器解析服務。</summary>
    public class ServiceContextGenerator : DispatchProxy
    {

        /// <summary>建立服務內容代理物件。</summary>
        /// <typeparam name="T">要代理的服務型別。</typeparam>
        /// <param name="resolver">用於解析服務的 Autofac 容器。</param>
        /// <returns>可攔截方法並回傳對應服務的代理實例。</returns>
        public static T Create<T>(IComponentContext resolver)
        {
            object proxy = Create<T, ServiceContextGenerator>();
            var generator = (ServiceContextGenerator)proxy;

            generator._targetType = typeof(T);

            var factory = resolver.Resolve<ILifetimeScope>();
            bool isDisposable = typeof(IDisposable).IsAssignableFrom(generator._targetType);
            generator._scope = isDisposable ? factory.BeginLifetimeScope() : factory;

            return (T)proxy;
        }



        /*==========================================================*/

        private ConcurrentDictionary<Type, object> _cache = new ConcurrentDictionary<Type, object>();

        private ILifetimeScope _scope;
        private Type _targetType;



        private object dispose()
        {
            _scope?.Dispose();
            _cache?.Clear();
            _scope = null; /* 避免記憶體洩漏 */
            _cache = null;
            _targetType = null;
            return null;
        }



        /// <summary>攔截方法呼叫並回傳對應服務或處理基礎方法。</summary>
        /// <param name="targetMethod">被呼叫的方法資訊。</param>
        /// <param name="args">方法參數。</param>
        /// <returns>方法執行結果，通常為從容器解析出的服務實例。</returns>
        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            switch (targetMethod.Name)
            {
                case nameof(GetType): return _targetType;
                case nameof(GetHashCode): return GetHashCode();
                case nameof(ToString): return ToString();
                case nameof(IDisposable.Dispose): return dispose();
            }


            Type type = targetMethod.ReturnType;
            if (type == null) { throw new NotImplementedException(); }

            object result = _cache.GetOrAdd(type, _ => _scope.Resolve(type));
            return result;
        }

    }

}

