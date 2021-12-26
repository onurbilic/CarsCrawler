namespace CarsCrawler.Infrastructure.Caching;

public interface ICacheService
{
    T? Get<T>(string key);
    void Add(string key, object data, TimeSpan? expiry = null);
    void Remove(string key);
    bool Any(string key);
}