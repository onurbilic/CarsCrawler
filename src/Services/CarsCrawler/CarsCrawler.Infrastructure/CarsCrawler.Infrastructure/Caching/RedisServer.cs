using System;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace CarsCrawler.Infrastructure.Caching;

public class RedisServer
{
    public RedisServer(IConfiguration configuration)
    {
        var connectionMultiplexer = ConnectionMultiplexer.Connect(CreateRedisConfigurationString(configuration));
        Database = connectionMultiplexer.GetDatabase(Convert.ToInt32(configuration.GetSection("RedisSettings:DatabaseIndexId").Value));
    }

    public IDatabase Database { get; }
        
    //TODO : Tüm statik stringler constantlardan gelecek.
    private ConfigurationOptions CreateRedisConfigurationString(IConfiguration configuration)
    {
        var options = ConfigurationOptions.Parse(configuration.GetSection("RedisSettings:Hosts").Value);
        options.Password = configuration.GetSection("RedisSettings:Password").Value;
        options.ConnectTimeout = Convert.ToInt32(configuration.GetSection("RedisSettings:ConnectTimeout").Value);
        options.ConnectRetry = Convert.ToInt32(configuration.GetSection("RedisSettings:ConnectRetry").Value);

        return options;
    }

}