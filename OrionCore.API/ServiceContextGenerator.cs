using System;
using System.Collections.Concurrent;
using System.Reflection;
using Autofac;


namespace Orion.Api
{

    /// <summary></summary>
    public class ServiceContextGenerator : DispatchProxy
    {

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



        /// <summary></summary>
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
