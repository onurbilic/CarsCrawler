namespace CarsCrawler.Infrastructure.Repositories.Mongo;

public class MongoSettings
{
    public interface IMongoDbSettings
    {
        string DatabaseName { get; set; }
        string? ConnectionString { get; set; }
    }

    public class MongoDbSettings : IMongoDbSettings
    {
        public MongoDbSettings(string databaseName)
        {
            DatabaseName = databaseName;
        }

        public string DatabaseName { get; set; }
        public string? ConnectionString { get; set; }
    }
}