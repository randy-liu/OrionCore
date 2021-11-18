using Orion.Api.Tests;
using System.Collections.Generic;
using Xunit;
using System.IO;
using Microsoft.EntityFrameworkCore;
using System;

namespace Orion.Api.Extensions.Tests
{
	public class WhereHasExtensionsTests
	{

		private OrionApiDbContext _dc;

		public WhereHasExtensionsTests()
		{
			_dc = OrionApiDbContext.CreateUseNpgsql();
		}



		public static IEnumerable<object[]> RunTest_Data
		{
			get
			{
				yield return new object[] { 
					new InvoiceIssueDomain { InvoicePrefix = "FF" }, "WHERE",  (Action<string, string>)Assert.Contains
				};
				yield return new object[] { 
					new InvoiceIssueDomain { InvoicePrefix = "" }, "WHERE", (Action<string, string>)Assert.DoesNotContain
				};
			}
		}

		[Theory]
		[MemberData(nameof(RunTest_Data))]
		public void RunTest(InvoiceIssueDomain domain, string expected, Action<string, string> assert)
		{

			var query = _dc.InvoiceIssue.WhereHas(x => x.InvoicePrefix == domain.InvoicePrefix);
			var sql = query.ToSql();

			assert(expected, sql);	
		}


        [Fact]
        public void RunTest2()
        {
            var len = 22;

            var query = _dc.InvoiceIssue.WhereHas(x => x.InvoicePrefix.Length == len);
            var sql = query.ToString();
                       
        }


        [Fact]
		public void NullTest()
		{
			var domain = new InvoiceIssueDomain();

			var query = _dc.InvoiceIssue.WhereHas(x => x.InvoicePrefix == domain.InvoicePrefix.ToString());
			var sql = query.ToSql();

			Assert.DoesNotContain("WHERE", sql);
		}


	}



	public class InvoiceIssueDomain
	{
		/// <summary>發票字軌</summary>
		public string InvoicePrefix { get; set; }

	}

}
