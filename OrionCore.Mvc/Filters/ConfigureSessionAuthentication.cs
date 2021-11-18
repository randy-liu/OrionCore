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

    public class ConfigureSessionAuthentication : IPostConfigureOptions<CookieAuthenticationOptions>
    {
        private readonly IMemoryCache _cache;

        public ConfigureSessionAuthentication(IMemoryCache cache)
        {
            _cache = cache;
        }

        public void PostConfigure(string name, CookieAuthenticationOptions options)
        {
            options.SessionStore = new DictionaryStore();
        }
    }



    /*====================================================*/



    public class DictionaryStore : ITicketStore
    {
        private static readonly ConcurrentDictionary<string, AuthenticationTicket> _cache = new ConcurrentDictionary<string, AuthenticationTicket>();

        private const string _keyPrefix = "AuthSessionStore";


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


        public async Task<string> StoreAsync(AuthenticationTicket ticket)
        {
            /* 刪除過期的 Ticket */
            clearExpires();

            string key = _keyPrefix + Guid.NewGuid();
            await RenewAsync(key, ticket);
            return key;
        }


        public Task RenewAsync(string key, AuthenticationTicket ticket)
        {
            _cache[key] = ticket;
            return Task.CompletedTask;
        }


        public Task<AuthenticationTicket> RetrieveAsync(string key)
        {
            AuthenticationTicket ticket;
            _cache.TryGetValue(key, out ticket);
            return Task.FromResult(ticket);
        }


        public Task RemoveAsync(string key)
        {
            _cache.TryRemove(key, out AuthenticationTicket ticket);
            return Task.CompletedTask;
        }

    }








    /*====================================================*/

    public class MemoryCacheStore : ITicketStore
    {
        private const string _keyPrefix = "AuthSessionStore";

        private readonly IMemoryCache _cache;

        public MemoryCacheStore(IMemoryCache cache)
        {
            _cache = cache;
        }

        public async Task<string> StoreAsync(AuthenticationTicket ticket)
        {
            var key = _keyPrefix + Guid.NewGuid();
            await RenewAsync(key, ticket);
            return key;
        }


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


        public Task<AuthenticationTicket> RetrieveAsync(string key)
        {
            AuthenticationTicket ticket;
            _cache.TryGetValue(key, out ticket);
            return Task.FromResult(ticket);
        }


        public Task RemoveAsync(string key)
        {
            _cache.Remove(key);
            return Task.CompletedTask;
        }

    }













}
