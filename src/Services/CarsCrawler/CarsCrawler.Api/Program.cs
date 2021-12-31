using CarsCrawler.Infrastructure.Caching;
using CarsCrawler.Infrastructure.RabbitMq;
using CarsCrawler.Infrastructure.Repositories.Mongo;
using CarsCrawler.Infrastructure.Utils;
using MassTransit;
using Microsoft.Extensions.Options;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.AspNetCore.Diagnostics;
using HealthChecks.RabbitMQ;
using System.Diagnostics;

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
var rabbitSettings = configuration.GetSection("Settings").Get<ProjectSetting>();

builder.Services.AddSingleton<IBus>(sp => BusConfigurator.Create(rabbitSettings.RabbitMqInfo));
builder.Services.AddCors();

var app = builder.Build();

app.UseDeveloperExceptionPage();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

