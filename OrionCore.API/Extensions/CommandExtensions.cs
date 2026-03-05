using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text.RegularExpressions;
using Orion.Api.Models;

namespace Orion.Api.Extensions
{

	/// <summary></summary>
	public static class CommandExtensions
	{

		/// <summary></summary>
		public static string ParamPrefix(this DbCommand command)
		{
			string prefix = command.GetType().Name.Contains("Oracle") ? ":" : "@";
			return prefix;
		}


		/// <summary></summary>
		public static DbParameter AddParameter(this DbCommand command, string name, object value)
		{
			DbParameter param = command.CreateParameter();
			param.ParameterName = name;			
			command.Parameters.Add(param);


			if (value == null)
			{ param.Value = DBNull.Value; return param; }

			if (value is bool boolean)
			{ param.Value = boolean ? 1 : 0; return param; }

			if (value is Enum @enum)
			{ param.Value = @enum.ToString(); return param; }


			var booleanN = value as bool?;
			if (booleanN != null)
			{ param.Value = booleanN.Value ? 1 : 0; return param; }


			param.Value = value;
			return param;
		}



		private static string clearSelect(string commandText)
		{
			string upper = commandText.ToUpper();

			int depth = 0;
			int length = upper.Length;
			int fromPoint = upper.IndexOf("FROM");
			for (int i = 0; i < length; i++)
			{
				if (upper[i] == '(') { depth++; continue; }
				if (depth == 0 && i >= fromPoint) { break; }
				if (upper[i] != ')') { continue; }

				depth--;
				if (i > fromPoint) { fromPoint = upper.IndexOf("FROM", i); }
			}

			/* 忽略最後的 ORDER BY 語法 */
			int find = upper.LastIndexOf("ORDER BY");
			if (find != -1) { commandText = commandText.Substring(0, find); }

			return "SELECT 1 " + commandText.Substring(fromPoint);
		}



		private static int getCommandCount(DbCommand command)
		{
			string orgCommandText = command.CommandText;
			List<DbParameter> paramList = command.Parameters.Cast<DbParameter>().ToList();


			command.CommandText = $"SELECT COUNT(*) FROM ({clearSelect(orgCommandText)}) PAGE_QUERY";

			command.Parameters.Clear();
			paramList
				.Where(x => command.CommandText.Contains(x.ParameterName))
				.AddRangeTo(command.Parameters);

			int totalItems = command.FetchOne<int>();


			command.CommandText = orgCommandText;

			command.Parameters.Clear();
			paramList.AddRangeTo(command.Parameters);

			return totalItems;
		}




		/// <summary></summary>
		public static bool IsDataExists(this DbCommand command)
		{
			if (!command.Connection.State.HasFlag(ConnectionState.Open))
			{ command.Connection.Open(); }

			string commandText = command.CommandText;
			command.CommandText = $@"SELECT COUNT(*) FROM DUAL WHERE EXISTS( {commandText} )";

			var result = command.FetchOne<bool>();
			command.CommandText = commandText;

			return result;
		}



		/// <summary></summary>
		public static DbCommand AddCommand(this DbCommand command, FormattableString commandText)
		{
			string prefix = command.ParamPrefix();

			DbParameterCollection cmdParams = command.Parameters;
			object[] args = commandText.GetArguments();

			string format = commandText.Format;
			var names = new string[args.Length];

			for (int i = 0; i < args.Length; i++)
			{
				string name = $"{prefix}p{i + cmdParams.Count}";
				object value = args[i];

				if (value is IEnumerable && !(value is string))
				{
					var valueList = (value as IEnumerable).Cast<object>().ToList((v, j) => new { Name = name + "_" + j, Value = v });

					if (valueList.Count == 0)
					{
						names[i] = name;
						AddParameter(command, name, null);
					}
					else
					{
						names[i] = valueList.Select(x => x.Name).JoinBy(", ");
						foreach (var item in valueList) { AddParameter(command, item.Name, item.Value); }
					}
				}
				else
				{
					Match match = Regex.Match(format, "{" + i + ":([^}]+)}"); /* {0(:[^}]+)} */
					if (match.Success)
					{
						value = string.Format("{0:" + match.Groups[1] + "}", value);
						format = format.Replace(match.Value, "{" + i + "}");
					}

					names[i] = name;
					AddParameter(command, name, value);
				}
			}
			command.CommandText += string.Format(format, names);

			return command;
		}



