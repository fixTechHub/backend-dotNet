using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiDotNet.Data;
using WebApiDotNet.Models;
using WebApiDotNet.Repository.IRepository;
using System;

namespace WebApiDotNet.Repository
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly IMongoCollection<Service> _services;

        public ServiceRepository(MongoDbContext context)
        {
            _services = context.Services;
        }

        public async Task<List<Service>> GetAllAsync()
        {
            return await _services.Find(s => !s.IsDeleted).ToListAsync();
        }

        public async Task<List<Service>> GetDeletedAsync()
        {
            return await _services.Find(s => s.IsDeleted).ToListAsync();
        }

        public async Task<Service> GetByIdAsync(string id)
        {
            return await _services.Find(s => s.Id == id && !s.IsDeleted).FirstOrDefaultAsync();
        }

        public async Task<Service> GetDeletedByIdAsync(string id)
        {
            return await _services.Find(s => s.Id == id && s.IsDeleted).FirstOrDefaultAsync();
        }

        public async Task<Service> CreateAsync(Service service)
        {
            service.CreatedAt = DateTime.UtcNow;
            service.UpdatedAt = DateTime.UtcNow;
            await _services.InsertOneAsync(service);
            return service;
        }

        public async Task<Service> UpdateAsync(string id, Service service)
        {
            service.UpdatedAt = DateTime.UtcNow;
            await _services.ReplaceOneAsync(s => s.Id == id, service);
            return service;
        }

        public async Task DeleteAsync(string id)
        {
            var update = Builders<Service>.Update
                .Set(s => s.IsDeleted, true)
                .Set(s => s.IsActive, false)
                .Set(s => s.DeletedAt, DateTime.UtcNow)
                .Set(s => s.UpdatedAt, DateTime.UtcNow);
            await _services.UpdateOneAsync(s => s.Id == id && !s.IsDeleted, update);
        }

        public async Task RestoreAsync(string id)
        {
            var update = Builders<Service>.Update
                .Set(s => s.IsDeleted, false)
                .Set(s => s.DeletedAt, null)
                .Set(s => s.UpdatedAt, DateTime.UtcNow);
            await _services.UpdateOneAsync(s => s.Id == id && s.IsDeleted, update);
        }

        public async Task<bool> ExistsAsync(string id)
        {
            return await _services.Find(s => s.Id == id && !s.IsDeleted).AnyAsync();
        }

        public async Task<bool> ExistsDeletedAsync(string id)
        {
            return await _services.Find(s => s.Id == id && s.IsDeleted).AnyAsync();
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _services.Find(s => s.ServiceName == name && !s.IsDeleted).AnyAsync();
        }

        public async Task<bool> ExistsByIconAsync(string icon)
        {
            return await _services.Find(s => s.Icon == icon && !s.IsDeleted).AnyAsync();
        }
    }
} 