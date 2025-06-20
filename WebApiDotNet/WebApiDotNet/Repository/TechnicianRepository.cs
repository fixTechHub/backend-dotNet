using MongoDB.Driver;
using WebApiDotNet.Models;
using WebApiDotNet.Repository.IRepository;
using WebApiDotNet.Data;

namespace WebApiDotNet.Repository
{
    public class TechnicianRepository : ITechnicianRepository
    {
        private readonly IMongoCollection<Technician> _collection;

        public TechnicianRepository(MongoDbContext context)
        {
            _collection = context.Technicians;
        }

        public async Task<List<Technician>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<Technician?> GetByIdAsync(string id)
        {
            return await _collection.Find(t => t.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Technician?> UpdateStatusAsync(string id, string status, string? note = null)
        {
            var filter = Builders<Technician>.Filter.Eq(t => t.Id, id);
            var update = Builders<Technician>.Update
                .Set(t => t.Status, status)
                .Set(t => t.UpdatedAt, DateTime.UtcNow);
            if (note != null)
                update = update.Set("note", note);
            var options = new FindOneAndUpdateOptions<Technician> { ReturnDocument = ReturnDocument.After };
            return await _collection.FindOneAndUpdateAsync(filter, update, options);
        }
    }
} 