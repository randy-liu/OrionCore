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

            /// <summary>建立搜尋造訪者並立即掃描運算式樹。</summary>
            /// <param name="expr">要搜尋的運算式樹根節點。</param>
            public ExpressionFinder(Expression expr)
            {
                List = new List<TFind>();
                Visit(expr);
            }

            /// <summary>造訪運算式節點並收集符合型別的節點。</summary>
            /// <param name="expr">目前要造訪的運算式節點。</param>
            /// <returns>造訪後的運算式節點。</returns>
            public override Expression Visit(Expression expr)
            {
                var target = expr as TFind;
                if (target != null) { List.Add(target); }

                return base.Visit(expr);
            }
        }


        /// <summary>在運算式樹中找出指定型別的節點。</summary>
        /// <typeparam name="T">要搜尋的節點型別。</typeparam>
        /// <param name="expr">要搜尋的運算式樹。</param>
        /// <returns>符合型別的節點清單。</returns>
        public static List<T> FindByType<T>(this Expression expr) where T : Expression
        {
            var finder = new ExpressionFinder<T>(expr);
            return finder.List;
        }




        /// <summary>從 Lambda 運算式中取得第一個對應參數型別的成員資訊。</summary>
        /// <param name="expr">要分析的 Lambda 運算式。</param>
        /// <returns>成員資訊；找不到時回傳 `null`。</returns>
        public static MemberInfo GetMember(this LambdaExpression expr)
        {
            var paramType = expr.Parameters[0].Type;

            return FindByType<MemberExpression>(expr)
                .Where(x => x.Expression.Type == paramType)
                .Select(x => x.Member)
                .FirstOrDefault();
        }


        /// <summary>從 Lambda 運算式中取得第一個對應參數型別的屬性資訊。</summary>
        /// <param name="expr">要分析的 Lambda 運算式。</param>
        /// <returns>屬性資訊；找不到時回傳 `null`。</returns>
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

            /// <summary>建立節點替換造訪者。</summary>
            /// <param name="oldExpr">要被替換的節點。</param>
            /// <param name="newExpr">用來取代的節點。</param>
            public ExpressionReplacer(Expression oldExpr, Expression newExpr)
            {
                _oldExpr = oldExpr;
                _newExpr = newExpr;
            }

            /// <summary>造訪運算式並替換目標節點。</summary>
            /// <param name="expr">目前要造訪的節點。</param>
            /// <returns>替換後的節點。</returns>
            public override Expression Visit(Expression expr)
            {
                if (expr == _oldExpr) { expr = _newExpr; }
                return base.Visit(expr);
            }
        }



        /// <summary>以新節點取代運算式樹中的指定節點。</summary>
        /// <param name="source">要進行替換的來源運算式樹。</param>
        /// <param name="oldExpr">要被替換的來源運算式節點。</param>
        /// <param name="newExpr">替換後的新運算式節點。</param>
        /// <returns>替換後的運算式樹。</returns>
        public static Expression Replace(this Expression source, Expression oldExpr, Expression newExpr)
        {
            var replacer = new ExpressionReplacer(oldExpr, newExpr);
            Expression expr = replacer.Visit(source);
            return expr;
        }



		/// <summary>將兩個條件以 `AndAlso` 串接為新條件。</summary>
		/// <typeparam name="T">目標型別。</typeparam>
		/// <param name="first">第一個布林條件運算式（也作為參數基準）。</param>
		/// <param name="second">要串接的第二個布林條件運算式。</param>
		/// <returns>串接後條件。</returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            /* 替換參數 */
            Expression expr = Replace(second.Body, second.Parameters[0], first.Parameters[0]);

            /* 組合成新的 LambdaExpression */
            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(first.Body, expr), first.Parameters);
        }



        /// <summary>將兩個條件以 `OrElse` 串接為新條件。</summary>
        /// <typeparam name="T">目標型別。</typeparam>
        /// <param name="first">第一個布林條件運算式（也作為參數基準）。</param>
        /// <param name="second">要串接的第二個條件。</param>
        /// <returns>串接後條件。</returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            /* 替換參數 */
            Expression expr = Replace(second.Body, second.Parameters[0], first.Parameters[0]);

            /* 組合成新的 LambdaExpression */
            return Expression.Lambda<Func<T, bool>>(Expression.OrElse(first.Body, expr), first.Parameters);
        }




    }
}
