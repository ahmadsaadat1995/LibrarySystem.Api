using LibrarySystem.Core.Entitties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.Core.OrderAggregate
{
    public class OrderItem : BaseEntity<int>
    {

        public OrderItem()
        {
        }
        public OrderItem(BookItemOrdered book, int quantity, decimal price)
        {
            Book = book;
            Quantity = quantity;
            Price = price;
        }

        public BookItemOrdered Book { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

    }
}
