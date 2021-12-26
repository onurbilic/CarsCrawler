using CarsCrawler.Infrastructure.Utils;
using MassTransit;

namespace CarsCrawler.Infrastructure.RabbitMq;

public class BusConfigurator
{
    public static IBusControl Create(RabbitMqSetting appSettings)
    {
        return Bus.Factory.CreateUsingRabbitMq(factory =>
        {
            factory.Host("cluster", "/", h =>
            {
                h.Username(appSettings.UserName);
                h.Password(appSettings.Password);

                var rabbitCluster = appSettings.ServerName.Split(";");

                h.UseCluster(c =>
                {
                    foreach (var server in rabbitCluster)
                        c.Node(server);
                });
            });
        });
    }

}