using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Orion.Mvc.Extensions
{

    /// <summary>提供 `IApplicationBuilder` 常用中介軟體註冊擴充方法。</summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>將表單中的 `handler` 欄位值寫入路由值。</summary>
        /// <param name="app">應用程式管線建構器。</param>
        /// <returns>原始 `IApplicationBuilder`。</returns>
        public static IApplicationBuilder UseFormDataToRouteHandler(this IApplicationBuilder app)
        {
            /* 增加 Middlewave */
            app.Use((context, next) =>
            {
                HttpRequest req = context.Request;

                if (req.HasFormContentType && req.Form.ContainsKey("handler"))
                { req.RouteValues["handler"] = req.Form["handler"]; }

                return next();
            });

            return app;
        }





        /// <summary>在例外發生時清理 Elmah XML 舊紀錄。</summary>
        /// <param name="app">應用程式管線建構器。</param>
        /// <param name="size">保留的最新紀錄檔數量。</param>
        /// <param name="logPath">Elmah XML 記錄檔目錄。</param>
        /// <returns>原始 `IApplicationBuilder`。</returns>
        public static IApplicationBuilder UseElmahClear(this IApplicationBuilder app, int size, string logPath)
        {
            if (logPath.StartsWith("~"))
            {
                var env = app.ApplicationServices.GetService<IWebHostEnvironment>();
                logPath = env.ContentRootFileProvider.GetFileInfo(logPath.Substring(1)).PhysicalPath;
            }

            /* 增加 Middlewave */
            app.Use(async (context, next) =>
            {
                try
                {
                    await next();
                }
                catch
                {
                    clearElmahOldXmlLog(size, logPath);
                    throw;
                }
            });

            return app;
        }


        private static bool _clearElmahFlag = false;

        /// <summary>清理 Elmah XML 舊紀錄，只保留指定數量的新檔案。</summary>
        /// <param name="size">保留的最新紀錄檔數量。</param>
        /// <param name="logPath">Elmah XML 記錄檔目錄。</param>
        private static void clearElmahOldXmlLog(int size, string logPath)
        {
            if (_clearElmahFlag) { return; }
            _clearElmahFlag = true;

            try
            {
                IEnumerable<FileInfo> files = new DirectoryInfo(logPath)
                    .EnumerateFiles("*.xml")
                    .OrderByDescending(file =>
                    {
                        try { return file.LastWriteTime; }
                        catch (Exception) { return DateTime.MaxValue; }
                    })
                    .Skip(size);

                foreach (var file in files)
                {
                    try { file.Delete(); } catch (Exception) { }
                }
            }
            catch (Exception ex)
            {
                NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
                log.Warn(ex, "Elmah clear error");
            }

            _clearElmahFlag = false;
            return;
        }










    }

}
