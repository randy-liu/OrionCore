using System.Collections.Generic;
using System.IO;

namespace Orion.Api.Extensions
{
    /// <summary></summary>
    public static class TextReaderExtensions
    {
        /// <summary></summary>
        public static IEnumerable<string> Lines(this TextReader reader)
        {
            string line;
            while ((line = reader.ReadLine()) != null) { yield return line; }
        }
    }
}
