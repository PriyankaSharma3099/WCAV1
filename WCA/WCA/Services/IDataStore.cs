using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WCA.Services
{
    public interface IDataStore<T>
    {
        Task<bool> AddJobAsync(T job);
        Task<bool> UpdateJobAsync(T job);
        Task<bool> DeleteJobAsync(string id);
        Task<T> GetJobAsync(string id);
        Task<IEnumerable<T>> GetJobsAsync(bool forceRefresh = false);
    }
}
