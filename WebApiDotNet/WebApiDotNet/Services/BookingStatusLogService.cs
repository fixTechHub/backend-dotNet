using WebApiDotNet.Models;
using WebApiDotNet.DTOs;
using WebApiDotNet.Repository.IRepository;
using WebApiDotNet.Data;
using MongoDB.Driver;

namespace WebApiDotNet.Services
{
    public class BookingStatusLogService : IBookingStatusLogService
    {
        private readonly IBookingStatusLogRepository _repository;
        private readonly MongoDbContext _context;

        public BookingStatusLogService(IBookingStatusLogRepository repository, MongoDbContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task<IEnumerable<BookingStatusLogResponseDTO>> GetAllAsync()
        {
            var logs = await _repository.GetAllAsync();
            return await MapToResponseDTOs(logs);
        }

        public async Task<BookingStatusLogResponseDTO?> GetByIdAsync(string id)
        {
            var log = await _repository.GetByIdAsync(id);
            if (log == null) return null;

            return await MapToResponseDTO(log);
        }

        public async Task<IEnumerable<BookingStatusLogResponseDTO>> GetByBookingIdAsync(string bookingId)
        {
            var logs = await _repository.GetByBookingIdAsync(bookingId);
            return await MapToResponseDTOs(logs);
        }

        public async Task<IEnumerable<BookingStatusLogResponseDTO>> GetByChangedByAsync(string changedBy)
        {
            var logs = await _repository.GetByChangedByAsync(changedBy);
            return await MapToResponseDTOs(logs);
        }

        public async Task<IEnumerable<BookingStatusLogResponseDTO>> GetByRoleAsync(string role)
        {
            var logs = await _repository.GetByRoleAsync(role);
            return await MapToResponseDTOs(logs);
        }

        public async Task<IEnumerable<BookingStatusLogResponseDTO>> GetByDateRangeAsync(DateTime fromDate, DateTime toDate)
        {
            var logs = await _repository.GetByDateRangeAsync(fromDate, toDate);
            return await MapToResponseDTOs(logs);
        }

        public async Task<(IEnumerable<BookingStatusLogResponseDTO> Items, long TotalCount)> GetFilteredAsync(BookingStatusLogFilterDTO filter)
        {
            // Sử dụng method mới với MongoDB Aggregation Pipeline để tránh N+1 Query
            var logs = await _repository.GetFilteredWithJoinsAsync(filter);
            var totalCount = await _repository.GetCountAsync(filter);

            return (logs, totalCount);
        }

        public async Task<IEnumerable<BookingStatusLogResponseDTO>> GetBookingHistoryAsync(string bookingId)
        {
            var logs = await _repository.GetByBookingIdAsync(bookingId);
            return await MapToResponseDTOs(logs);
        }

        private async Task<BookingStatusLogResponseDTO> MapToResponseDTO(BookingStatusLog log)
        {
            var response = new BookingStatusLogResponseDTO
            {
                Id = log.Id ?? string.Empty,
                BookingId = log.BookingId,
                FromStatus = log.FromStatus,
                ToStatus = log.ToStatus,
                ChangedBy = log.ChangedBy,
                Role = log.Role,
                Note = log.Note,
                CreatedAt = log.CreatedAt
            };

            // Try to get user information if available
            try
            {
                var userCollection = _context.Users;
                var user = await userCollection.Find(u => u.Id == log.ChangedBy).FirstOrDefaultAsync();
                if (user != null)
                {
                    response.ChangedByUserName = user.FullName;
                    response.ChangedByUserEmail = user.Email;
                }
            }
            catch (Exception ex)
            {
                // Log error but don't fail the entire operation
                Console.WriteLine($"Error fetching user info: {ex.Message}");
            }

            // Fetch booking info
            try
            {
                var bookingCollection = _context.Bookings;
                var booking = await bookingCollection.Find(b => b.Id == log.BookingId).FirstOrDefaultAsync();
                if (booking != null)
                {
                    response.BookingCode = booking.BookingCode;
                    response.BookingDescription = booking.Description;
                }
            }
            catch (Exception ex)
            {
                // Log error but don't fail the entire operation
                Console.WriteLine($"Error fetching booking info: {ex.Message}");
            }

            return response;
        }

        private async Task<IEnumerable<BookingStatusLogResponseDTO>> MapToResponseDTOs(IEnumerable<BookingStatusLog> logs)
        {
            var responseLogs = new List<BookingStatusLogResponseDTO>();
            
            foreach (var log in logs)
            {
                var response = await MapToResponseDTO(log);
                responseLogs.Add(response);
            }

            return responseLogs;
        }
    }
}
