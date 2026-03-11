using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Orion.Api.Extensions;

namespace Orion.Api.Models
{

    /// <summary>WhereBuilder 的查詢參數</summary>
    public class WhereParams
    {
        /// <summary>從可讀屬性有值的物件建立等值查詢條件集合。</summary>
        /// <typeparam name="TParams">來源物件型別。</typeparam>
        /// <param name="obj">用來產生查詢條件的來源物件。</param>
        /// <returns>包含來源物件有值屬性之等值條件的查詢參數。</returns>
        public static WhereParams<TParams> CreateByObject<TParams>(TParams obj)
        {
            WhereParams param = new WhereParams<TParams>();
            var props = typeof(TParams).GetProperties().Where(x => x.CanRead);

            foreach (var prop in props)
            {
                var value = prop.GetValue(obj);
                if (!OrionUtils.HasValue(value)) { continue; }

                param.SetValues(prop.Name, WhereOperator.Equals, new[] { value });
            }

            return (WhereParams<TParams>)param;
        }



        /*==============================================================*/
        /*==============================================================*/

        /// <summary>原始查詢參數集合（欄位名稱對應查詢條件）。</summary>
        public Dictionary<string, WhereParamsPair> Source { get; internal set; } = new Dictionary<string, WhereParamsPair>();



        /*===========================================================*/


        /// <summary>以指定欄位名稱設定查詢運算子與查詢值。</summary>
        /// <typeparam name="TValue">欄位值型別。</typeparam>
        /// <param name="name">欄位名稱。</param>
        /// <param name="oper">套用到欄位的查詢運算子。</param>
        /// <param name="values">要寫入欄位的查詢值集合；傳入 <c>null</c> 時會視為空集合。</param>
        /// <returns>目前的查詢參數實例，供鏈式呼叫使用。</returns>
        public WhereParams SetValues<TValue>(string name, WhereOperator oper, params TValue[] values)
        {
            if (values == null) { values = new TValue[0]; }

            string[] strings = values.Convert<string>().ToArray();
            Source[name] = new WhereParamsPair(oper, strings);
            return this;
        }


        /// <summary>更新既有欄位條件的查詢運算子。</summary>
        /// <param name="name">欄位名稱。</param>
        /// <param name="oper">要套用的新查詢運算子。</param>
        /// <returns>目前的查詢參數實例；若欄位不存在則不異動。</returns>
        public WhereParams SetOperator(string name, WhereOperator oper)
        {
            if (!Source.ContainsKey(name)) { return this; }

            Source[name].Operator = oper;
            return this;
        }


        /// <summary>移除指定欄位的查詢條件與查詢值。</summary>
        /// <param name="name">欄位名稱。</param>
        /// <returns>目前的查詢參數實例，供鏈式呼叫使用。</returns>
        public WhereParams Remove(string name)
        {
            Source.Remove(name);
            return this;
        }


        /// <summary>取得指定欄位的查詢運算子。</summary>
        /// <param name="name">欄位名稱。</param>
        /// <returns>欄位存在時回傳其運算子，否則回傳 <see cref="WhereOperator.NotValue"/>。</returns>
        public WhereOperator GetOperator(string name)
        {
            return Source.ContainsKey(name) ? Source[name].Operator : WhereOperator.NotValue;
        }


        /// <summary>取得指定欄位的原始查詢值集合。</summary>
        /// <param name="name">欄位名稱。</param>
        /// <returns>欄位存在時回傳其查詢值陣列，否則回傳空陣列。</returns>
        public virtual object[] GetValues(string name)
        {
            if (!Source.ContainsKey(name)) { return new string[0]; }
            return Source[name].Values;
        }


    }






    /// <summary>提供強型別欄位指定方式的查詢參數容器。</summary>
    /// <typeparam name="TParams">查詢目標模型型別。</typeparam>
    public class WhereParams<TParams> : WhereParams
    {


