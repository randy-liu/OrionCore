using System;
using System.Collections.Concurrent;
using System.Linq;
using Autofac;
using Autofac.Builder;
using Autofac.Core;
using Autofac.Core.Registration;

namespace Orion.Api.Extensions
{


	/// <summary>Autofac 常用註冊擴充方法。</summary>
    public static class AutofacExtensions
    {

        /// <summary>註冊 ServiceContext 代理物件的建立方式。</summary>
        /// <typeparam name="TInterface">ServiceContext 介面型別。</typeparam>
        /// <param name="builder">容器建構器。</param>
        /// <returns>可繼續設定的註冊建構器。</returns>
        public static IRegistrationBuilder<TInterface, SimpleActivatorData, SingleRegistrationStyle> RegisterServiceContext<TInterface>(this ContainerBuilder builder)
        {
            return builder.Register(r =>
            {
                var proxy = ServiceContextGenerator.Create<TInterface>(r);
                return proxy;
            });
        }


        /// <summary>在元件啟用時套用 Getter 快取代理包裝。</summary>
        /// <typeparam name="TLimit">註冊限制型別。</typeparam>
        /// <typeparam name="TActivatorData">啟用資料型別。</typeparam>
        /// <typeparam name="TStyle">註冊樣式型別。</typeparam>
        /// <param name="registration">原始註冊設定。</param>
        /// <returns>套用包裝後的註冊設定。</returns>
        public static IRegistrationBuilder<TLimit, TActivatorData, TStyle> GetterCacheWrap<TLimit, TActivatorData, TStyle>(this IRegistrationBuilder<TLimit, TActivatorData, TStyle> registration)
        {
            registration.OnActivating(e =>
            {
                var proxy = GetterCacheWrapper.Wrap(e.Instance);
                e.ReplaceInstance(proxy);
            });

            return registration;
        }






        /*============================================================*/


        /// <summary>註冊單例 `Notifier`，並同時對外提供 `INotifier`。</summary>
        /// <param name="builder">容器建構器。</param>
        /// <returns>`Notifier` 的註冊建構器。</returns>
        public static IRegistrationBuilder<Notifier, ConcreteReflectionActivatorData, SingleRegistrationStyle> RegisterOrionNotifier(this ContainerBuilder builder)
        {
            return builder.RegisterType<Notifier>().SingleInstance().AsSelf().As<INotifier>();
        }


        /// <summary>註冊 Orion Logger 相關 Autofac 模組。</summary>
        /// <param name="builder">容器建構器。</param>
        /// <returns>模組註冊器。</returns>
        public static IModuleRegistrar RegisterOrionLogger(this ContainerBuilder builder)
        {
            return builder.RegisterModule<OrionLoggerModule>();
        }



        /// <summary>Autofac NLog 模組，會根據 class Type 配置 log name</summary>
        class OrionLoggerModule : Module
        {

            /// <summary>在模組載入後註冊類別到 ContainerBuilder</summary>
            /// <param name="builder">Autofac 容器建構器。</param>
            protected override void Load(ContainerBuilder builder)
            {
                builder.RegisterType<OrionNLogLogger>().As<IOrionLogger>();
                builder.RegisterType<OrionNLogLoggerFactory>().As<IOrionLoggerFactory>();
            }


            /// <summary>配置 Resolve 事件，可以在物件解析建構子 Parameter 時，取得被注入的類別</summary>
            /// <param name="registry">元件登錄建構器。</param>
            /// <param name="registration">目前正在登錄的元件定義。</param>
            protected override void AttachToComponentRegistration(IComponentRegistryBuilder registry, IComponentRegistration registration)
            {
                registration.Preparing += (object sender, PreparingEventArgs e) =>
                {
                    /* 取得要建構的類別 Type */
                    Type typePreparing = e.Component.Activator.LimitType;

                    /* 建立 ResolvedParameter，會根據條件判斷才去建立需要的類別 */
                    var parameter = new ResolvedParameter(
                        (p, i) => p.ParameterType == typeof(IOrionLogger),
                        (p, i) => new OrionNLogLogger(typePreparing)
                    );

                    /* 增加 Parameter 到優先候選清單中 */
                    e.Parameters = e.Parameters.Concat(new[] { parameter });
                };
            }

        }

         
    }
}
