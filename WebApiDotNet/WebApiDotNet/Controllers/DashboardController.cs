using Microsoft.AspNetCore.Mvc;
using WebApiDotNet.Services;
using WebApiDotNet.DTOs;

namespace WebApiDotNet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IBookingService _bookingService;
        private readonly ITechnicianService _technicianService;
        private readonly ICouponUsageService _couponUsageService;
        private readonly IReportService _reportService;
        private readonly ISystemReportService _systemReportService;

        public DashboardController(
            IUserService userService,
            IBookingService bookingService,
            ITechnicianService technicianService,
            ICouponUsageService couponUsageService,
            IReportService reportService,
            ISystemReportService systemReportService)
        {
            _userService = userService;
            _bookingService = bookingService;
            _technicianService = technicianService;
            _couponUsageService = couponUsageService;
            _reportService = reportService;
            _systemReportService = systemReportService;
        }

        // USER
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers() => Ok(await _userService.GetAllAsync());

        [HttpGet("users/{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        // BOOKING
        [HttpGet("bookings")]
        public async Task<IActionResult> GetAllBookings()
        {
            try
            {
                var result = await _bookingService.GetAllAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Lỗi khi lấy danh sách booking: " + ex.Message);
                Console.WriteLine("❌ Stack trace: " + ex.StackTrace);
                return StatusCode(500, new { message = "Lỗi server", error = ex.Message, details = ex.StackTrace });
            }
        }

        [HttpGet("bookings/{id}")]
        public async Task<IActionResult> GetBookingById(string id)
        {
            var booking = await _bookingService.GetByIdAsync(id);
            if (booking == null) return NotFound();
            return Ok(booking);
        }

        // TECHNICIAN
        [HttpGet("technicians")]
        public async Task<IActionResult> GetAllTechnicians() => Ok(await _technicianService.GetAllAsync());

        [HttpGet("technicians/{id}")]
        public async Task<IActionResult> GetTechnicianById(string id)
        {
            var technician = await _technicianService.GetByIdAsync(id);
            if (technician == null) return NotFound();
            return Ok(technician);
        }

        [HttpPatch("technicians/{id}/status")]
        public async Task<IActionResult> UpdateTechnicianStatus(string id, [FromBody] UpdateTechnicianStatusDto dto)
        {
            var updated = await _technicianService.UpdateStatusAsync(id, dto.Status, dto.Note);
            if (updated == null)
                return NotFound(new { message = "Technician not found" });
            return Ok(updated);
        }

        // COUPON USAGE
        [HttpGet("couponusages")]
        public async Task<IActionResult> GetAllCouponUsages() => Ok(await _couponUsageService.GetAllUsagesAsync());

        [HttpGet("couponusages/{couponId}")]
        public async Task<IActionResult> GetCouponUsageByCouponId(string couponId)
        {
            var usage = await _couponUsageService.GetByCouponIdAsync(couponId);
            if (usage == null) return NotFound();
            return Ok(usage);
        }

        [HttpGet("couponusages/id/{id}")]
        public async Task<IActionResult> GetCouponUsageById(string id)
        {
            var usage = await _couponUsageService.GetByIdAsync(id);
            if (usage == null) return NotFound();
            return Ok(usage);
        }

        // REPORT
        [HttpGet("reports")]
        public async Task<IActionResult> GetAllReports() => Ok(await _reportService.GetAllAsync());

        // SYSTEM REPORT
        [HttpGet("systemreports")]
        public async Task<IActionResult> GetAllSystemReports() => Ok(await _systemReportService.GetAllAsync());
        
        [HttpPatch("systemreports/{id}/status")]
        public async Task<IActionResult> UpdateStatus(string id, [FromBody] string status)
        {
            var updated = await _systemReportService.UpdateStatusAsync(id, status);
            if (updated == null)
                return NotFound(new { message = "Report not found" });
            return Ok(updated);
        }
    }
}
