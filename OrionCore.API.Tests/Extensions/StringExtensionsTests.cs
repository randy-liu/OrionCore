using Orion.Api.Tests;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using Xunit;

namespace Orion.Api.Extensions.Tests
{
	public class StringExtensionsTests
	{
		public enum JobStatus
		{
			Create,
			Execute,
		}

		public enum Floor
		{
			None,
			F1,
			F2,
			F3,
			F4,
		}


		/// <summary>New Case 新增案例</summary>
		private static object[] n(params object[] values) { return values; }



		/*=======================================================*/


		public static IEnumerable<object[]> ToEnum_Test_Data()
		{
			return new[] 
			{
				n( "Create", JobStatus.Create ),
				n( "Execute", JobStatus.Execute ),
			};
		}

		[Theory]
		[MemberData(nameof(ToEnum_Test_Data))]
		public void ToEnum_Test(string enumStr, JobStatus expected)
		{
			JobStatus result = enumStr.ToEnum<JobStatus>();
			Assert.Equal(expected, result);
		}


		public static IEnumerable<object[]> ToEnum_FailTest_Data()
		{
			return new[] 
			{
				n( "Creates" ),
				n( "SSSS" ),
				n( (string)null ),
			};
		}

		[Theory]
		[MemberData(nameof(ToEnum_FailTest_Data))]
		public void ToEnum_FailTest(string enumStr)
		{
			Assert.ThrowsAny<Exception>(() => enumStr.ToEnum<JobStatus>());
		}




		public static IEnumerable<object[]> ToEnumOrDefault_Test_Data()
		{
			return new[] 
			{
				n( null, JobStatus.Create ),
				n( "SSSS", JobStatus.Create ),
				n( "Create", JobStatus.Create ),
				n( "Execute", JobStatus.Execute ),
			};
		}

		[Theory]
		[MemberData(nameof(ToEnumOrDefault_Test_Data))]
		public void ToEnumOrDefault_Test(string enumStr, JobStatus expected)
		{
			JobStatus result = enumStr.ToEnum<JobStatus>(JobStatus.Create);
			Assert.Equal(expected, result);
		}




        /*=======================================================*/

        [Theory]
        [InlineData(null, 0)]
        [InlineData("", 0)]
        [InlineData("1, 2, 3", 3)]
        public void ToIdsList_Test(string value, int length)
        {
            var list = value.ToIdsList<string>();
            Assert.Equal(list.Count, length);
        }


        [Theory]
        [InlineData(null, 0)]
        [InlineData("", 0)]
        [InlineData("1,2,3", 3)]
        [InlineData("1,2,3,3,3", 3)]
        [InlineData("1,2,3,sss,ffff", 3)]
        public void ToIdsList_IntTest(string value, int length)
        {
            var list = value.ToIdsList<int>();
            Assert.Equal(list.Count, length);
        }

        [Theory]
        [InlineData(null, 0)]
        [InlineData("", 0)]
        [InlineData("F1,F2,F3", 3)]
        [InlineData("F1,F2,F3,F3,F3", 3)]
        [InlineData("F1,F2,F3,sss,ffff", 3)]
        public void ToIdsList_EnumTest(string value, int length)
        {
            var list = value.ToIdsList<Floor>();
            Assert.Equal(list.Count, length);
        }



		 

    }



}
