using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiDotNet.Data;
using WebApiDotNet.Models;
using WebApiDotNet.Repository.IRepository;

namespace WebApiDotNet.Repository
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly IMongoCollection<Service> _services;

        public ServiceRepository(MongoDbContext context)
        {
            _services = context.Services;
        }

        public async Task<IEnumerable<Service>> GetAllAsync()
        {
            return await _services.Find(_ => true).ToListAsync();
        }

        public async Task<Service> GetByIdAsync(string id)
        {
            return await _services.Find(s => s.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Service> CreateAsync(Service service)
        {
            await _services.InsertOneAsync(service);
            return service;
        }

        public async Task<Service> UpdateAsync(string id, Service service)
        {
            await _services.ReplaceOneAsync(s => s.Id == id, service);
            return service;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _services.DeleteOneAsync(s => s.Id == id);
            return result.DeletedCount > 0;
        }
    }
} 