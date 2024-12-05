using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using LibrarySystem.Core.Entitties;
using Microsoft.Extensions.Configuration;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendPasswordResetEmailAsync(EmailMessage emailMessage, string token, string resetUrl)
    {
        var smtpServer = _configuration["EmailSettings:SmtpServer"];
        var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);
        var senderEmail = _configuration["EmailSettings:SenderEmail"];
        var senderPassword = _configuration["EmailSettings:SenderPassword"];

       
        using (var client = new SmtpClient(smtpServer, smtpPort))
        {
            client.EnableSsl = true;  
            client.Credentials = new NetworkCredential(senderEmail, senderPassword);  

            var mailMessage = new MailMessage(senderEmail, emailMessage.To)
            {
                Subject = emailMessage.Subject,
                Body = emailMessage.Body,
                IsBodyHtml = true  
            };

            try
            {
                await client.SendMailAsync(mailMessage);
            }
            catch (SmtpException ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                throw;  
            }
        }
    }
}
