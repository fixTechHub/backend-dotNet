using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using WebApiDotNet.Services;
using WebApiDotNet.DTOs;

namespace WebApiDotNet.Controllers
{
    [ApiController]
    [Authorize(Roles = "ADMIN")]
    [Route("api/[controller]")]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceService _serviceService;
        public ServiceController(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _serviceService.GetAllAsync();
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _serviceService.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("deleted")]
        public async Task<IActionResult> GetDeleted()
        {
            var result = await _serviceService.GetDeletedAsync();
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateServiceDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (dto == null)
                return BadRequest(new { errors = new { general = new[] { "Dữ liệu không hợp lệ" } } });
            try
            {
                var created = await _serviceService.CreateAsync(dto);
                return Ok(created);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                if (msg.Contains("Dịch vụ đã tồn tại") && msg.Contains("Tên"))
                    return BadRequest(new { errors = new { ServiceName = new[] { msg } } });
                return BadRequest(new { errors = new { general = new[] { msg } } });
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateServiceDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var updated = await _serviceService.UpdateAsync(id, dto);
                return Ok(updated);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                if (msg.Contains("Dịch vụ đã tồn tại") && msg.Contains("Tên"))
                    return BadRequest(new { errors = new { ServiceName = new[] { msg } } });
                return BadRequest(new { errors = new { general = new[] { msg } } });
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _serviceService.DeleteAsync(id);
            return Ok(new { message = "Đã ẩn dịch vụ thành công" });
        }

        [HttpPost("{id}/restore")]
        public async Task<IActionResult> Restore(string id)
        {
            try
            {
                await _serviceService.RestoreAsync(id);
                return Ok(new { message = "Khôi phục dịch vụ thành công" });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
} 