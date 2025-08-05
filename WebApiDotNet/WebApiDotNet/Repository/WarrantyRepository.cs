using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiDotNet.Models;
using WebApiDotNet.Repository.IRepository;
using WebApiDotNet.Data;
using WebApiDotNet.DTOs;
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
            var warranties = await _collection.Find(_ => true).ToListAsync();
            foreach (var w in warranties)
            {
                if (w.ExpireAt == null) w.ExpireAt = null;
                if (w.ResolutionNote == null) w.ResolutionNote = null;
                if (w.RejectionReason == null) w.RejectionReason = null;
            }
            return warranties;
        }

        public async Task<Warranty?> GetByIdAsync(string id)
        {
            var w = await _collection.Find(w => w.Id == id).FirstOrDefaultAsync();
            if (w != null)
            {
                if (w.ExpireAt == null) w.ExpireAt = null;
                if (w.ResolutionNote == null) w.ResolutionNote = null;
                if (w.RejectionReason == null) w.RejectionReason = null;
            }
            return w;
        }

        public async Task<Warranty?> UpdateStatusAsync(string id, string status, bool isReviewedByAdmin)
        {
            var filter = Builders<Warranty>.Filter.Eq(w => w.Id, id);
            var statusEnum = (WarrantyStatus)Enum.Parse(typeof(WarrantyStatus), status, true);
            var update = Builders<Warranty>.Update
                .Set(w => w.Status, statusEnum)
                .Set(w => w.IsReviewedByAdmin, isReviewedByAdmin)
                .Set(w => w.UpdatedAt, DateTime.UtcNow);
            var options = new FindOneAndUpdateOptions<Warranty> { ReturnDocument = ReturnDocument.After };
            return await _collection.FindOneAndUpdateAsync(filter, update, options);
        }

        public async Task<Warranty?> UpdateDetailsAsync(string id, UpdateWarrantyDetailsDto dto)
        {
            var filter = Builders<Warranty>.Filter.Eq(w => w.Id, id);
            var statusEnum = (WarrantyStatus)Enum.Parse(typeof(WarrantyStatus), dto.Status, true);
            
            var update = Builders<Warranty>.Update
                .Set(w => w.Status, statusEnum)
                .Set(w => w.IsReviewedByAdmin, dto.IsReviewedByAdmin)
                .Set(w => w.UpdatedAt, DateTime.UtcNow);

            // Chỉ update ResolutionNote nếu có giá trị
            if (!string.IsNullOrEmpty(dto.ResolutionNote))
            {
                update = update.Set(w => w.ResolutionNote, dto.ResolutionNote);
            }

            // Chỉ update RejectionReason nếu có giá trị
            if (!string.IsNullOrEmpty(dto.RejectionReason))
            {
                update = update.Set(w => w.RejectionReason, dto.RejectionReason);
            }

            var options = new FindOneAndUpdateOptions<Warranty> { ReturnDocument = ReturnDocument.After };
            return await _collection.FindOneAndUpdateAsync(filter, update, options);
        }
    }
} 