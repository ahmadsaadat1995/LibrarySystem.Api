using LibrarySystem.Core.Entitties;
using LibrarySystem.Core.Repositories.Contract;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LibrarySystem.Repository.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;
        public BasketRepository(IConnectionMultiplexer connection)
        {
           _database=  connection.GetDatabase();
        }
        public async Task<bool> DeleteBasketAsync(string BasketId)
        => await _database.KeyDeleteAsync(BasketId);

        public async Task<CustomerBasket?> GetBasketAsync(string BasketId)
        {
          var basket = await _database.StringGetAsync(BasketId);
            if (basket.IsNullOrEmpty)
            {
                return null;  // Return null if no basket is found
            }
            else
            {
              return  JsonSerializer.Deserialize<CustomerBasket>(basket);
            }
        }

        public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket)
        {
            var JsonBasket = JsonSerializer.Serialize(basket);
            var CreatedOrUpdated = await _database.StringSetAsync(basket.Id, JsonBasket, TimeSpan.FromDays(1));
            if (!CreatedOrUpdated)
              return null;
            return await GetBasketAsync(basket.Id);
        }

    }
}
