using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Orion.Api.Extensions;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Orion.Mvc.Extensions
{

    /// <summary>提供驗證碼產生與驗證的擴充方法。</summary>
    public static class CaptchaExtensions
    {
        private static Random _random = new Random();
        private static readonly FontCollection _fontCollection = new FontCollection();
        private static readonly object _fontLock = new object();
        private static FontFamily? _fontFamily;
        private static string _baseChars = "2345789ABCDEFGHJKLMNPRSTUVWXYZ";
        private static string _storeName = "CaptchaStore";
 


        /// <summary>從內嵌資源載入驗證碼字型。</summary>
        /// <returns>可用於繪製驗證碼的字型家族。</returns>
        /// <exception cref="InvalidOperationException">找不到內嵌字型資源時拋出。</exception>
        private static FontFamily ensureFontFamily()
        {
            if (_fontFamily.HasValue) { return _fontFamily.Value; }

            lock (_fontLock)
            {
                if (_fontFamily.HasValue) { return _fontFamily.Value; }

                var assembly = Assembly.GetExecutingAssembly();
                using Stream stream = assembly.GetManifestResourceStream("Orion.Mvc.OCR-b.ttf")
                    ?? throw new InvalidOperationException("找不到驗證碼字型資源 Orion.Mvc.OCR-b.ttf。");

                _fontFamily = _fontCollection.Add(stream);
                return _fontFamily.Value;
            }
        }

        /// <summary>解析驗證碼字型顏色字串。</summary>
        /// <param name="fontColor">HTML 色碼或色彩名稱。</param>
        /// <returns>ImageSharp 可使用的顏色物件。</returns>
        /// <exception cref="ArgumentException">`fontColor` 無法解析為有效顏色時拋出。</exception>
        private static Color parseCaptchaColor(string fontColor)
        {
            if (!Color.TryParse(fontColor, out Color color))
            {
                throw new ArgumentException($"無法解析驗證碼顏色：{fontColor}", nameof(fontColor));
            }

            return color;
        }

        /// <summary>依指定長度產生隨機驗證碼字串。</summary>
        /// <param name="length">驗證碼長度。</param>
        /// <returns>由 `_baseChars` 組成的隨機字串。</returns>
        private static string randomCode(int length)
        {
            string code = Enumerable.Range(0, length)
                .Select(i => _random.Next(0, _baseChars.Length))
                .Select(r => _baseChars[r])
                .JoinBy("");

            return code;
        }



        /*========================================================*/

        /// <summary>驗證 Controller 中輸入的驗證碼是否正確。</summary>
        /// <param name="controller">目前 Controller。</param>
        /// <param name="code">使用者輸入驗證碼。</param>
        /// <returns>驗證成功時回傳 `true`。</returns>
        public static bool IsCaptchaValid(this Controller controller, string code)
        {
            if (code.NoText()) { return false; }

            var store = controller.TempData[_storeName] as string;
            return code.Equals(store, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>產生 Controller 可回傳的驗證碼 PNG 圖片結果。</summary>
        /// <param name="controller">目前 Controller。</param>
        /// <param name="length">驗證碼長度。</param>
        /// <param name="colorName">字型顏色（HTML 色碼或名稱）。</param>
        /// <returns>PNG 影像串流結果。</returns>
        /// <exception cref="ArgumentException">`colorName` 無法解析為有效顏色時拋出。</exception>
        /// <exception cref="InvalidOperationException">找不到內嵌字型資源時拋出。</exception>
        public static FileStreamResult CaptchaResult(this Controller controller, int length, string colorName)
        {
            string code = randomCode(length); 
            controller.TempData[_storeName] = code;

            Stream stream = CreateCaptchaPng(code, colorName);
            return controller.File(stream, "image/png");
        }




        /*========================================================*/

        /// <summary>驗證 Razor Page 中輸入的驗證碼是否正確。</summary>
        /// <param name="page">目前 PageModel。</param>
        /// <param name="code">使用者輸入驗證碼。</param>
        /// <returns>驗證成功時回傳 `true`。</returns>
        public static bool IsCaptchaValid(this PageModel page, string code)
        {
            if (code.NoText()) { return false; }

            var store = page.TempData[_storeName] as string;
            return code.Equals(store, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>產生 Razor Page 可回傳的驗證碼 PNG 圖片結果。</summary>
        /// <param name="page">目前 PageModel。</param>
        /// <param name="length">驗證碼長度。</param>
        /// <param name="colorName">字型顏色（HTML 色碼或名稱）。</param>
        /// <returns>PNG 影像串流結果。</returns>
        /// <exception cref="ArgumentException">`colorName` 無法解析為有效顏色時拋出。</exception>
        /// <exception cref="InvalidOperationException">找不到內嵌字型資源時拋出。</exception>
        public static FileStreamResult CaptchaResult(this PageModel page, int length, string colorName)
        {
            string code = randomCode(length); 
            page.TempData[_storeName] = code;

            Stream stream = CreateCaptchaPng(code, colorName);
            return page.File(stream, "image/png");
        }





        /*========================================================*/

        /// <summary>建立驗證碼 PNG 圖檔串流。</summary>
        /// <param name="code">要繪製的驗證碼文字。</param>
        /// <param name="fontColor">字型顏色（HTML 色碼或名稱）。</param>
        /// <returns>包含 PNG 內容的可讀取串流。</returns>
        /// <exception cref="ArgumentException">`fontColor` 無法解析為有效顏色時拋出。</exception>
        /// <exception cref="InvalidOperationException">找不到內嵌字型資源時拋出。</exception>
        public static Stream CreateCaptchaPng(string code, string fontColor)
        {
            Color color = parseCaptchaColor(fontColor);
            FontFamily fontFamily = ensureFontFamily();
            Font font = fontFamily.CreateFont(34, FontStyle.Bold);

            int width = 30 * code.Length;
            int height = 34;

            var stream = new MemoryStream();
            using var image = new Image<Rgba32>(width, height, Color.White);
            image.Mutate(x =>
            {
                var options = new RichTextOptions(font)
                {
                    Origin = new PointF(0, -4)
                };
                x.DrawText(options, code, color);
            });
            image.SaveAsPng(stream, new PngEncoder());
            stream.Position = 0;

            return stream;
        }
         

    }
}
