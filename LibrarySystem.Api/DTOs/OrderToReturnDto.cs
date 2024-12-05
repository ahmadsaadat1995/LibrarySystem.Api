using LibrarySystem.Core.OrderAggregate;

namespace LibrarySystem.Api.DTOs
{
    public class OrderToReturnDto
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; }
        public DateTimeOffset DateTime { get; set; }
        public string Status { get; set; }
        public Address ShippingAddress { get; set; }
        public string DeliveryMethod { get; set; }
        public decimal DeliveryMethodCost { get; set; }
        public ICollection<OrderItemDTO> OrderItems { get; set; } = new HashSet<OrderItemDTO>();
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }

        public string PaymentIntentId { get; set; }


    }
}
