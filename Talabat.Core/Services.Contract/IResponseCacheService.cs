using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Services.Contract
{
	public interface IResponseCacheService<T>
	{
		Task CacheResponseAsync(string key, T Response)
	}
}
