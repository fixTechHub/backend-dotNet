using AutoMapper;
using WebApiDotNet.DTOs;
using WebApiDotNet.Models;
using WebApiDotNet.Repository.IRepository;

namespace WebApiDotNet.Services
{
    public class CouponService : ICouponService
    {
        private readonly ICouponRepository _repository;
        private readonly IMapper _mapper;
        private readonly IBookingService _bookingService;
        private readonly IUserService _userService;

        public CouponService(ICouponRepository repository, IMapper mapper, IBookingService bookingService, IUserService userService)
        {
            _repository = repository;
            _mapper = mapper;
            _bookingService = bookingService;
            _userService = userService;
        }

        public async Task<List<CouponDto>> GetAllCouponsAsync()
        {
            var coupons = await _repository.GetAllAsync();
            var now = DateTime.UtcNow;
            var tasks = new List<Task>();
            foreach (var coupon in coupons)
            {
                if (coupon.IsActive && coupon.EndDate < now)
                {
                    coupon.IsActive = false;
                    tasks.Add(_repository.UpdateAsync(coupon.Id, coupon));
                }
            }
            if (tasks.Count > 0)
                await Task.WhenAll(tasks);
            return _mapper.Map<List<CouponDto>>(coupons);
        }

        public async Task<List<CouponDto>> GetDeletedCouponsAsync()
        {
            var coupons = await _repository.GetDeletedAsync();
            return _mapper.Map<List<CouponDto>>(coupons);
        }

        public async Task<CouponDto> GetCouponByIdAsync(string id)
        {
            var coupon = await _repository.GetByIdAsync(id);
            var now = DateTime.UtcNow;
            if (coupon != null && coupon.IsActive && coupon.EndDate < now)
            {
                coupon.IsActive = false;
                await _repository.UpdateAsync(coupon.Id, coupon);
            }
            return _mapper.Map<CouponDto>(coupon);
        }

        public async Task CreateCouponAsync(CreateCouponDto dto)
        {
            // 1. Kiểm tra code đã tồn tại
            if (await _repository.ExistsByCodeAsync(dto.Code))
                throw new Exception("Mã coupon đã tồn tại");

            // 1.1. Validate Audience
            var validAudiences = new[] { "ALL", "NEW_USER", "EXISTING_USER", "SPECIFIC_USERS" };
            if (!string.IsNullOrEmpty(dto.Audience) && !validAudiences.Contains(dto.Audience))
                throw new Exception($"Audience phải là một trong: {string.Join(", ", validAudiences)}");
            if (dto.Audience == "SPECIFIC_USERS")
            {
                if (dto.UserIds == null || !dto.UserIds.Any())
                    throw new Exception("Nhập các user là bắt buộc khi Audience là SPECIFIC USERS");
            }

            // 2. Validate Type, Value, MaxDiscount
            if (dto.Type == "PERCENT")
            {
                if (dto.Value < 0)
                    throw new Exception("Value phải lớn hơn 0 với loại PERCENT");
                if (!dto.MaxDiscount.HasValue || dto.MaxDiscount.Value < 1000)
                    throw new Exception("Max Discount phải lớn hơn 1,000 VND với loại PERCENT");
            }
            else if (dto.Type == "FIXED")
            {
                if (dto.Value < 1000)
                    throw new Exception("Value phải lớn hơn hoặc bằng 1000 VND với loại FIXED");
            }
            else
            {
                throw new Exception("Type không hợp lệ");
            }

            // 3. Validate MinOrderValue
            if (dto.MinOrderValue < 1000)
                throw new Exception("Min Order Value phải lớn hơn hoặc bằng 1000 VND");

            // 4. Validate ngày/thời gian
            if (dto.StartDate >= dto.EndDate)
                throw new Exception("Start Date phải nhỏ hơn End Date");

            // 5. Nếu EndDate < ngày hiện tại thì set IsActive = false
            bool isActive = dto.EndDate >= DateTime.UtcNow;

            // 6. Map và tạo coupon
            var coupon = _mapper.Map<Coupon>(dto);
            coupon.IsActive = isActive;

            await _repository.CreateAsync(coupon);
        }

