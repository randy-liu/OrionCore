using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Orion.Api.Extensions
{
    /// <summary>Initialization Extension Utility</summary>
    internal static class Utils
    {

        private static readonly MethodInfo _lambda;
        private static readonly MethodInfo _enumerableOrderBy;
        private static readonly MethodInfo _queryableOrderBy;
        private static readonly MethodInfo _queryableThenBy;


        static Utils()
        {
            _lambda = typeof(Expression).GetMethods()
                .Where(m => m.IsGenericMethod)
                .Where(m => m.Name == "Lambda")
                .Where(m => m.GetParameters().Length == 2)
                .First();

            _enumerableOrderBy = typeof(EnumerableExtensions).GetMethods()
                .Where(m => m.IsGenericMethod)
                .Where(m => m.Name == "OrderBy")
                .Where(m => m.GetParameters().Length == 3)
                .Where(m => m.GetGenericArguments().Length == 2)
                .First();

            _queryableOrderBy = typeof(QueryableExtensions).GetMethods()
                .Where(m => m.IsGenericMethod)
                .Where(m => m.Name == "OrderBy")
                .Where(m => m.GetParameters().Length == 3)
                .Where(m => m.GetGenericArguments().Length == 2)
                .First();

            _queryableThenBy = typeof(QueryableExtensions).GetMethods()
                .Where(m => m.IsGenericMethod)
                .Where(m => m.Name == "ThenBy")
                .Where(m => m.GetParameters().Length == 3)
                .Where(m => m.GetGenericArguments().Length == 2)
                .First();
        }


        /// <summary>取得 Lambda 泛型方法資訊。</summary>
        /// <param name="funcType">Lambda 委派型別。</param>
        /// <returns>對應泛型型別的 Lambda 方法資訊。</returns>
        public static MethodInfo LambdaMethod(Type funcType)
        {
            return _lambda.MakeGenericMethod(funcType);
        }

        /// <summary>取得 Enumerable 的 OrderBy 泛型方法資訊。</summary>
        /// <param name="modelType">來源模型型別。</param>
        /// <param name="propType">排序欄位型別。</param>
        /// <returns>對應泛型型別的 OrderBy 方法資訊。</returns>
        public static MethodInfo EnumerableOrderByMethod(Type modelType, Type propType)
        {
            return _enumerableOrderBy.MakeGenericMethod(modelType, propType);
        }

        /// <summary>取得 Queryable 的 OrderBy 泛型方法資訊。</summary>
        /// <param name="modelType">來源模型型別。</param>
        /// <param name="propType">排序欄位型別。</param>
        /// <returns>對應泛型型別的 OrderBy 方法資訊。</returns>
        public static MethodInfo QueryableOrderByMethod(Type modelType, Type propType)
        {
            return _queryableOrderBy.MakeGenericMethod(modelType, propType);
        }

        /// <summary>取得 Queryable 的 ThenBy 泛型方法資訊。</summary>
        /// <param name="modelType">來源模型型別。</param>
        /// <param name="propType">排序欄位型別。</param>
        /// <returns>對應泛型型別的 ThenBy 方法資訊。</returns>
        public static MethodInfo QueryableThenByMethod(Type modelType, Type propType)
        {
            return _queryableThenBy.MakeGenericMethod(modelType, propType);
        }

        /// <summary>建立指定屬性的鍵值 Lambda 表達式。</summary>
        /// <param name="modelType">模型型別。</param>
        /// <param name="prop">要建立表達式的屬性資訊。</param>
        /// <returns>對應屬性的 Lambda 表達式。</returns>
        public static LambdaExpression KeyExpression(Type modelType, PropertyInfo prop)
        {
            var paramExpr = Expression.Parameter(modelType, "x");
            var propExpr = Expression.Property(paramExpr, prop);

            Type funcType = typeof(Func<,>).MakeGenericType(modelType, prop.PropertyType);
            MethodInfo lambdaMethod = LambdaMethod(funcType);

            var keyExpression = lambdaMethod.Invoke(null, new object[] { propExpr, new[] { paramExpr } });
            return (LambdaExpression)keyExpression;
        }

    }
}
