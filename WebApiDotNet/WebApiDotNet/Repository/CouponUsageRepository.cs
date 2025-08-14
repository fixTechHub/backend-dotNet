using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;
using WebApiDotNet.Data;
using WebApiDotNet.Models;
using WebApiDotNet.Repository.IRepository;

namespace WebApiDotNet.Repository
{
    public class CouponUsageRepository : ICouponUsageRepository
    {
        private readonly IMongoCollection<CouponUsage> _collection;

        public CouponUsageRepository(MongoDbContext context)
        {
            _collection = context.Couponusages;
        }

        public async Task<List<CouponUsage>> GetAllAsync() =>
            await _collection.Find(_ => true).ToListAsync();

        public async Task<List<CouponUsage>> GetByCouponIdAsync(string couponId) =>
            await _collection.Find(x => x.CouponId == ObjectId.Parse(couponId)).ToListAsync();

        public async Task CreateAsync(CouponUsage usage) =>
            await _collection.InsertOneAsync(usage);
        public async Task<CouponUsage?> GetByConditionAsync(Expression<Func<CouponUsage, bool>> filter)
        {
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<CouponUsage?> GetByIdAsync(string id)
        {
            return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<bool> HasUserUsedCouponAsync(string couponId, string userId)
        {
            var filter = Builders<CouponUsage>.Filter.And(
                Builders<CouponUsage>.Filter.Eq(u => u.CouponId, ObjectId.Parse(couponId)),
                Builders<CouponUsage>.Filter.Eq(u => u.UserId, ObjectId.Parse(userId))
            );
            return await _collection.Find(filter).AnyAsync();
        }
    }
}
