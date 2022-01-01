using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using CarsCrawler.Consumers.Consumer;
using CarsCrawler.Consumers.Producer;
using CarsCrawler.Infrastructure.Caching;
using CarsCrawler.Infrastructure.RabbitMq;
using CarsCrawler.Infrastructure.Repositories.Mongo;
using CarsCrawler.Infrastructure.Utils;
using MassTransit;
using Microsoft.Extensions.Options;
using CarsCrawler.SharedBusiness.Commands;
using RabbitMQ.Client;

namespace CarsCrawler.Consumers
{
    public static class Program
    {
        private static IConfigurationRoot? Configuration;

        private static void Main(string[] args)
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Development.json")
                .Build();

            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    if (Configuration != null)
                    {
                        services.AddSingleton<IConfiguration>(Configuration);
                    
                        services.Configure<MongoDbSettings>(Configuration.GetSection("MongoDbSettings"));

                        services.AddSingleton<IMongoDbSettings>(serviceProvider =>
                            serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value);

                        services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));

                        services.AddSingleton<RedisServer>();

                        services.AddSingleton<ICacheService, RedisCacheManager>();

                        var rabbitSettings = Configuration.GetSection("Settings").GetSection("RabbitMqInfo");

                        services.AddMassTransit(x =>
                        {
                            x.AddConsumer<SearchConsumer>(typeof(SearchConsumerDefinition));
                            x.AddConsumer<CarDetailConsumer>(typeof(CarDetailConsumerDefinition));
                            x.UsingRabbitMq((context, cfg) =>
                            {
                                cfg.Host(rabbitSettings.GetSection("ServerName").Value, "/", h =>
                                {
                                    h.Username(rabbitSettings.GetSection("UserName").Value);
                                    h.Password(rabbitSettings.GetSection("Password").Value);
                                });

                                cfg.ConfigureEndpoints(context);
                            });
                        });

                        services.AddMassTransitHostedService();
                        services.AddHostedService<CarDetailProducer>();
                    }
                });
    }
}