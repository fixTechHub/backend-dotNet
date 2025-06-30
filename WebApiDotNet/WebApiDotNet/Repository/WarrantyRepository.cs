using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiDotNet.Models;
using WebApiDotNet.Repository.IRepository;
using WebApiDotNet.Data;
using MongoDB.Driver;

namespace WebApiDotNet.Repository
{
    public class WarrantyRepository : IWarrantyRepository
    {
        private readonly IMongoCollection<Warranty> _collection;

        public WarrantyRepository(MongoDbContext context)
        {
            _collection = context.Warranties;
        }

        public async Task<List<Warranty>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<Warranty?> GetByIdAsync(string id)
        {
            return await _collection.Find(w => w.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Warranty?> UpdateStatusAsync(string id, string status, bool isReviewedByAdmin)
        {
            var filter = Builders<Warranty>.Filter.Eq(w => w.Id, id);
            var update = Builders<Warranty>.Update
                .Set(w => w.Status, status)
                .Set(w => w.IsReviewedByAdmin, isReviewedByAdmin)
                .Set(w => w.UpdatedAt, DateTime.UtcNow);
            var options = new FindOneAndUpdateOptions<Warranty> { ReturnDocument = ReturnDocument.After };
            return await _collection.FindOneAndUpdateAsync(filter, update, options);
        }
    }
} 