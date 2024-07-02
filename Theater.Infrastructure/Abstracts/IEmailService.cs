namespace Theater.Infrastructure.Abstracts
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to,string subject,string body);

    }
}
