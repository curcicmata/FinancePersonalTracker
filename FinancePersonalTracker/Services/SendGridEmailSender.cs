using FinancePersonalTracker.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace FinancePersonalTracker.Services
{
    public class SendGridEmailSender(IOptions<SendGridOptions> optionsAccessor) : IEmailSender<ApplicationUser>
    {
        private readonly SendGridOptions _options = optionsAccessor.Value;

        public async Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink)
        {
            var subject = "Confirm your email";
            var htmlMessage = $"Please confirm your account by <a href='{confirmationLink}'>clicking here</a>.";

            await Execute(email, subject, htmlMessage);
        }

        public Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode)
        {
            throw new NotImplementedException();
        }

        public async Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink)
        {
            var subject = "Reset your password";
            var htmlMessage = $"Reset your password by <a href='{resetLink}'>clicking here</a>.";

            await Execute(email, subject, htmlMessage);
        }

        private async Task Execute(string email, string subject, string htmlMessage)
        {
            var client = new SendGridClient(_options.ApiKey);
            var from = new EmailAddress(_options.FromEmail, _options.FromName);
            var to = new EmailAddress(email);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent: null, htmlMessage);
            await client.SendEmailAsync(msg);
        }
    }
}
