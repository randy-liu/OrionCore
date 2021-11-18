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





		/*===========================================================================*/

		public static IEnumerable<object[]> OperatorTest_Data()
		{
			yield return new object[] { "DDD..FF", WhereOperator.Between };
			yield return new object[] { "..FF", WhereOperator.LessEquals };
			yield return new object[] { "DDD..", WhereOperator.GreaterEquals };
			yield return new object[] { "DDD|FF", WhereOperator.In };
			yield return new object[] { "!DDD|FF", WhereOperator.NotIn };
			yield return new object[] { "<=FF", WhereOperator.LessEquals };
			yield return new object[] { ">=FF", WhereOperator.GreaterEquals };
			yield return new object[] { "!=FF", WhereOperator.NotEquals };
			yield return new object[] { "^=FF", WhereOperator.StartsWith };
			yield return new object[] { "$=FF", WhereOperator.EndsWith };
			yield return new object[] { "*=FF", WhereOperator.Contains };
			yield return new object[] { "=FF", WhereOperator.Equals };
			yield return new object[] { "<FF", WhereOperator.LessThan };
			yield return new object[] { ">FF", WhereOperator.GreaterThan };
			yield return new object[] { "FF", WhereOperator.Equals };
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
			yield return new object[] { "InvoicePrefix", "", 0 };
			yield return new object[] { "InvoicePrefix", "DDD", 1 };
			yield return new object[] { "InvoicePrefix", "DDD|FF", 2 };
			yield return new object[] { "InvoicePrefix", ">=FF", 1 };

			yield return new object[] { "ProductQty", "", 0 };
			yield return new object[] { "ProductQty", "fff", 0 };
			yield return new object[] { "ProductQty", "11", 1 };
			yield return new object[] { "ProductQty", "12|34", 2 };
			yield return new object[] { "ProductQty", ">=12", 1 };

			yield return new object[] { "Sum", "", 0 };
			yield return new object[] { "Sum", "sss", 0 };
			yield return new object[] { "Sum", "11.11", 1 };
			yield return new object[] { "Sum", "12.32|34", 2 };
			yield return new object[] { "Sum", ">=12.89", 1 };

			yield return new object[] { "ModifyBy", "aaa", 0 };
			yield return new object[] { "ModifyBy", "11", 1 };
			yield return new object[] { "ModifyBy", "12|34", 2 };
			yield return new object[] { "ModifyBy", ">=12", 1 };

			yield return new object[] { "ModifyDate", "2016-0sss2-29", 0 };
			yield return new object[] { "ModifyDate", "2016-02-29", 1 };
			yield return new object[] { "ModifyDate", "2016-02-29|2016-02-29", 2 };
			yield return new object[] { "ModifyDate", ">=2016-02-29", 1 };
			yield return new object[] { "ModifyDate", ">=2016-02-29 12:00:00", 1 };

			yield return new object[] { "ClockIn", ">=12:00:00", 1 };
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
