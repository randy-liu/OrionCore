
using System.Collections.Generic;

namespace Orion.Mvc.UI
{
	/// <summary></summary>
	public interface IBreadcrumb
	{
		/// <summary></summary>
		string Name { get; }

		/// <summary></summary>
		string Url { get; }

		/// <summary></summary>
		string Icon { get; }
	}


	internal class BreadcrumbNode : IBreadcrumb
	{
		public string Name { get; set; }
		public string Url { get; set; }
		public string Icon { get; set; }
		public string Pattern { get; set; }
		public List<BreadcrumbNode> ChildNodes { get; set; }
		public BreadcrumbNode Parent { get; set; }
	}



}
