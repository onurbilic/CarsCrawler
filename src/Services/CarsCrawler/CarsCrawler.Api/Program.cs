using CarsCrawler.Infrastructure.Caching;
using CarsCrawler.Infrastructure.RabbitMq;
using Microsoft.Extensions.Configuration;
using CarsCrawler.Infrastructure.Repositories.Mongo;
using CarsCrawler.Infrastructure.Utils;
using MassTransit;
using Microsoft.Extensions.Options;

IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.Development.json")
    .Build();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<MongoDbSettings>(configuration.GetSection("MongoDbSettings"));

builder.Services.AddSingleton<IMongoDbSettings>(serviceProvider =>
    serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value);

builder.Services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));

// builder.Services.AddSingleton<RedisServer>();
// builder.Services.AddSingleton<ICacheService, RedisCacheManager>();
var rabbitSettings = configuration.GetSection("Settings").Get<ProjectSetting>();

builder.Services.AddSingleton<IBus>(sp => BusConfigurator.Create(rabbitSettings.RabbitMqInfo));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();