using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Orion.Api.Extensions;

namespace Orion.Api
{
    /// <summary>變數監看</summary>
    public static class VariableMonitor
    {
        private static readonly Collector _collector = new Collector();

        /// <summary>加入要監看的變數運算式。</summary>
        /// <typeparam name="T">變數值型別。</typeparam>
        /// <param name="variableExpr">取得變數值的運算式。</param>
        /// <returns>目前收集器實例。</returns>
        public static Collector Add<T>(Expression<Func<T>> variableExpr)
        {
            return _collector.Add(variableExpr);
        }
        /// <summary>加入要監看的變數運算式，並指定額外子鍵值。</summary>
        /// <typeparam name="T">變數值型別。</typeparam>
        /// <param name="variableExpr">取得變數值的運算式。</param>
        /// <param name="subKey">附加在型別名稱後的子鍵字串。</param>
        /// <returns>目前收集器實例。</returns>
        public static Collector Add<T>(Expression<Func<T>> variableExpr, string subKey)
        {
            return _collector.Add(variableExpr, subKey);
        }


        /// <summary>取得目前所有監看變數的快照結果。</summary>
        /// <returns>以鍵值對表示的變數資料。</returns>
        public static Dictionary<string, object> Get()
        {
            return _collector.Get();
        }


        /// <summary>變數收集器。</summary>
        public class Collector
        {
            private Action<Dictionary<string, object>> _collectVariable = dict => { };

            /// <summary>加入要監看的變數運算式。</summary>
            /// <typeparam name="T">變數值型別。</typeparam>
            /// <param name="variableExpr">取得變數值的運算式。</param>
            /// <returns>目前收集器實例。</returns>
            public Collector Add<T>(Expression<Func<T>> variableExpr)
            {
                return Add(variableExpr, null);
            }

            /// <summary>加入要監看的變數運算式，並指定額外子鍵值。</summary>
            /// <typeparam name="T">變數值型別。</typeparam>
            /// <param name="variableExpr">取得變數值的運算式。</param>
            /// <param name="subKey">附加在型別名稱後的子鍵字串。</param>
            /// <returns>目前收集器實例。</returns>
            public Collector Add<T>(Expression<Func<T>> variableExpr, string subKey)
            {
                var memberExpr = variableExpr.FindByType<MemberExpression>().FirstOrDefault();

                string key = parseKey(memberExpr, subKey);
                if (key == null) { throw new ArgumentException($"無法解析 {variableExpr} 中的成員欄位"); }
                var getter = variableExpr.Compile();

                _collectVariable += (dict => { dict[key] = getter(); });

                return this;
            }


            /// <summary>執行所有已註冊的變數擷取，回傳結果字典。</summary>
            /// <returns>包含所有監看值的鍵值對。</returns>
            public Dictionary<string, object> Get()
            {
                var dict = new Dictionary<string, object>();
                _collectVariable(dict);
                return dict;
            }


            private string parseKey(Expression expr, string subKey)
            {
                var stack = new Stack<string>();

                MemberExpression memberExpr = null;
                while (expr is MemberExpression)
                {
                    memberExpr = expr as MemberExpression;

                    stack.Push(memberExpr.Member.Name);
                    expr = memberExpr.Expression;
                }
                if (memberExpr == null) { return null; }

                subKey = subKey.HasText() ? $"({subKey})" : "";
                stack.Push($"{memberExpr.Member.DeclaringType.Name}{subKey}");

                return string.Join(".", stack);
            }




        }

    }
}
