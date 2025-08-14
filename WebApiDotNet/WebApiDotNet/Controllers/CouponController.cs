using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApiDotNet.DTOs;
using WebApiDotNet.Services;

namespace WebApiDotNet.Controllers
{
    [ApiController]
    [Authorize(Roles = "ADMIN")]
    [Route("api/[controller]")]
    public class CouponController : ControllerBase
    {
        private readonly ICouponService _service;
        private readonly ICouponUsageService _couponUsageService;

        public CouponController(ICouponService service, ICouponUsageService couponUsageService)
        {
            _service = service;
            _couponUsageService = couponUsageService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _service.GetAllCouponsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Lỗi khi lấy coupon: " + ex.Message);
                return StatusCode(500, new { message = "Lỗi server", error = ex.Message });
            }
        }

        [HttpGet("deleted")]
        public async Task<IActionResult> GetDeleted()
        {
            try
            {
                var result = await _service.GetDeletedCouponsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Lỗi khi lấy danh sách coupon đã xóa: " + ex.Message);
                return StatusCode(500, new { message = "Lỗi server", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _service.GetCouponByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCouponDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (dto == null)
                return BadRequest(new { errors = new { general = new[] { "Dữ liệu không hợp lệ" } } });

            try
            {
                await _service.CreateCouponAsync(dto);
                return Ok(new { message = "Tạo coupon thành công" });
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                if (msg.Contains("code", StringComparison.OrdinalIgnoreCase) || msg.Contains("Mã coupon"))
                    return BadRequest(new { errors = new { Code = new[] { msg } } });
                if (msg.Contains("Value phải lớn hơn 0") || msg.Contains("Value phải lớn hơn 1000") || msg.Contains("Value phải lớn hơn 0 và nhỏ hơn hoặc bằng 100"))
                    return BadRequest(new { errors = new { Value = new[] { msg } } });
                if (msg.Contains("Max Discount"))
                    return BadRequest(new { errors = new { MaxDiscount = new[] { msg } } });
                if (msg.Contains("Min Order Value"))
                    return BadRequest(new { errors = new { MinOrderValue = new[] { msg } } });
                if (msg.Contains("Audience"))
                    return BadRequest(new { errors = new { Audience = new[] { msg } } });
                if (msg.Contains("user") && msg.Contains("Audience"))
                    return BadRequest(new { errors = new { UserIds = new[] { msg } } });
                if (msg.Contains("Start Date"))
                    return BadRequest(new { errors = new { StartDate = new[] { msg } } });
                return BadRequest(new { errors = new { general = new[] { msg } } });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateCouponDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                await _service.UpdateCouponAsync(id, dto);
                return Ok(new { message = "Cập nhật coupon thành công" });
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                if (msg.Contains("code", StringComparison.OrdinalIgnoreCase) || msg.Contains("Mã coupon"))
                    return BadRequest(new { errors = new { Code = new[] { msg } } });
                if (msg.Contains("Value phải lớn hơn 0") || msg.Contains("Value phải lớn hơn 1000") || msg.Contains("Value phải lớn hơn 0 và nhỏ hơn hoặc bằng 100"))
                    return BadRequest(new { errors = new { Value = new[] { msg } } });
                if (msg.Contains("Max Discount"))
                    return BadRequest(new { errors = new { MaxDiscount = new[] { msg } } });
                if (msg.Contains("Min Order Value"))
                    return BadRequest(new { errors = new { MinOrderValue = new[] { msg } } });
                if (msg.Contains("Audience"))
                    return BadRequest(new { errors = new { Audience = new[] { msg } } });
                if (msg.Contains("user") && msg.Contains("Audience"))
                    return BadRequest(new { errors = new { UserIds = new[] { msg } } });
                if (msg.Contains("Start Date"))
                    return BadRequest(new { errors = new { StartDate = new[] { msg } } });
                return BadRequest(new { errors = new { general = new[] { msg } } });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _service.DeleteCouponAsync(id);
                return Ok(new { message = "Đã ẩn coupon thành công" });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost("{id}/restore")]
        public async Task<IActionResult> Restore(string id)
        {
            try
            {
                await _service.RestoreCouponAsync(id);
                return Ok(new { message = "Khôi phục coupon thành công" });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost("{couponId}/validate")]
        public async Task<IActionResult> ValidateCouponUsage(string couponId, [FromBody] ValidateCouponDto dto)
        {
            try
            {
                var canUse = await _service.CanUserUseCouponAsync(couponId, dto.UserId);
                return Ok(new { 
                    canUse, 
                    message = canUse ? "Valid coupon for this user" : "Invalid coupon for this user" 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi server", error = ex.Message });
            }
        }

        [HttpGet("{couponId}/usage-info")]
        public async Task<IActionResult> GetCouponUsageInfo(string couponId)
        {
            try
            {
                var usageInfo = await _service.GetCouponUsageInfoAsync(couponId);
                return Ok(usageInfo);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("user")]
        [AllowAnonymous] // Cho phép user đăng nhập truy cập
        public async Task<IActionResult> GetUserCoupons([FromQuery] string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest(new { message = "UserId là bắt buộc" });
                }

                var userCoupons = await _service.GetUserCouponsAsync(userId);
                return Ok(new { coupons = userCoupons });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi server", error = ex.Message });
            }
        }

        [HttpPost("validate-for-booking")]
        [AllowAnonymous] // Cho phép NodeJS gọi
        public async Task<IActionResult> ValidateCouponForBooking([FromBody] ValidateCouponForBookingDto dto)
        {
            try
            {
                if (dto == null || string.IsNullOrEmpty(dto.CouponCode) || string.IsNullOrEmpty(dto.UserId))
                {
                    return BadRequest(new { message = "CouponCode và UserId là bắt buộc" });
                }

                // Tìm coupon theo code
                var coupon = await _service.GetCouponByCodeAsync(dto.CouponCode);
                if (coupon == null)
                {
                    return BadRequest(new { 
                        isValid = false, 
                        message = "Mã giảm giá không tồn tại" 
                    });
                }

                // Kiểm tra xem user có thể sử dụng coupon này không
                var canUse = await _service.CanUserUseCouponAsync(coupon.Id, dto.UserId);
                
                if (!canUse)
                {
                    return BadRequest(new { 
                        isValid = false, 
                        message = "Mã giảm giá không thể sử dụng cho user này" 
                    });
                }

                // Kiểm tra giá trị đơn hàng
                if (dto.OrderAmount < coupon.MinOrderValue)
                {
                    return BadRequest(new { 
                        isValid = false, 
                        message = $"Đơn hàng phải có giá trị tối thiểu {coupon.MinOrderValue:N0} VND" 
                    });
                }

                // Tính toán giảm giá
                double discountAmount = 0;
                if (coupon.Type == "PERCENT")
                {
                    discountAmount = dto.OrderAmount * (coupon.Value / 100);
                    if (coupon.MaxDiscount.HasValue && discountAmount > coupon.MaxDiscount.Value)
                    {
                        discountAmount = coupon.MaxDiscount.Value;
                    }
                }
                else if (coupon.Type == "FIXED")
                {
                    discountAmount = coupon.Value;
                }

                return Ok(new { 
                    isValid = true,
                    coupon = new {
                        id = coupon.Id,
                        code = coupon.Code,
                        type = coupon.Type,
                        value = coupon.Value,
                        maxDiscount = coupon.MaxDiscount,
                        minOrderValue = coupon.MinOrderValue,
                        description = coupon.Description
                    },
                    discountAmount = discountAmount,
                    finalAmount = dto.OrderAmount - discountAmount,
                    message = "Mã giảm giá hợp lệ"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi server", error = ex.Message });
            }
        }

        [HttpPost("track-usage")]
        [AllowAnonymous] // Cho phép NodeJS gọi
        public async Task<IActionResult> TrackCouponUsage([FromBody] TrackCouponUsageDto dto)
        {
            try
            {
                if (dto == null || string.IsNullOrEmpty(dto.CouponCode) || string.IsNullOrEmpty(dto.UserId) || string.IsNullOrEmpty(dto.BookingId))
                {
                    return BadRequest(new { message = "CouponCode, UserId và BookingId là bắt buộc" });
                }

                // Tìm coupon theo code
                var coupon = await _service.GetCouponByCodeAsync(dto.CouponCode);
                if (coupon == null)
                {
                    return BadRequest(new { message = "Mã giảm giá không tồn tại" });
                }

                // Track usage
                await _couponUsageService.TrackUsageAsync(coupon.Id, dto.UserId, dto.BookingId);

                return Ok(new { message = "Đã ghi nhận sử dụng mã giảm giá thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
