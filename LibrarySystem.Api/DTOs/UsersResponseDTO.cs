namespace LibrarySystem.Api.DTOs
{
    public class UsersResponseDTO
    {
        public int TotalUsers { get; set; }
        public IReadOnlyList<UsersReturnDTO> Users { get; set; }
    }
}