        public async Task UpdateCouponAsync(string id, UpdateCouponDto dto)
        {
            if (!await _repository.ExistsAsync(id))
                throw new Exception("Không tìm thấy coupon");

            // 1. Kiểm tra code trùng (nếu đổi code)
            var existingCoupon = await _repository.GetByIdAsync(id);
            if (existingCoupon.Code != dto.Code && await _repository.ExistsByCodeAsync(dto.Code))
                throw new Exception("Mã coupon đã tồn tại");

            // 1.1. Validate Audience
            var validAudiences = new[] { "ALL", "NEW_USER", "EXISTING_USER", "SPECIFIC_USERS" };
            if (!string.IsNullOrEmpty(dto.Audience) && !validAudiences.Contains(dto.Audience))
                throw new Exception($"Audience phải là một trong: {string.Join(", ", validAudiences)}");
            if (dto.Audience == "SPECIFIC_USERS")
            {
                if (dto.UserIds == null || !dto.UserIds.Any())
                    throw new Exception("Nhập các user là bắt buộc khi Audience là SPECIFIC USERS");
            }

            // 2. Validate Type, Value, MaxDiscount (giống như Create)
            if (dto.Type == "PERCENT")
            {
                if (dto.Value < 0)
                    throw new Exception("Value phải lớn hơn 0 với loại PERCENT");
                if (!dto.MaxDiscount.HasValue || dto.MaxDiscount.Value < 1000)
                    throw new Exception("Max Discount phải lớn hơn 1000 VND với loại PERCENT");
            }
            else if (dto.Type == "FIXED")
            {
                if (dto.Value < 1000)
                    throw new Exception("Value phải lớn hơn hoặc bằng 1000 VND với loại FIXED");
            }
            else
            {
                throw new Exception("Type không hợp lệ");
            }

            // 3. Validate MinOrderValue
            if (dto.MinOrderValue < 1000)
                throw new Exception("Min Order Value phải lớn hơn hoặc bằng 1000 VND");

            // 4. Validate ngày/thời gian
            if (dto.StartDate >= dto.EndDate)
                throw new Exception("Start Date phải nhỏ hơn End Date");

            // 5. Nếu EndDate < ngày hiện tại thì set IsActive = false
            bool isActive = dto.EndDate >= DateTime.UtcNow;

            // 6. Map và update coupon
            _mapper.Map(dto, existingCoupon);
            existingCoupon.IsActive = isActive;

            await _repository.UpdateAsync(id, existingCoupon);
        }

        public async Task DeleteCouponAsync(string id)
        {
            if (!await _repository.ExistsAsync(id))
                throw new Exception("Không tìm thấy coupon");

            await _repository.DeleteAsync(id);
        }

        public async Task RestoreCouponAsync(string id)
        {
            try
            {
                await _repository.RestoreAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Không tìm thấy coupon đã xóa");
            }
        }

        public async Task<bool> CanUserUseCouponAsync(string couponId, string userId)
        {
            var coupon = await _repository.GetByIdAsync(couponId);
            if (coupon == null || !coupon.IsActive) return false;

            switch (coupon.Audience)
            {
                case CouponAudience.NEW_USER:
                    var hasBookings = await _bookingService.HasUserBookingsAsync(userId);
                    return !hasBookings;
                case CouponAudience.EXISTING_USER:
                    var user = await _userService.GetByIdAsync(userId);
                    return user != null && user.Status == "Active";
                case CouponAudience.ALL:
                    return true;
                case CouponAudience.SPECIFIC_USERS:
                    return coupon.UserIds?.Contains(userId) == true;
                default:
                    return false;
            }
        }
    }
}