using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApiDotNet.Services;
using WebApiDotNet.DTOs;

namespace WebApiDotNet.Controllers
{
    [ApiController]
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
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateServiceDto dto)
        {
            var created = await _serviceService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateServiceDto dto)
        {
            var updated = await _serviceService.UpdateAsync(id, dto);
            return Ok(updated);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var deleted = await _serviceService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
} 