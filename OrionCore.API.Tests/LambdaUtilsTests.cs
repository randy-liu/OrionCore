using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace OrionCore.API.Tests
{
	public class LambdaUtilsTests
	{

		public static IEnumerable<object[]> RunTest_Data
		{
			get
			{
				var find = new UserModel{ Name = "OK" };

				yield return new object[] {
					(Expression<Func<UserModel, string>>)(x => x.Name)
				};
				yield return new object[] {
					(Expression<Func<UserModel, string>>)(x => x.Name.Length.ToString())
				};
				yield return new object[] {
					(Expression<Func<UserModel, string>>)(x => x.Name == find.Name ? x.Name : "dd")
				};
				yield return new object[] {
					(Expression<Func<UserModel, string>>)(x => (string)Convert.ChangeType(x.UserId, typeof(string)))
				};
				yield return new object[] {
					(Expression<Func<UserModel, bool>>)(x => x.Name == find.Name)
				};
				yield return new object[] {
					(Expression<Func<UserModel, bool>>)(x => x.Name == "dd")
				};

			}
		}

		[Theory]
		[MemberData(nameof(RunTest_Data))]
		public void FindByType_RunTest(LambdaExpression expr)
		{
			var list = LambdaUtils.FindByType<MemberExpression>(expr);
			Assert.True(list.Count > 0);
		}





		/*===============================================================*/

		[Fact]
		public void GetMethod_RunTest()
		{
			MethodInfo method = LambdaUtils.GetMethod<IEnumerable<int>>(x => x.Any(y => true));
			Assert.NotNull(method);
		}

		[Fact]
		public void GetMethod_RunTest2()
		{
			MethodInfo method = LambdaUtils.GetMethod<IEnumerable<int>>(x => x.Contains(1));
			Assert.NotNull(method);
		}

		[Fact]
		public void GetMethod_RunTest3()
		{
			MethodInfo method = LambdaUtils.GetMethod(() => new[] { 1 }.Contains(1));
			Assert.NotNull(method);
		}


		/*===============================================================*/

		[Fact]
		public void GetGenericMethod_RunTest()
		{
			MethodInfo method = LambdaUtils.GetGenericMethodDefinition<IEnumerable<int>>(x => x.Any(y => true));
			Assert.NotNull(method);
		}

		[Fact]
		public void GetGenericMethod_RunTest2()
		{
			MethodInfo method = LambdaUtils.GetGenericMethodDefinition<IEnumerable<int>>(x => x.Contains(1));
			Assert.NotNull(method);
		}

		[Fact]
		public void GetGenericMethod_RunTest3()
		{
			MethodInfo method = LambdaUtils.GetGenericMethodDefinition(() => new[] { 1 }.Contains(1));
			Assert.NotNull(method);
		}




		public class UserModel
		{
			public int UserId { get; set; }
			public string Name { get; set; }
			public DateTime CreateDate { get; set; }
			public List<string> Column { get; set; }
			public List<UserItemModel> Items { get; set; }
			public DbSet<UserItemModel> EntitySet { get; set; }

		}

		public class UserItemModel
		{
			public string Name { get; set; }
		}

	}



}
