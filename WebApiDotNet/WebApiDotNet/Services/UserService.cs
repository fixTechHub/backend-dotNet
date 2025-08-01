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
        private readonly IBookingRepository _bookingRepository;
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;

        public UserService(IUserRepository repository, IBookingRepository bookingRepository, IRoleService roleService, IMapper mapper)
        {
            _repository = repository;
            _bookingRepository = bookingRepository;
            _roleService = roleService;
            _mapper = mapper;
        }

        public async Task<List<UserDto>> GetAllAsync()
        {
            var users = await _repository.GetAllAsync();
            var userDtos = _mapper.Map<List<UserDto>>(users);
            
            // Populate role names
            await PopulateRoleNames(userDtos);
            
            return userDtos;
        }

        public async Task<UserDto?> GetByIdAsync(string id)
        {
            var user = await _repository.GetByIdAsync(id);
            if (user == null)
                return null;
                
            var userDto = _mapper.Map<UserDto>(user);
            await PopulateRoleName(userDto);
            return userDto;
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
            
            var userDto = _mapper.Map<UserDto>(user);
            await PopulateRoleName(userDto);
            return userDto;
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
            
            var userDto = _mapper.Map<UserDto>(user);
            await PopulateRoleName(userDto);
            return userDto;
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
            
            var userDto = _mapper.Map<UserDto>(user);
            await PopulateRoleName(userDto);
            return userDto;
        }

        public async Task<List<UserDto>> FilterUsersAsync(UserFilterCriteria criteria)
        {
            var users = await _repository.GetAllAsync();
            var bookings = await _bookingRepository.GetAllAsync();
            var now = DateTime.UtcNow;
            var filtered = users.AsQueryable();

            // New User
            if (criteria.IsNewUser == true)
            {
                var userIdsWithBooking = bookings.Select(b => b.CustomerId).Distinct().ToHashSet();
                filtered = filtered.Where(u => !userIdsWithBooking.Contains(u.Id));
            }

            // Intermission User (no booking in last 3 months)
            if (criteria.IsIntermissionUser == true)
            {
                var threeMonthsAgo = now.AddMonths(-3);
                var activeUserIds = bookings.Where(b => b.CreatedAt >= threeMonthsAgo).Select(b => b.CustomerId).Distinct().ToHashSet();
                filtered = filtered.Where(u => !activeUserIds.Contains(u.Id));
            }

            // Booking Value Base
            if (criteria.MinTotalBookingValue.HasValue)
            {
                var userBookingValue = bookings
                    .GroupBy(b => b.CustomerId)
                    .ToDictionary(g => g.Key, g => g.Sum(b => b.FinalPrice ?? 0));
                filtered = filtered.Where(u =>
                    userBookingValue.ContainsKey(u.Id) &&
                    userBookingValue[u.Id] >= criteria.MinTotalBookingValue.Value);
            }

            // Time Base
            if (!string.IsNullOrEmpty(criteria.BookingTimeFrom) && !string.IsNullOrEmpty(criteria.BookingTimeTo))
            {
                if (TimeSpan.TryParse(criteria.BookingTimeFrom, out var from) && TimeSpan.TryParse(criteria.BookingTimeTo, out var to))
                {
                    Console.WriteLine($"[DEBUG] Filter by booking time: from={from}, to={to}");
                    foreach (var b in bookings)
                    {
                        if (b.Schedule != null)
                        {
                            Console.WriteLine($"[DEBUG] BookingId={b.Id}, CustomerId={b.CustomerId}, StartTime={b.Schedule.StartTime}, TimeOfDay={b.Schedule.StartTime.TimeOfDay}");
                        }
                    }
                    var userIds = bookings
                        .Where(b =>
                            b.Schedule != null &&
                            b.Schedule.StartTime.TimeOfDay >= from &&
                            b.Schedule.StartTime.TimeOfDay <= to)
                        .Select(b => b.CustomerId)
                        .Distinct()
                        .ToHashSet();
                    Console.WriteLine($"[DEBUG] Matched userIds: {string.Join(",", userIds)}");
                    filtered = filtered.Where(u => userIds.Contains(u.Id));
                }
            }

            // Quantity Base (booking count in current month)
            if (criteria.MinBookingCountInMonth.HasValue)
            {
                var userBookingCount = bookings
                    .Where(b => b.CreatedAt.Month == now.Month && b.CreatedAt.Year == now.Year)
                    .GroupBy(b => b.CustomerId)
                    .ToDictionary(g => g.Key, g => g.Count());
                filtered = filtered.Where(u =>
                    userBookingCount.ContainsKey(u.Id) &&
                    userBookingCount[u.Id] >= criteria.MinBookingCountInMonth.Value);
            }

            // Rank
            if (!string.IsNullOrEmpty(criteria.Rank))
            {
                var userBookingCount = bookings
                    .Where(b => b.CreatedAt.Year == now.Year)
                    .GroupBy(b => b.CustomerId)
                    .ToDictionary(g => g.Key, g => g.Count());
                switch (criteria.Rank)
                {
                    case "Silver":
                        filtered = filtered.Where(u => userBookingCount.ContainsKey(u.Id) && userBookingCount[u.Id] >= 5 && userBookingCount[u.Id] < 20);
                        break;
                    case "Gold":
                        filtered = filtered.Where(u => userBookingCount.ContainsKey(u.Id) && userBookingCount[u.Id] >= 20 && userBookingCount[u.Id] < 50);
                        break;
                    case "Diamond":
                        filtered = filtered.Where(u => userBookingCount.ContainsKey(u.Id) && userBookingCount[u.Id] >= 50 && userBookingCount[u.Id] < 100);
                        break;
                    case "VIP":
                        var userBookingValue = bookings
                            .GroupBy(b => b.CustomerId)
                            .ToDictionary(g => g.Key, g => g.Sum(b => b.FinalPrice ?? 0));
                        filtered = filtered.Where(u =>
                            (userBookingCount.ContainsKey(u.Id) && userBookingCount[u.Id] > 100) ||
                            (userBookingValue.ContainsKey(u.Id) && userBookingValue[u.Id] > 50000000));
                        break;
                }
            }

            var userDtos = filtered.Select(u => _mapper.Map<UserDto>(u)).ToList();
            await PopulateRoleNames(userDtos);
            return userDtos;
        }

        private async Task PopulateRoleName(UserDto userDto)
        {
            if (!string.IsNullOrEmpty(userDto.Role))
            {
                var role = await _roleService.GetByIdAsync(userDto.Role);
                userDto.RoleName = role?.Name;
            }
        }

        private async Task PopulateRoleNames(List<UserDto> userDtos)
        {
            // Get all unique role IDs
            var roleIds = userDtos.Where(u => !string.IsNullOrEmpty(u.Role))
                                 .Select(u => u.Role)
                                 .Distinct()
                                 .ToList();

            // Get all roles in one query
            var roles = await _roleService.GetAllAsync();
            var roleDict = roles.ToDictionary(r => r.Id, r => r.Name);

            // Populate role names
            foreach (var userDto in userDtos)
            {
                if (!string.IsNullOrEmpty(userDto.Role) && roleDict.ContainsKey(userDto.Role))
                {
                    userDto.RoleName = roleDict[userDto.Role];
                }
            }
        }
    }
}
