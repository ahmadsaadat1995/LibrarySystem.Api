using AutoMapper;
using LibrarySystem.Api.DTOs;
using LibrarySystem.Api.Errors;
using LibrarySystem.Core.Entitties;
using LibrarySystem.Core.OrderAggregate;
using LibrarySystem.Core.Repositories.Contract;
using LibrarySystem.Core.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibrarySystem.Api.Controllers
{
    public class OrderController :ApiControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderService _orderService;
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public OrderController(IUnitOfWork unitOfWork , IOrderService orderService ,IBasketRepository basketRepository , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _orderService = orderService;
            _basketRepository = basketRepository;
            _mapper = mapper;
        }
        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(Order) , StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse) , StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var MappedAddress = _mapper.Map<Address>(orderDto.ShippingAddress);

            var Basket =await _basketRepository.GetBasketAsync(orderDto.BasketId);
            
            if (Basket is null)
                return BadRequest(new ApiResponse(400 , " Basket Not Found "));
            var OriginalBasket = _mapper.Map<CustomerBasket>(Basket);

            Basket.DeliveryMethodId = orderDto.DeliveryMethodId;
            var updatedBasket = await _basketRepository.UpdateBasketAsync(Basket);

           
            var Order = await _orderService.CreateOrderAsync(BuyerEmail, orderDto.BasketId, orderDto.DeliveryMethodId, MappedAddress);
            if(Order is null)
            {
                await _basketRepository.UpdateBasketAsync(OriginalBasket);
                return BadRequest(new ApiResponse(400, "Basket Not Found "));
            }
            return Ok(Order);
        }
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(OrderToReturnDto) , StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse) , StatusCodes.Status404NotFound)]

        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUser()
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var Orders =await _orderService.GetOrdersAsync(BuyerEmail);
            if (Orders is null)
                return NotFound(new ApiResponse(404, "there is No Orders "));
            var MappedOrders = _mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDto>>(Orders);
            return Ok(MappedOrders);
        }

        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(OrderToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderForSpecificUser(int id)
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var Order =await _orderService.GetOrderByIdAsync(BuyerEmail, id);
            if (Order is null)
                return NotFound(new ApiResponse(404, $"There is No Order With {id} For This User "));
            var MappedOrder = _mapper.Map<Order, OrderToReturnDto>(Order);
            return Ok(MappedOrder);
        }

        
        [HttpGet("DeliveryMethods")]
        public async Task<ActionResult<DeliveryMethod>> GetDeliveryMethods()
        {
           var DeliveryMethods  = await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
           return Ok(DeliveryMethods);

        }
    }
}
