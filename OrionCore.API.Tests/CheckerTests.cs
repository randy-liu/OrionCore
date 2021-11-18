using System;
using System.Collections.Generic;
using Xunit;

namespace Orion.Api.Tests
{
	public class CheckerTests
	{


		public static IEnumerable<object[]> Has_Test_Data()
		{
			yield return new object[] { 1 };
			yield return new object[] { DateTime.Now };
			yield return new object[] { "ss" };
			yield return new object[] { new string[] { "sss" } };
			yield return new object[] { new List<string>() { "sss" } };
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
			yield return new object[] { null };
			yield return new object[] { 0 };
			yield return new object[] { "" };
			yield return new object[] { new string[] { } };
			yield return new object[] { new List<string>() }; 
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
			yield return new object[] { (int?)1, 1 };
			yield return new object[] { "1", "1" };
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
			yield return new object[] { (int?)null, 1 };
			yield return new object[] { 2, 1 };
			yield return new object[] { "", "1" };
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
