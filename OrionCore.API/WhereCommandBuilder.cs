using System;
using System.Collections;
using System.Linq;
using Orion.Api.Extensions;
using Orion.Api.Models;
using System.Data.Common;


namespace Orion.Api
{
	/// <summary>Where Command Build Tools</summary>
	public class WhereCommandBuilder
	{

		private readonly string _prefix;
		private readonly DbCommand _command;
		private readonly WhereParams _param;


		/// <summary>建立 `WhereCommandBuilder`。</summary>
		/// <param name="command">要附加查詢條件的資料庫命令。</param>
		/// <param name="param">Where 條件參數。</param>
		public WhereCommandBuilder(DbCommand command, WhereParams param)
		{
			_command = command;
			_param = param;

			_prefix = command.ParamPrefix();
		}





		private void append(string sqlColumn, FormattableString condition)
		{
			DbParameterCollection cmdParams = _command.Parameters;
			object value = condition.GetArguments()[0];

			string format = condition.Format;
			string name = _prefix + "p" + cmdParams.Count;

			if (value is string) 
			{
				DbParameter param = _command.CreateParameter();
				param.ParameterName = name;
				param.Value = value;
				cmdParams.Add(param);
			}
			else
			{
				var valueList = (value as IEnumerable).Cast<object>().ToList((v, i) => new { Name = name + "_" + i, Value = v });
				name = valueList.Select(x => x.Name).JoinBy(", ");
				foreach (var item in valueList) 
				{
					DbParameter param = _command.CreateParameter();
					param.ParameterName = item.Name;
					param.Value = item.Value;
					cmdParams.Add(param); 
				}
			}

			_command.CommandText += $" AND {sqlColumn} " + string.Format(format, new[] { name });
		}




		/*=====================================================================*/


		/// <summary>依查詢參數設定將條件附加到命令文字與參數集合。</summary>
		/// <param name="paramColumn">Where 參數中的欄位名稱。</param>
		/// <param name="sqlColumn">資料庫欄位名稱；未指定時使用 <paramref name="paramColumn"/>。</param>
		/// <returns>目前建構器實例，供串接呼叫。</returns>
		public WhereCommandBuilder Bind(string paramColumn, string sqlColumn = null)
		{
			if (paramColumn == null) { return this; }
			if (sqlColumn == null) { sqlColumn = paramColumn; }

			WhereOperator oper = _param.GetOperator(paramColumn);
			object[] values = _param.GetValues(paramColumn);
			if (values == null || values.Length == 0) { return this; }


			switch (oper)
			{
				case WhereOperator.Contains:
					append(sqlColumn, $"LIKE '%' || {values[0]} || '%'"); break;
				case WhereOperator.NotContains:
					append(sqlColumn, $"NOT LIKE '%' || {values[0]} || '%'"); break;
				case WhereOperator.StartsWith:
					append(sqlColumn, $"LIKE {values[0]} || '%'"); break;
				case WhereOperator.NotStartsWith:
					append(sqlColumn, $"NOT LIKE {values[0]} || '%'"); break;
				case WhereOperator.EndsWith: 
					append(sqlColumn, $"LIKE '%' || {values[0]}"); break;
				case WhereOperator.NotEndsWith:
					append(sqlColumn, $"NOT LIKE '%' || {values[0]}"); break;
				case WhereOperator.Equals:
					append(sqlColumn, $"= {values[0]}"); break;
				case WhereOperator.NotEquals:
					append(sqlColumn, $"<> {values[0]}"); break;

				case WhereOperator.LessThan:
					append(sqlColumn, $"< {values[0]}"); break;
				case WhereOperator.LessEquals:
					append(sqlColumn, $"<= {values[0]}"); break;
				case WhereOperator.GreaterThan:
					append(sqlColumn, $"> {values[0]}"); break;
				case WhereOperator.GreaterEquals:
					append(sqlColumn, $">= {values[0]}"); break;

				case WhereOperator.In: 
					append(sqlColumn, $"IN ({values})"); break;
				case WhereOperator.NotIn:
					append(sqlColumn, $"NOT IN ({values})"); break;

				case WhereOperator.Between: /* x >= values[0] && x <= values[1] */
					append(sqlColumn, $">= {values[0]}");
					if (values.Length > 1) { append(sqlColumn, $"<= {values[1]}"); }
					break;
			}


			return this;
		}
		 

	}
}
