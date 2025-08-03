using WebApiDotNet.Models;

namespace WebApiDotNet.Repository.IRepository
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync();
        Task<User?> GetByIdAsync(string id);
        Task<User?> GetByEmailAsync(string email);
        Task UpdateAsync(User user);
    }
}