        private string getPropertyName(LambdaExpression lambdaExpr)
        {
            PropertyInfo prop = lambdaExpr.GetProperty();
            if (prop != null) { return prop.Name; }

            throw new Exception("無法取得 " + lambdaExpr + " 的 Property 名稱");
        }



        /// <summary>以相同條件來源轉成另一個模型型別的查詢參數。</summary>
        /// <typeparam name="TParams2">要轉換成的模型型別。</typeparam>
        /// <returns>共用目前條件集合的新查詢參數實例。</returns>
        public WhereParams<TParams2> As<TParams2>()
        {
            var newParams = new WhereParams<TParams2>();
            newParams.Source = Source;
            return newParams;
        }



        /// <summary>設定欄位值與查詢條件</summary>
        private WhereParams<TParams> setValues<TProperty>(LambdaExpression lambdaExpr, WhereOperator oper, params TProperty[] values)
        {
            if (values == null) { values = new TProperty[0]; }
            string[] strings = values.Convert<string>().ToArray();

            string name = getPropertyName(lambdaExpr);
            Source[name] = new WhereParamsPair(oper, strings);
            return this;
        }

        /// <summary>以集合屬性欄位指定方式設定查詢運算子與查詢值。</summary>
        /// <typeparam name="TProperty">欄位元素型別。</typeparam>
        /// <param name="column">欄位名稱。</param>
        /// <param name="oper">套用到欄位的查詢運算子。</param>
        /// <param name="values">要寫入欄位的查詢值集合；傳入 <c>null</c> 時會視為空集合。</param>
        /// <returns>目前的查詢參數實例，供鏈式呼叫使用。</returns>
        public WhereParams<TParams> SetValues<TProperty>(Expression<Func<TParams, IEnumerable<TProperty>>> column, WhereOperator oper, params TProperty[] values)
        {
            return setValues(column, oper, values);
        }
        /// <summary>以 <see cref="List{T}"/> 屬性欄位指定方式設定查詢運算子與查詢值。</summary>
        /// <typeparam name="TProperty">欄位元素型別。</typeparam>
        /// <param name="column">欄位名稱。</param>
        /// <param name="oper">套用到欄位的查詢運算子。</param>
        /// <param name="values">要寫入欄位的查詢值集合；傳入 <c>null</c> 時會視為空集合。</param>
        /// <returns>目前的查詢參數實例，供鏈式呼叫使用。</returns>
        public WhereParams<TParams> SetValues<TProperty>(Expression<Func<TParams, List<TProperty>>> column, WhereOperator oper, params TProperty[] values)
        {
            return setValues(column, oper, values);
        }
        /// <summary>以陣列屬性欄位指定方式設定查詢運算子與查詢值。</summary>
        /// <typeparam name="TProperty">欄位元素型別。</typeparam>
        /// <param name="column">欄位名稱。</param>
        /// <param name="oper">套用到欄位的查詢運算子。</param>
        /// <param name="values">要寫入欄位的查詢值集合；傳入 <c>null</c> 時會視為空集合。</param>
        /// <returns>目前的查詢參數實例，供鏈式呼叫使用。</returns>
        public WhereParams<TParams> SetValues<TProperty>(Expression<Func<TParams, TProperty[]>> column, WhereOperator oper, params TProperty[] values)
        {
            return setValues(column, oper, values);
        }
        /// <summary>以單值屬性欄位指定方式設定查詢運算子與查詢值。</summary>
        /// <typeparam name="TProperty">欄位值型別。</typeparam>
        /// <param name="column">欄位名稱。</param>
        /// <param name="oper">套用到欄位的查詢運算子。</param>
        /// <param name="values">要寫入欄位的查詢值集合；傳入 <c>null</c> 時會視為空集合。</param>
        /// <returns>目前的查詢參數實例，供鏈式呼叫使用。</returns>
        public WhereParams<TParams> SetValues<TProperty>(Expression<Func<TParams, TProperty>> column, WhereOperator oper, params TProperty[] values)
        {
            return setValues(column, oper, values);
        }



