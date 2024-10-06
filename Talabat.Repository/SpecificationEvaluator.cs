using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Repository
{
	internal static class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
	{
		public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecifications<TEntity> spec)
		{
			var query = inputQuery;  //_dbContext.set<TEntity>

			if (spec.Criteria is not null)
				query = query.Where(spec.Criteria);



			if (spec.OrderBy is not null)   // P => P.name
				query = query.OrderBy(spec.OrderBy);

			else if (spec.OrderByDesc is not null) // P=> P.price
				query = query.OrderByDescending(spec.OrderByDesc);


			if (spec.IsPaginationEnabled)
				query = query.Skip(spec.Skip).Take(spec.Take);


			//query = _dbContext.set<TEntity>().where(E => E.Id == 1)

			query = spec.Includes.Aggregate(query, (currentQuery, includeExpression) => currentQuery.Include(includeExpression));


			return query;

		}

	}
}
