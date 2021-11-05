using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace OrionCore.API.Extensions
{
	/// <summary>定義 DataTable 的 Extension</summary>
	public static class DataTableExtensions
	{

		/// <summary>將 DataRow 與 POCO Mapping，回傳 IEnumerable of POCO</summary>
		public static IEnumerable<TModel> ToModel<TModel>(this DataTable table) where TModel : new()
		{
			if (table == null) { throw new ArgumentNullException("table", "不可以為 Null"); }
			if (table.Rows.Count == 0) { return Enumerable.Empty<TModel>(); }


            Action<DataRow, TModel> mapping = (row, model) => { };

            foreach (var prop in typeof(TModel).GetProperties())
            {
                if (!prop.CanWrite) { continue; }
                if (!table.Columns.Contains(prop.Name)) { continue; }

                mapping += (row, model) =>
                {
                    object value = OrionUtils.ConvertType(row[prop.Name], prop.PropertyType);
                    if (value == null) { return; }

                    prop.SetValue(model, value);
                };
            }

            return table.Rows.Cast<DataRow>().Select(row =>
            {
                var model = new TModel();
                mapping(row, model);
                return model;
            });
		}



        /// <summary>將 IEnumerable of POCO 回傳 DataTable</summary>
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
