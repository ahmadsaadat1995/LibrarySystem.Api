namespace LibrarySystem.Api.DTOs
{
    public class UsersReturnDTO
    {
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }

    }
}
