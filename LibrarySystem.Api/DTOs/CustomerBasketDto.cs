using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Api.DTOs
{
    public class CustomerBasketDto
    {
        [Required]
        public string Id { get; set; }
        public List<BasketItemDto> Books { get; set; }

        public string? PaymentId { get; set; }
        public string? ClientSecret { get; set; }
        public int? DeliveryMethodId { get; set; }

    }
}
