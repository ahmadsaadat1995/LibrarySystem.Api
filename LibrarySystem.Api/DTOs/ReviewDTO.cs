using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Api.DTOs
{
 
    public class ReviewDTO
    {

        [Required]
        public int BookId { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "The comment must be less than 500 characters.")]
        public string Comment { get; set; }

        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
    }

    
}
