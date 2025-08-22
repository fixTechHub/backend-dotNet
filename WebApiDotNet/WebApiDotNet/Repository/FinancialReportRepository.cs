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

        // Helper method để tạo filter cho Schedule với format hợp lệ
        private FilterDefinition<Booking> GetValidScheduleFilter()
        {
            return Builders<Booking>.Filter.Or(
                Builders<Booking>.Filter.Type("schedule", BsonType.Document),
                Builders<Booking>.Filter.Type("schedule", BsonType.Null),
                Builders<Booking>.Filter.Not(Builders<Booking>.Filter.Exists("schedule"))
            );
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
            try
            {
                Console.WriteLine("🔍 Getting all bookings financial data...");
                
                // Trước tiên, hãy kiểm tra xem có bao nhiêu bookings trong database
                var totalBookings = await _bookingCollection.CountDocumentsAsync(_ => true);
                Console.WriteLine($"📊 Total bookings in database: {totalBookings}");
                
                if (totalBookings == 0)
                {
                    Console.WriteLine("⚠️ No bookings found in database");
                    return new List<BookingFinancialDto>();
                }
                
                // Sử dụng filter để bỏ qua những booking có Schedule không phải là object
                Console.WriteLine("🔍 Using filter to skip bookings with invalid Schedule format...");
                
                // Filter chỉ lấy những bookings có Schedule là object/document hoặc null
                var filter = GetValidScheduleFilter();
                
                var allBookings = await _bookingCollection.Find(filter).ToListAsync();
                Console.WriteLine($"📊 Found {allBookings.Count} bookings with valid Schedule format");
                
                var result = new List<BookingFinancialDto>();
                foreach (var booking in allBookings)
                {
                    try
                    {
                        result.Add(new BookingFinancialDto
                        {
                            Id = booking.Id,
                            BookingCode = booking.BookingCode,
                            CustomerId = booking.CustomerId,
                            CustomerName = "Unknown", // Will be populated later if needed
                            TechnicianId = booking.TechnicianId,
                            TechnicianName = "Unknown", // Will be populated later if needed
                            ServiceId = booking.ServiceId,
                            ServiceName = "Unknown", // Will be populated later if needed
                            FinalPrice = booking.FinalPrice,
                            HoldingAmount = booking.HoldingAmount,
                            CommissionAmount = booking.CommissionAmount,
                            TechnicianEarning = booking.TechnicianEarning,
                            CreatedAt = booking.CreatedAt,
                            Status = booking.Status.ToString(),
                            PaymentStatus = booking.PaymentStatus.ToString()
                        });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"⚠️ Error processing booking {booking.Id}: {ex.Message}");
                        // Continue with next booking
                    }
                }
                
                Console.WriteLine($"✅ Successfully processed {result.Count} bookings");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error in GetAllBookingsFinancialAsync: {ex.Message}");
                Console.WriteLine($"❌ Stack trace: {ex.StackTrace}");
                throw;
            }
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

                // Get booking count for this technician - chỉ cần kiểm tra TechnicianId
                var bookingFilter = Builders<Booking>.Filter.Eq(b => b.TechnicianId, technician.Id);
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
                Email = user?.Email ?? "Unknown",
                Phone = user?.Phone ?? "Unknown",
                TotalEarning = technician.TotalEarning,
                TotalCommissionPaid = technician.TotalCommissionPaid,
                TotalHoldingAmount = technician.TotalHoldingAmount,
                TotalWithdrawn = technician.TotalWithdrawn,
                Bookings = bookings
            };
        }

        public async Task<List<BookingFinancialDto>> GetBookingsByTechnicianIdAsync(string technicianId)
        {
            try
            {
                // Kết hợp filter cho TechnicianId và Schedule format
                var filter = Builders<Booking>.Filter.And(
                    Builders<Booking>.Filter.Eq(b => b.TechnicianId, technicianId),
                    GetValidScheduleFilter()
                );
                var bookings = await _bookingCollection.Find(filter).ToListAsync();

                var result = new List<BookingFinancialDto>();
                foreach (var booking in bookings)
                {
                    try
                    {
                        result.Add(new BookingFinancialDto
                        {
                            Id = booking.Id,
                            BookingCode = booking.BookingCode,
                            CustomerId = booking.CustomerId,
                            CustomerName = "Unknown",
                            TechnicianId = booking.TechnicianId,
                            TechnicianName = "Unknown",
                            ServiceId = booking.ServiceId,
                            ServiceName = "Unknown",
                            FinalPrice = booking.FinalPrice,
                            HoldingAmount = booking.HoldingAmount,
                            CommissionAmount = booking.CommissionAmount,
                            TechnicianEarning = booking.TechnicianEarning,
                            CreatedAt = booking.CreatedAt,
                            Status = booking.Status.ToString(),
                            PaymentStatus = booking.PaymentStatus.ToString()
                        });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"⚠️ Error processing booking {booking.Id}: {ex.Message}");
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error in GetBookingsByTechnicianIdAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<double> GetTotalRevenueAsync()
        {
            var filter = Builders<Booking>.Filter.And(
                Builders<Booking>.Filter.Eq(b => b.Status, BookingStatus.DONE),
                GetValidScheduleFilter()
            );
            var total = await _bookingCollection.Find(filter).ToListAsync();
            return total.Sum(b => b.FinalPrice ?? 0);
        }

        public async Task<double> GetTotalHoldingAmountAsync()
        {
            var filter = Builders<Booking>.Filter.And(
                Builders<Booking>.Filter.Eq(b => b.Status, BookingStatus.DONE),
                GetValidScheduleFilter()
            );
            var total = await _bookingCollection.Find(filter).ToListAsync();
            return total.Sum(b => b.HoldingAmount ?? 0);
        }

        public async Task<double> GetTotalCommissionAmountAsync()
        {
            var filter = Builders<Booking>.Filter.And(
                Builders<Booking>.Filter.Eq(b => b.Status, BookingStatus.DONE),
                GetValidScheduleFilter()
            );
            var total = await _bookingCollection.Find(filter).ToListAsync();
            return total.Sum(b => b.CommissionAmount ?? 0);
        }

        public async Task<double> GetTotalTechnicianEarningAsync()
        {
            var filter = Builders<Booking>.Filter.And(
                Builders<Booking>.Filter.Eq(b => b.Status, BookingStatus.DONE),
                GetValidScheduleFilter()
            );
            var total = await _bookingCollection.Find(filter).ToListAsync();
            return total.Sum(b => b.TechnicianEarning ?? 0);
        }

        public async Task<double> GetTotalWithdrawnAsync()
        {
            var filter = Builders<Technician>.Filter.Ne(t => t.Status, TechnicianStatus.DELETED);
            var technicians = await _technicianCollection.Find(filter).ToListAsync();
            return technicians.Sum(t => t.TotalWithdrawn);
        }
    }
}
