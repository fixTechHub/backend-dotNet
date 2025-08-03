using Microsoft.AspNetCore.Mvc;
using WebApiDotNet.DTOs;
using WebApiDotNet.Services;
using WebApiDotNet.Attributes;

namespace WebApiDotNet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [RequireAdmin]
    public class FinancialReportController : ControllerBase
    {
        private readonly IFinancialReportService _financialReportService;

        public FinancialReportController(IFinancialReportService financialReportService)
        {
            _financialReportService = financialReportService;
        }

        /// <summary>
        /// Lấy tổng quan tài chính
        /// </summary>
        [HttpGet("summary")]
        public async Task<ActionResult<FinancialSummaryDto>> GetFinancialSummary()
        {
            try
            {
                var summary = await _financialReportService.GetFinancialSummaryAsync();
                return Ok(new { success = true, data = summary });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Lấy danh sách tất cả booking với thông tin tài chính
        /// </summary>
        [HttpGet("bookings")]
        public async Task<ActionResult<List<BookingFinancialDto>>> GetAllBookingsFinancial()
        {
            try
            {
                var bookings = await _financialReportService.GetAllBookingsFinancialAsync();
                return Ok(new { success = true, data = bookings });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Lấy danh sách tất cả technician với thông tin tài chính tổng hợp
        /// </summary>
        [HttpGet("technicians/summary")]
        public async Task<ActionResult<List<TechnicianFinancialSummaryDto>>> GetAllTechniciansFinancialSummary()
        {
            try
            {
                var technicians = await _financialReportService.GetAllTechniciansFinancialSummaryAsync();
                return Ok(new { success = true, data = technicians });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Lấy chi tiết tài chính của một technician cụ thể
        /// </summary>
        [HttpGet("technicians/{technicianId}/details")]
        public async Task<ActionResult<TechnicianFinancialDto>> GetTechnicianFinancialDetails(string technicianId)
        {
            try
            {
                var technician = await _financialReportService.GetTechnicianFinancialDetailsAsync(technicianId);
                if (technician == null)
                    return NotFound(new { success = false, message = "Technician not found" });

                return Ok(new { success = true, data = technician });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Lấy danh sách booking của một technician cụ thể
        /// </summary>
        [HttpGet("technicians/{technicianId}/bookings")]
        public async Task<ActionResult<List<BookingFinancialDto>>> GetBookingsByTechnicianId(string technicianId)
        {
            try
            {
                var bookings = await _financialReportService.GetBookingsByTechnicianIdAsync(technicianId);
                return Ok(new { success = true, data = bookings });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Lấy tổng doanh thu (FinalPrice)
        /// </summary>
        [HttpGet("total-revenue")]
        public async Task<ActionResult<double>> GetTotalRevenue()
        {
            try
            {
                var totalRevenue = await _financialReportService.GetTotalRevenueAsync();
                return Ok(new { success = true, data = totalRevenue });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Lấy tổng HoldingAmount
        /// </summary>
        [HttpGet("total-holding-amount")]
        public async Task<ActionResult<double>> GetTotalHoldingAmount()
        {
            try
            {
                var totalHoldingAmount = await _financialReportService.GetTotalHoldingAmountAsync();
                return Ok(new { success = true, data = totalHoldingAmount });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Lấy tổng CommissionAmount
        /// </summary>
        [HttpGet("total-commission-amount")]
        public async Task<ActionResult<double>> GetTotalCommissionAmount()
        {
            try
            {
                var totalCommissionAmount = await _financialReportService.GetTotalCommissionAmountAsync();
                return Ok(new { success = true, data = totalCommissionAmount });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Lấy tổng TechnicianEarning
        /// </summary>
        [HttpGet("total-technician-earning")]
        public async Task<ActionResult<double>> GetTotalTechnicianEarning()
        {
            try
            {
                var totalTechnicianEarning = await _financialReportService.GetTotalTechnicianEarningAsync();
                return Ok(new { success = true, data = totalTechnicianEarning });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Lấy tổng TotalWithdrawn
        /// </summary>
        [HttpGet("total-withdrawn")]
        public async Task<ActionResult<double>> GetTotalWithdrawn()
        {
            try
            {
                var totalWithdrawn = await _financialReportService.GetTotalWithdrawnAsync();
                return Ok(new { success = true, data = totalWithdrawn });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }
    }
} 