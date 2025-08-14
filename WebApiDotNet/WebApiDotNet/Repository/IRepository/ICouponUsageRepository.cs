using System.Linq.Expressions;
using WebApiDotNet.Models;

namespace WebApiDotNet.Repository.IRepository
{
    public interface ICouponUsageRepository
    {
        Task<List<CouponUsage>> GetAllAsync();
        Task<List<CouponUsage>> GetByCouponIdAsync(string couponId);
        Task CreateAsync(CouponUsage usage);
        Task<CouponUsage?> GetByConditionAsync(Expression<Func<CouponUsage, bool>> filter);
        Task<CouponUsage?> GetByIdAsync(string id);
        Task<bool> HasUserUsedCouponAsync(string couponId, string userId);
    }
}
