using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;

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

			if (_dbContext.Products.Count() == 0)
			{
				var ProductsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/products.json");

				var Products = JsonSerializer.Deserialize<List<Product>>(ProductsData);

				if (Products is not null && Products.Count() > 0)
				{

					foreach (var Product in Products)
					{
						_dbContext.Set<Product>().Add(Product);
					}
					await _dbContext.SaveChangesAsync();
				}
			}


			if (_dbContext.DeliveryMethods.Count() == 0)
			{
				var deliveryMethodData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/delivery.json");

				var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryMethodData);

				if (deliveryMethods is not null && deliveryMethods.Count() > 0)
				{

					foreach (var deliveryMethod in deliveryMethods)
					{
						_dbContext.Set<DeliveryMethod>().Add(deliveryMethod);
					}
					await _dbContext.SaveChangesAsync();
				}
			}

		}
	}
}
