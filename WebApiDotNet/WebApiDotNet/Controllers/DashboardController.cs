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
        private readonly IRoleService _roleService;
        private readonly IServiceService _serviceService;
        private readonly ICommissionConfigService _commissionConfigService;

        public DashboardController(
            IUserService userService,
            IBookingService bookingService,
            ITechnicianService technicianService,
            ICouponUsageService couponUsageService,
            IReportService reportService,
            ISystemReportService systemReportService,
            IRoleService roleService,
            IServiceService serviceService,
            ICommissionConfigService commissionConfigService)
        {
            _userService = userService;
            _bookingService = bookingService;
            _technicianService = technicianService;
            _couponUsageService = couponUsageService;
            _reportService = reportService;
            _systemReportService = systemReportService;
            _roleService = roleService;
            _serviceService = serviceService;
            _commissionConfigService = commissionConfigService;
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

        [HttpPut("users/{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateUserDto updateUserDto)
        {
            if (updateUserDto == null)
            {
                return BadRequest();
            }

            var updatedUser = await _userService.UpdateAsync(id, updateUserDto);

            if (updatedUser == null)
            {
                return NotFound();
            }

            return Ok(updatedUser);
        }

        [HttpPost("users/{id}/lock")]
        public async Task<IActionResult> LockUser(string id, [FromBody] LockUserDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.LockedReason))
                return BadRequest(new { message = "Lock reason is required" });
            var lockedUser = await _userService.LockUserAsync(id, dto);
            if (lockedUser == null)
                return NotFound(new { message = "User not found" });
            return Ok(lockedUser);
        }

        [HttpPost("users/{id}/unlock")]
        public async Task<IActionResult> UnlockUser(string id)
        {
            try
            {
                var unlockedUser = await _userService.UnlockUserAsync(id);
                if (unlockedUser == null)
                {
                    return NotFound(new { message = "User not found" });
                }
                return Ok(unlockedUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpPost("users/filter")]
        public async Task<IActionResult> FilterUsers([FromBody] UserFilterCriteria criteria)
        {
            var users = await _userService.FilterUsersAsync(criteria);
            return Ok(users);
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

        [HttpGet("booking-count")]
        public async Task<IActionResult> GetBookingCountByMonth([FromQuery] int year, [FromQuery] int month)
        {
            var count = await _bookingService.CountByMonthAsync(year, month);
            return Ok(new { year, month, count });
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
            try
            {
                var updated = await _technicianService.UpdateStatusAsync(id, dto.Status, dto.Note);
                if (updated == null)
                    return NotFound(new { message = "Technician not found" });
                return Ok(updated);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("technician-count")]
        public async Task<IActionResult> GetTechnicianCountByMonth([FromQuery] int year, [FromQuery] int month)
        {
            var count = await _technicianService.CountByMonthAsync(year, month);
            return Ok(new { year, month, count });
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

        [HttpGet("reports/{id}")]
        public async Task<IActionResult> GetReportById(string id)
        {
            var report = await _reportService.GetByIdAsync(id);
            if (report == null) return NotFound();
            return Ok(report);
        }

        //COMMISSION CONFIG
        [HttpGet("commissionconfigs")]
        public async Task<IActionResult> GetCommissionConfigs() => Ok(await _commissionConfigService.GetAllAsync());

        [HttpGet("commissionconfigs/{id}")]
        public async Task<IActionResult> GetCommissionConfigById(string id)
        {
            var cf = await _commissionConfigService.GetByIdAsync(id);
            if (cf == null) return NotFound();
            return Ok(cf);
        }

        // SYSTEM REPORT
        [HttpGet("systemreports")]
        public async Task<IActionResult> GetAllSystemReports() => Ok(await _systemReportService.GetAllAsync());
        
        [HttpPatch("systemreports/{id}/status")]
        public async Task<IActionResult> UpdateStatus(string id, [FromBody] UpdateSystemReportStatusDto dto)
        {
            var updated = await _systemReportService.UpdateStatusAsync(id, dto.Status, dto.ResolutionNote, dto.ResolvedBy);
            if (updated == null)
                return NotFound(new { message = "Report not found" });
            return Ok(updated);
        }

        // ROLE
        [HttpGet("roles")]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _roleService.GetAllAsync();
            return Ok(roles);
        }
        //SERVICE
        [HttpGet("services")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _serviceService.GetAllAsync();
            return Ok(result);
        }
        [HttpGet("services/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _serviceService.GetByIdAsync(id);
            return Ok(result);
        }

        /// <summary>
        /// Lấy doanh thu tháng, chỉ tính các booking có status là DONE và có FinalPrice.
        /// </summary>
        [HttpGet("revenue")]
        public async Task<IActionResult> GetMonthlyRevenue([FromQuery] int year, [FromQuery] int month)
        {
            var revenue = await _bookingService.GetMonthlyRevenueAsync(year, month);
            // Có thể trả về thêm thông tin filter nếu muốn
            return Ok(new { year, month, revenue});
        }
    }
}
