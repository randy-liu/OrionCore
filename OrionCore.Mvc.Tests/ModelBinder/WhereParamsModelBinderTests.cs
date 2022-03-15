using Microsoft.AspNetCore.Mvc.ModelBinding;
using Orion.Api.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Xunit;

namespace Orion.Mvc.ModelBinder.Tests
{
	public class WhereParamsModelBinderTests
	{
		/// <summary>New Case 新增案例</summary>
		private static object[] n(params object[] values) { return values; }



		/*===========================================================================*/

		public static IEnumerable<object[]> OperatorTest_Data()
		{
			return new[]
			{
				n( "DDD..FF", WhereOperator.Between ),
				n( "..FF", WhereOperator.LessEquals ),
				n( "DDD..", WhereOperator.GreaterEquals ),
				n( "DDD|FF", WhereOperator.In ),
				n( "!DDD|FF", WhereOperator.NotIn ),
				n( "<=FF", WhereOperator.LessEquals ),
				n( ">=FF", WhereOperator.GreaterEquals ),
				n( "!=FF", WhereOperator.NotEquals ),
				n( "^=FF", WhereOperator.StartsWith ),
				n( "$=FF", WhereOperator.EndsWith ),
				n( "*=FF", WhereOperator.Contains ),
				n( "=FF", WhereOperator.Equals ),
				n( "<FF", WhereOperator.LessThan ),
				n( ">FF", WhereOperator.GreaterThan ),
				n( "FF", WhereOperator.Equals ),
			};
		}


		[Theory]
		[MemberData(nameof(OperatorTest_Data))]
		public void OperatorTest(string value, WhereOperator expected)
		{
			var modelState = new ModelStateDictionary();
			var collection = new NameValueCollection();
			collection["InvoicePrefix"] = value;

			var modelBinder = new WhereParamsModelBinder();
			var obj = modelBinder.CreateWhereParams(typeof(TestParamsDomain), collection, modelState);
			var param = obj as WhereParams<TestParamsDomain>;

			Assert.Equal(param.GetOperator(x => x.InvoicePrefix), expected);
		}





		/*===========================================================================*/

		public static IEnumerable<object[]> ConvertTest_Data()
		{
			return new[]
			{
				//n( "InvoicePrefix", "", 0 ),
				//n( "InvoicePrefix", "DDD", 1 ),
				//n( "InvoicePrefix", "DDD|FF", 2),
				//n( "InvoicePrefix", ">=FF", 1 ),
				//n( "ProductQty", "", 0 ),
				//n( "ProductQty", "fff", 0 ),
				//n( "ProductQty", "11", 1 ),
				//n( "ProductQty", "12|34", 2 ),
				//n( "ProductQty", ">=12", 1 ),
				//n( "Sum", "", 0 ),
				//n( "Sum", "sss", 0 ),
				//n( "Sum", "11.11", 1 ),
				//n( "Sum", "12.32|34", 2 ),
				//n( "Sum", ">=12.89", 1 ),
				//n( "ModifyBy", "aaa", 0 ),
				//n( "ModifyBy", "11", 1 ),
				//n( "ModifyBy", "12|34", 2 ),
				//n( "ModifyBy", ">=12", 1 ),
				//n( "ModifyDate", "2016-0sss2-29", 0 ),
				//n( "ModifyDate", "2016-02-29", 1 ),
				//n( "ModifyDate", "2016-02-29|2016-02-29", 2 ),
				//n( "ModifyDate", ">=2016-02-29", 1 ),
				n( "ModifyDate", ">=2016-02-29 12:00:00", 1 ),
				//n( "ClockIn", ">=12:00:00", 1 ),
			};
		}


		[Theory]
		[MemberData(nameof(ConvertTest_Data))]
		public void ConvertTest(string name, string value, int length)
		{
			var modelState = new ModelStateDictionary();
			var collection = new NameValueCollection();
			collection[name] = value;

			var modelBinder = new WhereParamsModelBinder();
			var obj = modelBinder.CreateWhereParams(typeof(TestParamsDomain), collection, modelState);
			var param = obj as WhereParams;

			Assert.Equal(param.GetValues(name).Length, length);
		}


	}






	class TestParamsDomain
	{
		/// <summary>數量</summary>
		public int? ProductQty { get; set; }

		/// <summary>總計</summary>
		public decimal Sum { get; set; }

		/// <summary>發票字軌</summary>
		public string InvoicePrefix { get; set; }

		/// <summary>修改者 Id</summary>
		public int ModifyBy { get; set; }

		/// <summary>修改時間</summary>
		public DateTime ModifyDate { get; set; }

		/// <summary>時間</summary>
		public TimeSpan ClockIn { get; set; }
	}

}
