using MongoDB.Bson;
using MongoDB.Driver;
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

    }
}
