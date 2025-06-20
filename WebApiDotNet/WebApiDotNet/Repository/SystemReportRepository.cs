using MongoDB.Driver;
using WebApiDotNet.Data;
using WebApiDotNet.Models;
using WebApiDotNet.Repository.IRepository;

namespace WebApiDotNet.Repository
{
    public class SystemReportRepository : ISystemReportRepository
    {
        private readonly IMongoCollection<SystemReport> _collection;

        public SystemReportRepository(MongoDbContext context)
        {
            _collection = context.SystemReports;
        }

        public async Task<List<SystemReport>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<SystemReport?> UpdateStatusAsync(string id, string status)
        {
            var filter = Builders<SystemReport>.Filter.Eq(r => r.Id, id);
            var update = Builders<SystemReport>.Update
                .Set(r => r.Status, status)
                .Set(r => r.UpdatedAt, DateTime.UtcNow);

            var options = new FindOneAndUpdateOptions<SystemReport>
            {
                ReturnDocument = ReturnDocument.After
            };

            return await _collection.FindOneAndUpdateAsync(filter, update, options);
        }
    }
}