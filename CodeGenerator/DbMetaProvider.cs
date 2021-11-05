using CodeGenerator.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CodeGenerator
{
	public class DbMetaProvider
	{
		private static string[] _nullableType = {
			"bool", "byte", "DateTime", "DateTimeOffset", "TimeSpan", "decimal", "double", "Guid", "int"
		};

		private static string[] _basicColumns = new[] {
			"CreateBy", "CreatedBy", "ModifyBy", "ModifiedBy",
			"CreateDate", "CreatedDate", "ModifyDate", "ModifiedDate",
		};




		private List<Type> _tableTypes = new List<Type>();

		public DbMetaProvider() { }

		public DbMetaProvider(string daoDllPath)
		{
			SetConnectionString(daoDllPath);
		}



		public void SetConnectionString(string daoDllPath)
		{
			var assembly = Assembly.LoadFrom(daoDllPath);

			Type dcType = assembly.DefinedTypes.FirstOrDefault(t => typeof(DbContext).IsAssignableFrom(t));
			if (dcType == null) { throw new Exception("找不到繼承自 DbContext 的 class"); }

			_tableTypes = dcType.GetProperties()
				.Select(p => p.PropertyType)
				.Where(t => t.IsGenericType)
				.Where(t => typeof(DbSet<>) == t.GetGenericTypeDefinition())
				.Select(t => t.GetGenericArguments().First())
				.ToList();
		}


		public List<TableInfo> GetTableList()
		{
			return _tableTypes
				.Select(t => new TableInfo
				{
					TableName = t.Name,
					Description = getDescription(t),
				})
				.ToList();
		}


		public TableMeta GetTableMeta(string tableName)
		{
			Type tableType = _tableTypes.First(t => t.Name == tableName);

			var table = new TableMeta();
			table.TableName = tableName;
			table.Name = tableName;
			table.Description = getDescription(tableType);


			table.Columns = tableType.GetProperties()
				.Select(p => getColumnMeta(p))
				.Where(x => x.CodeType != null)
				.ToList();


			table.PK = table.Columns.First(x => x.IsPrimaryKey);

			return table;		
		}



		private string getDescription(MemberInfo type)
		{
			return type.GetCustomAttribute<DescriptionAttribute>()?.Description;
		}


		private static Regex _enumRegex = new Regex(@"<enum>(\(([^)]+)\))?", RegexOptions.IgnoreCase);


		/// <summary>欄位類型對應處理</summary>
		private ColumnMeta getColumnMeta(PropertyInfo prop)
		{
			var column = new ColumnMeta
			{
				Name = prop.Name,
				IsBasic = _basicColumns.Contains(prop.Name),
				Description = getDescription(prop),
			};

			column.IsPrimaryKey = prop.GetCustomAttribute<KeyAttribute>() != null;
			column.DisplayName = Regex.Replace(column.Description, "Id$", "");
			column.CodeType = getCodeType(prop.PropertyType);
			if (column.CodeType == null) { return column; }

			column.IsNullable = column.CodeType.EndsWith("?");



			/* enum 處理 */
			if (!column.Description.ToLower().Contains("<enum>")) { return column; }

			column.IsEnum = true;
			column.CodeType = column.Name;

			column.Description = _enumRegex.Replace(column.Description, m =>
			{
				column.EnumList = m.Groups[2].Value
					.Split(',')
					.Select(x => x.Trim())
					.Where(x => !string.IsNullOrWhiteSpace(x))
					.ToArray();

				return "";
			});

			column.Description = column.Description.Trim();

			return column;
		}



		private Dictionary<Type, string> _codeTypeDict = new Dictionary<Type, string>
		{
			{ typeof(bool),"bool" },			     
			{ typeof(byte),"byte" },
			{ typeof(float),"float" },
			{ typeof(double),"double" },
			{ typeof(decimal),"decimal" },
			{ typeof(short),"short" },
			{ typeof(ushort),"ushort" },
			{ typeof(int),"int" },
			{ typeof(uint),"uint" },
			{ typeof(long),"long" },
			{ typeof(ulong),"ulong" },
			{ typeof(string),"string" },
			{ typeof(Guid),"Guid" },
			{ typeof(TimeSpan),"TimeSpan" },
			{ typeof(DateTime),"DateTime" },
			{ typeof(DateTimeOffset),"DateTimeOffset" },
		};


		private string getCodeType(Type type)
		{
			if (_codeTypeDict.ContainsKey(type)) { return _codeTypeDict[type]; }
			if (!type.IsGenericType) { return type.Name; }

			Type subType = type.GetGenericArguments().First();
			string subCodeType = getCodeType(subType);
			
			if (typeof(IEnumerable<>).IsAssignableFrom(type.GetGenericTypeDefinition())) 
			{ return $"{type.Name}<{subCodeType}>"; }
			if (typeof(Nullable<>).IsAssignableFrom(type.GetGenericTypeDefinition()))
			{ return subCodeType + "?"; }

			return null;
		}


	}
}
