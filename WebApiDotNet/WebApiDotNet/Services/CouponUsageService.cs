using AutoMapper;
using MongoDB.Bson;
using WebApiDotNet.DTOs;
using WebApiDotNet.Models;
using WebApiDotNet.Repository.IRepository;

namespace WebApiDotNet.Services
{
    public class CouponUsageService : ICouponUsageService
    {
        private readonly ICouponUsageRepository _repository;
        private readonly ICouponRepository _couponRepository;
        private readonly IMapper _mapper;

        public CouponUsageService(ICouponUsageRepository repository, ICouponRepository couponRepository, IMapper mapper)
        {
            _repository = repository;
            _couponRepository = couponRepository;
            _mapper = mapper;
        }

        public async Task<List<CouponUsageDto>> GetAllUsagesAsync()
        {
            var usages = await _repository.GetAllAsync();
            return _mapper.Map<List<CouponUsageDto>>(usages);
        }

        public async Task<List<CouponUsageDto>> GetByCouponIdAsync(string couponId)
        {
            var usages = await _repository.GetByCouponIdAsync(couponId);
            return _mapper.Map<List<CouponUsageDto>>(usages);
        }

        public async Task TrackUsageAsync(string couponId, string userId, string bookingId)
        {
            var coupon = await _couponRepository.GetByIdAsync(couponId);
            if (coupon == null || !coupon.IsActive)
                throw new Exception("Coupon không tồn tại hoặc không khả dụng");

            // 🔍 Kiểm tra nếu đã từng dùng coupon này cho bookingId này bởi userId  
            var existingUsage = await _repository.GetByConditionAsync(u => u.CouponId == ObjectId.Parse(couponId) && u.UserId == ObjectId.Parse(userId));

            if (existingUsage != null)
                throw new Exception("Bạn đã sử dụng mã giảm giá này cho đơn đặt dịch vụ này rồi!");

            var usage = new CouponUsage
            {
                CouponId = ObjectId.Parse(couponId),
                UserId = ObjectId.Parse(userId),
                BookingId = ObjectId.Parse(bookingId),
                UsedAt = DateTime.UtcNow
            };

            coupon.UsedCount += 1;
            coupon.UpdatedAt = DateTime.UtcNow;

            await _repository.CreateAsync(usage);
            await _couponRepository.UpdateAsync(couponId, coupon);
        }

        public async Task<CouponUsageDto?> GetByIdAsync(string id)
        {
            var usage = await _repository.GetByIdAsync(id);
            return usage == null ? null : _mapper.Map<CouponUsageDto>(usage);
        }
    }
}
