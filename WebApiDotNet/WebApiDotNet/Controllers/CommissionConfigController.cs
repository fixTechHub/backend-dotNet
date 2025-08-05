using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WebApiDotNet.DTOs;
using WebApiDotNet.Services;

namespace WebApiDotNet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommissionConfigController : ControllerBase
    {
        private readonly ICommissionConfigService _commissionConfigService;

        public CommissionConfigController(ICommissionConfigService commissionConfigService)
        {
            _commissionConfigService = commissionConfigService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _commissionConfigService.GetAllAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Lỗi khi lấy CommissionConfig: {ex.Message}");
                return StatusCode(500, new { message = "Lỗi server", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var result = await _commissionConfigService.GetByIdAsync(id);
                if (result == null) return NotFound();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi server", error = ex.Message });
            }
        }

        [HttpGet("deleted")]
        public async Task<IActionResult> GetDeleted()
        {
            try
            {
                var result = await _commissionConfigService.GetDeletedAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi server", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCommissionConfigDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (dto == null)
                return BadRequest(new { errors = new { general = new[] { "Dữ liệu không hợp lệ" } } });

            try
            {
                var created = await _commissionConfigService.CreateAsync(dto);
                return Ok(new { message = "Tạo cấu hình hoa hồng thành công", data = created });
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                return BadRequest(new { errors = new { general = new[] { msg } } });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateCommissionConfigDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updated = await _commissionConfigService.UpdateAsync(id, dto);
                return Ok(new { message = "Cập nhật cấu hình hoa hồng thành công", data = updated });
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                return BadRequest(new { errors = new { general = new[] { msg } } });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _commissionConfigService.DeleteAsync(id);
                return Ok(new { message = "Đã ẩn cấu hình hoa hồng thành công" });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost("{id}/restore")]
        public async Task<IActionResult> Restore(string id)
        {
            try
            {
                await _commissionConfigService.RestoreAsync(id);
                return Ok(new { message = "Khôi phục cấu hình hoa hồng thành công" });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
