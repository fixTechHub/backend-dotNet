using Microsoft.AspNetCore.Mvc;
using WebApiDotNet.DTOs;
using WebApiDotNet.Services;

namespace WebApiDotNet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CouponController : Controller
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
            if (dto == null)
                return BadRequest(new { message = "Dữ liệu không hợp lệ" });

            try
            {
                await _service.CreateCouponAsync(dto);
                return Ok(new { message = "Created successfully" });
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Lỗi khi tạo coupon: " + ex.Message);
                return StatusCode(500, new { message = "Lỗi server", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, UpdateCouponDto dto)
        {
            try
            {
                await _service.UpdateCouponAsync(id, dto);
                return Ok(new { message = "Updated successfully" });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _service.DeleteCouponAsync(id);
                return Ok(new { message = "Deleted successfully" });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        
    }

}
