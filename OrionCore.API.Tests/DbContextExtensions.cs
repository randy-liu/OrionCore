using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Orion.Api.Extensions
{
	/// <summary>定義 DatatContext 的 Extension</summary>
	public static class DbContextExtensions
	{

		/// <summary>將查詢條件的所有實體置於 pending delete 狀態。</summary>
		public static void RemoveRange<TEntity>(this DbSet<TEntity> table, Expression<Func<TEntity, bool>> predicate) where TEntity : class
		{
			table.RemoveRange(table.Where(predicate));
		}

		/// <summary>
		/// 取得 LINQ 查詢對應的 SQL 語句。
		/// 使用 EF Core 公開 API（ToQueryString），並將輸出正規化為跨平台穩定格式。
		/// </summary>
		public static string ToSql<TEntity>(this IQueryable<TEntity> query) where TEntity : class
		{
			if (query == null) { throw new ArgumentNullException(nameof(query)); }

			string sql = query.ToQueryString().Replace("\"", "");
			return normalizeSql(sql);
		}

		private static string normalizeSql(string sql)
		{
			sql = sql.Replace("\r\n", "\n").Replace('\r', '\n');
			return string.Join("\r\n", sql.Split('\n').Select(line => line.TrimEnd()));
		}
	}
}
