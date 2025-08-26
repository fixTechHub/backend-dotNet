using Microsoft.AspNetCore.Mvc;
using WebApiDotNet.DTOs;
using WebApiDotNet.Services;    
using Microsoft.AspNetCore.Authorization;

namespace WebApiDotNet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "ADMIN")]
    public class TechnicianSubscriptionController : ControllerBase
    {
        private readonly ITechnicianSubscriptionService _subscriptionService;

        public TechnicianSubscriptionController(ITechnicianSubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _subscriptionService.GetAllAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Lỗi khi lấy subscriptions: " + ex.Message);
                return StatusCode(500, new { message = "Lỗi server", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var result = await _subscriptionService.GetByIdAsync(id);
                if (result == null) return NotFound();
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Lỗi khi lấy subscription: " + ex.Message);
                return StatusCode(500, new { message = "Lỗi server", error = ex.Message });
            }
        }

        [HttpGet("technician/{technicianId}")]
        public async Task<IActionResult> GetByTechnicianId(string technicianId)
        {
            try
            {
                var result = await _subscriptionService.GetByTechnicianIdAsync(technicianId);
                if (result == null) return NotFound();
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Lỗi khi lấy subscription của technician: " + ex.Message);
                return StatusCode(500, new { message = "Lỗi server", error = ex.Message });
            }
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActiveSubscriptions()
        {
            try
            {
                var result = await _subscriptionService.GetActiveSubscriptionsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Lỗi khi lấy active subscriptions: " + ex.Message);
                return StatusCode(500, new { message = "Lỗi server", error = ex.Message });
            }
        }

        [HttpGet("expired")]
        public async Task<IActionResult> GetExpiredSubscriptions()
        {
            try
            {
                var result = await _subscriptionService.GetExpiredSubscriptionsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Lỗi khi lấy expired subscriptions: " + ex.Message);
                return StatusCode(500, new { message = "Lỗi server", error = ex.Message });
            }
        }

        [HttpGet("package/{packageId}")]
        public async Task<IActionResult> GetByPackage(string packageId)
        {
            try
            {
                var result = await _subscriptionService.GetSubscriptionsByPackageAsync(packageId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Lỗi khi lấy subscriptions theo package: " + ex.Message);
                return StatusCode(500, new { message = "Lỗi server", error = ex.Message });
            }
        }

        [HttpGet("stats/count")]
        public async Task<IActionResult> GetActiveCount()
        {
            try
            {
                var count = await _subscriptionService.CountActiveSubscriptionsAsync();
                return Ok(new { activeSubscriptions = count });
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Lỗi khi đếm active subscriptions: " + ex.Message);
                return StatusCode(500, new { message = "Lỗi server", error = ex.Message });
            }
        }

        [HttpGet("stats/revenue")]
        public async Task<IActionResult> GetTotalRevenue()
        {
            try
            {
                var revenue = await _subscriptionService.GetTotalRevenueAsync();
                return Ok(new { totalRevenue = revenue });
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Lỗi khi lấy total revenue: " + ex.Message);
                return StatusCode(500, new { message = "Lỗi server", error = ex.Message });
            }
        }

        [HttpGet("stats/revenue/monthly")]
        public async Task<IActionResult> GetMonthlyRevenue([FromQuery] int year, [FromQuery] int month)
        {
            try
            {
                if (year < 1900 || year > 2100 || month < 1 || month > 12)
                    return BadRequest(new { message = "Invalid year or month" });

                var revenue = await _subscriptionService.GetMonthlyRevenueAsync(year, month);
                return Ok(new { year, month, revenue });
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Lỗi khi lấy monthly revenue: " + ex.Message);
                return StatusCode(500, new { message = "Lỗi server", error = ex.Message });
            }
        }

        [HttpGet("stats/revenue/yearly/{year}")]
        public async Task<IActionResult> GetYearlyRevenue(int year)
        {
            try
            {
                if (year < 1900 || year > 2100)
                    return BadRequest(new { message = "Invalid year" });

                var stats = await _subscriptionService.GetRevenueStatisticsAsync(year);
                return Ok(stats);
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Lỗi khi lấy yearly revenue: " + ex.Message);
                return StatusCode(500, new { message = "Lỗi server", error = ex.Message });
            }
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetSubscriptionsSummary()
        {
            try
            {
                var result = await _subscriptionService.GetSubscriptionsSummaryAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Lỗi khi lấy subscriptions summary: " + ex.Message);
                return StatusCode(500, new { message = "Lỗi server", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTechnicianSubscriptionDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (dto == null)
                return BadRequest(new { errors = new { general = new[] { "Dữ liệu không hợp lệ" } } });

            try
            {
                if (string.IsNullOrWhiteSpace(dto.TechnicianId))
                    return BadRequest(new { errors = new { TechnicianId = new[] { "TechnicianId is required" } } });

                if (string.IsNullOrWhiteSpace(dto.PackageId))
                    return BadRequest(new { errors = new { PackageId = new[] { "PackageId is required" } } });

                // Bỏ validation Amount vì không cần thiết nữa
                // if (dto.Amount <= 0)
                //     return BadRequest(new { errors = new { Amount = new[] { "Amount must be greater than 0" } } });

                var result = await _subscriptionService.CreateAsync(dto);
                return Ok(new { message = "Tạo subscription thành công", data = result });
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Lỗi khi tạo subscription: " + ex.Message);
                return BadRequest(new { errors = new { general = new[] { ex.Message } } });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateTechnicianSubscriptionDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var result = await _subscriptionService.UpdateAsync(id, dto);
                if (result == null) return NotFound();
                return Ok(new { message = "Cập nhật subscription thành công", data = result });
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Lỗi khi cập nhật subscription: " + ex.Message);
                return BadRequest(new { errors = new { general = new[] { ex.Message } } });
            }
        }

        [HttpPut("{id}/payment-status")]
        public async Task<IActionResult> UpdatePaymentStatus(string id, [FromBody] UpdatePaymentStatusDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.PaymentStatus))
                    return BadRequest(new { message = "PaymentStatus is required" });

                var result = await _subscriptionService.UpdatePaymentStatusAsync(id, dto.PaymentStatus, dto.TransactionId);
                if (!result) return NotFound();
                return Ok(new { message = "Cập nhật payment status thành công" });
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Lỗi khi cập nhật payment status: " + ex.Message);
                return StatusCode(500, new { message = "Lỗi server", error = ex.Message });
            }
        }

        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> CancelSubscription(string id, [FromBody] CancelSubscriptionDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.Reason))
                    return BadRequest(new { message = "Reason is required" });

                var result = await _subscriptionService.CancelSubscriptionAsync(id, dto.Reason);
                if (!result) return NotFound();
                return Ok(new { message = "Hủy subscription thành công" });
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Lỗi khi hủy subscription: " + ex.Message);
                return StatusCode(500, new { message = "Lỗi server", error = ex.Message });
            }
        }

        [HttpPut("{id}/renew")]
        public async Task<IActionResult> RenewSubscription(string id, [FromBody] RenewSubscriptionDto dto)
        {
            try
            {
                var result = await _subscriptionService.RenewSubscriptionAsync(id, dto.NewEndDate);
                if (!result) return NotFound();
                return Ok(new { message = "Gia hạn subscription thành công" });
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Lỗi khi gia hạn subscription: " + ex.Message);
                return StatusCode(500, new { message = "Lỗi server", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var result = await _subscriptionService.DeleteAsync(id);
                if (!result) return NotFound();
                return Ok(new { message = "Xóa subscription thành công" });
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Lỗi khi xóa subscription: " + ex.Message);
                return StatusCode(500, new { message = "Lỗi server", error = ex.Message });
            }
        }
    }

    // Helper DTOs for specific operations
    public class UpdatePaymentStatusDto
    {
        public string PaymentStatus { get; set; }
        public string? TransactionId { get; set; }
    }

    public class CancelSubscriptionDto
    {
        public string Reason { get; set; }
    }

    public class RenewSubscriptionDto
    {
        public DateTime NewEndDate { get; set; }
    }
}