using System;
using Microsoft.EntityFrameworkCore;
using Orion.Api.Extensions;
using Orion.Api.Models;
using Xunit;

namespace Orion.Api.Tests
{
	public class SqlServerSupportTests
	{
		[Fact]
		public void UseOrionSqlServer_ConfiguresSqlServerProvider()
		{
			var builder = new DbContextOptionsBuilder<OrionApiDbContext>();
			builder.UseOrionSqlServer("Data Source=localhost;Initial Catalog=Orion_API_Tests;Integrated Security=True;TrustServerCertificate=True");

			using var db = new OrionApiDbContext(builder.Options);
			Assert.Equal("Microsoft.EntityFrameworkCore.SqlServer", db.Database.ProviderName);
		}

		[Fact]
		public void ApplyTableInfoQueryByProvider_MapsTableInfoForSqlServer()
		{
			using var db = OrionApiDbContext.CreateUseSqlServer();
			var entityType = db.Model.FindEntityType(typeof(TableInfo));

			Assert.NotNull(entityType);
			Assert.Null(entityType.FindPrimaryKey());
		}

		[Fact]
		public void SqlServer_GetTableInfo_SmokeTest_WhenConnectionConfigured()
		{
			string connectionString = Environment.GetEnvironmentVariable("ORIONCORE_SQLSERVER_TEST_CONN");
			if (string.IsNullOrWhiteSpace(connectionString)) { return; }

			using var db = OrionApiDbContext.CreateUseSqlServer(connectionString);
			_ = db.GetTableInfo();
		}
	}
}
