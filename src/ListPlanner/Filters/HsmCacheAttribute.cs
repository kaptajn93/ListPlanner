using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;

namespace ListPlanner.Filters
{
    public class HsmCacheAttribute : ActionFilterAttribute
    {
        private readonly IMemoryCache _cache;

        public HsmCacheAttribute()
        {
            _cache = new MemoryCache(new MemoryCacheOptions
            {
                
            });
        }


        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            object value;
            _cache.TryGetValue(CacheKey, out value);

            var result = value as JsonResult;
            if(result != null)
            {
                filterContext.Result = result;
            }
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var result = filterContext.Result;
            _cache.Set(CacheKey, result);
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
        
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
        
        }


        public string CacheKey { get; set; }

        public int Duration { get; set; }
    }
}