using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Product = Talabat.Core.Entities.Product;

namespace Talabat.Service.PaymentService
{

	public class PaymentService : IPaymentService
	{
		private readonly IConfiguration _configuration;
		private readonly IBasketRepository _basketRepo;
		private readonly IUnitOfWork _unitOfWork;

		public PaymentService(IConfiguration configuration, IBasketRepository basketRepo,IUnitOfWork unitOfWork)
        {
			_configuration = configuration;
			_basketRepo = basketRepo;
			_unitOfWork = unitOfWork;
		}
		public async Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string basketId)
		{
			StripeConfiguration.ApiKey = _configuration["StripeSettings:Secretkey"];
			var basket = await _basketRepo.GetBasketAsync(basketId);

			if (basket == null) return null;

			var shippingPrice = 0m;

			if (basket.Items?.Count > 0)
			{
				var productRepo = _unitOfWork.Repository<Product>();
				foreach (var item in basket.Items) 
				{
					var product = await productRepo.GetByIdAsync(item.Id);
					if (item.Price != product.Price)
						item.Price = product.Price;



				}
			}
		}
	}
}
