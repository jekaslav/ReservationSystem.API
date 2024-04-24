using System;
using System.Threading.Tasks;

namespace ReservationSystem.Services.Interfaces
{
    public interface IRedisCacheService
    {
        Task<bool> AddDataAsync<T>(string key, T data, TimeSpan timeSpan);
        
        Task<T> GetDataAsync<T>(string key);
        
        Task<bool> RemoveDataAsync(string key);
    }
}