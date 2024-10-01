using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Repository.Data
{
	public static class StoreContextSeed
	{
		public async static Task SeedAsync(StoreContext _dbContext)
		{
			if (_dbContext.ProductBrands.Count()==0)
			{
				var brandsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/brands.json");

				var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

				if (brands is not null && brands.Count() > 0)
				{

					foreach (var brand in brands)
					{
						_dbContext.Set<ProductBrand>().Add(brand);
					}
					await _dbContext.SaveChangesAsync();
				} 
			}
			
			if (_dbContext.ProductCategories.Count()==0)
			{
				var categoriessData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/categories.json");

				var categories = JsonSerializer.Deserialize<List<ProductCategory>>(categoriessData);

				if (categories is not null && categories.Count() > 0)
				{

					foreach (var categorie in categories)
					{
						_dbContext.Set<ProductCategory>().Add(categorie);
					}
					await _dbContext.SaveChangesAsync();
				} 
			}


		}
	}
}
