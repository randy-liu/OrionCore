namespace OrionCore.API.Models
{

	public class TableInfo
	{
		/// <summary></summary>
		public string Schema { get; set; }

		/// <summary></summary>
		public string Name { get; set; }

		/// <summary></summary>
		public long TotalRows { get; set; }

		/// <summary></summary>
		public long TotalBytes { get; set; }

		/// <summary></summary>
		public long TableBytes { get; set; }

		/// <summary></summary>
		public long IndexBytes { get; set; }

		/// <summary></summary>
		public long UnusedBytes { get; set; }

		/// <summary>超大屬性存儲技術 (The Oversized-Attribute Storage Technique)</summary>
		public long ToastBytes { get; set; }
	}

}
