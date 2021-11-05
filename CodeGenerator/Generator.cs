using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CodeGenerator.Models;
using CodeGenerator.Templates;

namespace CodeGenerator
{
	public class Generator
	{
		private DbMetaProvider _dbMetaProvider = new DbMetaProvider();

		public List<Type> TemplateTypes { get; private set; }

		public string BasePath { get; set; }
		public string DataContextName { get; set; }
		public string NameSpace { get; set; }
		public bool IsOverride { get; set; }


		public Generator(DbMetaProvider dbMetaProvider)
		{
			_dbMetaProvider = dbMetaProvider;

			TemplateTypes = Assembly.GetExecutingAssembly().ExportedTypes
				.Where(x => x.IsClass && !x.IsAbstract && !x.IsInterface)
				.Where(x => typeof(TemplateBase).IsAssignableFrom(x))
				.Where(x => !x.Namespace.EndsWith("Templates"))
				.OrderBy(x => x.FullName)
				.ToList();
		}


		public void Execute(IEnumerable<string> templateNames, IEnumerable<string> tableNames, Action<bool, string> executed)
		{

			var selectTplTypes = templateNames
				.Select(x => TemplateTypes.Single(y => y.FullName.EndsWith(x)))
				.ToList();


			var tableMetaList = tableNames
				.Select(x => _dbMetaProvider.GetTableMeta(x))
				.ToList();


			foreach (var tplType in selectTplTypes)
			foreach (TableMeta tableMeta in tableMetaList)
			{
				tableMeta.NameSpace = NameSpace;

				var tpl = (TemplateBase)Activator.CreateInstance(tplType);
				tpl.TableMeta = tableMeta;
				tpl.DataContextName = DataContextName;

				bool isGenerate = tpl.Generate(BasePath, IsOverride);
				executed(isGenerate, tpl.FilePath);
			}

			List<ColumnMeta> enumMetaList = tableMetaList
				.SelectMany(t => t.Columns)
				.Where(x => x.IsEnum)
				.Where(x => x.Name != "UseStatus")                
				.OrderByDescending(x => x.EnumList.Length)
				.GroupBy(x => x.Name)
				.Select(g => g.First())
				.ToList();

			foreach (ColumnMeta enumMeta in enumMetaList)
			{
				var tpl = new EnumTemplate();
				tpl.TableMeta = new TableMeta
				{
					NameSpace = NameSpace,
					Columns = new List<ColumnMeta> { enumMeta }
				};
				tpl.DataContextName = DataContextName;

				bool isGenerate = tpl.Generate(BasePath, IsOverride);
				executed(isGenerate, tpl.FilePath);
			}






		}


	}
}
