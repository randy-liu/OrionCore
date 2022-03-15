using System;
using System.Collections.Generic;
using Xunit;

namespace Orion.Api.Tests
{
	public class CheckerTests
	{

		/// <summary>New Case 新增案例</summary>
		private static object[] n(object values) { return new object[] { values }; }
		private static object[] n(object values1, object values2) { return new object[] { values1, values2 }; }
		
		
		
		
		/*===============================================================*/

		public static IEnumerable<object[]> Has_Test_Data()
		{
			return new[] 
			{
				n( 1 ),
				n( DateTime.Now ),
				n( "ss" ),
				n( new string[] { "sss" } ),
				n( new List<string>() { "sss" } ),
			};
		}


		[Theory]
		[MemberData(nameof(Has_Test_Data))]
		public void Has_Test(object value)
		{
			Checker.Has(value, "{0} Error Msg");
			Assert.True(true);
		}


		public static IEnumerable<object[]> Has_FailTest_Data()
		{
			return new[] 
			{
				n( (string)null ),
				n( 0 ),
				n( "" ),
				n( new string[] { } ),
				n( new List<string>() ),
			};
		}

		[Theory]
		[MemberData(nameof(Has_FailTest_Data))]
		public void Has_FailTest(object value)
		{
			Assert.Throws<UserException>(() => Checker.Has(value, "{0} Error Msg"));
		}








		/*===================================================================*/

		public static IEnumerable<object[]> Is_Test_Data()
		{
			return new[]
			{
				n( (int?)1, 1 ),
				n( "1", "1" ),
			};
		}


		[Theory]
		[MemberData(nameof(Is_Test_Data))]
		public void Is_Test(object value, object value2)
		{
			Checker.Is(value, value2, "Error Msg");
			Assert.True(true);
		}




		public static IEnumerable<object[]> Is_FailTest_Data()
		{
			return new[] 
			{
				n( null, 1 ),
				n( 2, 1 ),
				n( "", "1" ),
			};
		}

		[Theory]
		[MemberData(nameof(Is_FailTest_Data))]
		public void Is_FailTest(object value, object value2)
		{
			Assert.Throws<UserException>(() => Checker.Is(value, value2, "Error Msg portId: {P001:'sss'}"));
		}





		/*===================================================================*/

		[Fact]
		public void Min_Test()
		{
			Checker.Min(2, 1, "{0} {1} Error Msg");
			Checker.Min(2f, 1f, "{0} {1} Error Msg");
			Checker.Min(2.1, 1.1, "{0} {1} Error Msg");
			Checker.Min((decimal)2.1, (decimal)1.1, "{0} {1} Error Msg");
			Assert.True(true);
		}

		[Fact]
		public void Min_FailTest()
		{
			Assert.Throws<UserException>(() => Checker.Min(1, 2, "{0} {1} Error Msg"));
			Assert.Throws<UserException>(() => Checker.Min(1f, 2f, "{0} {1} Error Msg"));
			Assert.Throws<UserException>(() => Checker.Min(1.1, 2.1, "{0} {1} Error Msg"));
			Assert.Throws<UserException>(() => Checker.Min((decimal)1.1, (decimal)2.1, "{0} {1} Error Msg"));
		}
		
		
		
		
		
		/*===================================================================*/

		[Fact]
		public void Max_Test()
		{
			Checker.Max(1, 2, "{0} {1} Error Msg");
			Checker.Max(1f, 2f, "{0} {1} Error Msg");
			Checker.Max(1.1, 2.1, "{0} {1} Error Msg");
			Checker.Max((decimal)1.1, (decimal)2.1, "{0} {1} Error Msg");
			Assert.True(true);
		}

		[Fact]
		public void Max_FailTest()
		{
			Assert.Throws<UserException>(() => Checker.Max(2, 1, "{0} {1} Error Msg"));
			Assert.Throws<UserException>(() => Checker.Max(2f, 1f, "{0} {1} Error Msg"));
			Assert.Throws<UserException>(() => Checker.Max(2.1, 1.1, "{0} {1} Error Msg"));
			Assert.Throws<UserException>(() => Checker.Max((decimal)2.1, (decimal)1.1, "{0} {1} Error Msg"));
		}


		/*===================================================================*/

		[Fact]
		public void Range_Test()
		{
			Checker.Range(1, 0, 2, "{0} {1} {2} Error Msg");
			Checker.Range(1f, 0f, 2f, "{0} {1} {2} Error Msg");
			Checker.Range(1.1, 0.1, 2.1, "{0} {1} {2} Error Msg");
			Checker.Range((decimal)1.1, (decimal)0.1, (decimal)2.1, "{0} {1} {2} Error Msg");
			Assert.True(true);
		}

