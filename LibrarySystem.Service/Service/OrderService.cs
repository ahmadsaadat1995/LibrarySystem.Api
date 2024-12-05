using LibrarySystem.Core.Entitties;
using LibrarySystem.Core.OrderAggregate;
using LibrarySystem.Core.Repositories.Contract;
using LibrarySystem.Core.Services.Contract;
using LibrarySystem.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.Service.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        public OrderService(IBasketRepository basketRepository , IUnitOfWork unitOfWork , IPaymentService paymentService)
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
        }
     
        public async Task<Order?> CreateOrderAsync(string BuyerEmail, string BasketId,  int DeliveryMethodId, Address ShippingAddress)
        {
            var basket =await _basketRepository.GetBasketAsync(BasketId);
            var OrderItems = new List<OrderItem>();
            if(basket?.Books.Count > 0)
            {
                foreach (var item in basket.Books)
                {
                    var spec = new BookWithAdditionalInfoSpecification(item.Id);
                    var book =await _unitOfWork.Repository<Book>().GetByEntitySpecAsync(spec);
                    if (book is null)
                    {
                        Console.WriteLine($"book with ID {item.Id} not found.");
                        continue;
                    }
                    var BookItemOrdered = new BookItemOrdered(book.Id , book.Title , book.PictureUrl);
                    var BookItem = new OrderItem(BookItemOrdered ,item.Quantity , book.Price);
                    OrderItems.Add(BookItem);
                }
            }
            var subTotal = OrderItems.Sum(item => item.Quantity * item.Price);

            var DeliveryMethod =await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(DeliveryMethodId);

            var Spec = new OrderWithPaymentIntentIdSpecification(basket.PaymentId);

            var ExOrder =await _unitOfWork.Repository<Order>().GetByEntitySpecAsync(Spec);

            if(ExOrder is not null)
            {
               _unitOfWork.Repository<Order>().Delete(ExOrder);
               await _paymentService.CreateOrUpdatePaymentIntentId(BasketId);
            }
            else
            {
              
                if (string.IsNullOrEmpty(basket.PaymentId))
                {
 
                    await _paymentService.CreateOrUpdatePaymentIntentId(BasketId);
                }
            }

            var order = new Order(BuyerEmail , ShippingAddress , DeliveryMethod ,OrderItems , subTotal , basket.PaymentId);

             await _unitOfWork.Repository<Order>().AddAsync(order);

            var result =await _unitOfWork.CompleteAsync();
            if (result <= 0)
                return null;
            return order;
        }

      

        public async Task<Order?> GetOrderByIdAsync(string BuyerEmail, int OrderId)
        {
            var Spec = new OrderSpecefication(BuyerEmail, OrderId);
            var order =await _unitOfWork.Repository<Order>().GetByEntitySpecAsync(Spec);
            return order;
        }

        public async Task<IReadOnlyList<Order>> GetOrdersAsync(string BuyerEmail)
        {
            var Spec = new OrderSpecefication(BuyerEmail);
            var orders = await _unitOfWork.Repository<Order>().GetAllAsyncWithSpec(Spec);
            return orders;
        }
    }
}
