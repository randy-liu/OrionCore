using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator.Models
{
	public class TableMeta
	{
		public int Id { get; set; }

		public string NameSpace { get; set; }

		public string TableName { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public List<ColumnMeta> Columns { get; set; }

		public ColumnMeta PK { get; set; }

		public string Variable
		{
			get { return char.ToLower(Name[0]) + Name.Substring(1); }
			set { }
		}

	}
}
