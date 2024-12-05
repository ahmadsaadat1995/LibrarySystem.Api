using LibrarySystem.Core.Entitties;
using LibrarySystem.Core.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.Core.Services.Contract
{
    public interface IPaymentService
    {
        Task<CustomerBasket?> CreateOrUpdatePaymentIntentId(string BasketId);
        Task<Order> UpdatePatmentIntentIdSucceedOrFailed ( string PaymentIntentId,bool flag ); 
    }
}