		[Fact]
		public void Range_FailTest()
		{
			Assert.Throws<UserException>(() => Checker.Range(4, 1, 2, "{0} {1} {2} Error Msg"));
			Assert.Throws<UserException>(() => Checker.Range(4f, 1f, 2f, "{0} {1} {2} Error Msg"));
			Assert.Throws<UserException>(() => Checker.Range(4.1, 1.1, 2.2, "{0} {1} {2} Error Msg"));
			Assert.Throws<UserException>(() => Checker.Range((decimal)4.1, (decimal)1.1, (decimal)1.1, "{0} {1} {2} Error Msg"));
		}




		/*===================================================================*/

		[Fact]
		public void MinLength_Test()
		{
			Checker.MinLength(null, 1, "{0} {1} Error Msg");
			Checker.MinLength("1", 1, "{0} {1} Error Msg");
			Checker.MinLength(new string[] { "1" }, 1, "{0} {1} Error Msg");
			Checker.MinLength(new List<string> { "1" }, 1, "{0} {1} Error Msg");
			Assert.True(true);
		}

		[Fact]
		public void MinLength_FailTest()
		{
			Assert.Throws<UserException>(() => Checker.MinLength("1", 3, "{0} {1} Error Msg"));
			Assert.Throws<UserException>(() => Checker.MinLength(new string[] { "1" }, 3, "{0} {1} Error Msg"));
			Assert.Throws<UserException>(() => Checker.MinLength(new List<string> { "1" }, 3, "{0} {1} Error Msg"));
		}



		/*===================================================================*/

		[Fact]
		public void MaxLength_Test()
		{
			Checker.MaxLength(null, 1, "{0} {1} Error Msg");
			Checker.MaxLength("1", 1, "{0} {1} Error Msg");
			Checker.MaxLength(new string[] { "1" }, 1, "{0} {1} Error Msg");
			Checker.MaxLength(new List<string> { "1" }, 1, "{0} {1} Error Msg");
			Assert.True(true);
		}

		[Fact]
		public void MaxLength_FailTest()
		{
			Assert.Throws<UserException>(() => Checker.MaxLength("11", 1, "{0} {1} Error Msg"));
			Assert.Throws<UserException>(() => Checker.MaxLength(new string[] { "1", "1" }, 1, "{0} {1} Error Msg"));
			Assert.Throws<UserException>(() => Checker.MaxLength(new List<string> { "1", "1" }, 1, "{0} {1} Error Msg"));
		}


		/*===================================================================*/

		[Fact]
		public void RangeLength_Test()
		{
			Checker.RangeLength(null, 1, 2, "{0} {1} Error Msg");
			Checker.RangeLength("1", 1, 2, "{0} {1} Error Msg");
			Checker.RangeLength(new string[] { "1" }, 1, 2, "{0} {1} Error Msg");
			Checker.RangeLength(new List<string> { "1" }, 1, 2, "{0} {1} Error Msg");
			Assert.True(true);
		}

		[Fact]
		public void RangeLength_FailTest()
		{
			Assert.Throws<UserException>(() => Checker.RangeLength("112", 1, 2, "{0} {1} Error Msg"));
			Assert.Throws<UserException>(() => Checker.RangeLength(new string[] { "1", "1", "1" }, 1, 2, "{0} {1} Error Msg"));
			Assert.Throws<UserException>(() => Checker.RangeLength(new List<string> { "1", "1", "1" }, 1, 2, "{0} {1} Error Msg"));
		}



		/*===================================================================*/

		[Fact]
		public void Contains_Test()
		{
			Checker.Contains(null, "a", "{0} {1} Error Msg");
			Checker.Contains("a", "a", "{0} {1} Error Msg");
			Checker.Contains("a", new string[] { "a" }, "{0} {1} Error Msg");
			Checker.Contains("a", new string[] { "A" }, "{0} {1} Error Msg");
			Checker.Contains("A", new string[] { "a" }, "{0} {1} Error Msg");
			Checker.Contains(1, new int[] { 1 }, "{0} {1} Error Msg");            
			Assert.True(true);
		}

		[Fact]
		public void Contains_FailTest()
		{
			Assert.Throws<UserException>(() => Checker.Contains("a", "b", "{0} {1} Error Msg"));
			Assert.Throws<UserException>(() => Checker.Contains("a", new string[] { "b" }, "{0} {1} Error Msg"));
			Assert.Throws<UserException>(() => Checker.Contains(1, new int[] { 2 }, "{0} {1} Error Msg"));
		}


		/*===================================================================*/


		[Fact]
		public void Pattern_Test()
		{
			Checker.Pattern(null, "^[a-z]+$", "{0} {1} Error Msg");
			Checker.Pattern("abc", "^[a-z]+$", "{0} {1} Error Msg");
			Checker.Pattern("ABC", "(?i)^[a-z]+$", "{0} {1} Error Msg");
			Assert.True(true);
		}

		[Fact]
		public void Pattern_FailTest()
		{
			Assert.Throws<UserException>(() => Checker.Pattern("Abc", "^[a-z]+$", "{0} {1} Error Msg"));
			Assert.Throws<UserException>(() => Checker.Pattern("123", "(?i)^[a-z]+$", "{0} {1} Error Msg"));
		}




	}
}
