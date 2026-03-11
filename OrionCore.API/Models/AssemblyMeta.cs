using System;

namespace Orion.Api.Models
{
	/// <summary>Assembly Meta 資訊</summary>
	public class AssemblyMeta
    {
		/// <summary>組件名稱。</summary>
		public string Name { get; internal set; }

		/// <summary>組件標題。</summary>
		public string Title { get; internal set; }
        
        /// <summary>組件描述。</summary>
        public string Description { get; internal set; }
        
        /// <summary>組件組態資訊。</summary>
        public string Configuration { get; internal set; }
        
        /// <summary>公司名稱。</summary>
        public string Company { get; internal set; }
        
        /// <summary>產品名稱。</summary>
        public string Product { get; internal set; }
        
        /// <summary>版權資訊。</summary>
        public string Copyright { get; internal set; }
        
        /// <summary>商標資訊。</summary>
        public string Trademark { get; internal set; }
        
        /// <summary>文化設定。</summary>
        public string Culture { get; internal set; }
        
        /// <summary>組件 Guid。</summary>
        public string Guid { get; internal set; }
        
        /// <summary>組件版本。</summary>
        public Version Version { get; internal set; }

        /// <summary>檔案版本。</summary>
        public string FileVersion { get; internal set; }
    }
}
