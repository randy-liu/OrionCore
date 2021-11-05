using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

namespace OrionCore.API.Extensions
{
    /// <summary></summary>
    public static class DeclareExtensions
    {
        private static readonly ConcurrentDictionary<Type, string> _typeCache = new ConcurrentDictionary<Type, string>();
        private static readonly ConcurrentDictionary<MethodInfo, string> _methodCache = new ConcurrentDictionary<MethodInfo, string>();


        /// <summary>取得類型的宣告名稱</summary>
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


        /// <summary>取得方法的宣告名稱</summary>
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
