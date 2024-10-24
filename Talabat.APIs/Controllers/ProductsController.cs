using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specifications;
using Talabat.Core.Specifications.Product_Specs;

namespace Talabat.APIs.Controllers
{

	public class ProductsController : BaseApiController
	{
		///private readonly IGenericRepository<Product> _productRepo;
		///private readonly IGenericRepository<ProductBrand> _brandRepo;
		///private readonly IGenericRepository<ProductCategory> _categoriesRepo;
		
		private readonly IMapper _mapper;
		private readonly IProductService _productService;

		public ProductsController(IMapper mapper, IProductService productService)
		{
			_mapper = mapper;
			_productService = productService;
		}


		[CachedAttribute(600)]
		// api/products
		[HttpGet]
		public async Task<ActionResult<Pagination<ProductToReturnDtos>>> GetProducts([FromQuery] ProductSpecParams specParams)
		{

			var products = await _productService.GetProductsAsync(specParams);


			var count = await _productService.GetCountAsync(specParams);
			
			var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDtos>>(products);
			
			return Ok(new Pagination<ProductToReturnDtos>(specParams.PageIndex,count,specParams.PageSize,data));
		}


		[ProducesResponseType(typeof(ProductToReturnDtos), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
		// api/Products/1
		[HttpGet("{id}")]
		public async Task<ActionResult<ProductToReturnDtos>> GetProduct(int id)
		{

			var product = await _productService.GetProductAsync(id);

			if (product == null)
			{
				return NotFound(new ApiResponse(404));   //404
			}

			return Ok(_mapper.Map<Product, ProductToReturnDtos>(product));   //200
		}



		[CachedAttribute(600)]
		[HttpGet("brands")] // GET : /api/products/brands

		public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
		{
			var brands = await _productService.GetBrandsAsync();
			return Ok(brands);
		}
		
		[CachedAttribute(600)]
		[HttpGet("categories")] //GET : /api/products/categories
		public async Task<ActionResult<IReadOnlyList<ProductCategory>>> GetCategories()
		{
			var categories = await _productService.GetCategoriesAsync();
			return Ok(categories);
		}



	}
} 
