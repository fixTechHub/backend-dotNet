using Microsoft.Extensions.Options;
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

        public async Task<List<Coupon>> GetAllAsync() =>
            await _coupons.Find(_ => true).ToListAsync();

        public async Task<Coupon> GetByIdAsync(string id) =>
            await _coupons.Find(c => c.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Coupon coupon) =>
            await _coupons.InsertOneAsync(coupon);

        public async Task UpdateAsync(string id, Coupon coupon) =>
            await _coupons.ReplaceOneAsync(c => c.Id == id, coupon);

        public async Task DeleteAsync(string id) =>
            await _coupons.DeleteOneAsync(c => c.Id == id);
    }
}