        /// <summary>更新指定欄位條件的查詢運算子。</summary>
        /// <typeparam name="TProperty">欄位值型別。</typeparam>
        /// <param name="column">欄位名稱。</param>
        /// <param name="oper">要套用的新查詢運算子。</param>
        /// <returns>目前的查詢參數實例；若欄位不存在則不異動。</returns>
        public WhereParams<TParams> SetOperator<TProperty>(Expression<Func<TParams, TProperty>> column, WhereOperator oper)
        {
            string name = getPropertyName(column);
            SetOperator(name, oper);
            return this;
        }



        /// <summary>移除指定欄位的查詢條件與查詢值。</summary>
        /// <typeparam name="TProperty">欄位值型別。</typeparam>
        /// <param name="column">欄位名稱。</param>
        /// <returns>目前的查詢參數實例，供鏈式呼叫使用。</returns>
        public WhereParams<TParams> Remove<TProperty>(Expression<Func<TParams, TProperty>> column)
        {
            string name = getPropertyName(column);
            Remove(name);
            return this;
        }


        /// <summary>取得指定欄位目前的查詢運算子。</summary>
        /// <typeparam name="TProperty">欄位值型別。</typeparam>
        /// <param name="column">欄位名稱。</param>
        /// <returns>欄位存在時回傳其運算子，否則回傳 <see cref="WhereOperator.NotValue"/>。</returns>
        public WhereOperator GetOperator<TProperty>(Expression<Func<TParams, TProperty>> column)
        {
            string name = getPropertyName(column);
            return GetOperator(name);
        }






        /*===========================================================*/


        /// <summary>取得指定欄位並轉型後的查詢值集合。</summary>
        /// <param name="name">欄位名稱。</param>
        /// <returns>欄位存在時回傳依屬性型別轉換後的值陣列，否則回傳空陣列。</returns>
        public override object[] GetValues(string name)
        {
            if (!Source.ContainsKey(name)) { return new object[] { }; }

            Type type = typeof(TParams).GetProperty(name).PropertyType;
            if (type.IsArray)
            {
                type = type.GetElementType();
            }
            else if (type.IsGenericType && typeof(IEnumerable).IsAssignableFrom(type))
            {
                type = type.GenericTypeArguments.First();
            }

            return Source[name].Values.Select(x => x.ConvertTo(type)).ToArray();
        }


        private TProperty[] getValues<TProperty>(LambdaExpression lambdaExpr)
        {
            string name = getPropertyName(lambdaExpr);
            return GetValues(name).Select(x => x.ConvertTo<TProperty>()).ToArray();
        }



        /// <summary>取得集合屬性欄位的查詢值集合。</summary>
        /// <typeparam name="TProperty">欄位元素型別。</typeparam>
        /// <param name="column">欄位名稱。</param>
        /// <returns>指定欄位的查詢值陣列；若無資料則為空陣列。</returns>
        public TProperty[] GetValues<TProperty>(Expression<Func<TParams, IEnumerable<TProperty>>> column)
        {
            return getValues<TProperty>(column);
        }
        /// <summary>取得 <see cref="List{T}"/> 屬性欄位的查詢值集合。</summary>
        /// <typeparam name="TProperty">欄位元素型別。</typeparam>
        /// <param name="column">欄位名稱。</param>
        /// <returns>指定欄位的查詢值陣列；若無資料則為空陣列。</returns>
        public TProperty[] GetValues<TProperty>(Expression<Func<TParams, List<TProperty>>> column)
        {
            return getValues<TProperty>(column);
        }
        /// <summary>取得陣列屬性欄位的查詢值集合。</summary>
        /// <typeparam name="TProperty">欄位元素型別。</typeparam>
        /// <param name="column">欄位名稱。</param>
        /// <returns>指定欄位的查詢值陣列；若無資料則為空陣列。</returns>
        public TProperty[] GetValues<TProperty>(Expression<Func<TParams, TProperty[]>> column)
        {
            return getValues<TProperty>(column);
        }
        /// <summary>取得單值屬性欄位的查詢值集合。</summary>
        /// <typeparam name="TProperty">欄位值型別。</typeparam>
        /// <param name="column">欄位名稱。</param>
        /// <returns>指定欄位的查詢值陣列；若無資料則為空陣列。</returns>
        public TProperty[] GetValues<TProperty>(Expression<Func<TParams, TProperty>> column)
        {
            return getValues<TProperty>(column);
        }




