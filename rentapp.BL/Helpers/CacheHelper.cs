using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

namespace rentapp.BL.Helpers
{
    public class CacheHelper<T> where T : class
    {
        private readonly string cacheKey;
        private readonly IQueryable<T> queryable;
        private readonly Action<List<T>> action;
        private readonly ObjectCache cache;
        private string typeName;
        private static ConcurrentDictionary<string, object> locks = new ConcurrentDictionary<string, object>();
        private CacheItemPolicy cacheItemPolicy;

        public CacheHelper(string cacheKey, IQueryable<T> queryable, string typeName, Action<List<T>> action = null, CacheItemPolicy cacheItemPolicy = null)
        {
            this.cacheKey = cacheKey;
            this.queryable = queryable;
            this.typeName = typeName;
            this.action = action;
            this.cacheItemPolicy = cacheItemPolicy;
            cache = MemoryCache.Default;
            if (!locks.ContainsKey(cacheKey))
            {
                locks.TryAdd(cacheKey, new object());
            }
        }

        public List<T> LoadCache(CacheItemPolicy cacheItemPolicy = null)
        {
            try
            {
                if (cacheItemPolicy != null)
                {
                    this.cacheItemPolicy = cacheItemPolicy;
                }

                List<T> objs = queryable.AsNoTracking().ToList();

                action?.Invoke(objs);

                cache.Set(new CacheItem(cacheKey, objs),
                    cacheItemPolicy ?? this.cacheItemPolicy ??
                    new CacheItemPolicy()
                    {
                        AbsoluteExpiration = ObjectCache.InfiniteAbsoluteExpiration,
                        Priority = CacheItemPriority.NotRemovable
                    });

                return objs;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed at LoadCache - {typeName}", ex);
            }
        }

        public void RemoveCache()
        {
            cache.Remove(cacheKey);
        }

        public T LoadCache(T entity, T updatedEntity)
        {
            var objs = GetCollection();
            objs.Remove(entity);
            if (updatedEntity != null)
            {
                objs.Add(updatedEntity);
            }

            return updatedEntity;
        }

        public List<T> GetCollection()
        {
            try
            {
                List<T> objs = cache[cacheKey] as List<T>;
                if (objs == null)
                {
                    var objectLock = locks[cacheKey];
                    lock (objectLock)
                    {
                        objs = cache[cacheKey] as List<T>;
                        if (objs == null)
                        {
                            objs = LoadCache();
                        }
                    }
                }

                return objs;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed at GetCollection - {typeName}", ex);
            }
        }
    }
}
