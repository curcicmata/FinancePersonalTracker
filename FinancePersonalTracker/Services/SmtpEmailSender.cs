using FinancePersonalTracker.Data;
using FinancePersonalTracker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Text.Encodings.Web;

namespace FinancePersonalTracker.Services
{

    public class SmtpEmailSender(IOptions<SmtpOptions> options) : IEmailSender<ApplicationUser>
    {
        private readonly SmtpOptions _options = options.Value;

        public async Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink)
        {
            var subject = "Confirm your email";
            var htmlMessage = $"Please confirm your account by <a href='{confirmationLink}'>clicking here</a>.";

            await SendEmailAsync(email, subject, htmlMessage);
        }

        public Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode)
        {
            throw new NotImplementedException();
        }

        public async Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink)
        {
            var subject = "Reset your password";
            var htmlMessage = $"Reset your password by <a href='{HtmlEncoder.Default.Encode(resetLink)}'>clicking here</a>.";

            await SendEmailAsync(email, subject, htmlMessage);
        }

        private async Task SendEmailAsync(string toEmail, string subject, string htmlContent)
        {
            using var message = new MailMessage
            {
                From = new MailAddress(_options.FromEmail, _options.FromName),
                Subject = subject,
                Body = htmlContent,
                IsBodyHtml = true
            };
            message.To.Add(toEmail);

            using var client = new SmtpClient(_options.SmtpServer, _options.Port)
            {
                Credentials = new NetworkCredential(_options.Username, _options.Password),
                EnableSsl = _options.EnableSsl
            };

            await client.SendMailAsync(message);
        }
    }

}
