using MongoDB.Driver;
using WebApiDotNet.Data;
using WebApiDotNet.Models;
using WebApiDotNet.Repository.IRepository;

namespace WebApiDotNet.Repository
{
    public class ReportRepository : IReportRepository
    {
        private readonly IMongoCollection<Report> _collection;

        public ReportRepository(MongoDbContext context)
        {
            _collection = context.Reports;
        }

        public async Task<List<Report>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<Report> GetByIdAsync(string id)
        {
            return await _collection.Find(report => report.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Report> UpdateAsync(Report report)
        {
            var filter = Builders<Report>.Filter.Eq(r => r.Id, report.Id);
            var update = Builders<Report>.Update
                .Set(r => r.Status, report.Status)
                .Set(r => r.UpdatedAt, DateTime.UtcNow);

            await _collection.UpdateOneAsync(filter, update);
            return await GetByIdAsync(report.Id);
        }
    }
}