using System.Threading.Tasks;

namespace WebApiDotNet.Services
{
    public interface IBookingPriceService
    {
        Task<decimal> GetMonthlyRevenueAsync(int year, int month);
    }
} 