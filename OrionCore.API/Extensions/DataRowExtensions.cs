using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Orion.Api.Extensions
{
    /// <summary>定義 DataRow 的 Extension</summary>
    public static class DataRowExtensions
    {

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


        public static string String(this DataRow row, string columnName)
        {
            return toType<string>(row, columnName, null);
        }


        public static int Int(this DataRow row, string columnName)
        {
            return toType<int>(row, columnName, 0);
        }

        public static double Double(this DataRow row, string columnName)
        {
            return toType<double>(row, columnName, 0d);
        }

        public static decimal Decimal(this DataRow row, string columnName)
        {
            return toType<decimal>(row, columnName, 0m);
        }

        public static DateTime DateTime(this DataRow row, string columnName)
        {
            return toType<DateTime>(row, columnName, System.DateTime.MinValue);
        }

        public static DateTime? DateTimeN(this DataRow row, string columnName)
        {
            return toType<DateTime?>(row, columnName, null);
        }


        public static bool ContainsColumn(this DataRow row, string column)
        {
            if (row == null) { return false; }
            return row.Table.Columns.Contains(column);
        }







    }

}