		/// <summary></summary>
		public static DbCommand Where(this DbCommand command, object whereValues)
		{
			return Where(command, toDictionary(whereValues));
		}

		/// <summary></summary>
		public static DbCommand Where(this DbCommand command, IDictionary<string, object> whereValues)
		{
			string prefix = command.ParamPrefix();

			string whereCondition = whereValues.Keys.Select(x => $"{x} = {prefix}{x}").JoinBy(" AND ");
			command.CommandText += " AND " + whereCondition;

			foreach (var item in whereValues)
			{ AddParameter(command, prefix + item.Key, item.Value); }

			return command;
		}


		/// <summary></summary>
		public static DbCommand Where(this DbCommand command, string whereSql, object value)
		{
			string prefix = command.ParamPrefix();
			string name = Regex.Match(whereSql, $@"(?<={prefix})\w+").Value;

			if (value is IEnumerable && !(value is string))
			{
				var valueList = (value as IEnumerable).Cast<object>().ToList((v, i) => new { Name = name + i, Value = v });

				if (valueList.Count == 0)
				{
					AddParameter(command, prefix + name, null);
				}
				else
				{
					foreach (var item in valueList) { AddParameter(command, prefix + item.Name, item.Value); }

					string nameList = valueList.Select(x => prefix + x.Name).JoinBy(", ");
					whereSql = whereSql.Replace(prefix + name, nameList);
				}
			}
			else
			{
				AddParameter(command, prefix + name, value);
			}

			command.CommandText += " AND " + whereSql;

			return command;
		}




		/// <summary></summary>
		public static DbCommand WhereHas(this DbCommand command, string whereSql, object value)
		{
			if (!OrionUtils.HasValue(value)) { return command; }
			return Where(command, whereSql, value);
		}



		/// <summary></summary>
		public static DbCommand WhereHas(this DbCommand command, string whereSql, object value, DbType dbType)
		{
			if (!OrionUtils.HasValue(value)) { return command; }

			string prefix = command.ParamPrefix();
			string name = Regex.Match(whereSql, $@"(?<={prefix})\w+").Value;
			AddParameter(command, prefix + name, value).DbType = dbType;
			command.CommandText += " AND " + whereSql;

			return command;
		}


		/// <summary></summary>
		public static DbCommand OrderBy(this DbCommand command, string fields, string defaultFields = null)
		{
			if (fields.NoText()) { fields = defaultFields; }

			string orderSql = Regex.Matches(fields, @"(-?)(\w+)")
				.Cast<Match>()
				.Select(m => m.Groups)
				.Select(g => g[2].Value + (g[1].Value == "-" ? " DESC" : " ASC"))
				.JoinBy(", ");

			command.CommandText += " ORDER BY " + orderSql;

			return command;
		}




		/*############################################################################*/

		/// <summary></summary>
		public static DataTable FetchDataTable(this DbCommand command)
		{
			using DbDataAdapter adapter = DbProviderFactories.GetFactory(command.Connection).CreateDataAdapter();
			adapter.SelectCommand = command;

			var dataTable = new DataTable();
			adapter.Fill(dataTable);
			return dataTable;
		}



		/// <summary></summary>
		public static DataRow FetchDataRow(this DbCommand command)
		{
			string commandText = command.CommandText;
			command.CommandText = $"SELECT ORIGIN_QUERY.* FROM ({commandText}) ORIGIN_QUERY WHERE ROWNUM = 1";

			DataTable dataTable = command.FetchDataTable();
			command.CommandText = commandText;

			return dataTable.FirstOrDefault();
		}


		/// <summary></summary>
		public static T FetchOne<T>(this DbCommand command)
		{
			DataRow row = FetchDataRow(command);
			if (row == null) { return default(T); }

			T value = row[0].ConvertTo<T>();
			return value;
		}


