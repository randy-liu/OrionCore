using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Orion.Api.Extensions
{
    /// <summary>定義 DataRow 的 Extension</summary>
    public static class DataRowExtensions
    {

        /// <summary>將資料列轉為欄位名稱對應值的 Dictionary。</summary>
        /// <param name="row">資料列。</param>
        /// <returns>欄位與值對照的 Dictionary。</returns>
        public static Dictionary<string, object> ToDictionary(this DataRow row)
        {
            if (row == null) { return null; }

            var dict = new Dictionary<string, object>();

            foreach (DataColumn col in row.Table.Columns)
            { dict[col.ColumnName] = row[col]; }

            return dict;
        }




        /*##############################################################*/

        private static T toType<T>(DataRow row, string columnName, T empty)
        {
            if (row == null) { return empty; }

            object value = row[columnName];
            if (value == null) { return empty; }
            if (value is T tValue) { return tValue; }

            return value.ConvertTo<T>();
        }


        /// <summary>取得字串欄位值。</summary>
        /// <param name="row">資料列。</param>
        /// <param name="columnName">欄位名稱。</param>
        /// <returns>欄位字串值。</returns>
        public static string String(this DataRow row, string columnName)
        {
            return toType<string>(row, columnName, null);
        }



        /// <summary>取得整數欄位值。</summary>
        /// <param name="row">資料列。</param>
        /// <param name="columnName">欄位名稱。</param>
        /// <returns>欄位整數值；無法轉換時回傳 0。</returns>
        public static int Int(this DataRow row, string columnName)
        {
            return toType<int>(row, columnName, 0);
        }
        /// <summary>取得可空整數欄位值。</summary>
        /// <param name="row">資料列。</param>
        /// <param name="columnName">欄位名稱。</param>
        /// <returns>欄位整數值；無法轉換時回傳 `null`。</returns>
        public static int? IntN(this DataRow row, string columnName)
        {
            return toType<int?>(row, columnName, null);
        }



        /// <summary>取得雙精度浮點數欄位值。</summary>
        /// <param name="row">資料列。</param>
        /// <param name="columnName">欄位名稱。</param>
        /// <returns>欄位雙精度值；無法轉換時回傳 0。</returns>
        public static double Double(this DataRow row, string columnName)
        {
            return toType<double>(row, columnName, 0d);
        }
        /// <summary>取得可空雙精度浮點數欄位值。</summary>
        /// <param name="row">資料列。</param>
        /// <param name="columnName">欄位名稱。</param>
        /// <returns>欄位雙精度值；無法轉換時回傳 `null`。</returns>
        public static double? DoubleN(this DataRow row, string columnName)
        {
            return toType<double?>(row, columnName, null);
        }



        /// <summary>取得十進位欄位值。</summary>
        /// <param name="row">資料列。</param>
        /// <param name="columnName">欄位名稱。</param>
        /// <returns>欄位十進位值；無法轉換時回傳 0。</returns>
        public static decimal Decimal(this DataRow row, string columnName)
        {
            return toType<decimal>(row, columnName, 0m);
        }
        /// <summary>取得可空十進位欄位值。</summary>
        /// <param name="row">資料列。</param>
        /// <param name="columnName">欄位名稱。</param>
        /// <returns>欄位十進位值；無法轉換時回傳 `null`。</returns>
        public static decimal? DecimalN(this DataRow row, string columnName)
        {
            return toType<decimal?>(row, columnName, null);
        }



        /// <summary>取得日期時間欄位值。</summary>
        /// <param name="row">資料列。</param>
        /// <param name="columnName">欄位名稱。</param>
        /// <returns>欄位日期時間值；無法轉換時回傳 `DateTime.MinValue`。</returns>
        public static DateTime DateTime(this DataRow row, string columnName)
        {
            return toType<DateTime>(row, columnName, System.DateTime.MinValue);
        }
        /// <summary>取得可空日期時間欄位值。</summary>
        /// <param name="row">資料列。</param>
        /// <param name="columnName">欄位名稱。</param>
        /// <returns>欄位日期時間值；無法轉換時回傳 `null`。</returns>
        public static DateTime? DateTimeN(this DataRow row, string columnName)
        {
            return toType<DateTime?>(row, columnName, null);
        }




        /// <summary>判斷資料列是否包含指定欄位。</summary>
        /// <param name="row">資料列。</param>
        /// <param name="column">欄位名稱。</param>
        /// <returns>包含指定欄位時回傳 `true`。</returns>
        public static bool ContainsColumn(this DataRow row, string column)
        {
            if (row == null) { return false; }
            return row.Table.Columns.Contains(column);
        }


    }

}
