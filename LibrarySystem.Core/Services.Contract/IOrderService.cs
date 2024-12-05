using LibrarySystem.Core.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.Core.Services.Contract
{
    public interface IOrderService 
    {
        Task<Order?> CreateOrderAsync(string BuyerEmail , string BasketId, int DeliveryMethodId , Address ShippingAddress);
        Task<IReadOnlyList<Order>> GetOrdersAsync(string BuyerEmail);
        Task<Order?> GetOrderByIdAsync (string  BuyerEmail , int OrderId);
    }
}
