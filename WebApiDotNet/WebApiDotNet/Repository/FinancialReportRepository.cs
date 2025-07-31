using MongoDB.Driver;
using WebApiDotNet.DTOs;
using WebApiDotNet.Models;
using WebApiDotNet.Repository.IRepository;
using WebApiDotNet.Data;
using MongoDB.Bson;

namespace WebApiDotNet.Repository
{
    public class FinancialReportRepository : IFinancialReportRepository
    {
        private readonly IMongoCollection<Booking> _bookingCollection;
        private readonly IMongoCollection<Technician> _technicianCollection;
        private readonly IMongoCollection<User> _userCollection;

        public FinancialReportRepository(MongoDbContext context)
        {
            _bookingCollection = context.Bookings;
            _technicianCollection = context.Technicians;
            _userCollection = context.Users;
        }

        public async Task<FinancialSummaryDto> GetFinancialSummaryAsync()
        {
            var totalRevenue = await GetTotalRevenueAsync();
            var totalHoldingAmount = await GetTotalHoldingAmountAsync();
            var totalCommissionAmount = await GetTotalCommissionAmountAsync();
            var totalTechnicianEarning = await GetTotalTechnicianEarningAsync();
            var totalWithdrawn = await GetTotalWithdrawnAsync();

            return new FinancialSummaryDto
            {
                TotalRevenue = totalRevenue,
                TotalHoldingAmount = totalHoldingAmount,
                TotalCommissionAmount = totalCommissionAmount,
                TotalTechnicianEarning = totalTechnicianEarning,
                TotalWithdrawn = totalWithdrawn
            };
        }

        public async Task<List<BookingFinancialDto>> GetAllBookingsFinancialAsync()
        {
            var filter = Builders<Booking>.Filter.Type("Schedule", BsonType.Document);
            var bookings = await _bookingCollection.Find(filter).ToListAsync();

            var bookingFinancials = new List<BookingFinancialDto>();
            foreach (var booking in bookings)
            {
                bookingFinancials.Add(new BookingFinancialDto
                {
                    Id = booking.Id,
                    BookingCode = booking.BookingCode,
                    CustomerId = booking.CustomerId,
                    TechnicianId = booking.TechnicianId,
                    ServiceId = booking.ServiceId,
                    FinalPrice = booking.FinalPrice,
                    HoldingAmount = booking.HoldingAmount,
                    CommissionAmount = booking.CommissionAmount,
                    TechnicianEarning = booking.TechnicianEarning,
                    CreatedAt = booking.CreatedAt,
                    Status = booking.Status.ToString(),
                    PaymentStatus = booking.PaymentStatus.ToString()
                });
            }

            return bookingFinancials;
        }

        public async Task<List<TechnicianFinancialSummaryDto>> GetAllTechniciansFinancialSummaryAsync()
        {
            var filter = Builders<Technician>.Filter.Ne(t => t.Status, TechnicianStatus.DELETED);
            var technicians = await _technicianCollection.Find(filter).ToListAsync();

            var technicianSummaries = new List<TechnicianFinancialSummaryDto>();
            foreach (var technician in technicians)
            {
                // Get user info
                var userFilter = Builders<User>.Filter.Eq(u => u.Id, technician.UserId);
                var user = await _userCollection.Find(userFilter).FirstOrDefaultAsync();

                // Get booking count for this technician
                var bookingFilter = Builders<Booking>.Filter.Eq(b => b.TechnicianId, technician.Id) &
                                  Builders<Booking>.Filter.Type("Schedule", BsonType.Document);
                var bookingCount = await _bookingCollection.CountDocumentsAsync(bookingFilter);

                technicianSummaries.Add(new TechnicianFinancialSummaryDto
                {
                    TechnicianId = technician.Id,
                    TechnicianName = user?.FullName ?? "Unknown",
                    TotalEarning = technician.TotalEarning,
                    TotalCommissionPaid = technician.TotalCommissionPaid,
                    TotalHoldingAmount = technician.TotalHoldingAmount,
                    TotalWithdrawn = technician.TotalWithdrawn,
                    TotalBookings = (int)bookingCount
                });
            }

            return technicianSummaries;
        }

