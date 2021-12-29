using CarsCrawler.Infrastructure.Utils;
using MassTransit;

namespace CarsCrawler.Infrastructure.RabbitMq
{
    public class BusConfigurator
    {
        public static IBusControl Create(RabbitMqSetting appSettings)
        {
            return Bus.Factory.CreateUsingRabbitMq(factory =>
            {
                factory.Host(appSettings.ServerName, "/", h =>
                {
                    h.Username(appSettings.UserName);
                    h.Password(appSettings.Password);
                });
            });
        }

    }
}

