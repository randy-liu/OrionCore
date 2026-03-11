using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Orion.Api.Extensions
{
	/// <summary>定義 DataTable 的 Extension</summary>
	public static class DataTableExtensions
	{

        /// <summary>將 `DataTable` 轉為 Dictionary。</summary>
        /// <typeparam name="TKey">Dictionary 鍵值型別。</typeparam>
        /// <typeparam name="TElement">Dictionary 值型別。</typeparam>
        /// <param name="table">資料表。</param>
        /// <param name="keySelector">鍵值選擇器。</param>
        /// <param name="elementSelector">元素選擇器。</param>
        /// <returns>由指定鍵值選擇器建立的 Dictionary。</returns>
        public static Dictionary<TKey, TElement> ToDictionary<TKey, TElement>(this DataTable table, Func<DataRow, TKey> keySelector, Func<DataRow, TElement> elementSelector)
        {
            if (table == null) { return new Dictionary<TKey, TElement>(); }
            return table.AsEnumerable().ToDictionary(keySelector, elementSelector);
        }

        /// <summary>以 tuple 鍵值選擇器將 `DataTable` 轉為 Dictionary。</summary>
        /// <typeparam name="TKey">Dictionary 鍵值型別。</typeparam>
        /// <typeparam name="TElement">Dictionary 值型別。</typeparam>
        /// <param name="table">資料表。</param>
        /// <param name="keyValueSelector">鍵值選擇器。</param>
        /// <returns>由 tuple 鍵值建立的 Dictionary。</returns>
        public static Dictionary<TKey, TElement> ToDict<TKey, TElement>(this DataTable table, Func<DataRow, (TKey, TElement)> keyValueSelector)
        {
            return table.AsEnumerable().Select(keyValueSelector).ToDictionary(x => x.Item1, x => x.Item2);
        }




        /// <summary>取得第一筆資料列，找不到時回傳 `null`。</summary>
        /// <param name="table">資料表。</param>
        /// <returns>第一筆資料列；無資料時為 `null`。</returns>
        public static DataRow FirstOrDefault(this DataTable table)
        {
            return table.AsEnumerable().FirstOrDefault();
        }
        /// <summary>依條件取得第一筆資料列，找不到時回傳 `null`。</summary>
        /// <param name="table">資料表。</param>
        /// <param name="predicate">篩選條件。</param>
        /// <returns>符合條件的第一筆資料列；無資料時為 `null`。</returns>
        public static DataRow FirstOrDefault(this DataTable table, Func<DataRow, bool> predicate)
        {
            return table.AsEnumerable().FirstOrDefault(predicate);
        }


        /// <summary>投影 `DataTable` 為指定序列。</summary>
        /// <typeparam name="T">投影後元素型別。</typeparam>
        /// <param name="table">資料表。</param>
        /// <param name="selector">選擇器。</param>
        /// <returns>投影後序列。</returns>
        public static IEnumerable<T> Select<T>(this DataTable table, Func<DataRow, T> selector)
        {
            return table.AsEnumerable().Select(selector);
        }

        /// <summary>篩選符合條件的資料列。</summary>
        /// <param name="table">資料表。</param>
        /// <param name="predicate">篩選條件。</param>
        /// <returns>符合條件的資料列序列。</returns>
        public static IEnumerable<DataRow> Where(this DataTable table, Func<DataRow, bool> predicate)
        {
            return table.AsEnumerable().Where(predicate);
        }

        /// <summary>判斷 `DataTable` 是否包含任一資料列。</summary>
        /// <param name="table">資料表。</param>
        /// <returns>有任一資料列時回傳 `true`。</returns>
        public static bool Any(this DataTable table)
        {
            return table.AsEnumerable().Any();            
        }

        /// <summary>判斷是否存在符合條件的資料列。</summary>
        /// <param name="table">資料表。</param>
        /// <param name="predicate">篩選條件。</param>
        /// <returns>存在符合條件資料列時回傳 `true`。</returns>
        public static bool Any(this DataTable table, Func<DataRow, bool> predicate)
        {
            return table.AsEnumerable().Any(predicate);
        }


        /// <summary>將 `DataTable` 投影後轉為 `List`。</summary>
        /// <typeparam name="T">投影後元素型別。</typeparam>
        /// <param name="table">資料表。</param>
        /// <param name="selector">選擇器。</param>
        /// <returns>投影後的 `List`。</returns>
        public static List<T> ToList<T>(this DataTable table, Func<DataRow, T> selector)
        {
            return table.AsEnumerable().ToList(selector);
        }

        /// <summary>將 `DataTable` 投影後轉為 `HashSet`。</summary>
        /// <typeparam name="T">投影後元素型別。</typeparam>
        /// <param name="table">資料表。</param>
        /// <param name="selector">選擇器。</param>
        /// <returns>投影後的 `HashSet`。</returns>
        public static HashSet<T> ToHashSet<T>(this DataTable table, Func<DataRow, T> selector)
        {
            return table.AsEnumerable().ToHashSet(selector);
        }




        /*##############################################################*/

        /// <summary>將 `DataTable` 每筆 `DataRow` 映射為模型序列。</summary>
        /// <typeparam name="TModel">目標模型型別。</typeparam>
        /// <param name="table">來源資料表。</param>
        /// <returns>轉換後模型序列。</returns>
        public static IEnumerable<TModel> ToModel<TModel>(this DataTable table) where TModel : new()
        {
            if (table == null) { throw new ArgumentNullException("table", "不可以為 Null"); }
            if (table.Rows.Count == 0) { return Enumerable.Empty<TModel>(); }

            Action<DataRow, TModel> mapping = (row, model) => { };

            Dictionary<string, PropertyInfo> props = typeof(TModel).GetProperties().ToDictionary(p => p.Name.ToUpper());

            foreach (DataColumn col in table.Columns)
            {
                string colName = col.ColumnName;
                PropertyInfo prop = props.GetValueOrDefault(colName.ToUpper());

                if (prop == null) { throw new ArgumentException($"缺少 {col.ColumnName} 的 Property"); }
                if (!prop.CanWrite) { throw new ArgumentException($"{col.ColumnName} 的 Property 不可寫入"); }
 
                mapping += (row, model) =>
                {
                    object value = row[colName].ConvertTo(prop.PropertyType);
                    if (value == null) { return; }

                    prop.SetValue(model, value);
                };
            }

 
            return table.AsEnumerable().Select(row =>
            {
                var model = new TModel();
                mapping(row, model);
                return model;
            });
        }

         
        /// <summary>將模型序列轉為 `DataTable`。</summary>
        /// <typeparam name="TModel">來源模型型別。</typeparam>
        /// <param name="source">來源資料。</param>
        /// <returns>轉換後 `DataTable`。</returns>
        public static DataTable ToDataTable<TModel>(this IEnumerable<TModel> source) 
        {
            if (source == null) { throw new ArgumentNullException("source", "不可以為 Null"); }

            var table = new DataTable();

            Action<TModel, DataRow> mapping = (model, row) => { };

            foreach (var prop in typeof(TModel).GetProperties())
            {
                if (!prop.CanRead) { continue; }

                table.Columns.Add(prop.Name, prop.PropertyType);
                               
                mapping += (model, row) =>
                {
                    row[prop.Name] = prop.GetValue(model);
                };
            }

            foreach (var model in source)
            {
                var row = table.NewRow();
                mapping(model, row);
                table.Rows.Add(row);
            }

            return table;
        }




    }

}
