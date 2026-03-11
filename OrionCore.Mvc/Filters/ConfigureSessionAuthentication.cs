using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Orion.Api.Extensions;


namespace Orion.Mvc.Filters
{

    /// <summary>棄用</summary>
    public class ConfigureSessionAuthentication : IPostConfigureOptions<CookieAuthenticationOptions>
    {
        //private readonly IMemoryCache _cache;

        ///// <summary></summary>
        //public ConfigureSessionAuthentication(IMemoryCache cache)
        //{
        //    _cache = cache;
        //}

        /// <summary>後置設定 Cookie 驗證選項，指定自訂 `ITicketStore`。</summary>
        /// <param name="name">驗證方案名稱。</param>
        /// <param name="options">Cookie 驗證選項。</param>
        public void PostConfigure(string name, CookieAuthenticationOptions options)
        {
            options.SessionStore = new DictionaryStore();
        }
    }

    /* 用 MemoryCache 來紀錄登入者資料 
        services.AddSingleton<IPostConfigureOptions<CookieAuthenticationOptions>, ConfigureSessionAuthentication>();
     */





    /*====================================================*/



    /// <summary>以記憶體字典實作的 `ITicketStore`。</summary>
    public class DictionaryStore : ITicketStore
    {
        private static readonly ConcurrentDictionary<string, AuthenticationTicket> _cache = new ConcurrentDictionary<string, AuthenticationTicket>();

        private const string _keyPrefix = nameof(DictionaryStore);


        /// <summary>建立 `DictionaryStore`。</summary>
        public DictionaryStore() { }


        /// <summary>刪除過期的 Ticket</summary>
        private void clearExpires()
        {
            var now = DateTimeOffset.Now;

            _cache
                .Where(x => x.Value.Properties.ExpiresUtc < now)
                .ToList() /* 先複製一份避免 Dictionary 刪除時對 IEnumerable 的影響 */
                .ForEach(x => _cache.TryRemove(x.Key, out AuthenticationTicket ticket));
        }


        /// <summary>儲存驗證票證並回傳鍵值。</summary>
        /// <param name="ticket">要儲存的驗證票證。</param>
        /// <returns>票證儲存鍵值。</returns>
        public async Task<string> StoreAsync(AuthenticationTicket ticket)
        {
            /* 刪除過期的 Ticket */
            clearExpires();

            string key = _keyPrefix + Guid.NewGuid();
            await RenewAsync(key, ticket);
            return key;
        }


        /// <summary>更新指定鍵值的驗證票證。</summary>
        /// <param name="key">票證鍵值。</param>
        /// <param name="ticket">新的驗證票證內容。</param>
        /// <returns>已完成的工作。</returns>
        public Task RenewAsync(string key, AuthenticationTicket ticket)
        {
            _cache[key] = ticket;
            return Task.CompletedTask;
        }


        /// <summary>依鍵值取得驗證票證。</summary>
        /// <param name="key">票證鍵值。</param>
        /// <returns>對應的驗證票證。</returns>
        public Task<AuthenticationTicket> RetrieveAsync(string key)
        {
            AuthenticationTicket ticket;
            _cache.TryGetValue(key, out ticket);
            return Task.FromResult(ticket);
        }


        /// <summary>移除指定鍵值的驗證票證。</summary>
        /// <param name="key">票證鍵值。</param>
        /// <returns>已完成的工作。</returns>
        public Task RemoveAsync(string key)
        {
            _cache.TryRemove(key, out AuthenticationTicket ticket);
            return Task.CompletedTask;
        }

    }








    /*====================================================*/

    /// <summary>以 `IMemoryCache` 實作的 `ITicketStore`。</summary>
    public class MemoryCacheStore : ITicketStore
    {
        private const string _keyPrefix = nameof(MemoryCacheStore);

        private readonly IMemoryCache _cache;

        /// <summary>建立 `MemoryCacheStore`。</summary>
        /// <param name="cache">記憶體快取。</param>
        public MemoryCacheStore(IMemoryCache cache)
        {
            _cache = cache;
        }

        /// <summary>儲存驗證票證並回傳鍵值。</summary>
        /// <param name="ticket">要儲存的驗證票證。</param>
        /// <returns>票證儲存鍵值。</returns>
        public async Task<string> StoreAsync(AuthenticationTicket ticket)
        {
            var key = _keyPrefix + Guid.NewGuid();
            await RenewAsync(key, ticket);
            return key;
        }


        /// <summary>更新指定鍵值的驗證票證與過期策略。</summary>
        /// <param name="key">票證鍵值。</param>
        /// <param name="ticket">新的驗證票證內容。</param>
        /// <returns>已完成的工作。</returns>
        public Task RenewAsync(string key, AuthenticationTicket ticket)
        {
            var options = new MemoryCacheEntryOptions
            {
                Priority = CacheItemPriority.NeverRemove
            };

            var expiresUtc = ticket.Properties.ExpiresUtc;

            if (expiresUtc.HasValue)
            { options.SetAbsoluteExpiration(expiresUtc.Value); }

            options.SetSlidingExpiration(TimeSpan.FromMinutes(480));

            _cache.Set(key, ticket, options);

            return Task.CompletedTask;
        }


        /// <summary>依鍵值取得驗證票證。</summary>
        /// <param name="key">票證鍵值。</param>
        /// <returns>對應的驗證票證。</returns>
        public Task<AuthenticationTicket> RetrieveAsync(string key)
        {
            AuthenticationTicket ticket;
            _cache.TryGetValue(key, out ticket);
            return Task.FromResult(ticket);
        }


        /// <summary>移除指定鍵值的驗證票證。</summary>
        /// <param name="key">票證鍵值。</param>
        /// <returns>已完成的工作。</returns>
        public Task RemoveAsync(string key)
        {
            _cache.Remove(key);
            return Task.CompletedTask;
        }

    }













}
