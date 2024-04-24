using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ReservationSystem.Services.Interfaces;
using StackExchange.Redis;

namespace ReservationSystem.Services.Services
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly ConnectionMultiplexer Redis;
        private readonly IDatabase Database;

        public RedisCacheService(IConfiguration configuration)
        {
            var connectionString = configuration.GetValue<string>("Redis:ConnectionString");
            Redis = ConnectionMultiplexer.Connect(connectionString);
            Database = Redis.GetDatabase();
        }


        public async Task<bool> AddDataAsync<T>(string key, T data, TimeSpan timeSpan)
        {
            var serializedData = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            return await Database.StringSetAsync(key, serializedData, timeSpan);
        }

        public async Task<T> GetDataAsync<T>(string key)
        {
            var serializedData = await Database.StringGetAsync(key);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(serializedData);
        }
        

        public async Task<bool> RemoveDataAsync(string key)
        {
            return await Database.KeyDeleteAsync(key);
        }
    }
}