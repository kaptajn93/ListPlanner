using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        //insert into cache and check if its already there
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var query = filterContext.HttpContext.Request.Query.ToDictionary(x => x.Key, x => (object)x.Value);
            var cacheKey = GetFullCacheKey(query);

            object value;
            _cache.TryGetValue(cacheKey, out value);

            var result = value as JsonResult;
            if(result != null)
            {
                filterContext.Result = result;
            }
        }



        private string GetFullCacheKey(IDictionary<string, object> arguments)
        {
            var sb = new StringBuilder();

            foreach (var argument in arguments)
            {
                sb.AppendFormat("{0}_{1}", argument.Key, argument.Value);
            }

            var key = CacheKey + sb;
            return key;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var query = filterContext.HttpContext.Request.Query.ToDictionary(x => x.Key, x => (object)x.Value);
            var cacheKey = GetFullCacheKey(query);
            
            var result = filterContext.Result;
            _cache.Set(cacheKey, result);
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