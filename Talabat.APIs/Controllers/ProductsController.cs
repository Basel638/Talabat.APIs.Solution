using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specifications;
using Talabat.Core.Specifications.Product_Specs;

namespace Talabat.APIs.Controllers
{

	public class ProductsController : BaseApiController
	{
		private readonly IGenericRepository<Product> _productRepo;
		private readonly IMapper _mapper;
		private readonly IGenericRepository<ProductBrand> _brandRepo;
		private readonly IGenericRepository<ProductCategory> _categoriesRepo;

		public ProductsController(IGenericRepository<Product> productRepo, IMapper mapper, IGenericRepository<ProductBrand> brandRepo, IGenericRepository<ProductCategory> categoriesRepo)
		{
			_productRepo = productRepo;
			_mapper = mapper;
			_brandRepo = brandRepo;
			_categoriesRepo = categoriesRepo;
		}
		// api/products
		[HttpGet]
		public async Task<ActionResult<Pagination<ProductToReturnDtos>>> GetProducts([FromQuery] ProductSpecParams specParams)
		{
			var spec = new ProductWithBrandAndCategorySpecifications(specParams);

			var products = await _productRepo.GetAllWithSpecAsync(spec);

			var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDtos>>(products);

			var countSpec = new ProductWithFilterationForCountSpecification(specParams);

			var count = await _productRepo.GetCountAsync(countSpec);
			
			return Ok(new Pagination<ProductToReturnDtos>(specParams.PageIndex,count,specParams.PageSize,data));
		}


		[ProducesResponseType(typeof(ProductToReturnDtos), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
		// api/Products/1
		[HttpGet("{id}")]
		public async Task<ActionResult<ProductToReturnDtos>> GetProduct(int id)
		{
			var spec = new ProductWithBrandAndCategorySpecifications(id);

			var product = await _productRepo.GetWithSpecAsync(spec);

			if (product == null)
			{
				return NotFound(new ApiResponse(404));   //404
			}

			return Ok(_mapper.Map<Product, ProductToReturnDtos>(product));   //200
		}



		[HttpGet("brands")] // GET : /api/products/brands

		public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
		{
			var brands = await _brandRepo.GetAllAsync();
			return Ok(brands);
		}

		[HttpGet("categories")] //GET : /api/products/categories

		public async Task<ActionResult<IReadOnlyList<ProductCategory>>> GetCategories()
		{
			var categories = await _categoriesRepo.GetAllAsync();
			return Ok(categories);
		}

    }
} 
