public interface IRedisService
{
   T Get<T>(string key);
   void Set(string key, object data);
   bool IsSet(string key);
   void Remove(string key);
}