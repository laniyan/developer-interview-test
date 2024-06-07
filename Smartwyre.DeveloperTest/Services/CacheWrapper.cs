using Microsoft.Extensions.Caching.Memory;
using Smartwyre.DeveloperTest.Services.Interfaces;
using System;

namespace Smartwyre.DeveloperTest.Services
{
    public class CacheWrapper : ICacheWrapper
    {
        private readonly IMemoryCache _memoryCache;

        public CacheWrapper(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public decimal GetCachedRebateAmount(string key)
        {
            return _memoryCache.Get<decimal>(key);
        }

        public void CacheRebateAmount(string key, decimal amount)
        {
            _memoryCache.Set(key, amount, TimeSpan.FromDays(1));
        }
    }

}