using Microsoft.AspNetCore.Mvc;
using WebApiDotNet.DTOs;
using WebApiDotNet.Services;
using Microsoft.AspNetCore.Authorization;

namespace WebApiDotNet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "ADMIN")]
    public class PackageController : ControllerBase
    {
        private readonly IPackageService _packageService;

        public PackageController(IPackageService packageService)
        {
            _packageService = packageService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _packageService.GetAllAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Lỗi khi lấy packages: " + ex.Message);
                return StatusCode(500, new { message = "Lỗi server", error = ex.Message });
            }
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActivePackages()
        {
            try
            {
                var result = await _packageService.GetActivePackagesAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Lỗi khi lấy active packages: " + ex.Message);
                return StatusCode(500, new { message = "Lỗi server", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var result = await _packageService.GetByIdAsync(id);
                if (result == null) return NotFound();
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Lỗi khi lấy package: " + ex.Message);
                return StatusCode(500, new { message = "Lỗi server", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePackageDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (dto == null)
                return BadRequest(new { errors = new { general = new[] { "Dữ liệu không hợp lệ" } } });

            try
            {
                if (string.IsNullOrWhiteSpace(dto.Name))
                    return BadRequest(new { errors = new { Name = new[] { "Name is required" } } });

                if (dto.Price <= 0)
                    return BadRequest(new { errors = new { Price = new[] { "Price must be greater than 0" } } });

                await _packageService.CreateAsync(dto);
                return Ok(new { message = "Tạo package thành công" });
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Lỗi khi tạo package: " + ex.Message);
                return BadRequest(new { errors = new { general = new[] { ex.Message } } });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdatePackageDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                if (dto.Price.HasValue && dto.Price.Value <= 0)
                    return BadRequest(new { errors = new { Price = new[] { "Price must be greater than 0" } } });

                await _packageService.UpdateAsync(id, dto);
                return Ok(new { message = "Cập nhật package thành công" });
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Lỗi khi cập nhật package: " + ex.Message);
                return BadRequest(new { errors = new { general = new[] { ex.Message } } });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var result = await _packageService.DeleteAsync(id);
                if (!result) return NotFound();
                return Ok(new { message = "Xóa package thành công" });
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Lỗi khi xóa package: " + ex.Message);
                return StatusCode(500, new { message = "Lỗi server", error = ex.Message });
            }
        }

        [HttpGet("stats/monthly")]
        public async Task<IActionResult> GetMonthlyCount([FromQuery] int year, [FromQuery] int month)
        {
            try
            {
                if (year < 1900 || year > 2100 || month < 1 || month > 12)
                    return BadRequest(new { message = "Invalid year or month" });

                var count = await _packageService.CountByMonthAsync(year, month);
                return Ok(new { year, month, count });
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Lỗi khi lấy thống kê monthly: " + ex.Message);
                return StatusCode(500, new { message = "Lỗi server", error = ex.Message });
            }
        }
    }
}