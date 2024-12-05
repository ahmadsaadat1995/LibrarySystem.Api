using LibrarySystem.Core.Entitties;

public interface IEmailService
{
   Task SendPasswordResetEmailAsync(EmailMessage emailMessage, string token, string resetUrl);
}
