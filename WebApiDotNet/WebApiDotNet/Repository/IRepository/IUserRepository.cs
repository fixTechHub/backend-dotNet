using WebApiDotNet.Models;

namespace WebApiDotNet.Repository.IRepository
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync();
        Task<User?> GetByIdAsync(string id);
        Task UpdateAsync(User user);
    }
}