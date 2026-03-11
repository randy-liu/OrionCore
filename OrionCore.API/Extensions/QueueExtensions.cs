using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Orion.Api.Extensions
{
    /// <summary>定義 Queue, ConcurrentQueue 的 Extension</summary>
    public static class QueueExtensions
    {

		/// <summary>逐筆 Dequeue 一般 `Queue` 並回傳列舉結果。</summary>
		/// <typeparam name="T">元素型別。</typeparam>
		/// <param name="source">來源佇列。</param>
		/// <returns>被取出的元素序列。</returns>
        public static IEnumerable<T> EnumerateDequeue<T>(this Queue<T> source)
        {
            while (source.Count > 0) { yield return source.Dequeue(); }
        }


		/// <summary>逐筆 Dequeue `ConcurrentQueue` 並回傳列舉結果。</summary>
		/// <typeparam name="T">元素型別。</typeparam>
		/// <param name="source">來源佇列。</param>
		/// <returns>被取出的元素序列。</returns>
        public static IEnumerable<T> EnumerateDequeue<T>(this ConcurrentQueue<T> source)
        {
            T outValue;
            while (source.TryDequeue(out outValue)) { yield return outValue; }
        }

    }
}
