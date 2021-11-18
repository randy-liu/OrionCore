using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Orion.Api.Extensions
{
    /// <summary>定義 Queue, ConcurrentQueue 的 Extension</summary>
    public static class QueueExtensions
    {

        /// <summary></summary>
        public static IEnumerable<T> EnumerateDequeue<T>(this Queue<T> source)
        {
            while (source.Count > 0) { yield return source.Dequeue(); }
        }


        /// <summary></summary>
        public static IEnumerable<T> EnumerateDequeue<T>(this ConcurrentQueue<T> source)
        {
            T outValue;
            while (source.TryDequeue(out outValue)) { yield return outValue; }
        }

    }
}
