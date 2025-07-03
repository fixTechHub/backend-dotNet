using AutoMapper;
using WebApiDotNet.DTOs;
using WebApiDotNet.Models;
using WebApiDotNet.Repository.IRepository;

namespace WebApiDotNet.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _repository;
        private readonly IMapper _mapper;

        public BookingService(IBookingRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<BookingDto>> GetAllAsync()
        {
            var bookings = await _repository.GetAllAsync();
            return _mapper.Map<List<BookingDto>>(bookings);
        }

        public async Task<BookingDto?> GetByIdAsync(string id)
        {
            var booking = await _repository.GetByIdAsync(id);
            return booking == null ? null : _mapper.Map<BookingDto>(booking);
        }

        public async Task<int> CountByMonthAsync(int year, int month)
        {
            return await _repository.CountByMonthAsync(year, month);
        }
    }
} 