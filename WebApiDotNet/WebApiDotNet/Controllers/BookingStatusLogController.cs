using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApiDotNet.DTOs;
using WebApiDotNet.Services;

namespace WebApiDotNet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "ADMIN")]
    public class BookingStatusLogController : ControllerBase
    {
        private readonly IBookingStatusLogService _service;

        public BookingStatusLogController(IBookingStatusLogService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get all booking status logs
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookingStatusLogResponseDTO>>> GetAll()
        {
            try
            {
                var logs = await _service.GetAllAsync();
                return Ok(logs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        /// <summary>
        /// Get booking status log by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<BookingStatusLogResponseDTO>> GetById(string id)
        {
            try
            {
                var log = await _service.GetByIdAsync(id);
                if (log == null)
                    return NotFound(new { message = "Booking status log not found" });

                return Ok(log);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        /// <summary>
        /// Get booking status logs by booking ID
        /// </summary>
        [HttpGet("booking/{bookingId}")]
        public async Task<ActionResult<IEnumerable<BookingStatusLogResponseDTO>>> GetByBookingId(string bookingId)
        {
            try
            {
                var logs = await _service.GetByBookingIdAsync(bookingId);
                return Ok(logs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        /// <summary>
        /// Get booking status logs by user who changed the status
        /// </summary>
        [HttpGet("changed-by/{changedBy}")]
        public async Task<ActionResult<IEnumerable<BookingStatusLogResponseDTO>>> GetByChangedBy(string changedBy)
        {
            try
            {
                var logs = await _service.GetByChangedByAsync(changedBy);
                return Ok(logs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        /// <summary>
        /// Get booking status logs by role
        /// </summary>
        [HttpGet("role/{role}")]
        public async Task<ActionResult<IEnumerable<BookingStatusLogResponseDTO>>> GetByRole(string role)
        {
            try
            {
                var logs = await _service.GetByRoleAsync(role);
                return Ok(logs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        /// <summary>
        /// Get booking status logs by date range
        /// </summary>
        [HttpGet("date-range")]
        public async Task<ActionResult<IEnumerable<BookingStatusLogResponseDTO>>> GetByDateRange(
            [FromQuery] DateTime fromDate, 
            [FromQuery] DateTime toDate)
        {
            try
            {
                var logs = await _service.GetByDateRangeAsync(fromDate, toDate);
                return Ok(logs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        /// <summary>
        /// Get filtered and paginated booking status logs
        /// </summary>
        [HttpGet("filtered")]
        public async Task<ActionResult<object>> GetFiltered([FromQuery] BookingStatusLogFilterDTO filter)
        {
            try
            {
                var (items, totalCount) = await _service.GetFilteredAsync(filter);
                
                return Ok(new
                {
                    Items = items,
                    TotalCount = totalCount,
                    Page = filter.Page,
                    PageSize = filter.PageSize,
                    TotalPages = (int)Math.Ceiling((double)totalCount / filter.PageSize)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        /// <summary>
        /// Get complete booking history
        /// </summary>
        [HttpGet("history/{bookingId}")]
        public async Task<ActionResult<IEnumerable<BookingStatusLogResponseDTO>>> GetBookingHistory(string bookingId)
        {
            try
            {
                var logs = await _service.GetBookingHistoryAsync(bookingId);
                return Ok(logs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }
    }
}
