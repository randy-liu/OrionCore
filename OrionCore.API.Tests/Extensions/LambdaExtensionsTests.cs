using Orion.Api.Tests;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using Xunit;

namespace Orion.Api.Extensions.Tests
{
	public class LambdaExtensionsTests
	{
		/// <summary>New Case 新增案例</summary>
		private static object[] n<T>(Expression<Func<UserModel, T>> expr)
		{
			return new object[] { expr };
		}





		/*===============================================================*/

		public static IEnumerable<object[]> RunTest_Data()
		{
			var find = new UserModel { Name = "OK" };

			return new[]
			{
				n(x => x.Name),
				n(x => x.Name.Length.ToString()),
				n(x => x.Name == find.Name ? x.Name : "dd"),
				n(x => (string)Convert.ChangeType(x.UserId, typeof(string))),
				n(x => x.Name == find.Name),
				n(x => x.Name == "dd"),
			};
		}

		[Theory]
		[MemberData(nameof(RunTest_Data))]
		public void FindByType_RunTest(LambdaExpression expr)
		{
			var list = expr.FindByType<MemberExpression>();
			Assert.True(list.Count > 0);
		}




	}





}
