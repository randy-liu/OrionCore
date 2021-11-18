using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using Orion.Api.Tests;
using Xunit;
using Microsoft.EntityFrameworkCore;


namespace Orion.Api.Extensions.Tests
{
	public class QueryableExtensionsTests
	{
		private OrionApiDbContext _dc;

        public QueryableExtensionsTests()
        {
			_dc = OrionApiDbContext.CreateUseNpgsql();
        }

         

        [Fact]
        public void AdvancedOrderBy_RunTest()
        {
            var sqlA = _dc.InvoiceIssue
                .AdvancedOrderBy("InvoiceId")
                .ToSql();

            Assert.Contains("ORDER BY i.\"InvoiceId\" DESC", sqlA);


            var sqlB = _dc.InvoiceIssue
                .AdvancedOrderBy("InvoiceId,-InvoicePrefix,InvoiceDate")
                .ToSql();

            Assert.Contains("ORDER BY i.\"InvoiceId\", i.\"InvoicePrefix\" DESC, i.\"InvoiceDate\"", sqlB);
            
        }


        [Fact]
		public void MaxOrDefault_RunTest()
		{
			new List<UserModel> { }
                .AsQueryable()
                .MaxOrDefault(x => x.UserId);

			new List<UserModel> { new UserModel { UserId = 2 } }
                .AsQueryable()
                .MaxOrDefault(x => x.UserId);
		}

		[Fact]
		public void MinOrDefault_RunTest()
		{
			new List<UserModel> { }
                .AsQueryable()
                .MinOrDefault(x => x.UserId);

			new List<UserModel> { new UserModel{ UserId = 2 } }
                .AsQueryable()
                .MinOrDefault(x => x.UserId);
		}

	}

}
