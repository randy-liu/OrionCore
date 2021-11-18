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


        public static MethodInfo LambdaMethod(Type funcType)
        {
            return _lambda.MakeGenericMethod(funcType);
        }

        public static MethodInfo EnumerableOrderByMethod(Type modelType, Type propType)
        {
            return _enumerableOrderBy.MakeGenericMethod(modelType, propType);
        }

        public static MethodInfo QueryableOrderByMethod(Type modelType, Type propType)
        {
            return _queryableOrderBy.MakeGenericMethod(modelType, propType);
        }

        public static MethodInfo QueryableThenByMethod(Type modelType, Type propType)
        {
            return _queryableThenBy.MakeGenericMethod(modelType, propType);
        }

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