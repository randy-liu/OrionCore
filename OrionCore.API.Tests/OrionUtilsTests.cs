using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;
using Orion.Api.Models;

namespace Orion.Api.Tests
{
    public class OrionUtilsTests
    {

		public class IssueDomain
		{
			public decimal Sum { get; set; }
			public string InvoicePrefix { get; set; }
			public UseStatus UseStatus { get; set; }
		}


		/// <summary>New Case 新增案例</summary>
		private static object[] n(params object[] values) { return values; }



		/*=======================================================*/


		public static IEnumerable<object[]> ParseSolarDay_Test_Data()
		{
			return new[]
			{
				n( 110131, new DateTime(2010,5, 11) ),
				n( 116131, new DateTime(2016, 5, 10) ),
			};
		}

		[Theory]
		[MemberData(nameof(ParseSolarDay_Test_Data))]
		public void ParseSolarDay_Test(int value, DateTime? date)
		{
			DateTime? res = OrionUtils.ParseSolarDay(value);
			Assert.Equal(res, date);
		}



		/*=======================================================*/



        public static IEnumerable<object[]> ParseCnDate_Test_Data()
        {
			return new[]
			{
				n( "104", null ),
				n( "104.5.5", new DateTime(2015, 5, 5) ),
				n( "105.02.05", new DateTime(2016, 2, 5) ),
			};
		}

        [Theory]
		[MemberData(nameof(ParseCnDate_Test_Data))]
        public void ParseCnDate_Test(string value, DateTime? date)
        {
            DateTime? res = OrionUtils.ParseCnDate(value);
            Assert.Equal(res, date);
        }




		/*=======================================================*/

		[Fact]
		public void IsLockedProcessId_Test()
		{
			OrionUtils.IsLockedProcessId("TTT");
			
		}




	}


}
