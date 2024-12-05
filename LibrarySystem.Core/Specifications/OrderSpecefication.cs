using LibrarySystem.Core.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.Core.Specifications
{
    public class OrderSpecefication : BaseSpecification<Order>
    {
        public OrderSpecefication(string email) : base(o => o.BuyerEmail == email)
        {
            Includes.Add(o => o.Items);
            Includes.Add(o => o.DeliveryMethod);
            AddOrderDesc(o => o.OrederDate);
        }
        public OrderSpecefication(string email , int id  ): base(o => o.BuyerEmail == email && o.Id == id)
        {
            Includes.Add(o => o.Items);
            Includes.Add(o => o.DeliveryMethod);
        }


    }
}
