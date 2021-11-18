using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Orion.Api.Extensions
{
    public static class PrintDocumentExtensions
    {
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


        public static Stream ToImageStream(this PrintDocument doc, ImageFormat format)
        {
            using Bitmap bitmap = ToBitmap(doc);

            var stream = new MemoryStream();
            bitmap.Save(stream, format);
            stream.Seek(0, SeekOrigin.Begin);

            return stream;
        }

        public static Stream ToJpegStream(this PrintDocument doc)
        {
            return ToImageStream(doc, ImageFormat.Jpeg);
        }

        public static Stream ToPngStream(this PrintDocument doc)
        {
            return ToImageStream(doc, ImageFormat.Png);
        }


    }
}
