using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;


namespace OrionCore.Mvc.Filters
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
            options.SessionStore = new MemoryCacheStore(_cache);
        }
    }




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

            options.SetSlidingExpiration(TimeSpan.FromMinutes(60));

            _cache.Set(key, ticket, options);

            return Task.FromResult(0);
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
            return Task.FromResult(0);
        }

    }
}
