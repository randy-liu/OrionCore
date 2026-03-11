namespace Orion.Api.Models
{

		/// <summary>資料表空間統計資訊。</summary>
	public class TableInfo
	{
		/// <summary>資料表所屬 Schema。</summary>
		public string Schema { get; set; }

		/// <summary>資料表名稱。</summary>
		public string Name { get; set; }

		/// <summary>總資料列數。</summary>
		public long TotalRows { get; set; }

		/// <summary>總使用空間（位元組）。</summary>
		public long TotalBytes { get; set; }

		/// <summary>資料表主體空間（位元組）。</summary>
		public long TableBytes { get; set; }

		/// <summary>索引空間（位元組）。</summary>
		public long IndexBytes { get; set; }

		/// <summary>未使用空間（位元組）。</summary>
		public long UnusedBytes { get; set; }

		/// <summary>超大屬性存儲技術 (The Oversized-Attribute Storage Technique)</summary>
		public long ToastBytes { get; set; }
	}

}
