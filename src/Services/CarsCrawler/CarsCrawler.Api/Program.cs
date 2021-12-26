using CarsCrawler.Infrastructure.Caching;
using Microsoft.Extensions.Configuration;
using CarsCrawler.Infrastructure.Repositories.Mongo;
using Microsoft.Extensions.Options;

IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.Development.json")
    .Build();


var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://*:5010");


// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<MongoSettings>(configuration.GetSection("MongoDbSettings"));
builder.Services.AddSingleton(configuration);
builder.Services.AddSingleton<MongoSettings.IMongoDbSettings>(serviceProvider =>
    serviceProvider.GetRequiredService<IOptions<MongoSettings.MongoDbSettings>>().Value);

builder.Services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));

builder.Services.AddSingleton<RedisServer>();
builder.Services.AddSingleton<ICacheService, RedisCacheManager>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();