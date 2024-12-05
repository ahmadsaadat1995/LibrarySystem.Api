using LibrarySystem.Core.Entitties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.Core.Repositories.Contract
{
    public interface IBasketRepository
    {
         Task<CustomerBasket?> GetBasketAsync(string BasketId);
         Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket);
         Task<bool> DeleteBasketAsync(string BasketId);
    }
}
