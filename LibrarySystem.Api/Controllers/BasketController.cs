using AutoMapper;
using LibrarySystem.Api.DTOs;
using LibrarySystem.Api.Errors;
using LibrarySystem.Core.Entitties;
using LibrarySystem.Core.Repositories.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Api.Controllers
{
    [Authorize]
    public class BasketController : ApiControllerBase
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository basketRepository, IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }

        [HttpGet("{BasketId}")]
        public async Task<ActionResult<CustomerBasket>> GetCustomerBasket (string BasketId)
        {
          var basket =  await  _basketRepository.GetBasketAsync(BasketId);
            return basket is null ? new CustomerBasket(BasketId) : basket;
        }
        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto basket)
        {
            var MappedBasket = _mapper.Map<CustomerBasketDto, CustomerBasket>(basket);
            var CreatedOrUpdatedBasket =await _basketRepository.UpdateBasketAsync(MappedBasket);
            if (CreatedOrUpdatedBasket is null)
                return BadRequest(new ApiResponse(400));
            return CreatedOrUpdatedBasket;
        }
        [HttpDelete("{BasketId}")]
        public async Task<ActionResult<bool>> DeleteBasket (string BasketId)
         =>   await _basketRepository.DeleteBasketAsync(BasketId);
        


    }
}
