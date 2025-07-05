using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiDotNet.Models;

namespace WebApiDotNet.Repository.IRepository
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllAsync();
        Task<List<Category>> GetDeletedAsync();
        Task<Category> GetByIdAsync(string id);
        Task<Category> CreateAsync(Category category);
        Task<Category> UpdateAsync(string id, Category category);
        Task DeleteAsync(string id);
        Task RestoreAsync(string id);
        Task<bool> ExistsAsync(string id);
    }
} 