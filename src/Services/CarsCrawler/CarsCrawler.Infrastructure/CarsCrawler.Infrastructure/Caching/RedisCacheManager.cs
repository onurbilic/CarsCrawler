using Newtonsoft.Json;

namespace CarsCrawler.Infrastructure.Caching;

public class RedisCacheManager : ICacheService
{
    private readonly RedisServer _redisServer;

    public RedisCacheManager(RedisServer redisServer)
    {
        _redisServer = redisServer;
    }

    public void Add(string key, object data, TimeSpan? expiry = null)
    {
        var jsonData = JsonConvert.SerializeObject(data);
        _redisServer.Database.StringSet(key, jsonData, expiry ?? TimeSpan.FromHours(1));
    }

    public bool Any(string key)
    {
        return _redisServer.Database.KeyExists(key);
    }

    public T? Get<T>(string key)
    {
        if (Any(key))
        {
            string jsonData = _redisServer.Database.StringGet(key);
            return JsonConvert.DeserializeObject<T>(jsonData);
        }

        return default;
    }

    public void Remove(string key)
    {
        _redisServer.Database.KeyDelete(key);
    }

    // public void Clear()
    // {
    //     _redisServer.FlushDatabase();
    // }
}