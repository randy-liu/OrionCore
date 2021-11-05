using CodeGenerator.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.CodeDom.Compiler;
using System;

namespace CodeGenerator.Templates
{

	public abstract class TemplateBase
	{

        public string FilePath { get; set; }
		public string DataContextName { get; set; }
		public TableMeta TableMeta { get; set; }



        public bool HasColumn(string name)
		{
			return TableMeta.Columns.Any(x => x.Name == name.Trim());
		}

		public IEnumerable<ColumnMeta> ActiveColumns
		{
			get
			{
				return TableMeta.Columns.Where(x => !x.IsBasic);
			}
		}

		public IEnumerable<ColumnMeta> EnumColumns
		{
			get
			{
				return ActiveColumns.Where(x => x.IsEnum).Where(x => x.Name != "UseStatus");
			}
		}

        public IEnumerable<ColumnMeta> WithColumns(params string[] columns)
        {
            return TableMeta.Columns.Where(x => columns.Contains(x.Name));
        }







		public abstract string TransformText();


		public bool Generate(string basepath, bool isOverride = false)
		{
			/* 執行樣版 */
			string content = TransformText();

			/* 檢查存在 */
			string filePath = Path.Combine(basepath, FilePath);
			if (!isOverride && File.Exists(filePath)) { return false; }

			/* 檢查分配目錄 */
			string folder = Path.GetDirectoryName(filePath);
			if (!Directory.Exists(folder)) { Directory.CreateDirectory(folder); }

			/* 輸出檔案 UTF8 BOM */
			File.WriteAllText(filePath, content, new UTF8Encoding(true));

			return true;
		}




		/*=====================================================*/

		protected StringBuilder GenerationEnvironment { get; private set; } = new StringBuilder();


		public void Write(string textToAppend)
		{
			if (string.IsNullOrEmpty(textToAppend)) { return; }
			GenerationEnvironment.Append(textToAppend);
			return;
		}

		public void WriteLine(string textToAppend)
		{
			Write(textToAppend);
			GenerationEnvironment.AppendLine();
		}

		public void Write(string format, params object[] args)
		{
			Write(string.Format(format, args));
		}

		public void WriteLine(string format, params object[] args)
		{
			WriteLine(string.Format(format, args));
		}

		public ToStringInstanceHelper ToStringHelper { get; private set; } = new ToStringInstanceHelper();


		/*============================================*/

		public class ToStringInstanceHelper
		{
			public string ToStringWithCulture(object objectToConvert)
			{
				return objectToConvert?.ToString() ?? string.Empty;
			}
		}
	

	}
}
