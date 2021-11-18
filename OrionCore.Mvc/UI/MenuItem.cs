using System.Collections.Generic;

namespace Orion.Mvc.UI
{
	/// <summary></summary>
	public class MenuItem 
	{
		/// <summary></summary>
		public string Name { get; set; }

		/// <summary></summary>
		public string Url { get; set; }

		/// <summary></summary>
		public string Target { get; set; }

		/// <summary></summary>
		public List<string> ACT { get; set; }

		/// <summary></summary>
		public string Icon { get; set; }

		/// <summary></summary>
		public string Pattern { get; set; }

		/// <summary></summary>
		public bool IsActive { get; set; }

		/// <summary></summary>
		public bool CanAccess { get; set; }

		/// <summary></summary>
		public bool HasUrl
		{
			get { return !string.IsNullOrWhiteSpace(Url); }
		}

        /// <summary></summary>
        public bool UseFirstSubUrl { get; set; }

        /// <summary></summary>
        public List<MenuItem> SubItems { get; set; }

        /// <summary></summary>
        public bool HasSubItems
        {
            get { return (SubItems != null && SubItems.Count > 0); }
        }

        /// <summary></summary>
        public MenuItem Clone()
        {
            return (MenuItem)this.MemberwiseClone();
        }

	}
}