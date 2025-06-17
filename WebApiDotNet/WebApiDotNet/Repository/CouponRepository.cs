using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using WebApiDotNet.Data;
using WebApiDotNet.Models;
using WebApiDotNet.Repository.IRepository;

namespace WebApiDotNet.Repository
{
    public class CouponRepository : ICouponRepository
    {
        private readonly IMongoCollection<Coupon> _coupons;

        public CouponRepository(MongoDbContext context)
        {
            _coupons = context.Coupons;
        }

        public async Task<List<Coupon>> GetAllAsync()
        {
            return await _coupons.Find(c => !c.IsDeleted).ToListAsync();
        }

        public async Task<List<Coupon>> GetDeletedAsync()
        {
            return await _coupons.Find(c => c.IsDeleted).ToListAsync();
        }

        public async Task<Coupon> GetByIdAsync(string id)
        {
            return await _coupons.Find(c => c.Id == id && !c.IsDeleted).FirstOrDefaultAsync();
        }

        public async Task<Coupon> GetByCodeAsync(string code)
        {
            return await _coupons.Find(c => c.Code == code && !c.IsDeleted).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Coupon coupon)
        {
            coupon.CreatedAt = DateTime.UtcNow;
            coupon.UpdatedAt = DateTime.UtcNow;
            await _coupons.InsertOneAsync(coupon);
        }

        public async Task UpdateAsync(string id, Coupon coupon)
        {
            coupon.UpdatedAt = DateTime.UtcNow;
            await _coupons.ReplaceOneAsync(c => c.Id == id && !c.IsDeleted, coupon);
        }

        public async Task DeleteAsync(string id)
        {
            var update = Builders<Coupon>.Update
                .Set(c => c.IsDeleted, true)
                .Set(c => c.DeletedAt, DateTime.UtcNow)
                .Set(c => c.UpdatedAt, DateTime.UtcNow);

            await _coupons.UpdateOneAsync(c => c.Id == id && !c.IsDeleted, update);
        }

        public async Task RestoreAsync(string id)
        {
            var update = Builders<Coupon>.Update
                .Set(c => c.IsDeleted, false)
                .Set(c => c.DeletedAt, null)
                .Set(c => c.UpdatedAt, DateTime.UtcNow);

            await _coupons.UpdateOneAsync(c => c.Id == id && c.IsDeleted, update);
        }

        public async Task<bool> ExistsAsync(string id)
        {
            return await _coupons.Find(c => c.Id == id && !c.IsDeleted).AnyAsync();
        }

        public async Task<bool> ExistsByCodeAsync(string code)
        {
            return await _coupons.Find(c => c.Code == code && !c.IsDeleted).AnyAsync();
        }
    }
}
