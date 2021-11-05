using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using OrionCore.API.Extensions;

namespace OrionCore.API
{
    /// <summary>變數監看</summary>
    public static class VariableMonitor
    {
        private static readonly Collector _collector = new Collector();

        /// <summary></summary>
        public static Collector Add<T>(Expression<Func<T>> variableExpr)
        {
            return _collector.Add(variableExpr);
        }
        /// <summary></summary>
        public static Collector Add<T>(Expression<Func<T>> variableExpr, string subKey)
        {
            return _collector.Add(variableExpr, subKey);
        }


        /// <summary></summary>
        public static Dictionary<string, object> Get()
        {
            return _collector.Get();
        }


        /// <summary></summary>
        public class Collector
        {
            private Action<Dictionary<string, object>> _collectVariable = dict => { };

            /// <summary></summary>
            public Collector Add<T>(Expression<Func<T>> variableExpr)
            {
                return Add(variableExpr, null);
            }

            /// <summary></summary>
            public Collector Add<T>(Expression<Func<T>> variableExpr, string subKey)
            {
                var memberExpr = variableExpr.FindByType<MemberExpression>().FirstOrDefault();

                string key = parseKey(memberExpr, subKey);
                if (key == null) { throw new ArgumentException($"無法解析 {variableExpr} 中的成員欄位"); }
                var getter = variableExpr.Compile();

                _collectVariable += (dict => { dict[key] = getter(); });

                return this;
            }


            /// <summary></summary>
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
