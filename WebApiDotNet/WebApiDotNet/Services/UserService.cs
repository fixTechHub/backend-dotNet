using AutoMapper;
using WebApiDotNet.DTOs;
using WebApiDotNet.Models;
using WebApiDotNet.Repository.IRepository;
using BCrypt.Net;

namespace WebApiDotNet.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<UserDto>> GetAllAsync()
        {
            var users = await _repository.GetAllAsync();
            return _mapper.Map<List<UserDto>>(users);
        }

        public async Task<UserDto?> GetByIdAsync(string id)
        {
            var user = await _repository.GetByIdAsync(id);
            return user == null ? null : _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto?> UpdateAsync(string id, UpdateUserDto updateUserDto)
        {
            var user = await _repository.GetByIdAsync(id);
            if (user == null)
            {
                return null;
            }
            // Update user properties from DTO
            if (updateUserDto.Role != null) user.Role = updateUserDto.Role;
            if (updateUserDto.Status != null)
            {
                if (Enum.TryParse<UserStatus>(updateUserDto.Status, out var status))
                {
                    user.Status = status;
                }
            }
            user.UpdatedAt = DateTime.UtcNow;
            await _repository.UpdateAsync(user);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto?> LockUserAsync(string id, LockUserDto lockUserDto)
        {
             var user = await _repository.GetByIdAsync(id);
            if (user == null)
            {
                return null;
            }
            // Lock the user
            user.Status = UserStatus.INACTIVE;
            user.LockedReason = lockUserDto.LockedReason;
            user.UpdatedAt = DateTime.UtcNow;
            await _repository.UpdateAsync(user);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto?> UnlockUserAsync(string id)
        {
            var user = await _repository.GetByIdAsync(id);
            if (user == null)
            {
                return null;
            }
            // Unlock the user
            user.Status = UserStatus.ACTIVE;
            user.LockedReason = null; // Clear the locked reason
            user.UpdatedAt = DateTime.UtcNow;
            await _repository.UpdateAsync(user);
            return _mapper.Map<UserDto>(user);
        }
    }
}
