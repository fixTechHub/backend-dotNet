using Microsoft.AspNetCore.Mvc;
using WebApiDotNet.Services;

namespace WebApiDotNet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        // GET: api/report
        [HttpGet]
        public async Task<IActionResult> GetReports()
        {
            var reports = await _reportService.GetAllAsync();
            return Ok(reports);
        }
    }
}