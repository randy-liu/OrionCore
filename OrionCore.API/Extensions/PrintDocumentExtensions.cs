using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Orion.Api.Extensions
{
    /// <summary>提供 `PrintDocument` 轉圖檔相關擴充方法。</summary>
    public static class PrintDocumentExtensions
    {
        /// <summary>將 `PrintDocument` 渲染為 `Bitmap`。</summary>
        /// <param name="doc">列印文件。</param>
        /// <returns>渲染後影像。</returns>
        public static Bitmap ToBitmap(this PrintDocument doc)
        {
            PageSettings settings = doc.DefaultPageSettings;

            var bitmap = new Bitmap(settings.PaperSize.Width, settings.PaperSize.Height);

            using var graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.White);

            var args = new PrintPageEventArgs(graphics, settings.Bounds, settings.Bounds, settings);

            MethodInfo method = doc.GetType()
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .First(x => x.Name == "OnPrintPage");

            method.Invoke(doc, new object[] { args });

            return bitmap;
        }


        /// <summary>將 `PrintDocument` 轉為指定格式影像串流。</summary>
        /// <param name="doc">列印文件。</param>
        /// <param name="format">影像格式。</param>
        /// <returns>影像串流。</returns>
        public static Stream ToImageStream(this PrintDocument doc, ImageFormat format)
        {
            using Bitmap bitmap = ToBitmap(doc);

            var stream = new MemoryStream();
            bitmap.Save(stream, format);
            stream.Seek(0, SeekOrigin.Begin);

            return stream;
        }

        /// <summary>將 `PrintDocument` 轉為 JPEG 影像串流。</summary>
        /// <param name="doc">列印文件。</param>
        /// <returns>JPEG 影像串流。</returns>
        public static Stream ToJpegStream(this PrintDocument doc)
        {
            return ToImageStream(doc, ImageFormat.Jpeg);
        }

        /// <summary>將 `PrintDocument` 轉為 PNG 影像串流。</summary>
        /// <param name="doc">列印文件。</param>
        /// <returns>PNG 影像串流。</returns>
        public static Stream ToPngStream(this PrintDocument doc)
        {
            return ToImageStream(doc, ImageFormat.Png);
        }


    }
}
