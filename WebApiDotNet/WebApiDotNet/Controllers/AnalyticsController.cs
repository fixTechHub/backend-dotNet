using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WebApiDotNet.DTOs;
using Microsoft.AspNetCore.Authorization;
using WebApiDotNet.Services;

namespace WebApiDotNet.Controllers
{
    [ApiController]
    [Authorize(Roles = "ADMIN")]
    [Route("api/[controller]")]
    public class AnalyticsController : ControllerBase
    {
        private readonly IAnalyticsService _analyticsService;

        public AnalyticsController(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        [HttpGet("subscription-metrics")]
        public async Task<ActionResult<SubscriptionAnalyticsDto>> GetSubscriptionMetrics(
            [FromQuery] int year,
            [FromQuery] string timeRange = "year")
        {
            try
            {
                if (year <= 0)
                {
                    return BadRequest(new { message = "Year must be a positive number" });
                }

                if (string.IsNullOrEmpty(timeRange))
                {
                    timeRange = "year";
                }

                var metrics = await _analyticsService.GetSubscriptionAnalyticsAsync(year, timeRange);
                return Ok(metrics);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
