using Business.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;

        public CacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public T Get<T>(string key)
        {
            return _memoryCache.Get<T>(key);
        }

        public void Set<T>(string key, T value, TimeSpan duration)
        {
            _memoryCache.Set(key, value, duration);
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }

        public void Reset()
        {
            if (_memoryCache is MemoryCache cache)
            {
                cache.Clear();
            }
        }
    }

}
