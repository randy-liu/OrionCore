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


        /// <summary>從 Lambda 運算式中擷取方法資訊。</summary>
        /// <param name="expr">包含方法呼叫的運算式。</param>
        /// <returns>找到的 <see cref="MethodInfo"/>；找不到時為 <c>null</c>。</returns>
        public static MethodInfo GetMethod<T1>(Expression<Action<T1>> expr) { return getMethod(expr); }

        /// <summary>從 Lambda 運算式中擷取方法資訊。</summary>
        /// <param name="expr">包含方法呼叫的運算式。</param>
        /// <returns>找到的 <see cref="MethodInfo"/>；找不到時為 <c>null</c>。</returns>
        public static MethodInfo GetMethod(Expression<Action> expr) { return getMethod(expr); }

        /// <summary>尋找 Lambda Expression tree 中的 MethodInfo</summary>
        private static MethodInfo getMethod(Expression expr)
        {
            return expr.FindByType<MethodCallExpression>()
                .Select(x => x.Method)
                .FirstOrDefault();
        }


        /// <summary>從 Lambda 運算式中擷取泛型方法定義資訊。</summary>
        /// <param name="expr">包含方法呼叫的運算式。</param>
        /// <returns>泛型方法定義 <see cref="MethodInfo"/>；找不到時為 <c>null</c>。</returns>
        public static MethodInfo GetGenericMethodDefinition<T1>(Expression<Action<T1>> expr) { return getGenericMethodDefinition(expr); }

        /// <summary>從 Lambda 運算式中擷取泛型方法定義資訊。</summary>
        /// <param name="expr">包含方法呼叫的運算式。</param>
        /// <returns>泛型方法定義 <see cref="MethodInfo"/>；找不到時為 <c>null</c>。</returns>
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

        /// <summary>建立永遠回傳 <c>true</c> 的條件運算式。</summary>
        /// <typeparam name="T">條件目標型別。</typeparam>
        /// <returns>固定為真值的條件運算式。</returns>
        public static Expression<Func<T, bool>> True<T>()
        {
            return (T f) => true;
        }

        /// <summary>建立永遠回傳 <c>false</c> 的條件運算式。</summary>
        /// <typeparam name="T">條件目標型別。</typeparam>
        /// <returns>固定為假值的條件運算式。</returns>
        public static Expression<Func<T, bool>> False<T>()
        {
            return (T f) => false;
        }

    }
}
