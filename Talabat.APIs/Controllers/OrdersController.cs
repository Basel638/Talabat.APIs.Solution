using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Services.Contract;
using Address = Talabat.Core.Entities.Order_Aggregate.Address;

namespace Talabat.APIs.Controllers
{
	public class OrdersController : BaseApiController
	{
		private readonly IOrderService _orderService;
		private readonly IMapper _mapper;
		//private readonly UserManager<ApplicationUser> _userManager;

		public OrdersController(IOrderService orderService, IMapper mapper/*, UserManager<ApplicationUser> userManager*/)
		{
			_orderService = orderService;
			_mapper = mapper;
			//_userManager = userManager;
		}

		[Authorize]
		[HttpPost] // POST: /api/Orders
		[ProducesResponseType(typeof(OrderToReturnDto), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto orderDto)
		{

			// step get email from token Claims (Don't forget)
			var buyeremail = User.FindFirstValue(ClaimTypes.Email) ?? string.Empty;

			var address = _mapper.Map<AddressDto, Address>(orderDto.ShippingAddress);

			//var user = await _userManager.FindByEmailAsync(buyeremail);

			var order = await _orderService.CreateOrderAsync(buyeremail, orderDto.BasketId, orderDto.DeliveryMethodId, address);

			if (order == null) return BadRequest(new ApiResponse(400));

			return Ok(_mapper.Map<Order,OrderToReturnDto>(order));
		}

		[Authorize]

		[HttpGet] // GET : /api/Orders?email = ahmed.nasr@linkdev.com
		public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUser()
		{
			var buyeremail = User.FindFirstValue(ClaimTypes.Email) ?? string.Empty;

			var orders = await _orderService.GetOrdersForUserAsync(buyeremail);

			return Ok(_mapper.Map< IReadOnlyList<Order>,IReadOnlyList< OrderToReturnDto>>(orders));
		}

		[Authorize]

		[ProducesResponseType(typeof(OrderToReturnDto), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
		[HttpGet("{id}")] // Get: /api/Orders/1?email = ahmed.nasr@linkdev.com
		public async Task<ActionResult<OrderToReturnDto>> GetOrderForUser(int id)
		{
			var buyeremail = User.FindFirstValue(ClaimTypes.Email) ?? string.Empty;

			var order = await _orderService.GetOrderByIdForUserAsync(buyeremail, id);

			if (order == null) return NotFound(new ApiResponse(404));

			return Ok(_mapper.Map<Order, OrderToReturnDto>(order));
		}


		[HttpGet("deliveryMethods")] // GET: /api/Orders/deliveryMethods
		public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
		{
			var deliveryMethods = await _orderService.GetDeliveryMethodsAsync();
			return Ok(deliveryMethods);
		}
	}
}
