using LibrarySystem.Core.Entitties.Enums;
using System.Text.Json.Serialization;

namespace LibrarySystem.Api.DTOs
{
    public class BookDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PictureUrl { get; set; }
        public string AuthorName { get; set; }
        //public int CategoryId { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Category Category { get; set; }
        public List<string> Publishers { get; set; }

        // public List<PublisherDTO> BookPublishers { get; set; }

        public AdditionalInformationDTO AdditionalInfo { get; set; }
    }
}
