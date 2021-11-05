namespace CodeGenerator.Models
{
    public class ColumnMeta
	{

		public string Name { get; set; }

		public string CodeType { get; set; }

		public bool IsPrimaryKey { get; set; }

        public bool IsEnum { get; set; }

        public bool IsNullable { get; set; }

        /// <summary>是否為基礎欄位 CreateBy, ModifyBy, CreateDate, ModifyDate</summary>
        public bool IsBasic { get; set; }

		public string Description { get; set; }

        public string DisplayName { get; set; }


		public string Variable {
			get { return char.ToLower(Name[0]) + Name.Substring(1); }
			set { }
		}

        public string[] EnumList { get; set; } = new string[0];
    }
}
