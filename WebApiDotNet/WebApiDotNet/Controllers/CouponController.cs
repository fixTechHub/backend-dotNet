using Microsoft.AspNetCore.Mvc;
using WebApiDotNet.DTOs;
using WebApiDotNet.Services;
using WebApiDotNet.Attributes;

namespace WebApiDotNet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [RequireAdmin]
    public class CouponController : ControllerBase
    {
        private readonly ICouponService _service;

        public CouponController(ICouponService service)
        {
            _service = service;
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
    }
}
