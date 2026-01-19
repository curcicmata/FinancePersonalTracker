namespace FinancePersonalTracker.Services
{
    public class SendGridOptions
    {
        public string ApiKey { get; set; } = string.Empty;
        public string FromEmail { get; set; } = string.Empty;
        public string FromName { get; set; } = "Finance Personal Tracker";
    }
}
