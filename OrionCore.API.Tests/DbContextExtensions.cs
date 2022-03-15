using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;

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



		private static T getPrivate<T>(this object obj, string privateField) 
		{
			return (T)obj?.GetType().GetField(privateField, BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(obj);
		}

		public static string ToSql<TEntity>(this IQueryable<TEntity> query) where TEntity : class
		{
			IEnumerator<TEntity> enumerator = query.Provider.Execute<IEnumerable<TEntity>>(query.Expression).GetEnumerator();
			
			var relationalQueryContext = enumerator.getPrivate<RelationalQueryContext>("_relationalQueryContext");
			var relationalCommandCache = enumerator.getPrivate<RelationalCommandCache>("_relationalCommandCache");

#pragma warning disable EF1001 // Internal EF Core API usage.
			IRelationalCommand command = relationalCommandCache.GetRelationalCommand(relationalQueryContext.ParameterValues);
#pragma warning restore EF1001 // Internal EF Core API usage.

			return command.CommandText.Replace("\"", "");
		}





	}
}
