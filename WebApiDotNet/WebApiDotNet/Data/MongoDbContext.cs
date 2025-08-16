using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Data;
using WebApiDotNet.Models;

namespace WebApiDotNet.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IOptions<MongoDbSettings> options, ILogger<MongoDbContext> logger)
        {
            logger.LogInformation("🔌 Bắt đầu kết nối MongoDB...");

            var client = new MongoClient(options.Value.ConnectionString);
            _database = client.GetDatabase(options.Value.DatabaseName);

            logger.LogInformation("✅ Kết nối MongoDB thành công với database: {DatabaseName}", options.Value.DatabaseName);
        }

        public IMongoDatabase Database => _database;

        public IMongoCollection<Coupon> Coupons => _database.GetCollection<Coupon>("coupons");
        public IMongoCollection<CouponUsage> Couponusages => _database.GetCollection<CouponUsage>("couponusages");
        public IMongoCollection<Report> Reports => _database.GetCollection<Report>("reports");
        public IMongoCollection<SystemReport> SystemReports => _database.GetCollection<SystemReport>("systemreports");
        public IMongoCollection<User> Users => _database.GetCollection<User>("users");  
        public IMongoCollection<Booking> Bookings => _database.GetCollection<Booking>("bookings");
        public IMongoCollection<Technician> Technicians => _database.GetCollection<Technician>("technicians");
        public IMongoCollection<Role> Roles => _database.GetCollection<Role>("roles");
        public IMongoCollection<Category> Categories => _database.GetCollection<Category>("categories");
        public IMongoCollection<Warranty> Warranties => _database.GetCollection<Warranty>("bookingwarranties");
        public IMongoCollection<Service> Services => _database.GetCollection<Service>("services");
        public IMongoCollection<BookingPrice> BookingPrices => _database.GetCollection<BookingPrice>("bookingprices");
        public IMongoCollection<CommissionConfig> CommissionConfigs => _database.GetCollection<CommissionConfig>("commissionconfigs");
        public IMongoCollection<RefreshToken> RefreshTokens => _database.GetCollection<RefreshToken>("refreshtokens");
        public IMongoCollection<Package> Packages => _database.GetCollection<Package>("commissionpackages");
        public IMongoCollection<TechnicianSubscription> TechnicianSubscriptions => _database.GetCollection<TechnicianSubscription>("techniciansubscriptions");
        public IMongoCollection<BookingStatusLog> BookingStatusLogs => _database.GetCollection<BookingStatusLog>("bookingstatuslogs");

    }
}
