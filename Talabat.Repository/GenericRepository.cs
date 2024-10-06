using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specifications;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
	public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
	{
		private readonly StoreContext _dbContext;

		public GenericRepository(StoreContext dbContext) // Ask CLR for creating object from dbcontext Implicitly
        {
			_dbContext = dbContext;
		}

        public async Task<IReadOnlyList<T>> GetAllAsync()
		{
			//if(typeof(T)==typeof(Product))
			//	return (IEnumerable <T>) await _dbContext.Set<Product>().Include(P => P.Brand).Include(P=>P.Category).ToListAsync();
			
			
			return await _dbContext.Set<T>().AsNoTracking().ToListAsync();
		}

		public async Task<T?> GetAsync(int id)
		{

			//if (typeof(T) == typeof(Product))
			//	return  _dbContext.Set<Product>().Include(P => P.Brand).Include(P => P.Category).Where(P=> P.Id == id).FirstOrDefaultAsync() as T;
			return await _dbContext.Set<T>().FindAsync(id);
		}
	
		public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> spec)
		{
			return await ApplySpecifications(spec).AsNoTracking().ToListAsync();
		}


		public async Task<T?> GetWithSpecAsync(ISpecifications<T> spec)
		{
			return await ApplySpecifications(spec).FirstOrDefaultAsync();

		}

		private IQueryable<T> ApplySpecifications(ISpecifications<T> spec)
		{
			return SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>(), spec);
		}

		public async Task<int> GetCountAsync(ISpecifications<T> spec)
		{
			return await ApplySpecifications(spec).CountAsync();
		}
	}
}
