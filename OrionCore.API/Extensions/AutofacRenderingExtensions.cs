using Autofac;
using Orion.Api.Rendering;
using Orion.Api.Rendering.Contracts;
using Orion.Api.Rendering.Renderers;

namespace Orion.Api.Extensions
{
    /// <summary>提供渲染核心的 Autofac 註冊擴充方法。</summary>
    public static class AutofacRenderingExtensions
    {
        /// <summary>註冊 Orion 渲染核心元件。</summary>
        /// <param name="builder">容器建構器。</param>
        /// <returns>容器建構器本體。</returns>
        public static ContainerBuilder RegisterOrionRenderingCore(this ContainerBuilder builder)
        {
            builder.RegisterType<QuestPdfRenderer>().As<IRenderer>().SingleInstance();
            builder.RegisterType<ImageSharpPngRenderer>().As<IRenderer>().SingleInstance();
            builder.RegisterType<RenderGateway>().As<IRenderGateway>().SingleInstance();

            return builder;
        }
    }
}
