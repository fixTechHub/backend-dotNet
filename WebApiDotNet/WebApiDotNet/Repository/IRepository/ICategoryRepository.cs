using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiDotNet.Models;

namespace WebApiDotNet.Repository.IRepository
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category> GetByIdAsync(string id);
        Task<Category> CreateAsync(Category category);
        Task<bool> UpdateAsync(string id, Category category);
        Task<bool> DeleteAsync(string id);
    }
} 