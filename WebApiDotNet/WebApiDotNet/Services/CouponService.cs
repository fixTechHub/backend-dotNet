using AutoMapper;
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
        private readonly IMapper _mapper;
        private readonly ICouponUsageRepository _logRepo;

        public CouponService(ICouponRepository repository, IMapper mapper, ICouponUsageRepository logRepo)
        {
            _repository = repository;
            _mapper = mapper;
            _logRepo = logRepo;
        }

        public async Task<List<CouponDto>> GetAllCouponsAsync()
        {
            var coupons = await _repository.GetAllAsync();
            return _mapper.Map<List<CouponDto>>(coupons);
        }

        public async Task<CouponDto> GetCouponByIdAsync(string id)
        {
            var c = await _repository.GetByIdAsync(id);
            return c == null ? null : _mapper.Map<CouponDto>(c);
        }

        public async Task CreateCouponAsync(CreateCouponDto dto)
        {
            var coupon = _mapper.Map<Coupon>(dto);
            var now = DateTime.UtcNow;
            coupon.CreatedAt = now;
            coupon.UpdatedAt = now;
            coupon.UsedCount = 0;

            await _repository.CreateAsync(coupon);
        }
        public async Task UpdateCouponAsync(string id, UpdateCouponDto dto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) throw new Exception("Coupon not found");

            _mapper.Map(dto, existing);
            existing.UpdatedAt = DateTime.UtcNow;
            await _repository.UpdateAsync(id, existing);

        }

        public async Task DeleteCouponAsync(string id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) throw new Exception("Coupon not found");
            await _repository.DeleteAsync(id);
        }

    }
}