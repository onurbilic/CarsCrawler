namespace CarsCrawler.Infrastructure.Utils
{
    public class ProjectSetting
    {
        public string Environment { get; set; }

        public RabbitMqSetting RabbitMqInfo { get; set; }
        public ServiceSetting ListingService { get; set; }
        public ServiceSetting MerchantService { get; set; }
    }

    public class RabbitMqSetting
    {
        public string ServerName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class ServiceSetting
    {
        public string Url { get; set; }
        public string Username { get; set; }
        public string AppKey { get; set; }
        public string Hash { get; set; }
    }
}

