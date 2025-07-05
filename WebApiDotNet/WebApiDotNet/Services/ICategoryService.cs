using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiDotNet.DTOs;

namespace WebApiDotNet.Services
{
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetAllAsync();
        Task<List<CategoryDto>> GetDeletedAsync();
        Task<CategoryDto> GetByIdAsync(string id);
        Task<CategoryDto> CreateAsync(CreateCategoryDto dto);
        Task<CategoryDto> UpdateAsync(string id, UpdateCategoryDto dto);
        Task DeleteAsync(string id);
        Task RestoreAsync(string id);
        Task<bool> ExistsAsync(string id);
    }
} 