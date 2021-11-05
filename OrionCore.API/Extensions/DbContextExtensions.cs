using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace OrionCore.API.Extensions
{
	/// <summary>定義 DatatContext 的 Extension</summary>
	public static class DbContextExtensions
	{

		/// <summary>將查詢條件的所有實體置於 pending delete 狀態。</summary>
		public static void RemoveRange<TEntity>(this DbSet<TEntity> table, Expression<Func<TEntity, bool>> predicate) where TEntity : class
		{
			table.RemoveRange(table.Where(predicate));
		}






	}
}
