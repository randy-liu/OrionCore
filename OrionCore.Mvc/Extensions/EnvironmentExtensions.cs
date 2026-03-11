using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;

namespace Orion.Mvc.Extensions
{
    /// <summary>提供 `IWebHostEnvironment` 路徑處理擴充方法。</summary>
    public static class EnvironmentExtensions
    {

        /// <summary>將相對路徑轉為實體檔案路徑。</summary>
        /// <param name="env">Web 主機環境。</param>
        /// <param name="subpath">相對於內容根目錄的子路徑。</param>
        /// <returns>對應的實體路徑；無法對應時可能為 `null`。</returns>
        public static string MapPath(this IWebHostEnvironment env, string subpath)
        {
            IFileInfo fileInfo = env.ContentRootFileProvider.GetFileInfo(subpath);
            return fileInfo.PhysicalPath;
        }



    }



}
