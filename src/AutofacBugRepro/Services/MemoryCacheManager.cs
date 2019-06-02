using System;
using System.Runtime.Caching;

namespace AutofacBugRepro.Services
{
    /// <inheritdoc />
    /// <summary>
    ///     Represents a manager for caching between HTTP requests (long term caching)
    /// </summary>
    public class MemoryCacheManager : ICacheManager
    {
        protected static ObjectCache Cache => MemoryCache.Default;

        /// <summary>
        ///     Gets or sets the value associated with the specified key.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">The key of the value to get.</param>
        /// <returns>The value associated with the specified key.</returns>
        public virtual T Get<T>(string key)
        {
            return (T) Cache[key];
        }

        /// <inheritdoc />
        /// <summary>
        ///     Adds the specified key and object to the cache.
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="data">Data</param>
        /// <param name="cacheTime">Cache time</param>
        public virtual void Set(string key, object data, int cacheTime)
        {
            if (data == null) return;

            var policy = new CacheItemPolicy {AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(cacheTime)};
            Cache.Add(new CacheItem(key, data), policy);
        }

        /// <inheritdoc />
        /// <summary>
        ///     Gets a value indicating whether the value associated with the specified key is cached
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>Result</returns>
        public virtual bool IsSet(string key)
        {
            return Cache.Contains(key);
        }
    }
}