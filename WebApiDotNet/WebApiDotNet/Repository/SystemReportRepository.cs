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

        public async Task<SystemReport?> UpdateStatusAsync(string id, string status, string? resolutionNote = null, string? resolvedBy = null)
        {
            var filter = Builders<SystemReport>.Filter.Eq(r => r.Id, id);
            var update = Builders<SystemReport>.Update
                .Set(r => r.Status, Enum.TryParse<SystemReportStatus>(status, out var s) ? s : SystemReportStatus.PENDING)
                .Set(r => r.UpdatedAt, DateTime.UtcNow)
                .Set(r => r.ResolutionNote, resolutionNote)
                .Set(r => r.ResolvedBy, resolvedBy)
                .Set(r => r.ResolvedAt, status == "RESOLVED" ? DateTime.UtcNow : (DateTime?)null);

            var options = new FindOneAndUpdateOptions<SystemReport>
            {
                ReturnDocument = ReturnDocument.After
            };

            return await _collection.FindOneAndUpdateAsync(filter, update, options);
        }
    }
}