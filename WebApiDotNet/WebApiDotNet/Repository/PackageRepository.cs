using MongoDB.Driver;
using WebApiDotNet.Models;
using WebApiDotNet.Repository.IRepository;
using WebApiDotNet.Data;

namespace WebApiDotNet.Repository
{
    public class PackageRepository : IPackageRepository
    {
        private readonly IMongoCollection<Package> _collection;

        public PackageRepository(MongoDbContext context)
        {
            _collection = context.Packages;
        }

        public async Task<List<Package>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<Package?> GetByIdAsync(string id)
        {
            return await _collection.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Package> CreateAsync(Package package)
        {
            package.CreatedAt = DateTime.UtcNow;
            package.UpdatedAt = DateTime.UtcNow;
            await _collection.InsertOneAsync(package);
            return package;
        }

        public async Task<Package?> UpdateAsync(string id, Package package)
        {
            package.UpdatedAt = DateTime.UtcNow;
            var filter = Builders<Package>.Filter.Eq(p => p.Id, id);
            
            var update = Builders<Package>.Update
                .Set(p => p.Name, package.Name)
                .Set(p => p.Price, package.Price)
                .Set(p => p.Description, package.Description)
                .Set(p => p.Benefits, package.Benefits)
                .Set(p => p.IsActive, package.IsActive)
                .Set(p => p.UpdatedAt, package.UpdatedAt);
            
            var options = new FindOneAndUpdateOptions<Package> { ReturnDocument = ReturnDocument.After };
            return await _collection.FindOneAndUpdateAsync(filter, update, options);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var filter = Builders<Package>.Filter.Eq(p => p.Id, id);
            var result = await _collection.DeleteOneAsync(filter);
            return result.DeletedCount > 0;
        }

        public async Task<List<Package>> GetActivePackagesAsync()
        {
            var filter = Builders<Package>.Filter.Eq(p => p.IsActive, true);
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<int> CountByMonthAsync(int year, int month)
        {
            var builder = Builders<Package>.Filter;
            var start = new DateTime(year, month, 1);
            var end = start.AddMonths(1);
            var filter = builder.Gte(p => p.CreatedAt, start) & builder.Lt(p => p.CreatedAt, end);
            return (int)await _collection.CountDocumentsAsync(filter);
        }
    }
}