		/// <summary></summary>
		public static List<TModel> FetchList<TModel>(this DbCommand command) where TModel : new()
		{
			DataTable dataTable = command.FetchDataTable();
			return dataTable.ToModel<TModel>().ToList();
		}


		/// <summary></summary>
		public static TModel FetchModel<TModel>(this DbCommand command) where TModel : new()
		{
			string commandText = command.CommandText;
			command.CommandText = $"SELECT ORIGIN_QUERY.* FROM ({commandText}) ORIGIN_QUERY WHERE ROWNUM = 1";

			DataTable dataTable = command.FetchDataTable();
			command.CommandText = commandText;

			return dataTable.ToModel<TModel>().FirstOrDefault();
		}





		/// <summary></summary>
		public static DataTablePagination FetchPagination(this DbCommand command, int pageNumber, int pageSize)
		{
			string commandText = command.CommandText;


			var result = new DataTablePagination
			{
				PageNumber = Math.Max(1, pageNumber),
				PageSize = pageSize,
			};

			if (pageSize == -1)
			{
				result.DataTable = command.FetchDataTable();
				result.TotalItems = result.DataTable.Rows.Count;
				return result;
			}

			if (!command.Connection.State.HasFlag(ConnectionState.Open))
			{ command.Connection.Open(); }


			result.TotalItems = getCommandCount(command);

			if (result.TotalItems == 0)
			{
				result.DataTable = new DataTable();
				return result;
			}

			if (result.PageSize <= 0)
			{
				result.DataTable = new DataTable();
				return result;
			}

			int totalPages = (int)Math.Ceiling(((double)result.TotalItems) / result.PageSize);
			result.PageNumber = Math.Min(result.PageNumber, totalPages);

			int pageStart = (result.PageNumber - 1) * result.PageSize + 1;
			int pageEnd = result.PageNumber * result.PageSize + 1;

			command.CommandText = $@"
				SELECT * FROM
				(
					SELECT rownum RN, PAGE_QUERY.*
					FROM ({commandText}) PAGE_QUERY
					WHERE rownum < {pageEnd} 
				)
				WHERE RN >= {pageStart}
			";

			result.DataTable = command.FetchDataTable();

			command.CommandText = commandText;

			return result;
		}


		/// <summary>列舉 DataRow</summary>
		public static IEnumerable<DataRow> AsEnumerable(this DataTablePagination pagination)
		{
			if (pagination == null || pagination.DataTable == null)
			{ return Enumerable.Empty<DataRow>(); }

			return pagination.DataTable.AsEnumerable();
		}









		/*############################################################################*/

		private static IDictionary<string, object> toDictionary(object obj)
		{
			if (obj is IDictionary<string, object> dict) { return dict; }
			return obj.AnonymousToDictionary();
		}




		/*#[Insert]###########################################################################*/

		/// <summary></summary>
		public static DbCommand BuildInsert(this DbCommand command, string tableName, object nameValues)
		{
			string prefix = command.ParamPrefix();
			IDictionary<string, object> nameValuesDict = toDictionary(nameValues);

			string columns = nameValuesDict.Keys.JoinBy(", ");
			string parames = nameValuesDict.Keys.Select(x => prefix + x).JoinBy(", ");

			command.CommandText = $"INSERT INTO {tableName} ({columns}) VALUES ({parames})";

			command.Parameters.Clear();
			foreach (var item in nameValuesDict)
			{ AddParameter(command, prefix + item.Key, item.Value); }

			return command;
		}






		/*#[Update]###########################################################################*/

		/// <summary></summary>
		public static DbCommand BuildUpdate(this DbCommand command, string tableName, object setValues, object whereValues)
		{
			string prefix = command.ParamPrefix();
			IDictionary<string, object> setValuesDict = toDictionary(setValues);
			IDictionary<string, object> whereValuesDict = toDictionary(whereValues);

			if (whereValuesDict == null) { whereValuesDict = new Dictionary<string, object>(); }

			string setColumns = setValuesDict.Keys.Select(x => $"{x} = {prefix}S_{x}").JoinBy(", ");
			string whereCondition = whereValuesDict.Keys.Select(x => $"{x} = {prefix}W_{x}").JoinBy(" AND ");

			command.CommandText = $"UPDATE {tableName} SET {setColumns} WHERE {whereCondition}";

			command.Parameters.Clear();

			foreach (var item in setValuesDict)
			{ AddParameter(command, prefix + "S_" + item.Key, item.Value); }

			foreach (var item in whereValuesDict)
			{ AddParameter(command, prefix + "W_" + item.Key, item.Value); }

			return command;
		}





