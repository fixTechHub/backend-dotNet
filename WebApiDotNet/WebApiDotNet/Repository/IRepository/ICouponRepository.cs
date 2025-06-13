using WebApiDotNet.Models;

namespace WebApiDotNet.Repository.IRepository
{
    public interface ICouponRepository
    {
        Task<List<Coupon>> GetAllAsync();
        Task<Coupon> GetByIdAsync(string id);
        Task CreateAsync(Coupon coupon);
        Task UpdateAsync(string id, Coupon coupon);
        Task DeleteAsync(string id);
        //Task<Coupon> GetByCodeAsync(string code);
    }
}
