using System.Threading.Tasks;
using WebApiDotNet.Repository.IRepository;

namespace WebApiDotNet.Services
{
    public class BookingPriceService : IBookingPriceService
    {
        private readonly IBookingPriceRepository _repository;
        public BookingPriceService(IBookingPriceRepository repository)
        {
            _repository = repository;
        }
        public async Task<decimal> GetMonthlyRevenueAsync(int year, int month)
        {
            return await _repository.GetMonthlyRevenueAsync(year, month);
        }
    }
} 