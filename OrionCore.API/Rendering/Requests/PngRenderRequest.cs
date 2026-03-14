using System;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Orion.Api.Rendering.Requests
{
    /// <summary>描述 PNG 渲染所需資訊。</summary>
    public sealed class PngRenderRequest : RenderRequest
    {
        /// <summary>建立 PNG 渲染請求。</summary>
        /// <param name="width">影像寬度（像素）。</param>
        /// <param name="height">影像高度（像素）。</param>
        /// <param name="rgbaPixels">RGBA32 像素資料。</param>
        /// <param name="pipeline">可選的 ImageSharp 後處理管線。</param>
        /// <exception cref="ArgumentOutOfRangeException">`width` 或 `height` 小於等於 0 時拋出。</exception>
        /// <exception cref="ArgumentNullException">`rgbaPixels` 為 `null` 時拋出。</exception>
        public PngRenderRequest(int width, int height, byte[] rgbaPixels, Action<Image<Rgba32>> pipeline = null)
            : base(RenderFormat.Png)
        {
            if (width <= 0) { throw new ArgumentOutOfRangeException(nameof(width), "寬度必須大於 0。"); }
            if (height <= 0) { throw new ArgumentOutOfRangeException(nameof(height), "高度必須大於 0。"); }

            Width = width;
            Height = height;
            RgbaPixels = rgbaPixels ?? throw new ArgumentNullException(nameof(rgbaPixels));
            Pipeline = pipeline;
        }

        /// <summary>取得影像寬度（像素）。</summary>
        public int Width { get; }

        /// <summary>取得影像高度（像素）。</summary>
        public int Height { get; }

        /// <summary>取得 RGBA32 像素資料。</summary>
        public byte[] RgbaPixels { get; }

        /// <summary>取得可選的 ImageSharp 後處理管線。</summary>
        public Action<Image<Rgba32>> Pipeline { get; }
    }
}
