namespace LibrarySystem.Api.DTOs
{
    public class UserDto
    {
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
        public string Token { get; set; }
        
    }
}
