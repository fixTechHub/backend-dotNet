using WebApiDotNet.DTOs;

namespace WebApiDotNet.Services
{
    public interface ICouponUsageService
    {
        Task<List<CouponUsageDto>> GetAllUsagesAsync();
        Task<List<CouponUsageDto>> GetByCouponIdAsync(string couponId);
        Task TrackUsageAsync(string couponId, string userId, string bookingId);
        Task<CouponUsageDto?> GetByIdAsync(string id);
    }
}
