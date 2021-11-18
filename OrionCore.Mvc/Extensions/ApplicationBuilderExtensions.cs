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

    public static class ApplicationBuilderExtensions
    {
        /// <summary>使用 FormData 給路由 handler</summary>
        public static IApplicationBuilder UseFormDataToRouteHandler(this IApplicationBuilder app)
        {
            app.Use((context, next) =>
            {
                HttpRequest req = context.Request;

                if (req.HasFormContentType && req.Form.ContainsKey("handler"))
                { req.RouteValues["handler"] = req.Form["handler"]; }

                return next();
            });

            return app;
        }





        /// <summary>使用 Elmah 清除 XML 的舊紀錄</summary>
        public static IApplicationBuilder UseElmahClear(this IApplicationBuilder app, int size, string logPath)
        {
            if (logPath.StartsWith("~"))
            {
                var env = app.ApplicationServices.GetService<IWebHostEnvironment>();
                logPath = env.ContentRootFileProvider.GetFileInfo(logPath.Substring(1)).PhysicalPath;
            }

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

        /// <summary>清除 Elmah XML 的舊紀錄</summary>
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
