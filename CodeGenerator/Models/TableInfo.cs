using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator.Models
{
	public class TableInfo
	{
		public int Id { get; set; }

		public string TableName { get; set; }

		public string Description { get; set; }

		public string DisplayName 
		{ 
			get { return string.Format("{0}   {1}", TableName, Description); }
			set { }
		}



	}
}
