using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrionCore.API;
using OrionCore.API.Extensions;

namespace OrionCore.Mvc.Extensions
{

	/// <summary></summary>
	public static class CaptchaExtensions
	{
		private static Random _random = new Random();
		private static FontFamily _fontFamily;
		private static string _baseChars = "2345789ABCDEFGHJKLMNPRSTUVWXYZ";
		private static string _storeName = "CaptchaStore";

		public static Color FontColor { get; set; } = Color.FromArgb(45, 109, 163);
		public static int Length { get; set; } = 5;



		/// <summary>載入字型檔</summary>
		private static FontFamily ensureFontFamily()
		{
			if (_fontFamily != null) { return _fontFamily; }

			var assembly = Assembly.GetExecutingAssembly();

			using (var pfc = new PrivateFontCollection())
			using (Stream stream = assembly.GetManifestResourceStream("OrionCore.Mvc.OCR-b.ttf"))
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
		 



		/// <summary></summary>
		public static bool IsCaptchaValid(this Controller controller, string code)
		{
			if(code.NoText()) { return false; }

			var store = controller.TempData[_storeName] as string;
			return code.Equals(store, StringComparison.OrdinalIgnoreCase);
		}


		/// <summary></summary>
		public static FileStreamResult CaptchaResult(this Controller controller)
		{
			string code = "";
			for (var i = 0; i < Length; i++)
			{
				code += _baseChars[_random.Next(0, _baseChars.Length)];
			}

			controller.TempData[_storeName] = code;

			Stream stream = CreateCaptchaPng(code);
			return controller.File(stream, "image/png");
		}



		/// <summary></summary>
		public static Stream CreateCaptchaPng(string code)
		{
			FontFamily fontFamily = ensureFontFamily();

			var brush = new SolidBrush(FontColor);
			int width = 30 * Length;
			int height = 34;


			var stream = new MemoryStream();
			using (var bmp = new Bitmap(width, height))
			using (var graphics = Graphics.FromImage(bmp))
			using (var font = new Font(fontFamily, height, FontStyle.Bold))
			{
				graphics.Clear(Color.White);
				graphics.DrawString(code, font, brush, 0, -4);

				bmp.Save(stream, ImageFormat.Png);
				stream.Position = 0;
			}

			return stream;
		}

	}
}
