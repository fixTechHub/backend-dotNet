using MongoDB.Driver;
using WebApiDotNet.Data;
using WebApiDotNet.DTOs;
using WebApiDotNet.Models;
using WebApiDotNet.Repository.IRepository;

namespace WebApiDotNet.Services
{
    public class CouponService : ICouponService
    {
        private readonly ICouponRepository _repository;

        public CouponService(ICouponRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<CouponDto>> GetAllCouponsAsync()
        {
            var coupons = await _repository.GetAllAsync();
            return coupons.Select(c => new CouponDto
            {
                Id = c.Id,
                Code = c.Code,
                Description = c.Description,
                Value = c.Value
            }).ToList();
        }

        public async Task<CouponDto> GetCouponByIdAsync(string id)
        {
            var c = await _repository.GetByIdAsync(id);
            return c == null ? null : new CouponDto
            {
                Id = c.Id,
                Code = c.Code,
                Description = c.Description,
                Value = c.Value
            };
        }

        public async Task CreateCouponAsync(CreateCouponDto dto)
        {
            var now = DateTime.UtcNow;
            var coupon = new Coupon
            {
                Code = dto.Code,
                Description = dto.Description,
                Type = dto.Type,
                Value = dto.Value,
                MaxDiscount = dto.MaxDiscount,
                MinOrderValue = dto.MinOrderValue,
                TotalUsageLimit = dto.TotalUsageLimit,
                UsedCount = 0,
                Audience = dto.Audience,
                UserIds = dto.UserIds,
                IsActive = dto.IsActive,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                CreatedAt = now,
                UpdatedAt = now
            };
            await _repository.CreateAsync(coupon);
        }
    }
}