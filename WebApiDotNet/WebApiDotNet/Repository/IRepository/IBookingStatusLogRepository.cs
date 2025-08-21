using WebApiDotNet.Models;
using WebApiDotNet.DTOs;

namespace WebApiDotNet.Repository.IRepository
{
    public interface IBookingStatusLogRepository
    {
        Task<IEnumerable<BookingStatusLog>> GetAllAsync();
        Task<BookingStatusLog?> GetByIdAsync(string id);
        Task<IEnumerable<BookingStatusLog>> GetByBookingIdAsync(string bookingId);
        Task<IEnumerable<BookingStatusLog>> GetByChangedByAsync(string changedBy);
        Task<IEnumerable<BookingStatusLog>> GetByRoleAsync(string role);
        Task<IEnumerable<BookingStatusLog>> GetByDateRangeAsync(DateTime fromDate, DateTime toDate);
        Task<IEnumerable<BookingStatusLog>> GetFilteredAsync(BookingStatusLogFilterDTO filter);
        Task<long> GetCountAsync(BookingStatusLogFilterDTO filter);
        Task<List<BookingStatusLogResponseDTO>> GetFilteredWithJoinsAsync(BookingStatusLogFilterDTO filter);
    }
}
