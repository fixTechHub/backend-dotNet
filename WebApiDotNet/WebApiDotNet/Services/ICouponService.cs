using WebApiDotNet.DTOs;
using WebApiDotNet.Models;

namespace WebApiDotNet.Services
{
    public interface ICouponService
    {
        Task<List<CouponDto>> GetAllCouponsAsync();
        Task<List<CouponDto>> GetDeletedCouponsAsync();
        Task<CouponDto> GetCouponByIdAsync(string id);
        Task CreateCouponAsync(CreateCouponDto dto);
        Task UpdateCouponAsync(string id, UpdateCouponDto dto);
        Task DeleteCouponAsync(string id);
        Task RestoreCouponAsync(string id);
    }
}
