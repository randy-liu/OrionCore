using System;

namespace Orion.Api.Rendering
{
    /// <summary>描述渲染完成後的輸出內容。</summary>
    public sealed class RenderResult
    {
        /// <summary>建立渲染結果物件。</summary>
        /// <param name="format">輸出格式。</param>
        /// <param name="content">輸出位元組內容。</param>
        /// <param name="contentType">輸出 MIME 類型。</param>
        /// <param name="fileExtension">輸出檔案副檔名（含 `.`）。</param>
        /// <exception cref="ArgumentNullException">`content` 為 `null` 時拋出。</exception>
        /// <exception cref="ArgumentException">`contentType` 或 `fileExtension` 為空白時拋出。</exception>
        public RenderResult(RenderFormat format, byte[] content, string contentType, string fileExtension)
        {
            Format = format;
            Content = content ?? throw new ArgumentNullException(nameof(content));
            ContentType = string.IsNullOrWhiteSpace(contentType)
                ? throw new ArgumentException("內容類型不可為空白。", nameof(contentType))
                : contentType;
            FileExtension = string.IsNullOrWhiteSpace(fileExtension)
                ? throw new ArgumentException("副檔名不可為空白。", nameof(fileExtension))
                : fileExtension;
        }

        /// <summary>取得輸出格式。</summary>
        public RenderFormat Format { get; }

        /// <summary>取得輸出位元組內容。</summary>
        public byte[] Content { get; }

        /// <summary>取得輸出 MIME 類型。</summary>
        public string ContentType { get; }

        /// <summary>取得輸出檔案副檔名（含 `.`）。</summary>
        public string FileExtension { get; }
    }
}
