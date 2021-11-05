using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;


namespace OrionCore.API.Extensions.Tests
{
	public class EnumerableExtensionsTests
	{ 


		[Fact]
		public void ForEachTest()
		{
			var list = new List<UserModel>
			{
				new UserModel{ UserId = 1 },
				new UserModel{ UserId = 1 },
			};

			list = list
				.Select(x => x.Clone())
				.Each(x => x.UserId = 2)
				.ToList();

			Assert.Equal(2, list[0].UserId);
		}


	}

}
