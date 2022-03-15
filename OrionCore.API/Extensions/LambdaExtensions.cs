using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Orion.Api.Extensions
{

    /// <summary>定義 Lambda Expression 的 Extension</summary>
    public static class LambdaExtensions
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
        public static List<T> FindByType<T>(this Expression expr) where T : Expression
        {
            var finder = new ExpressionFinder<T>(expr);
            return finder.List;
        }




        /// <summary>尋找 Lambda Expression tree 中的 MemberInfo</summary>
        public static MemberInfo GetMember(this LambdaExpression expr)
        {
            var paramType = expr.Parameters[0].Type;

            return FindByType<MemberExpression>(expr)
                .Where(x => x.Expression.Type == paramType)
                .Select(x => x.Member)
                .FirstOrDefault();
        }


        /// <summary>尋找 Lambda Expression tree 中的 PropertyInfo</summary>
        public static PropertyInfo GetProperty(this LambdaExpression expr)
        {
            var paramType = expr.Parameters[0].Type;

            return FindByType<MemberExpression>(expr)
                .Where(x => x.Expression.Type == paramType)
                .Select(x => x.Member)
                .OfType<PropertyInfo>()
                .FirstOrDefault();
        }


        ///// <summary>尋找 Lambda Expression tree 中的 PropertyInfo</summary>
        //public static PropertyInfo[] GetCustomProperties(this LambdaExpression expr)
        //{
        //    return FindByType<MemberExpression>(expr)
        //        .Select(x => x.Member)
        //        .OfType<PropertyInfo>()
        //        .Where(p => p.DeclaringType.Namespace?.StartsWith("System") != true)
        //        .Reverse()
        //        .ToArray();
        //}





        /*==========================================================*/

        /// <summary>參數替換造訪者</summary>
        class ExpressionReplacer : ExpressionVisitor
        {
            private readonly Expression _oldExpr;
            private readonly Expression _newExpr;

            public ExpressionReplacer(Expression oldExpr, Expression newExpr)
            {
                _oldExpr = oldExpr;
                _newExpr = newExpr;
            }

            public override Expression Visit(Expression expr)
            {
                if (expr == _oldExpr) { expr = _newExpr; }
                return base.Visit(expr);
            }
        }



        /// <summary>Expression 節點替換</summary>
        public static Expression Replace(this Expression source, Expression oldExpr, Expression newExpr)
        {
            var replacer = new ExpressionReplacer(oldExpr, newExpr);
            Expression expr = replacer.Visit(source);
            return expr;
        }



        /// <summary></summary>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            /* 替換參數 */
            Expression expr = Replace(second.Body, second.Parameters[0], first.Parameters[0]);

            /* 組合成新的 LambdaExpression */
            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(first.Body, expr), first.Parameters);
        }



        /// <summary>Expression Or 串接</summary>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            /* 替換參數 */
            Expression expr = Replace(second.Body, second.Parameters[0], first.Parameters[0]);

            /* 組合成新的 LambdaExpression */
            return Expression.Lambda<Func<T, bool>>(Expression.OrElse(first.Body, expr), first.Parameters);
        }




    }
}