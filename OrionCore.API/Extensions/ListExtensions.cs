using System;
using System.Collections.Generic;
using System.Linq;

namespace Orion.Api.Extensions
{
    /// <summary>定義 IList 的 Extension</summary>
    public static class ListExtensions
    {

        /// <summary>取出並移除清單第一筆元素。</summary>
        /// <typeparam name="T">元素型別。</typeparam>
        /// <param name="list">清單資料。</param>
        /// <returns>第一筆元素；清單為空時回傳型別預設值。</returns>
        public static T Shift<T>(this IList<T> list)
        {
            if (list.Count == 0) { return default(T); }

            T data = list[0];
            list.RemoveAt(0);
            return data;
        }


        /// <summary>依條件取出第一筆元素並自清單移除。</summary>
        /// <typeparam name="T">元素型別。</typeparam>
        /// <param name="list">清單資料。</param>
        /// <param name="predicate">篩選條件。</param>
        /// <returns>符合條件的元素；找不到時回傳型別預設值。</returns>
        public static T TakeOut<T>(this IList<T> list, Func<T, bool> predicate)
        {
            if (list.Count == 0) { return default(T); }
            T data = list.FirstOrDefault(predicate);
            if (data == null) { return data; }

            list.Remove(data);
            return data;
        }



    }
}
