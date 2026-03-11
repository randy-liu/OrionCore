using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Orion.Api.Extensions;

namespace Orion.Mvc.Extensions
{

    /// <summary>提供驗證碼產生與驗證的擴充方法。</summary>
    public static class CaptchaExtensions
    {
        private static Random _random = new Random();
        private static FontFamily _fontFamily;
        private static string _baseChars = "2345789ABCDEFGHJKLMNPRSTUVWXYZ";
        private static string _storeName = "CaptchaStore";
 


        /// <summary>載入驗證碼字型。</summary>
        /// <returns>字型載入成功時回傳 `FontFamily`，否則回傳 `null`。</returns>
        private static FontFamily ensureFontFamily()
        {
            if (_fontFamily != null) { return _fontFamily; }

            var assembly = Assembly.GetExecutingAssembly();

            using (var pfc = new PrivateFontCollection())
            using (Stream stream = assembly.GetManifestResourceStream("Orion.Mvc.OCR-b.ttf"))
            {
                if (stream == null) { return null; }

                byte[] fontData = new byte[stream.Length];
                stream.Read(fontData, 0, fontData.Length);

                IntPtr ptr = Marshal.AllocHGlobal(fontData.Length);
                Marshal.Copy(fontData, 0, ptr, fontData.Length);

                pfc.AddMemoryFont(ptr, fontData.Length);
                _fontFamily = pfc.Families[0];
                return _fontFamily;
            }
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
        public static Stream CreateCaptchaPng(string code, string fontColor)
        {
            Color color = ColorTranslator.FromHtml(fontColor);
            FontFamily fontFamily = ensureFontFamily();

            using var brush = new SolidBrush(color);
            int width = 30 * code.Length;
            int height = 34;

            var stream = new MemoryStream();
            using var bmp = new Bitmap(width, height);
            using var graphics = Graphics.FromImage(bmp);
            using var font = new Font(fontFamily, height, FontStyle.Bold);

            graphics.Clear(Color.White);
            graphics.DrawString(code, font, brush, 0, -4);

            bmp.Save(stream, ImageFormat.Png);
            stream.Position = 0;

            return stream;
        }
         

    }
}
