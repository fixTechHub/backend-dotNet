using Microsoft.AspNetCore.Mvc;
using WebApiDotNet.Models;
using WebApiDotNet.Services;

namespace WebApiDotNet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SystemReportController : ControllerBase
    {
        private readonly ISystemReportService _service;

        public SystemReportController(ISystemReportService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var reports = await _service.GetAllAsync();
            return Ok(reports);
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(string id, [FromBody] string status)
        {
            var updated = await _service.UpdateStatusAsync(id, status);
            if (updated == null)
                return NotFound(new { message = "Report not found" });
            return Ok(updated);
        }
    }
}
