using WebApiDotNet.Models;

namespace WebApiDotNet.Repository.IRepository
{
    public interface IBookingRepository
    {
        Task<List<Booking>> GetAllAsync();
        Task<Booking?> GetByIdAsync(string id);
        Task<int> CountByMonthAsync(int year, int month);
        Task<List<Booking>> GetByUserIdAsync(string userId);
    }
} 