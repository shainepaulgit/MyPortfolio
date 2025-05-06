using Microsoft.Extensions.Options;
using MyPortfolio.Configurations;
using System.Net.Mail;
using System.Net;

namespace MyPortfolio.Models.Services
{
    public class EmailSender
    {
        private readonly EmailSettings _emailSettings;
        public EmailSender(IOptions<EmailSettings> options)
        {
            _emailSettings = options.Value;
        }

        public async Task<string> SendEmailAsync(string email, bool isIdentity, string subject, string htmlMessage)
        {
            var sender = isIdentity ? _emailSettings.Username : email;
            var recipient = isIdentity ? email : _emailSettings.Username;
            using var mainAddress = new SmtpClient(_emailSettings.Server, _emailSettings.Port)
            {
                UseDefaultCredentials = _emailSettings.UseDefaultCredentials,
                EnableSsl = _emailSettings.UseSSL,
                Credentials = new NetworkCredential(_emailSettings.Username,_emailSettings.Password)
            };
            try
            {
                var message = new MailMessage();
                message.Subject = subject;
                message.To.Add(new MailAddress(recipient));
                message.Body = htmlMessage;
                message.From = new MailAddress(sender);
                message.IsBodyHtml = _emailSettings.IsBodyHtml;
                await mainAddress.SendMailAsync(message);
                return "Successfully Sent";
            }
            catch(Exception ex)
            {
                return $"{ex.Message}";
            }
            

        }
    }
}