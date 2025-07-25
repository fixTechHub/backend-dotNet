using AutoMapper;
using WebApiDotNet.DTOs;
using WebApiDotNet.Models;
using WebApiDotNet.Repository.IRepository;

namespace WebApiDotNet.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly ICommissionConfigRepository _commissionConfigRepository;
        private readonly IMapper _mapper;

        public BookingService(
            IBookingRepository bookingRepository,
            ICommissionConfigRepository commissionConfigRepository,
            IMapper mapper)
        {
            _bookingRepository = bookingRepository;
            _commissionConfigRepository = commissionConfigRepository;
            _mapper = mapper;
        }

        public async Task<List<BookingDto>> GetAllAsync()
        {
            var bookings = await _bookingRepository.GetAllAsync();
            return _mapper.Map<List<BookingDto>>(bookings);
        }

        public async Task<BookingDto?> GetByIdAsync(string id)
        {
            var booking = await _bookingRepository.GetByIdAsync(id);
            return booking == null ? null : _mapper.Map<BookingDto>(booking);
        }

        public async Task<int> CountByMonthAsync(int year, int month)
        {
            return await _bookingRepository.CountByMonthAsync(year, month);
        }

        public async Task<bool> HasUserBookingsAsync(string userId)
        {
            var bookings = await _bookingRepository.GetByUserIdAsync(userId);
            return bookings.Any();
        }

        public async Task<decimal> GetMonthlyRevenueAsync(int year, int month)
        {
            var bookings = await _bookingRepository.GetBookingsByMonthAsync(year, month);
            var config = await _commissionConfigRepository.GetAppliedConfigAsync();

            if (config == null)
                throw new Exception("Không có CommissionConfig nào có IsApplied = true.");

            decimal commissionPercent = (decimal)config.CommissionPercent / 100m;

            decimal revenue = bookings
                .Where(b => b.FinalPrice != null)
                .Sum(b => (decimal)b.FinalPrice!.Value * commissionPercent);

            return revenue;
        }
    }
} 