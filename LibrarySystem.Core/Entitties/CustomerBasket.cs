using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.Core.Entitties
{
    public class CustomerBasket
    {

        public CustomerBasket(string id)
        {
            Id = id;
        }
        public string Id { get; set; }

        public List<BasketItem> Books{ get; set; }
        public string? PaymentId { get; set; }
        public string? ClientSecret { get; set; }
        public int? DeliveryMethodId { get; set; }

    }
}
