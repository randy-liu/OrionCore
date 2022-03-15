using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Orion.Api.Extensions;

namespace Orion.Api
{

    /// <summary>定義 Lambda Expression 的 Extension</summary>
    public static class LambdaUtils
    {


        /// <summary>尋找 Lambda Expression tree 中的 MethodInfo</summary>
        public static MethodInfo GetMethod<T1>(Expression<Action<T1>> expr) { return getMethod(expr); }

        /// <summary>尋找 Lambda Expression tree 中的 MethodInfo</summary>
        public static MethodInfo GetMethod(Expression<Action> expr) { return getMethod(expr); }

        /// <summary>尋找 Lambda Expression tree 中的 MethodInfo</summary>
        private static MethodInfo getMethod(Expression expr)
        {
            return expr.FindByType<MethodCallExpression>()
                .Select(x => x.Method)
                .FirstOrDefault();
        }


        /// <summary>尋找 Lambda Expression tree 中的 Generic Definition MethodInfo</summary>
        public static MethodInfo GetGenericMethodDefinition<T1>(Expression<Action<T1>> expr) { return getGenericMethodDefinition(expr); }

        /// <summary>尋找 Lambda Expression tree 中的 Generic Definition MethodInfo</summary>
        public static MethodInfo GetGenericMethodDefinition(Expression<Action> expr) { return getGenericMethodDefinition(expr); }

        /// <summary>尋找 Lambda Expression tree 中的 Generic Definition MethodInfo</summary>
        private static MethodInfo getGenericMethodDefinition(Expression expr)
        {
            MethodInfo method = getMethod(expr);
            if (method == null) { return null; }

            if (method.IsGenericMethod && !method.IsGenericMethodDefinition)
            { method = method.GetGenericMethodDefinition(); }

            return method;
        }



        /*=========================================================*/

        /// <summary></summary>
        public static Expression<Func<T, bool>> True<T>()
        {
            return (T f) => true;
        }

        /// <summary></summary>
        public static Expression<Func<T, bool>> False<T>()
        {
            return (T f) => false;
        }

    }
}