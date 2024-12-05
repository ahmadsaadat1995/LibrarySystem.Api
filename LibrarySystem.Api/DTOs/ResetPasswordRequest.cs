namespace LibrarySystem.Api.DTOs
{
    public class ResetPasswordRequest
    {
        public string NewPassword { get; set; }  // The user's email address
        public string ConfirmPassword { get; set; }  // The new password provided by the user
        public string Email { get; set; }  // The new password provided by the user
        
    }
}
