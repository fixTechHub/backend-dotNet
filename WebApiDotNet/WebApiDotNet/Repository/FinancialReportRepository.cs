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
        private readonly IMongoCollection<Service> _serviceCollection;

        public FinancialReportRepository(MongoDbContext context)
        {
            _bookingCollection = context.Bookings;
            _technicianCollection = context.Technicians;
            _userCollection = context.Users;
            _serviceCollection = context.Services;
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
            // Sử dụng MongoDB Aggregation Pipeline để tránh N+1 Query Problem
            var pipeline = new[]
            {
                new BsonDocument("$match", new BsonDocument("Schedule", new BsonDocument("$type", "object"))),
                new BsonDocument("$lookup", new BsonDocument
                {
                    { "from", "Users" },
                    { "localField", "CustomerId" },
                    { "foreignField", "_id" },
                    { "as", "customer" }
                }),
                new BsonDocument("$lookup", new BsonDocument
                {
                    { "from", "Technicians" },
                    { "localField", "TechnicianId" },
                    { "foreignField", "_id" },
                    { "as", "technician" }
                }),
                new BsonDocument("$lookup", new BsonDocument
                {
                    { "from", "Services" },
                    { "localField", "ServiceId" },
                    { "foreignField", "_id" },
                    { "as", "service" }
                }),
                new BsonDocument("$project", new BsonDocument
                {
                    { "Id", "$_id" },
                    { "BookingCode", 1 },
                    { "CustomerId", 1 },
                    { "CustomerName", new BsonDocument("$ifNull", new BsonArray { "$customer.FullName", "$customer.Email", "Unknown" }) },
                    { "TechnicianId", 1 },
                    { "TechnicianName", new BsonDocument("$ifNull", new BsonArray { "$technician.FullName", "$technician.Email", "Unknown" }) },
                    { "ServiceId", 1 },
                    { "ServiceName", new BsonDocument("$ifNull", new BsonArray { "$service.ServiceName", "Unknown" }) },
                    { "FinalPrice", 1 },
                    { "HoldingAmount", 1 },
                    { "CommissionAmount", 1 },
                    { "TechnicianEarning", 1 },
                    { "CreatedAt", 1 },
                    { "Status", 1 },
                    { "PaymentStatus", 1 }
                })
            };

            var result = await _bookingCollection.Aggregate<BookingFinancialDto>(pipeline).ToListAsync();
            return result;
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
            // Sử dụng MongoDB Aggregation Pipeline để tránh N+1 Query Problem
            var pipeline = new[]
            {
                new BsonDocument("$match", new BsonDocument
                {
                    { "TechnicianId", technicianId },
                    { "Schedule", new BsonDocument("$type", "object") }
                }),
                new BsonDocument("$lookup", new BsonDocument
                {
                    { "from", "Users" },
                    { "localField", "CustomerId" },
                    { "foreignField", "_id" },
                    { "as", "customer" }
                }),
                new BsonDocument("$lookup", new BsonDocument
                {
                    { "from", "Technicians" },
                    { "localField", "TechnicianId" },
                    { "foreignField", "_id" },
                    { "as", "technician" }
                }),
                new BsonDocument("$lookup", new BsonDocument
                {
                    { "from", "Services" },
                    { "localField", "ServiceId" },
                    { "foreignField", "_id" },
                    { "as", "service" }
                }),
                new BsonDocument("$project", new BsonDocument
                {
                    { "Id", "$_id" },
                    { "BookingCode", 1 },
                    { "CustomerId", 1 },
                    { "CustomerName", new BsonDocument("$ifNull", new BsonArray { "$customer.FullName", "$customer.Email", "Unknown" }) },
                    { "TechnicianId", 1 },
                    { "TechnicianName", new BsonDocument("$ifNull", new BsonArray { "$technician.FullName", "$technician.Email", "Unknown" }) },
                    { "ServiceId", 1 },
                    { "ServiceName", new BsonDocument("$ifNull", new BsonArray { "$service.ServiceName", "Unknown" }) },
                    { "FinalPrice", 1 },
                    { "HoldingAmount", 1 },
                    { "CommissionAmount", 1 },
                    { "TechnicianEarning", 1 },
                    { "CreatedAt", 1 },
                    { "Status", 1 },
                    { "PaymentStatus", 1 }
                })
            };

            var result = await _bookingCollection.Aggregate<BookingFinancialDto>(pipeline).ToListAsync();
            return result;
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