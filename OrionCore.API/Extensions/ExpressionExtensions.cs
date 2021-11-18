using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Orion.Api.Extensions
{

    /// <summary>定義 Lambda Expression 的 Extension</summary>
    public static class ExpressionExtensions
    {

        /// <summary>尋找 Lambda Expression tree 中指定類型</summary>
        public static List<T> FindByType<T>(this Expression expr) where T : Expression
        {
            return LambdaUtils.FindByType<T>(expr);
        }

        /// <summary>尋找 Lambda Expression tree 中的 MemberInfo</summary>
        public static MemberInfo GetMember(this LambdaExpression expr)
        {
            return LambdaUtils.GetMember(expr);
        }

        /// <summary>尋找 Lambda Expression tree 中的 PropertyInfo</summary>
        public static PropertyInfo GetProperty(this LambdaExpression expr)
        {
            return LambdaUtils.GetProperty(expr);
        }


        /// <summary>尋找 Lambda Expression tree 中的 MethodInfo</summary>
        public static MethodInfo GetMethod(this Expression expr)
        {
            return LambdaUtils.GetMethod(expr);
        }

        /// <summary>尋找 Lambda Expression tree 中的 Generic Definition MethodInfo</summary>
        public static MethodInfo GetGenericMethodDefinition(this Expression expr)
        {
            return LambdaUtils.GetGenericMethodDefinition(expr);
        }






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