using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;

namespace Orion.Mvc.Extensions
{
    /// <summary></summary>
    public static class EnvironmentExtensions
    {

        /// <summary></summary>
        public static string MapPath(this IWebHostEnvironment env, string subpath)
        {
            IFileInfo fileInfo = env.ContentRootFileProvider.GetFileInfo(subpath);
            return fileInfo.PhysicalPath;
        }



    }



}
