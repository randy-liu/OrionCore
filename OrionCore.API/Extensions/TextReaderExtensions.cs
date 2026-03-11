using System.Collections.Generic;
using System.IO;

namespace Orion.Api.Extensions
{
    /// <summary>提供 `TextReader` 逐行讀取擴充方法。</summary>
    public static class TextReaderExtensions
    {
        /// <summary>以列舉方式逐行讀取文字內容。</summary>
        /// <param name="reader">文字讀取器。</param>
        /// <returns>逐行文字序列。</returns>
        public static IEnumerable<string> Lines(this TextReader reader)
        {
            string line;
            while ((line = reader.ReadLine()) != null) { yield return line; }
        }
    }
}
