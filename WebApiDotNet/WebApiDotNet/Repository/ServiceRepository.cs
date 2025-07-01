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
    }
} 