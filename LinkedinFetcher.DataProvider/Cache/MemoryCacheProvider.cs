using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace LinkedinFetcher.DataProvider.Cache
{
    /// <summary>
    /// This is a modified version of http://stackoverflow.com/questions/21269170
    /// I is a utility to help save entities to cache and get fresh ones once thier gone
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [ExcludeFromCodeCoverage]
    public class MemoryCacheProvider<T> : ICacheProvider<T> where T : class
    {
        private const int DefaultHoursToStore = 5;
        private readonly int _hoursToStore;
        // We DO want to get one per T, so this is OK:
        // ReSharper disable once StaticFieldInGenericType
        static readonly object CacheLock = new object();

        public MemoryCacheProvider() : this(DefaultHoursToStore)
        {
        }
        public MemoryCacheProvider(int hoursToStore)
        {
            _hoursToStore = hoursToStore;
        }

        public  T GetCachedData(Func<T> getFreshData)
        {
            return GetCachedData((str) => getFreshData(), String.Empty);
        }

        public T GetCachedData(Func<string, T> getFreshData, string entityId)
        {
            var key = typeof(T).AssemblyQualifiedName + entityId;
            //Returns null if the string does not exist, prevents a race condition where the cache invalidates between the contains check and the retreival.
            var cachedString = MemoryCache.Default.Get(key, null) as T;

            if (cachedString != null)
            {
                return cachedString;
            }

            lock (CacheLock)
            {
                //Check to see if anyone wrote to the cache while we where waiting our turn to write the new value.
                cachedString = MemoryCache.Default.Get(key, null) as T;

                if (cachedString != null)
                {
                    return cachedString;
                }

                //The value still did not exist so we now write it in to the cache.
                T expensiveDto = getFreshData(entityId);
                var cip = new CacheItemPolicy()
                {
                    AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddHours(_hoursToStore))
                };
                MemoryCache.Default.Set(key, expensiveDto, cip);
                return expensiveDto;
            }
        }
    }
}
