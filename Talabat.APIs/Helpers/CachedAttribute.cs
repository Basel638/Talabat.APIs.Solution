using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;
using Talabat.Core.Services.Contract;

namespace Talabat.APIs.Helpers
{
	public class CachedAttribute : Attribute, IAsyncActionFilter
	{
		private readonly int _timeToLiveInSeconds;

		public CachedAttribute(int timeToLiveInSeconds)
        {
			_timeToLiveInSeconds = timeToLiveInSeconds;
		}

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			var responseCacheService= context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();
			// Ask CLR for Creating Object from "ResponseCacheService" Explicitly


			var cachekey = GenerateCacheKeyFromRequest(context.HttpContext.Request);

			var response = await responseCacheService.GetCachedResponseAsync(cachekey);

			if(!string.IsNullOrEmpty(response)) 
			{
				var result = new ContentResult() {
					Content = response,
					ContentType = "application/json",
					StatusCode = 200,
				};

			context.Result = result;
				return;
			}


			// Response is not cached;
			var executedActionContext =  await next.Invoke();  // Will Execute the Next Action Filter oR The Action Itself

			if(executedActionContext.Result is OkObjectResult okObjectResult && okObjectResult.Value is not null) 
			{
				await responseCacheService.CacheResponseAsync(cachekey, okObjectResult.Value,TimeSpan.FromSeconds(_timeToLiveInSeconds));
			}
		}

		private string GenerateCacheKeyFromRequest(HttpRequest request)
		{
			// {{url}}/api/products?pageIndex=1&pageSize=5&sort=name

			var keyBuilder = new StringBuilder();

			//pageIndex = 1
			// pageSize = 5
			// sort = name
			keyBuilder.Append(request.Path); // /api/products

			foreach(var (key,value) in request.Query.OrderBy(x => x.Key))
			{
				keyBuilder.Append($"|{key}-{value}");
				// /api/products|pageindex-1
				// /api/products|pageindex-1|pagesize-5
				// /api/products|pageindex-1|pagesize-5|sort-name
			}
			return keyBuilder.ToString();
		}
	} 
}