        public async Task<TechnicianFinancialDto> GetTechnicianFinancialDetailsAsync(string technicianId)
        {
            var technicianFilter = Builders<Technician>.Filter.Eq(t => t.Id, technicianId);
            var technician = await _technicianCollection.Find(technicianFilter).FirstOrDefaultAsync();

            if (technician == null)
                return null;

            // Get user info
            var userFilter = Builders<User>.Filter.Eq(u => u.Id, technician.UserId);
            var user = await _userCollection.Find(userFilter).FirstOrDefaultAsync();

            // Get bookings for this technician
            var bookings = await GetBookingsByTechnicianIdAsync(technicianId);

            return new TechnicianFinancialDto
            {
                Id = technician.Id,
                UserId = technician.UserId,
                FullName = user?.FullName ?? "Unknown",
                Email = user?.Email ?? "",
                Phone = user?.Phone ?? "",
                TotalEarning = technician.TotalEarning,
                TotalCommissionPaid = technician.TotalCommissionPaid,
                TotalHoldingAmount = technician.TotalHoldingAmount,
                TotalWithdrawn = technician.TotalWithdrawn,
                Bookings = bookings
            };
        }

        public async Task<List<BookingFinancialDto>> GetBookingsByTechnicianIdAsync(string technicianId)
        {
            var filter = Builders<Booking>.Filter.Eq(b => b.TechnicianId, technicianId) &
                        Builders<Booking>.Filter.Type("Schedule", BsonType.Document);
            var bookings = await _bookingCollection.Find(filter).ToListAsync();

            var bookingFinancials = new List<BookingFinancialDto>();
            foreach (var booking in bookings)
            {
                bookingFinancials.Add(new BookingFinancialDto
                {
                    Id = booking.Id,
                    BookingCode = booking.BookingCode,
                    CustomerId = booking.CustomerId,
                    TechnicianId = booking.TechnicianId,
                    ServiceId = booking.ServiceId,
                    FinalPrice = booking.FinalPrice,
                    HoldingAmount = booking.HoldingAmount,
                    CommissionAmount = booking.CommissionAmount,
                    TechnicianEarning = booking.TechnicianEarning,
                    CreatedAt = booking.CreatedAt,
                    Status = booking.Status.ToString(),
                    PaymentStatus = booking.PaymentStatus.ToString()
                });
            }

            return bookingFinancials;
        }

        public async Task<double> GetTotalRevenueAsync()
        {
            var filter = Builders<Booking>.Filter.Type("Schedule", BsonType.Document) &
                        Builders<Booking>.Filter.Ne(b => b.FinalPrice, null);
            var bookings = await _bookingCollection.Find(filter).ToListAsync();
            return bookings.Sum(b => b.FinalPrice ?? 0);
        }

        public async Task<double> GetTotalHoldingAmountAsync()
        {
            var filter = Builders<Booking>.Filter.Type("Schedule", BsonType.Document) &
                        Builders<Booking>.Filter.Ne(b => b.HoldingAmount, null);
            var bookings = await _bookingCollection.Find(filter).ToListAsync();
            return bookings.Sum(b => b.HoldingAmount ?? 0);
        }

        public async Task<double> GetTotalCommissionAmountAsync()
        {
            var filter = Builders<Booking>.Filter.Type("Schedule", BsonType.Document) &
                        Builders<Booking>.Filter.Ne(b => b.CommissionAmount, null);
            var bookings = await _bookingCollection.Find(filter).ToListAsync();
            return bookings.Sum(b => b.CommissionAmount ?? 0);
        }

        public async Task<double> GetTotalTechnicianEarningAsync()
        {
            var filter = Builders<Booking>.Filter.Type("Schedule", BsonType.Document) &
                        Builders<Booking>.Filter.Ne(b => b.TechnicianEarning, null);
            var bookings = await _bookingCollection.Find(filter).ToListAsync();
            return bookings.Sum(b => b.TechnicianEarning ?? 0);
        }

        public async Task<double> GetTotalWithdrawnAsync()
        {
            var filter = Builders<Technician>.Filter.Ne(t => t.Status, TechnicianStatus.DELETED);
            var technicians = await _technicianCollection.Find(filter).ToListAsync();
            return technicians.Sum(t => t.TotalWithdrawn);
        }
    }
} 