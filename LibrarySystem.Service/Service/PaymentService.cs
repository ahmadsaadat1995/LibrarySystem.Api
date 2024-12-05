using LibrarySystem.Core.Entitties;
using LibrarySystem.Core.OrderAggregate;
using LibrarySystem.Core.Repositories.Contract;
using LibrarySystem.Core.Services.Contract;
using LibrarySystem.Core.Specifications;
using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.Service.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IConfiguration configuration , IBasketRepository basketRepository  , IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<CustomerBasket?> CreateOrUpdatePaymentIntentId(string BasketId)
        {
            StripeConfiguration.ApiKey = _configuration["StripeSettings:Secretkey"];
            var basket =await _basketRepository.GetBasketAsync(BasketId);
            if(basket is null)
             return null;
            var ShippingPrice = 0M;
            if (basket.DeliveryMethodId.HasValue)
            {
                var DeliveryMethod =await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(basket.DeliveryMethodId.Value);
                ShippingPrice = DeliveryMethod.Cost;
            }
            if(basket.Books.Count >=0)
            {
                foreach (var item in basket.Books)
                {
                    var book =await _unitOfWork.Repository<Book>().GetByIdAsync(item.Id);
                    if(item.Price != book.Price)
                        item.Price = book.Price;
                }
                var SubTotal = basket.Books.Sum(b => b.Quantity * b.Price);
                var service = new PaymentIntentService();
                PaymentIntent paymentIntent;

                if (string.IsNullOrEmpty(basket.PaymentId))
                {
                    var options = new PaymentIntentCreateOptions()
                    {
                        Amount = (long)(ShippingPrice * 100 + SubTotal * 100),
                        Currency = "usd",
                        PaymentMethodTypes = new List<string>() {"card"}

                    };
                    paymentIntent = await service.CreateAsync(options);
                    basket.PaymentId = paymentIntent.Id;
                    basket.ClientSecret = paymentIntent.ClientSecret;

                }
                else
                {
                    var options = new PaymentIntentUpdateOptions()
                    {
                        Amount = (long)(ShippingPrice * 100 + SubTotal * 100),
                    };
                   paymentIntent = await service.UpdateAsync(basket.PaymentId , options);
                    basket.PaymentId = paymentIntent.Id;
                    basket.ClientSecret = paymentIntent.ClientSecret;
                }
            }
           await _basketRepository.UpdateBasketAsync(basket);
            return basket;
        }

        public async Task<Order> UpdatePatmentIntentIdSucceedOrFailed(string PaymentIntentId, bool flag)
        {
            var Spec = new OrderWithPaymentIntentIdSpecification(PaymentIntentId);
            var order =await _unitOfWork.Repository<Order>().GetByEntitySpecAsync(Spec);
            if (flag)
                order.Status = OrderStatus.PaymentRecived;
            else
                order.Status = OrderStatus.PaymentFailed;

            _unitOfWork.Repository<Order>().Update(order);
            await _unitOfWork.CompleteAsync();
            return order;

        }
    }
}
