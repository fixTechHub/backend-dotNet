using WebApiDotNet.DTOs;
using WebApiDotNet.Models;

namespace WebApiDotNet.Services
{
    public interface IBookingStatusLogService
    {
        Task<IEnumerable<BookingStatusLogResponseDTO>> GetAllAsync();
        Task<BookingStatusLogResponseDTO?> GetByIdAsync(string id);
        Task<IEnumerable<BookingStatusLogResponseDTO>> GetByBookingIdAsync(string bookingId);
        Task<IEnumerable<BookingStatusLogResponseDTO>> GetByChangedByAsync(string changedBy);
        Task<IEnumerable<BookingStatusLogResponseDTO>> GetByRoleAsync(string role);
        Task<IEnumerable<BookingStatusLogResponseDTO>> GetByDateRangeAsync(DateTime fromDate, DateTime toDate);
        Task<(IEnumerable<BookingStatusLogResponseDTO> Items, long TotalCount)> GetFilteredAsync(BookingStatusLogFilterDTO filter);
        
        // Business logic methods
        Task<IEnumerable<BookingStatusLogResponseDTO>> GetBookingHistoryAsync(string bookingId);
    }
}
