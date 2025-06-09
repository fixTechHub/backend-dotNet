using WebApiDotNet.DTOs;
using WebApiDotNet.Models;

namespace WebApiDotNet.Services
{
    public interface ICouponService
    {
        Task<List<CouponDto>> GetAllCouponsAsync();
        Task<CouponDto> GetCouponByIdAsync(string id);
        Task CreateCouponAsync(CreateCouponDto dto);
    }
}
