using System.Text;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ServiceStack.Redis;

public class RedisService : IRedisService
{
    private readonly RedisConfig _appSettings;
    private readonly RedisEndpoint _redisEndpoint = null;
    private readonly ILogService _logService;

    public RedisService(IOptions<RedisConfig> appSettings, ILogService logService)
    {
        _appSettings = appSettings.Value;
        _redisEndpoint = new RedisEndpoint { Host = _appSettings.RedisEndPoint, Port = Convert.ToInt32(_appSettings.RedisPort), Password = _appSettings.RedisPassword, Ssl = true, SslProtocols = System.Security.Authentication.SslProtocols.Tls12 };
        _logService = logService;
    }

    public T Get<T>(string key)
    {
        try
        {
            using (IRedisClient client = new RedisClient(_redisEndpoint))
            {
                return client.Get<T>(key);
            }
        }
        catch (Exception ex)
        {
            _logService.Error(ex.Message);
            throw;
        }
    }

    public void Set(string key, object data)
    {
        try
        {
            using (IRedisClient client = new RedisClient(_redisEndpoint))
            {
                var dataSerialize = JsonConvert.SerializeObject(data, Formatting.Indented, new JsonSerializerSettings
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects
                });

                client.Set(key, Encoding.UTF8.GetBytes(dataSerialize), DateTime.Now);
            }
        }
        catch (Exception ex)
        {
            _logService.Error(ex.Message);
            throw;
        }
    }

    public bool IsSet(string key)
    {
        try
        {
            using (IRedisClient client = new RedisClient(_redisEndpoint))
            {
                return client.ContainsKey(key);
            }
        }
        catch (Exception ex)
        {
            _logService.Error(ex.Message);
            throw;
        }
    }

    public void Remove(string key)
    {
        try
        {
            using (IRedisClient client = new RedisClient(_redisEndpoint))
            {
                client.Remove(key);
            }
        }
        catch (Exception ex)
        {
            _logService.Error(ex.Message);
            throw;
        }
    }
}