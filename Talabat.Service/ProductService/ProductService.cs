using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specifications.Product_Specs;

namespace Talabat.Service.ProductService
{
	public class ProductService : IProductService
	{
		private readonly IUnitOfWork _unitOfWork;

		public ProductService(IUnitOfWork unitOfWork)
        {
			_unitOfWork = unitOfWork;
		}
		public async Task<IReadOnlyList<Product>> GetProductsAsync(ProductSpecParams specParams)
		{
			var spec = new ProductWithBrandAndCategorySpecifications(specParams);

			var products = await  _unitOfWork.Repository<Product>().GetAllWithSpecAsync(spec);

			return products;
		}
		public async Task<Product?> GetProductAsync(int productId)
		{
			var spec = new ProductWithBrandAndCategorySpecifications(productId);

			var product = await _unitOfWork.Repository<Product>().GetWithSpecAsync(spec);

			return product;

		}


		public async Task<IReadOnlyList<ProductBrand>> GetBrandsAsync()
		=> await _unitOfWork.Repository<ProductBrand>().GetAllAsync();

		public async Task<IReadOnlyList<ProductCategory>> GetCategoriesAsync()
		=> await _unitOfWork.Repository<ProductCategory>().GetAllAsync();

		public async Task<int> GetCountAsync(ProductSpecParams specParams)
		{
			var countSpec = new ProductWithFilterationForCountSpecification(specParams);

			var count = await _unitOfWork.Repository<Product>().GetCountAsync(countSpec);

			return count;

		}
	}
}
