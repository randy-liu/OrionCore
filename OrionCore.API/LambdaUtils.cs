using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Orion.Api
{

    /// <summary>定義 Lambda Expression 的 Extension</summary>
    public static class LambdaUtils
    {

        /// <summary>搜尋造訪者</summary>
        internal class ExpressionFinder<TFind> : ExpressionVisitor where TFind : Expression
        {
            public List<TFind> List { get; private set; }

            public ExpressionFinder(Expression expr)
            {
                List = new List<TFind>();
                Visit(expr);
            }

            public override Expression Visit(Expression expr)
            {
                var target = expr as TFind;
                if (target != null) { List.Add(target); }

                return base.Visit(expr);
            }
        }


        /// <summary>尋找 Lambda Expression tree 中指定類型</summary>
        public static List<T> FindByType<T>(Expression expr) where T : Expression
        {
            var finder = new ExpressionFinder<T>(expr);
            return finder.List;
        }


        /// <summary>尋找 Lambda Expression tree 中的 MemberInfo</summary>
        public static MemberInfo GetMember(LambdaExpression expr)
        {
            var paramType = expr.Parameters[0].Type;

            return FindByType<MemberExpression>(expr)
                .Where(x => x.Expression.Type == paramType)
                .Select(x => x.Member)
                .FirstOrDefault();
        }


        /// <summary>尋找 Lambda Expression tree 中的 PropertyInfo</summary>
        public static PropertyInfo GetProperty(LambdaExpression expr)
        {
            var paramType = expr.Parameters[0].Type;

            return FindByType<MemberExpression>(expr)
                .Where(x => x.Expression.Type == paramType)
                .Select(x => x.Member)
                .OfType<PropertyInfo>()
                .FirstOrDefault();
        }


        /// <summary>尋找 Lambda Expression tree 中的 PropertyInfo</summary>
        public static PropertyInfo[] GetCustomProperties(LambdaExpression expr)
        {
            return FindByType<MemberExpression>(expr)
                .Select(x => x.Member)
                .OfType<PropertyInfo>()
                .Where(p => p.DeclaringType.Namespace?.StartsWith("System") != true)
                .Reverse()
                .ToArray();
        }




        /*=========================================================*/


        /// <summary>尋找 Lambda Expression tree 中的 MethodInfo</summary>
        public static MethodInfo GetMethod<T1>(Expression<Action<T1>> expr) { return GetMethod((LambdaExpression)expr); }

        /// <summary>尋找 Lambda Expression tree 中的 MethodInfo</summary>
        public static MethodInfo GetMethod(Expression<Action> expr) { return GetMethod((LambdaExpression)expr); }

        /// <summary>尋找 Lambda Expression tree 中的 MethodInfo</summary>
        public static MethodInfo GetMethod(Expression expr)
        {
            return FindByType<MethodCallExpression>(expr)
                .Select(x => x.Method)
                .FirstOrDefault();
        }


        /// <summary>尋找 Lambda Expression tree 中的 Generic Definition MethodInfo</summary>
        public static MethodInfo GetGenericMethodDefinition<T1>(Expression<Action<T1>> expr) { return GetGenericMethodDefinition((LambdaExpression)expr); }

        /// <summary>尋找 Lambda Expression tree 中的 Generic Definition MethodInfo</summary>
        public static MethodInfo GetGenericMethodDefinition(Expression<Action> expr) { return GetGenericMethodDefinition((LambdaExpression)expr); }

        /// <summary>尋找 Lambda Expression tree 中的 Generic Definition MethodInfo</summary>
        public static MethodInfo GetGenericMethodDefinition(Expression expr)
        {
            MethodInfo method = GetMethod(expr);
            if (method == null) { return null; }

            if (method.IsGenericMethod && !method.IsGenericMethodDefinition)
            { method = method.GetGenericMethodDefinition(); }

            return method;
        }



        /*=========================================================*/

        /// <summary></summary>
        public static Expression<Func<T, bool>> False<T>()
        {
            return (T f) => false;
        }

    }
}