        /*===========================================================*/

        /// <summary>從布林條件運算式解析欄位、運算子與值後寫入查詢條件。</summary>
        /// <param name="conditionExpr">用於解析查詢條件的布林運算式。</param>
        /// <returns>目前的查詢參數實例，供鏈式呼叫使用。</returns>
        public WhereParams<TParams> Assign(Expression<Func<TParams, bool>> conditionExpr)
        {
            PropertyInfo prop = conditionExpr.GetProperty();
            if (prop == null) { throw new ArgumentException("無法取得 " + conditionExpr + " 的 Property 名稱"); }

            WhereOperator Oper = parseExprOperator(conditionExpr.Body);

            string[] values = parseExprValues(conditionExpr, prop.PropertyType);
            if (values == null) { throw new ArgumentException($"無法取得 {conditionExpr} 的值"); }

            Source[prop.Name] = new WhereParamsPair(Oper, values);
            return this;
        }


        private WhereOperator parseExprOperator(Expression expr)
        {
            if (expr is UnaryExpression) /* Not 運算元 */
            {
                var unaryExpr = expr as UnaryExpression;
                if (unaryExpr.NodeType != ExpressionType.Not) { throw new ArgumentException($"不支援 {unaryExpr} 的 {unaryExpr.NodeType} 類型"); }

                WhereOperator oper = parseExprOperator(unaryExpr.Operand);
                switch (oper)
                {
                    case WhereOperator.In: return WhereOperator.NotIn;
                    case WhereOperator.NotIn: return WhereOperator.In;
                    case WhereOperator.Equals: return WhereOperator.NotEquals;
                    case WhereOperator.NotEquals: return WhereOperator.Equals;
                    case WhereOperator.LessThan: return WhereOperator.GreaterEquals;
                    case WhereOperator.LessEquals: return WhereOperator.GreaterThan;
                    case WhereOperator.GreaterThan: return WhereOperator.LessEquals;
                    case WhereOperator.GreaterEquals: return WhereOperator.LessThan;
                    case WhereOperator.Contains: return WhereOperator.NotContains;
                    case WhereOperator.StartsWith: return WhereOperator.NotStartsWith;
                    case WhereOperator.EndsWith: return WhereOperator.NotEndsWith;
                    case WhereOperator.Between:
                    default:
                        throw new ArgumentException($"{oper} 不支援 Not 查詢");
                }
            }
            else if (expr is BinaryExpression)
            {
                var binaryExpr = expr as BinaryExpression;
                switch (binaryExpr.NodeType)
                {
                    case ExpressionType.Equal: return WhereOperator.Equals;
                    case ExpressionType.NotEqual: return WhereOperator.NotEquals;
                    case ExpressionType.GreaterThan: return WhereOperator.GreaterThan;
                    case ExpressionType.GreaterThanOrEqual: return WhereOperator.GreaterEquals;
                    case ExpressionType.LessThan: return WhereOperator.LessThan;
                    case ExpressionType.LessThanOrEqual: return WhereOperator.LessEquals;
                    default: throw new ArgumentException($"不支援 {binaryExpr} 的 {binaryExpr.NodeType} 類型");
                }
            }
            else if (expr is MethodCallExpression)
            {
                var methodCallExpr = expr as MethodCallExpression;
                bool isStringMethod = (methodCallExpr.Method.DeclaringType == typeof(string));

                switch (methodCallExpr.Method.Name)
                {
                    case nameof(string.Contains): return (isStringMethod ? WhereOperator.Contains : WhereOperator.In);
                    case nameof(string.StartsWith): return WhereOperator.StartsWith;
                    case nameof(string.EndsWith): return WhereOperator.EndsWith;
                    case nameof(Checker.IsIn): return WhereOperator.In;
                    case nameof(Checker.NotIn): return WhereOperator.NotIn;
                    default: throw new ArgumentException($"不支援 {methodCallExpr} 的 {methodCallExpr.Method.Name} 方法");
                }
            }
            throw new ArgumentException($"無法解析 {expr} 的運算元");
        }



