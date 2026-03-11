using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

namespace Orion.Api.Extensions
{
    /// <summary>取得型別與方法宣告名稱的擴充方法。</summary>
    public static class DeclareExtensions
    {
        private static readonly ConcurrentDictionary<Type, string> _typeCache = new ConcurrentDictionary<Type, string>();
        private static readonly ConcurrentDictionary<MethodInfo, string> _methodCache = new ConcurrentDictionary<MethodInfo, string>();


        /// <summary>取得型別宣告名稱，泛型型別會包含型別參數名稱。</summary>
        /// <param name="type">型別。</param>
        /// <returns>型別宣告字串。</returns>
        public static string GetDeclareName(this Type type)
        {
            return _typeCache.GetOrAdd(type, _ =>
            {
                string name = type.Name.Split('`').First();
                if (!type.IsGenericType) { return name; }

                string generics = type.GetGenericArguments()
                    .Select(x => x.Name)
                    .JoinBy(", ");

                return $"{name}<{generics}>";
            });
        }


        /// <summary>取得方法宣告名稱，包含參數型別清單。</summary>
        /// <param name="method">方法資訊。</param>
        /// <returns>方法宣告字串。</returns>
        public static string GetDeclareName(this MethodInfo method)
        {
            return _methodCache.GetOrAdd(method, _ =>
            {
                string @params = method.GetParameters()
                    .Select(p => GetDeclareName(p.ParameterType))
                    .JoinBy(", ");

                return $"{method.Name}({@params})";
            });
        }

    }
}
