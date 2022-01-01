// See https://aka.ms/new-console-template for more information

using CarsCrawler.Infrastructure.RabbitMq;
using CarsCrawler.Infrastructure.Repositories.Mongo;
using CarsCrawler.Infrastructure.Utils;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.Development.json")
    .Build();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<MongoDbSettings>(configuration.GetSection("MongoDbSettings"));

builder.Services.AddSingleton<IMongoDbSettings>(serviceProvider =>
    serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value);

builder.Services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));

// builder.Services.AddSingleton<RedisServer>();
// builder.Services.AddSingleton<ICacheService, RedisCacheManager>();

