using MongoDB.Driver;
using WebApiDotNet.Data;
using WebApiDotNet.Models;
using WebApiDotNet.DTOs;
using WebApiDotNet.Repository.IRepository;

namespace WebApiDotNet.Repository
{
    public class BookingStatusLogRepository : IBookingStatusLogRepository
    {
        private readonly IMongoCollection<BookingStatusLog> _collection;

        public BookingStatusLogRepository(MongoDbContext context)
        {
            _collection = context.BookingStatusLogs;
        }

        public async Task<IEnumerable<BookingStatusLog>> GetAllAsync()
        {
            try
            {
                var count = await _collection.CountDocumentsAsync(_ => true);
                Console.WriteLine($"🔍 BookingStatusLog collection has {count} documents");
                
                var result = await _collection.Find(_ => true).ToListAsync();
                Console.WriteLine($"📊 Retrieved {result.Count} documents from collection");
                
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error in GetAllAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<BookingStatusLog?> GetByIdAsync(string id)
        {
            return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<BookingStatusLog>> GetByBookingIdAsync(string bookingId)
        {
            return await _collection.Find(x => x.BookingId == bookingId).ToListAsync();
        }

        public async Task<IEnumerable<BookingStatusLog>> GetByChangedByAsync(string changedBy)
        {
            return await _collection.Find(x => x.ChangedBy == changedBy).ToListAsync();
        }

        public async Task<IEnumerable<BookingStatusLog>> GetByRoleAsync(string role)
        {
            return await _collection.Find(x => x.Role == role).ToListAsync();
        }

        public async Task<IEnumerable<BookingStatusLog>> GetByDateRangeAsync(DateTime fromDate, DateTime toDate)
        {
            return await _collection.Find(x => x.CreatedAt >= fromDate && x.CreatedAt <= toDate).ToListAsync();
        }

        public async Task<IEnumerable<BookingStatusLog>> GetFilteredAsync(BookingStatusLogFilterDTO filter)
        {
            var filterBuilder = Builders<BookingStatusLog>.Filter;
            var filterDefinition = filterBuilder.Empty;

            // Apply filters
            if (!string.IsNullOrEmpty(filter.BookingId))
                filterDefinition &= filterBuilder.Eq(x => x.BookingId, filter.BookingId);

            if (!string.IsNullOrEmpty(filter.ChangedBy))
                filterDefinition &= filterBuilder.Eq(x => x.ChangedBy, filter.ChangedBy);

            if (!string.IsNullOrEmpty(filter.Role))
                filterDefinition &= filterBuilder.Eq(x => x.Role, filter.Role);

            if (!string.IsNullOrEmpty(filter.FromStatus))
                filterDefinition &= filterBuilder.Eq(x => x.FromStatus, filter.FromStatus);

            if (!string.IsNullOrEmpty(filter.ToStatus))
                filterDefinition &= filterBuilder.Eq(x => x.ToStatus, filter.ToStatus);

            if (filter.FromDate.HasValue)
                filterDefinition &= filterBuilder.Gte(x => x.CreatedAt, filter.FromDate.Value);

            if (filter.ToDate.HasValue)
                filterDefinition &= filterBuilder.Lte(x => x.CreatedAt, filter.ToDate.Value);

            // Apply sorting
            var sortDefinition = (filter.SortBy?.ToLower() ?? "createdat") switch
            {
                "createdat" => filter.SortDescending 
                    ? Builders<BookingStatusLog>.Sort.Descending(x => x.CreatedAt)
                    : Builders<BookingStatusLog>.Sort.Ascending(x => x.CreatedAt),
                "bookingid" => filter.SortDescending
                    ? Builders<BookingStatusLog>.Sort.Descending(x => x.BookingId)
                    : Builders<BookingStatusLog>.Sort.Ascending(x => x.BookingId),
                "changedby" => filter.SortDescending
                    ? Builders<BookingStatusLog>.Sort.Descending(x => x.ChangedBy)
                    : Builders<BookingStatusLog>.Sort.Ascending(x => x.ChangedBy),
                _ => filter.SortDescending
                    ? Builders<BookingStatusLog>.Sort.Descending(x => x.CreatedAt)
                    : Builders<BookingStatusLog>.Sort.Ascending(x => x.CreatedAt)
            };

            // Apply pagination
            var skip = (filter.Page - 1) * filter.PageSize;

            return await _collection.Find(filterDefinition)
                .Sort(sortDefinition)
                .Skip(skip)
                .Limit(filter.PageSize)
                .ToListAsync();
        }

        public async Task<long> GetCountAsync(BookingStatusLogFilterDTO filter)
        {
            var filterBuilder = Builders<BookingStatusLog>.Filter;
            var filterDefinition = filterBuilder.Empty;

            // Apply same filters as GetFilteredAsync
            if (!string.IsNullOrEmpty(filter.BookingId))
                filterDefinition &= filterBuilder.Eq(x => x.BookingId, filter.BookingId);

            if (!string.IsNullOrEmpty(filter.ChangedBy))
                filterDefinition &= filterBuilder.Eq(x => x.ChangedBy, filter.ChangedBy);

            if (!string.IsNullOrEmpty(filter.Role))
                filterDefinition &= filterBuilder.Eq(x => x.Role, filter.Role);

            if (!string.IsNullOrEmpty(filter.FromStatus))
                filterDefinition &= filterBuilder.Eq(x => x.FromStatus, filter.FromStatus);

            if (!string.IsNullOrEmpty(filter.ToStatus))
                filterDefinition &= filterBuilder.Eq(x => x.ToStatus, filter.ToStatus);

            if (filter.FromDate.HasValue)
                filterDefinition &= filterBuilder.Gte(x => x.CreatedAt, filter.FromDate.Value);

            if (filter.ToDate.HasValue)
                filterDefinition &= filterBuilder.Lte(x => x.CreatedAt, filter.ToDate.Value);

            return await _collection.CountDocumentsAsync(filterDefinition);
        }
    }
}
