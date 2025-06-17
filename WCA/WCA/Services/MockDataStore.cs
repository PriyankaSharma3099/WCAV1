using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WCA.Models;

namespace WCA.Services
{
    public class MockDataStore : IDataStore<Job>
    {
        readonly List<Job> jobs;

        public MockDataStore()
        {
            jobs = new List<Job>()
            {
                new Job { Id = Guid.NewGuid().ToString(), Text = "First job", Description="This is an job description." },
                new Job { Id = Guid.NewGuid().ToString(), Text = "Second job", Description="This is an job description." },
                new Job { Id = Guid.NewGuid().ToString(), Text = "Third job", Description="This is an job description." },
                new Job { Id = Guid.NewGuid().ToString(), Text = "Fourth job", Description="This is an job description." },
                new Job { Id = Guid.NewGuid().ToString(), Text = "Fifth job", Description="This is an job description." },
                new Job { Id = Guid.NewGuid().ToString(), Text = "Sixth job", Description="This is an job description." }
            };
        }

        public async Task<bool> AddJobAsync(Job job)
        {
            jobs.Add(job);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateJobAsync(Job job)
        {
            var oldJob = jobs.Where((Job arg) => arg.Id == job.Id).FirstOrDefault();
            jobs.Remove(oldJob);
            jobs.Add(job);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteJobAsync(string id)
        {
            var oldJob = jobs.Where((Job arg) => arg.Id == id).FirstOrDefault();
            jobs.Remove(oldJob);

            return await Task.FromResult(true);
        }

        public async Task<Job> GetJobAsync(string id)
        {
            return await Task.FromResult(jobs.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Job>> GetJobsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(jobs);
        }
    }
}