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
		/// 此方法已重寫為使用 EF Core 8 支援的公開 API（ToQueryString），
		/// 不再依賴內部私有欄位反射。
		/// </summary>
		public static string ToSql<TEntity>(this IQueryable<TEntity> query) where TEntity : class
		{
			// EF Core 5+ 提供的公開 API，可直接取得查詢的 SQL 字串
			// 不需要反射或依賴內部實作
			return query.ToQueryString().Replace("\"", "");
		}
	}
}
