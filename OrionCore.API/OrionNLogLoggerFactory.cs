using System;
using System.Collections.Concurrent;

namespace Orion.Api
{ 

    /// <summary>建立並快取 <see cref="OrionNLogLogger"/> 的工廠。</summary>
    public class OrionNLogLoggerFactory : IOrionLoggerFactory
    {
        private static readonly ConcurrentDictionary<string, IOrionLogger> _cache = new ConcurrentDictionary<string, IOrionLogger>();

        /// <summary>依名稱取得或建立對應的 Logger 實例。</summary>
        /// <param name="name">Logger 名稱（同名稱會共用快取實例）。</param>
        /// <returns>對應名稱的 `IOrionLogger`。</returns>
        private IOrionLogger getInstance(string name)
        {
            return _cache.GetOrAdd(name, x => new OrionNLogLogger(name));
        }


        /// <summary>建立或取得預設名稱 <c>Default</c> 的記錄器。</summary>
        /// <returns>對應的記錄器實例。</returns>
        public IOrionLogger Create() { return getInstance("Default"); }

        /// <summary>依型別完整名稱建立或取得記錄器。</summary>
        /// <param name="type">要用來產生 Logger 名稱的型別（使用 `type.FullName`）。</param>
        /// <returns>對應的記錄器實例。</returns>
        public IOrionLogger Create(Type type) { return getInstance(type.FullName); }

        /// <summary>依名稱建立或取得記錄器。</summary>
        /// <param name="name">要建立或取得的 Logger 名稱。</param>
        /// <returns>對應的記錄器實例。</returns>
        public IOrionLogger Create(string name) { return getInstance(name); }

    }
}
