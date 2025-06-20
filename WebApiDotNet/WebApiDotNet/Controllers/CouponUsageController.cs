using Microsoft.AspNetCore.Mvc;
using WebApiDotNet.Services;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApiDotNet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CouponUsageController : ControllerBase
    {
        private readonly ICouponUsageService _service;

        public CouponUsageController(ICouponUsageService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllUsagesAsync();
            return Ok(result);
        }

        [HttpGet("{couponId}")]
        public async Task<IActionResult> GetByCouponId(string couponId)
        {
            var coupon = await _service.GetByCouponIdAsync(couponId);
            if (coupon == null)
                return NotFound("Không tìm thấy coupon");

            return Ok(coupon); // Trả về response body
        }
    }
}
