using WebApiDotNet.DTOs;

namespace WebApiDotNet.Services
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllAsync();
        Task<UserDto?> GetByIdAsync(string id);
    }
}