using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Orion.Mvc.Html.Tests
{
	public class HelperUtilsTests
	{

		[Theory]
		[InlineData("class", "input-sm", "form-control input-sm")]
		[InlineData("Class", "input-sm", "form-control input-sm")]
		public void AddCssClass_Test(string key, string value, string expected)
        {
			var htmlAttributes = new Dictionary<string, object> { {key, value} };

			HelperUtils.AddCssClass(htmlAttributes, "form-control");
			Assert.Equal(expected, htmlAttributes[key]);
        }



	
	}
}
