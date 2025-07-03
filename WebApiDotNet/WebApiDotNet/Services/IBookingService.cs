using WebApiDotNet.DTOs;

namespace WebApiDotNet.Services
{
    public interface IBookingService
    {
        Task<List<BookingDto>> GetAllAsync();
        Task<BookingDto?> GetByIdAsync(string id);
        Task<int> CountByMonthAsync(int year, int month);
    }
} 