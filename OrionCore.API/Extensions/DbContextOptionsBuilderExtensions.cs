using System;
using Microsoft.EntityFrameworkCore;

namespace Orion.Api.Extensions
{
	/// <summary>DbContextOptionsBuilder 常用資料庫設定擴充。</summary>
	public static class DbContextOptionsBuilderExtensions
	{
		/// <summary>使用 SQL Server 作為 DbContext provider。</summary>
		public static DbContextOptionsBuilder UseOrionSqlServer(this DbContextOptionsBuilder optionsBuilder, string connectionString)
		{
			if (optionsBuilder == null) { throw new ArgumentNullException(nameof(optionsBuilder)); }
			if (string.IsNullOrWhiteSpace(connectionString)) { throw new ArgumentException("connectionString cannot be empty.", nameof(connectionString)); }

			return optionsBuilder.UseSqlServer(connectionString);
		}

		/// <summary>使用 SQL Server 作為 DbContext provider。</summary>
		public static DbContextOptionsBuilder<TContext> UseOrionSqlServer<TContext>(this DbContextOptionsBuilder<TContext> optionsBuilder, string connectionString)
			where TContext : DbContext
		{
			if (optionsBuilder == null) { throw new ArgumentNullException(nameof(optionsBuilder)); }
			if (string.IsNullOrWhiteSpace(connectionString)) { throw new ArgumentException("connectionString cannot be empty.", nameof(connectionString)); }

			return optionsBuilder.UseSqlServer(connectionString);
		}
	}
}
