using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiDotNet.DTOs;

namespace WebApiDotNet.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllAsync();
        Task<CategoryDto> GetByIdAsync(string id);
        Task<CategoryDto> CreateAsync(CreateCategoryDto dto);
        Task<bool> UpdateAsync(string id, UpdateCategoryDto dto);
        Task<bool> DeleteAsync(string id);
    }
} 