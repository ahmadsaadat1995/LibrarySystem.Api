namespace LibrarySystem.Api.DTOs
{
    public class PublisherDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public List<string>? Books { get; set; }

    }
}
