using MongoDB.Driver;
using WebApiDotNet.Models;
using WebApiDotNet.Repository.IRepository;
using WebApiDotNet.Data;
using MongoDB.Bson;

namespace WebApiDotNet.Repository
{
    public class BookingRepository : IBookingRepository
    {
        private readonly IMongoCollection<Booking> _collection;

        public BookingRepository(MongoDbContext context)
        {
            _collection = context.Bookings;
        }

        public async Task<List<Booking>> GetAllAsync()
        {
            var filter = Builders<Booking>.Filter.Type("Schedule", BsonType.Document);
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<Booking?> GetByIdAsync(string id)
        {
            return await _collection.Find(b => b.Id == id).FirstOrDefaultAsync();
        }

        public async Task<int> CountByMonthAsync(int year, int month)
        {
            var builder = Builders<Booking>.Filter;
            var start = new DateTime(year, month, 1);
            var end = start.AddMonths(1);
            var filter = builder.Gte(b => b.CreatedAt, start) & builder.Lt(b => b.CreatedAt, end) & Builders<Booking>.Filter.Type("Schedule", BsonType.Document);
            return (int)await _collection.CountDocumentsAsync(filter);
        }

        public async Task<List<Booking>> GetByUserIdAsync(string userId)
        {
            var filter = Builders<Booking>.Filter.Eq(b => b.CustomerId, userId) & Builders<Booking>.Filter.Type("Schedule", BsonType.Document);
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<decimal> GetMonthlyRevenueAsync(int year, int month)
        {
            var start = new DateTime(year, month, 1);
            var end = start.AddMonths(1);
            var filter = Builders<Booking>.Filter.Gte(b => b.CreatedAt, start) &
                         Builders<Booking>.Filter.Lt(b => b.CreatedAt, end) &
                         Builders<Booking>.Filter.Ne(b => b.FinalPrice, null) &
                         Builders<Booking>.Filter.Eq(b => b.Status, BookingStatus.DONE);
            var bookings = await _collection.Find(filter).ToListAsync();
            decimal revenue = bookings.Sum(b => b.FinalPrice.HasValue ? (decimal)b.FinalPrice.Value * 0.2m : 0);
            return revenue;
        }
    }
} 