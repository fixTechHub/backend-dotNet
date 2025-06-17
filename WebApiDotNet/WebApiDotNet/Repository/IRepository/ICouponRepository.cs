using WebApiDotNet.Models;

namespace WebApiDotNet.Repository.IRepository
{
    public interface ICouponRepository
    {
        Task<List<Coupon>> GetAllAsync();
        Task<List<Coupon>> GetDeletedAsync();
        Task<Coupon> GetByIdAsync(string id);
        Task<Coupon> GetByCodeAsync(string code);
        Task CreateAsync(Coupon coupon);
        Task UpdateAsync(string id, Coupon coupon);
        Task DeleteAsync(string id);
        Task RestoreAsync(string id);
        Task<bool> ExistsAsync(string id);
        Task<bool> ExistsByCodeAsync(string code);
    }
}
