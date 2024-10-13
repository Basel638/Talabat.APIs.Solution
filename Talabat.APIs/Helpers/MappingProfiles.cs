using AutoMapper;
using Talabat.APIs.Dtos;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;

namespace Talabat.APIs.Helpers
{
	public class MappingProfiles: Profile
	{
	
		public MappingProfiles()
        {
			

			CreateMap<Product, ProductToReturnDtos>()
				.ForMember(d => d.Brand, O => O.MapFrom(s => s.Brand.Name))
				.ForMember(d => d.Category, O => O.MapFrom(s => s.Category.Name))
				//.ForMember(d => d.PictureUrl, O => O.MapFrom(s =>$" {_configuration["ApiBaseUrl"]}/{s.PictureUrl}"));
				.ForMember(P => P.PictureUrl, O => O.MapFrom<ProductPictureUrlResolver>());

			CreateMap<CustomerBasketDto, CustomerBasket>();
			CreateMap<BasketItemDto, BasketItem>();

			CreateMap<Address, AddressDto>().ReverseMap();	
		}
    }
}
