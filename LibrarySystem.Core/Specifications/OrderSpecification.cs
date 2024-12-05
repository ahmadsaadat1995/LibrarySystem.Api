using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.Core.Specifications
{
    public class OrderSpecification
    {
        public OrderSpecification(string buyerEmail)
        {
            BuyerEmail = buyerEmail;
        }
        public string BuyerEmail { get; set; }
    }
}