        private string[] parseExprValues(LambdaExpression expr, Type type)
        {
            var valueExpr = findValueExpr(expr.Parameters[0], expr.Body);
            if (valueExpr == null) { return null; }

            object value = Expression.Lambda(valueExpr).Compile().DynamicInvoke();
            bool isEnumerable = (value is IEnumerable && !(value is string));
            IEnumerable enumerable = isEnumerable ? value as IEnumerable : new object[] { value };

            string[] values = enumerable
                .Cast<object>()
                .Select(x => x.ConvertTo(type))
                .Select(x => x.ConvertTo<string>())
                .ToArray();

            return values;
        }


        private Expression findValueExpr(ParameterExpression paramExpr, Expression expr)
        {
            if (expr is BinaryExpression)
            {
                var binaryExpr = expr as BinaryExpression;

                return new[] { binaryExpr.Left, binaryExpr.Right, binaryExpr.Conversion }
                    .Select(x => findValueExpr(paramExpr, x))
                    .FirstOrDefault(x => x != null);
            }
            else if (expr is ConstantExpression)
            {
                return expr;
            }
            else if (expr is LambdaExpression)
            {
                var lambdaExpr = expr as LambdaExpression;
                return findValueExpr(paramExpr, lambdaExpr.Body);
            }
            else if (expr is MemberExpression)
            {
                var memberExpr = expr as MemberExpression;
                return memberExpr.Expression == paramExpr ? null : memberExpr;
            }
            else if (expr is MethodCallExpression)
            {
                var methodCallExpr = expr as MethodCallExpression;
                bool isStringMethod = (methodCallExpr.Method.DeclaringType == typeof(string));

                switch (methodCallExpr.Method.Name)
                {
                    case nameof(string.Contains):
                    case nameof(string.StartsWith):
                    case nameof(string.EndsWith):
                    case nameof(string.CompareTo):
                        return methodCallExpr.Arguments.Concat(new[] { methodCallExpr.Object })
                            .Select(x => findValueExpr(paramExpr, x))
                            .FirstOrDefault(x => x != null);
                    case nameof(Checker.IsIn):
                    case nameof(Checker.NotIn):
                        return methodCallExpr.Arguments[1];
                    default:
                        return methodCallExpr;
                }
            }
            else if (expr is UnaryExpression)
            {
                var unaryExpr = expr as UnaryExpression;
                return findValueExpr(paramExpr, unaryExpr.Operand);
            }

            return expr;
        }



    }








    /*##########################################################*/

    /// <summary>單一查詢欄位的條件與值集合。</summary>
    public class WhereParamsPair
    {
        /// <summary>查詢運算子。</summary>
        public WhereOperator Operator { get; set; }

        /// <summary>查詢值集合（字串表示）。</summary>
        public string[] Values { get; set; }

        /// <summary>建立空白查詢條件配對。</summary>
        public WhereParamsPair() { }

        /// <summary>建立查詢條件配對。</summary>
        /// <param name="oper">查詢運算子。</param>
        /// <param name="values">查詢值集合。</param>
        public WhereParamsPair(WhereOperator oper, string[] values)
        {
            Operator = oper;
            Values = values;
        }
    }




}
