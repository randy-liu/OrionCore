using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orion.Api.Models;

namespace Orion.Api.Extensions
{
	/// <summary>資料表空間統計查詢相關擴充。</summary>
	public static class TableInfoExtensions
	{
		private const string SqlServerProvider = "Microsoft.EntityFrameworkCore.SqlServer";
		private const string NpgsqlProvider = "Npgsql.EntityFrameworkCore.PostgreSQL";

		/// <summary>取得資料表空間統計資訊。</summary>
		public static List<TableInfo> GetTableInfo(this DbContext context)
		{
			return context.Set<TableInfo>().ToList();
		}

		/// <summary>設定 Npgsql 的 TableInfo 查詢。</summary>
		public static EntityTypeBuilder<TableInfo> TableInfoUseNpgsql(this ModelBuilder modelBuilder, DbContext context)
		{
			return modelBuilder.Entity<TableInfo>()
				.HasNoKey()
				.ToSqlQuery(@"
					SELECT
						*,
						CAST (0 AS BIGINT)AS UnusedBytes,
						(TotalBytes - IndexBytes - ToastBytes) AS TableBytes
					FROM (
						SELECT
							t.table_schema AS Schema,
							t.table_name AS Name,
							s.n_live_tup AS TotalRows,
							pg_total_relation_size(c.oid) AS TotalBytes,
							pg_indexes_size(c.oid) AS IndexBytes,
							COALESCE(pg_total_relation_size(c.reltoastrelid),0) AS ToastBytes
						FROM information_schema.tables t
						INNER JOIN pg_class c ON t.table_name = c.relname
						INNER JOIN pg_stat_user_tables s ON t.table_name = s.relname
						WHERE t.table_schema = 'public'
					) a
					ORDER BY Name ASC
				");
		}

		/// <summary>設定 SQL Server 的 TableInfo 查詢。</summary>
		public static EntityTypeBuilder<TableInfo> TableInfoUseSqlServer(this ModelBuilder modelBuilder, DbContext context)
		{
			return modelBuilder.Entity<TableInfo>()
				.HasNoKey()
				.ToSqlQuery(@"
					CREATE TABLE #T (
						[Name] VARCHAR(200),
						[Rows] VARCHAR(20),
						[Total] VARCHAR(20),
						[Data] VARCHAR(20),
						[Index] VARCHAR(20),
						[Unused] VARCHAR(20)
					);
					EXEC sp_MSforeachtable 'INSERT INTO #T EXEC sp_spaceused [?]';
					SELECT
						'' AS [Schema],
						[Name],
						CAST([Rows] AS BIGINT) AS [TotalRows],
						(CAST(REPLACE([Total],'KB','') AS BIGINT) * 1024) AS [TotalBytes],
						(CAST(REPLACE([Data],'KB','') AS BIGINT) * 1024) AS [TableBytes],
						(CAST(REPLACE([Index],'KB','') AS BIGINT) * 1024) AS [IndexBytes],
						(CAST(REPLACE([Unused],'KB','') AS BIGINT) * 1024) AS [UnusedBytes],
						CAST(0 AS BIGINT) AS [ToastBytes]
					FROM #T
					ORDER BY [Name] ASC
				");
		}

		/// <summary>依資料庫 provider 套用對應的 TableInfo 查詢映射。</summary>
		public static ModelBuilder ApplyTableInfoQueryByProvider(this ModelBuilder modelBuilder, DbContext context)
		{
			if (modelBuilder == null) { throw new ArgumentNullException(nameof(modelBuilder)); }
			if (context == null) { throw new ArgumentNullException(nameof(context)); }

			string providerName = context.Database.ProviderName;
			if (providerName == SqlServerProvider)
			{
				modelBuilder.TableInfoUseSqlServer(context);
			}
			else if (providerName == NpgsqlProvider)
			{
				modelBuilder.TableInfoUseNpgsql(context);
			}

			return modelBuilder;
		}
	}
}
