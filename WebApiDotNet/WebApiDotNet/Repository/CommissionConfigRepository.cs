using MongoDB.Driver;
using WebApiDotNet.Data;
using WebApiDotNet.Models;
using WebApiDotNet.Repository.IRepository;

namespace WebApiDotNet.Repository
{
    public class CommissionConfigRepository : ICommissionConfigRepository
    {
        private readonly IMongoCollection<CommissionConfig> _collection;

        public CommissionConfigRepository(MongoDbContext context)
        {
            _collection = context.CommissionConfigs;
        }

        public async Task<List<CommissionConfig>> GetAllAsync()
        {
            return await _collection.Find(c => !c.IsDeleted).ToListAsync();
        }

        public async Task<List<CommissionConfig>> GetDeletedAsync()
        {
            return await _collection.Find(c => c.IsDeleted).ToListAsync();
        }

        public async Task<CommissionConfig> GetByIdAsync(string id)
        {
            return await _collection.Find(c => c.Id == id && !c.IsDeleted).FirstOrDefaultAsync();
        }

        public async Task<CommissionConfig> CreateAsync(CommissionConfig config)
        {
            config.CreatedAt = DateTime.UtcNow;
            config.UpdatedAt = DateTime.UtcNow;
            await _collection.InsertOneAsync(config);
            return config;
        }

        public async Task<CommissionConfig> UpdateAsync(string id, CommissionConfig config)
        {
            config.UpdatedAt = DateTime.UtcNow;
            await _collection.ReplaceOneAsync(c => c.Id == id && !c.IsDeleted, config);
            return config;
        }

        public async Task DeleteAsync(string id)
        {
            var update = Builders<CommissionConfig>.Update
                .Set(c => c.IsDeleted, true)
                .Set(c => c.DeletedAt, DateTime.UtcNow)
                .Set(c => c.UpdatedAt, DateTime.UtcNow);
            await _collection.UpdateOneAsync(c => c.Id == id && !c.IsDeleted, update);
        }

        public async Task RestoreAsync(string id)
        {
            var update = Builders<CommissionConfig>.Update
                .Set(c => c.IsDeleted, false)
                .Set(c => c.DeletedAt, null)
                .Set(c => c.UpdatedAt, DateTime.UtcNow);
            await _collection.UpdateOneAsync(c => c.Id == id && c.IsDeleted, update);
        }

        public async Task<bool> ExistsAsync(string id)
        {
            return await _collection.Find(c => c.Id == id && !c.IsDeleted).AnyAsync();
        }
        public async Task<bool> ExistsDeletedAsync(string id)
        {
            return await _collection.Find(c => c.Id == id && c.IsDeleted).AnyAsync();
        }

        public async Task DisableAllIsAppliedExceptAsync(string exceptId)
        {
            var filter = Builders<CommissionConfig>.Filter.Where(x => x.Id != exceptId && x.IsApplied && !x.IsDeleted);
            var update = Builders<CommissionConfig>.Update.Set(x => x.IsApplied, false);
            await _collection.UpdateManyAsync(filter, update);
        }

        public async Task<List<CommissionConfig>> GetAllAppliedExceptAsync(string exceptId)
        {
            var filter = Builders<CommissionConfig>.Filter.Where(x => x.Id != exceptId && x.IsApplied && !x.IsDeleted);
            return await _collection.Find(filter).ToListAsync();
        }
        public async Task<CommissionConfig?> GetAppliedConfigAsync()
        {
            return await _collection.Find(c => c.IsApplied).FirstOrDefaultAsync();
        }
    }
}