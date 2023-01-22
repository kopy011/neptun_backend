using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using neptun_backend.Entities;
using neptun_backend.UnitOfWork;
using System.Collections;
using System.Reflection;

namespace neptun_backend.Services
{
    public interface ICacheService
    {
        void SetCache();
        bool TryGetValue<TEntity>(object cacheKey, out TEntity value);
        IEnumerable<TEntity> GetAll<TEntity>();
        void Set(object cacheKey, object value);
        void Remove(object cacheKey);
    }

    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IUnitOfWork _unitOfWork;

        public CacheService(IMemoryCache memoryCache, IUnitOfWork unitOfWork)
        {
            _memoryCache = memoryCache;
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<TEntity> GetAll<TEntity>()
        {
            var field = typeof(MemoryCache).GetProperty("EntriesCollection", BindingFlags.NonPublic | BindingFlags.Instance);
            var collection = field.GetValue(_memoryCache) as ICollection;
            var keys = new List<object>();
            if (collection != null)
            {
                foreach (var item in collection)
                {
                    var methodInfo = item.GetType().GetProperty("Key");
                    var val = methodInfo.GetValue(item);
                    keys.Add(val);
                }
            }
            var result = new List<TEntity>();

            foreach (var key in keys)
            {
                TEntity entity;
                if (TryGetValue(key, out entity))
                {
                    result.Add(entity);
                }
            }

            return result;
        }

        public void Remove(object cacheKey)
        {
            _memoryCache.Remove(cacheKey);
        }

        public void Set(object cacheKey, object value)
        {
            _memoryCache.Set(cacheKey, value);
        }

        public void SetCache()
        {
            _unitOfWork.GetRepository<Course>().GetAll().Include(c => c.Instructors).Include(c => c.Students).ToList().ForEach(course =>
            {
                _memoryCache.Set(course.Id, course);
            });
        }

        public bool TryGetValue<TEntity>(object cacheKey, out TEntity value)
        {
            return _memoryCache.TryGetValue<TEntity>(cacheKey, out value);
        }
    }
}
