﻿using AutoMapper;
using AutoMapper.Execution;
using Talabat.APIs.Dtos;
using Talabat.Core.Entities;

namespace Talabat.APIs.Helpers
{
	public class ProductPictureUrlResolver : IValueResolver<Product, ProductToReturnDtos, string>
	{
		private readonly IConfiguration _configuration;

		public ProductPictureUrlResolver(IConfiguration configuration)
        {
			_configuration = configuration;
		}
        public string Resolve(Product source, ProductToReturnDtos destination, string destMember, ResolutionContext context)
		{
			if(!string.IsNullOrEmpty(source.PictureUrl)) {

				return $"{_configuration["ApiBaseUrl"]}/{source.PictureUrl}.png";
			}

			return string.Empty ;
		}
	}
}
