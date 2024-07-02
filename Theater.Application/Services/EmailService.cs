using Theater.Infrastructure.Abstracts;
using Theater.Infrastructure.Configurations;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Theater.Application.Services
{
    public class EmailService : SmtpClient, IEmailService
    {
        private readonly EmailServiceOptions options;

        public EmailService(IOptions<EmailServiceOptions> options)
        {
            this.options = options.Value;


            Host = this.options.SmtpServer;
            Port = this.options.SmtpPort;
            Credentials = new NetworkCredential(this.options.UserName, this.options.Password);
            EnableSsl = true;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress(options.UserName, options.DisplayName);
            message.To.Add(to);
            message.Subject = subject;

            message.Body = body;
            message.IsBodyHtml = true;

            await SendMailAsync(message);
        }
    }
}
