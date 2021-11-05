using System;
using System.Reflection;
using Autofac;
using Autofac.Builder;

namespace OrionCore.API.Extensions
{


    /// <summary></summary>
    public static class AutofacExtensions
    {

        /// <summary>註冊 ServiceContext</summary>
        public static IRegistrationBuilder<TInterface, SimpleActivatorData, SingleRegistrationStyle> RegisterServiceContext<TInterface>(this ContainerBuilder builder)
        {
            return builder.Register(r => 
            {
                var proxy = ServiceContextGenerator.Create<TInterface>(r);
                return proxy;
            });
        }


        /// <summary>啟用 Getter 快取包裝</summary>
        public static IRegistrationBuilder<TLimit, TActivatorData, TStyle> GetterCacheWrap<TLimit, TActivatorData, TStyle>(this IRegistrationBuilder<TLimit, TActivatorData, TStyle> registration)
        {
            registration.OnActivating(e =>
            {
                var proxy = GetterCacheWrapper.Wrap(e.Instance);
                //object proxy = DispatchProxy.Create<TLimit, GetterCacheWrapper>();
                //((GetterCacheWrapper)proxy).SetTarget(e.Instance);

                e.ReplaceInstance(proxy);
            });

            return registration;
        }
    }
}
