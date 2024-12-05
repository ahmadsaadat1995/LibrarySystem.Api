using LibrarySystem.Core.Entitties.Enums;
using System.Text.Json.Serialization;

namespace LibrarySystem.Api.DTOs
{
    public class AdditionalInformationDTO
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Format Format { get; set; }
        public DateTime DatePublished { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Language Language { get; set; }
    }
}