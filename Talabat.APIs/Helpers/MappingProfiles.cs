using AutoMapper;
using Talabat.APIs.Dtos;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Entities.Order_Aggregate;

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

			CreateMap<Talabat.Core.Entities.Identity.Address, AddressDto>().ReverseMap();
			CreateMap<AddressDto, Talabat.Core.Entities.Order_Aggregate.Address>().ReverseMap();

			CreateMap<Order, OrderToReturnDto>()
						.ForMember(d => d.DeliveryMethod, O => O.MapFrom(s => s.DeliveryMethod.ShortName))
						.ForMember(d =>d.DeliveryMethodCost, O => O.MapFrom(s => s.DeliveryMethod.Cost));

			CreateMap<OrderItem, OrderItemDto>()
						.ForMember(d =>d.ProductName, O => O.MapFrom(s=>s.Product.ProductName))
						.ForMember(d=>d.ProductId, O=> O.MapFrom(s => s.Product.ProductId))
						.ForMember(d =>d.PictureUrl, O => O.MapFrom(s=>s.Product.PictureUrl))
						.ForMember(d => d.PictureUrl, O => O.MapFrom<OrderItemPictureUrlResolver>());
		}
    }
}
 