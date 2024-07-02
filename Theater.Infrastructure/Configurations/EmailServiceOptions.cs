namespace Theater.Infrastructure.Configurations
{
    public class EmailServiceOptions
    {
        public string DisplayName { get; set; }
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