		/*#[Delete]###########################################################################*/

		/// <summary></summary>
		public static DbCommand BuildDelete(this DbCommand command, string tableName, object whereValues)
		{
			string prefix = command.ParamPrefix();
			IDictionary<string, object> whereValuesDict = toDictionary(whereValues);

			if (whereValuesDict == null) { whereValuesDict = new Dictionary<string, object>(); }

			string whereCondition = whereValuesDict.Keys.Select(x => $"{x} = {prefix}{x}").JoinBy(" AND ");
			command.CommandText = $"DELETE FROM {tableName} WHERE {whereCondition}";

			command.Parameters.Clear();
			foreach (var item in whereValuesDict)
			{ AddParameter(command, prefix + item.Key, item.Value); }

			return command;
		}





		/*#[Procedure]###########################################################################*/

		/// <summary></summary>
		public static DbCommand BuildProcedure(this DbCommand command, string procedureName, params object[] parameters)
		{
			string prefix = command.ParamPrefix();
			command.CommandType = CommandType.StoredProcedure;
			command.CommandText = procedureName;

			foreach (var item in parameters)
			{
				string name = prefix + "p" + command.Parameters.Count;
				if (item is DbParameter dbParam)
				{
					if (dbParam.ParameterName.NoText()) { dbParam.ParameterName = name; }
					command.Parameters.Add(dbParam);
				}
				else
				{
					AddParameter(command, name, item);
				}
			}

			return command;
		}




		/*====================================================*/

		/// <summary>
		/// query = query.WhereBuilder(param)<para/>
		///     .Mapping(x =&gt; x.InvoicePrefix,  y =&gt; y.InvoicePrefix)<para/>
		///     .Mapping(x =&gt; x.Sum,            y =&gt; y.Total)<para/>
		///     .Mapping(x =&gt; x.ProductQty,     y =&gt; y.InvoiceIssueItems.Select(z =&gt; (int?)z.Qty))<para/>
		///     .Build();<para/>
		/// </summary>
		public static WhereCommandBuilder WhereBuilder(this DbCommand command, WhereParams param)
		{
			return new WhereCommandBuilder(command, param);
		}




	} 



	/// <summary></summary>
	public static class OracleParameterExtensions
	{

		private static T getValue<T>(DbParameter parameter, T empty)
		{
			if (parameter == null) { return empty; }

			//object dbValue = parameter.Value;
			//Type type = dbValue.GetType();

			//PropertyInfo isNullProp = type.GetProperty("IsNull");
			//if (isNullProp == null) { return empty; }

			//bool isNull = (bool)isNullProp.GetValue(dbValue);
			//if (isNull) { return empty; }

			//PropertyInfo valueProp = type.GetProperty("Value");
			//object value = valueProp.GetValue(dbValue);

			object value = parameter.Value;
			if (value == null) { return empty; }

			if (value is T tValue) { return tValue; }

			return value.ConvertTo<T>();
		}


		/// <summary></summary>
		public static string String(this DbParameter parameter)
		{
			return getValue<string>(parameter, null);
		}

		/// <summary></summary>
		public static bool Bool(this DbParameter parameter)
		{
			return getValue<bool>(parameter, false);
		}

		/// <summary></summary>
		public static int Int(this DbParameter parameter)
		{
			return getValue<int>(parameter, 0);
		}

		/// <summary></summary>
		public static long Long(this DbParameter parameter)
		{
			return getValue<long>(parameter, 0);
		}

		/// <summary></summary>
		public static decimal Decimal(this DbParameter parameter)
		{
			return getValue<decimal>(parameter, 0m);
		}


	}

}
