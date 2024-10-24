using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Services.Contract;

namespace Talabat.APIs.Controllers
{
	public class PaymentController : BaseApiController
	{
		private readonly IPaymentService _paymentService;
		private readonly ILogger<PaymentController> _logger;
		private const string whSecret = "whsec_8690c75ec2401ec262985f3ef6d932e99496df9a56c791fcbf3eeb8426592963";

		public PaymentController(IPaymentService paymentService, ILogger<PaymentController> logger)
        {
			_paymentService = paymentService;
			_logger = logger;
		}


		[Authorize]
		[ProducesResponseType(typeof(CustomerBasket),StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse),StatusCodes.Status400BadRequest)]
		[HttpPost("{basketid}")] // GET : /api/payments/{basketid}
		public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string basketId)
		{
			var basket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);

			if (basket == null) return BadRequest(new ApiResponse(400, "An Error with your Basket"));

			return Ok(basket);

		}


		[HttpPost("webhook")]
		public async Task<IActionResult> WebHook()
		{
			var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
		
				var stripeEvent = EventUtility.ConstructEvent(json,
					Request.Headers["Stripe-Signature"], whSecret);

				var paymentIntent = (PaymentIntent) stripeEvent.Data.Object;

			Order? order;

				// Handle the event
			
				switch(stripeEvent.Type)
				{
					case "payment_intent.succeeded":
					order=	await _paymentService.UpdateOrderStatus(paymentIntent.Id, true);
					_logger.LogInformation("Order Is Succeeded ya Hamda {0}", order?.PaymentIntentId);
					_logger.LogInformation("Unhandled event type: {0}", stripeEvent.Type);
					break;
						
					case "payment_intent.failed":
						order = await _paymentService.UpdateOrderStatus(paymentIntent.Id, false);
					_logger.LogInformation("Order Is Failed ya Hamda {0}", order?.PaymentIntentId);
					_logger.LogInformation("Unhandled event type: {0}", stripeEvent.Type);
					break;

					
				}

				return Ok();
			
			
		}
	}
}
