using WebApiDotNet.DTOs;

namespace WebApiDotNet.Services
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllAsync();
        Task<UserDto?> GetByIdAsync(string id);
        Task<UserDto?> UpdateAsync(string id, UpdateUserDto updateUserDto);
        Task<UserDto?> LockUserAsync(string id, LockUserDto lockUserDto);
        Task<UserDto?> UnlockUserAsync(string id);
        Task<List<UserDto>> FilterUsersAsync(UserFilterCriteria criteria);
    }
}