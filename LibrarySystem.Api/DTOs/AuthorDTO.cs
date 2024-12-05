namespace LibrarySystem.Api.DTOs
{
    public class AuthorDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; } 
        //public List<BookDTO> Books { get; set; } = new List<BookDTO>();

        public List<string>? Books { get; set; }

        public string Biography { get; set; }
        public string ProfilePictureUrl { get; set; }
        public DateTime? DateOfBirth { get; set; } 

    }
}
