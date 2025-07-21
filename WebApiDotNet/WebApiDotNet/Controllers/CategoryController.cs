using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WebApiDotNet.DTOs;
using WebApiDotNet.Services;

namespace WebApiDotNet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _categoryService.GetAllAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Lỗi khi lấy category: {ex.Message}");
                return StatusCode(500, new { message = "Lỗi server", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var result = await _categoryService.GetByIdAsync(id);
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
                var result = await _categoryService.GetDeletedAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi server", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (dto == null)
                return BadRequest(new { errors = new { general = new[] { "Dữ liệu không hợp lệ" } } });
            try
            {
                var created = await _categoryService.CreateAsync(dto);
                return Ok(new { message = "Tạo danh mục thành công", data = created });
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                if (msg.Contains("Danh mục đã tồn tại") && msg.Contains("Tên"))
                    return BadRequest(new { errors = new { CategoryName = new[] { msg } } });
                if (msg.Contains("Icon đã tồn tại"))
                    return BadRequest(new { errors = new { Icon = new[] { msg } } });
                return BadRequest(new { errors = new { general = new[] { msg } } });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateCategoryDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var updated = await _categoryService.UpdateAsync(id, dto);
                return Ok(new { message = "Cập nhật danh mục thành công", data = updated });
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                if (msg.Contains("Danh mục đã tồn tại") && msg.Contains("Tên"))
                    return BadRequest(new { errors = new { CategoryName = new[] { msg } } });
                if (msg.Contains("Icon đã tồn tại"))
                    return BadRequest(new { errors = new { Icon = new[] { msg } } });
                return BadRequest(new { errors = new { general = new[] { msg } } });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _categoryService.DeleteAsync(id);
                return Ok(new { message = "Đã ẩn danh mục thành công" });
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
                await _categoryService.RestoreAsync(id);
                return Ok(new { message = "Khôi phục danh mục thành công" });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
} 