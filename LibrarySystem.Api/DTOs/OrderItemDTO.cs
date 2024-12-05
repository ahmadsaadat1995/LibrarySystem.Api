using LibrarySystem.Core.OrderAggregate;

namespace LibrarySystem.Api.DTOs
{
    public class OrderItemDTO
    {
        public BookItemOrdered Book { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
