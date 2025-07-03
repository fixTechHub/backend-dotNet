using MongoDB.Driver;
using WebApiDotNet.Models;
using WebApiDotNet.Repository.IRepository;
using WebApiDotNet.Data;
using System;
using System.Threading.Tasks;

namespace WebApiDotNet.Repository
{
    public class BookingPriceRepository : IBookingPriceRepository
    {
        private readonly IMongoCollection<BookingPrice> _collection;

        public BookingPriceRepository(MongoDbContext context)
        {
            _collection = context.BookingPrices;
        }

        public async Task<decimal> GetMonthlyRevenueAsync(int year, int month)
        {
            var start = new DateTime(year, month, 1);
            var end = start.AddMonths(1);
            var filter = Builders<BookingPrice>.Filter.Gte(x => x.CreatedAt, start) &
                         Builders<BookingPrice>.Filter.Lt(x => x.CreatedAt, end) &
                         Builders<BookingPrice>.Filter.Ne(x => x.FinalPrice, null);
            var bookingPrices = await _collection.Find(filter).ToListAsync();
            decimal revenue = bookingPrices.Sum(x => x.FinalPrice.HasValue ? x.FinalPrice.Value * 0.2m : 0);
            return revenue;
        }
    }
} 