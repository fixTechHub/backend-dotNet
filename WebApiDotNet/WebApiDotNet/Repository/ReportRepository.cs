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
    }
}