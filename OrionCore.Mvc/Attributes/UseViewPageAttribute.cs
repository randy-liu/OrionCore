using System;

namespace OrionCore.Mvc.Attributes
{
	/// <summary></summary>
	[AttributeUsage(AttributeTargets.Method, Inherited = true)]
	public class UseViewPageAttribute : Attribute
	{
		/// <summary></summary>
		public string ViewName { get; private set; }

		/// <summary></summary>
		public string Title { get; private set; }


		/// <summary></summary>
		public UseViewPageAttribute(string viewName)
		{
			ViewName = viewName;
		}

		/// <summary></summary>
		public UseViewPageAttribute(string viewName, string title)
		{
			ViewName = viewName;
			Title = title;
		}

	}
}