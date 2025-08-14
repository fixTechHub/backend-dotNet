using WebApiDotNet.DTOs;
using WebApiDotNet.Models;

namespace WebApiDotNet.Services
{
    public interface ICouponService
    {
        Task<List<CouponDto>> GetAllCouponsAsync();
        Task<List<CouponDto>> GetDeletedCouponsAsync();
        Task<CouponDto> GetCouponByIdAsync(string id);
        Task<CouponDto> GetCouponByCodeAsync(string code);
        Task CreateCouponAsync(CreateCouponDto dto);
        Task UpdateCouponAsync(string id, UpdateCouponDto dto);
        Task DeleteCouponAsync(string id);
        Task RestoreCouponAsync(string id);
        Task<bool> CanUserUseCouponAsync(string couponId, string userId);
        Task<CouponUsageInfoDto> GetCouponUsageInfoAsync(string couponId);
        Task<List<CouponDto>> GetUserCouponsAsync(string userId);
    }
}
