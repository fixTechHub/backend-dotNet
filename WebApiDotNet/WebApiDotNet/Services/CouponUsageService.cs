using AutoMapper;
using MongoDB.Bson;
using WebApiDotNet.DTOs;
using WebApiDotNet.Models;
using WebApiDotNet.Repository.IRepository;
using System.Linq;

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

            // 🔄 Tăng số lần sử dụng
            coupon.UsedCount += 1;
            coupon.UpdatedAt = DateTime.UtcNow;

            // 🚫 Kiểm tra và tự động chuyển status từ active sang inactive khi đạt giới hạn
            if (coupon.UsedCount >= coupon.TotalUsageLimit)
            {
                coupon.IsActive = false;
                Console.WriteLine($"🔄 Mã giảm giá {coupon.Code} đã đạt giới hạn sử dụng ({coupon.UsedCount}/{coupon.TotalUsageLimit}). Tự động chuyển sang inactive.");
            }

            await _repository.CreateAsync(usage);
            await _couponRepository.UpdateAsync(couponId, coupon);
        }

        public async Task<CouponUsageDto?> GetByIdAsync(string id)
        {
            var usage = await _repository.GetByIdAsync(id);
            return usage == null ? null : _mapper.Map<CouponUsageDto>(usage);
        }

        /// <summary>
        /// Kiểm tra và cập nhật trạng thái của tất cả coupon dựa trên số lần sử dụng
        /// </summary>
        public async Task CheckAndUpdateCouponStatusesAsync()
        {
            var allCoupons = await _couponRepository.GetAllAsync();
            var activeCoupons = allCoupons.Where(c => c.IsActive).ToList();
            var updatedCoupons = new List<Coupon>();

            foreach (var coupon in activeCoupons)
            {
                if (coupon.UsedCount >= coupon.TotalUsageLimit)
                {
                    coupon.IsActive = false;
                    coupon.UpdatedAt = DateTime.UtcNow;
                    updatedCoupons.Add(coupon);
                    Console.WriteLine($"🔄 Mã giảm giá {coupon.Code} đã đạt giới hạn sử dụng ({coupon.UsedCount}/{coupon.TotalUsageLimit}). Chuyển sang inactive.");
                }
            }

            // Cập nhật tất cả coupon đã thay đổi
            if (updatedCoupons.Any())
            {
                var updateTasks = updatedCoupons.Select(c => _couponRepository.UpdateAsync(c.Id, c));
                await Task.WhenAll(updateTasks);
                Console.WriteLine($"✅ Đã cập nhật trạng thái của {updatedCoupons.Count} mã giảm giá.");
            }
        }

        /// <summary>
        /// Lấy thống kê sử dụng coupon
        /// </summary>
        public async Task<CouponUsageStatsDto> GetCouponUsageStatsAsync()
        {
            var allCoupons = await _couponRepository.GetAllAsync();
            var activeCoupons = allCoupons.Where(c => c.IsActive).ToList();
            var inactiveCoupons = allCoupons.Where(c => !c.IsActive).ToList();

            var stats = new CouponUsageStatsDto
            {
                TotalCoupons = allCoupons.Count,
                ActiveCoupons = activeCoupons.Count,
                InactiveCoupons = inactiveCoupons.Count,
                CouponsNearLimit = activeCoupons.Count(c => c.UsedCount >= c.TotalUsageLimit * 0.8), // 80% giới hạn
                CouponsAtLimit = activeCoupons.Count(c => c.UsedCount >= c.TotalUsageLimit),
                TotalUsageCount = allCoupons.Sum(c => c.UsedCount),
                AverageUsageRate = allCoupons.Any() ? allCoupons.Average(c => (double)c.UsedCount / c.TotalUsageLimit) : 0
            };

            return stats;
        }
    }
}
