using MongoDB.Driver;
using WebApiDotNet.Models;
using WebApiDotNet.Repository.IRepository;
using WebApiDotNet.Data;

namespace WebApiDotNet.Repository
{
    public class TechnicianSubscriptionRepository : ITechnicianSubscriptionRepository
    {
        private readonly IMongoCollection<TechnicianSubscription> _collection;
        private readonly IMongoCollection<Technician> _technicianCollection;
        private readonly IMongoCollection<Package> _packageCollection;
        private readonly IMongoCollection<User> _userCollection;

        public TechnicianSubscriptionRepository(MongoDbContext context)
        {
            _collection = context.TechnicianSubscriptions;
            _technicianCollection = context.Technicians;
            _packageCollection = context.Packages;
            _userCollection = context.Users;
        }

        public async Task<List<TechnicianSubscription>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<TechnicianSubscription?> GetByIdAsync(string id)
        {
            return await _collection.Find(s => s.Id == id).FirstOrDefaultAsync();
        }

        public async Task<TechnicianSubscription?> GetByTechnicianIdAsync(string technicianId)
        {
            return await _collection.Find(s => s.TechnicianId == technicianId).FirstOrDefaultAsync();
        }

        public async Task<TechnicianSubscription> CreateAsync(TechnicianSubscription subscription)
        {
            subscription.CreatedAt = DateTime.UtcNow;
            subscription.UpdatedAt = DateTime.UtcNow;
            await _collection.InsertOneAsync(subscription);
            return subscription;
        }

        public async Task<TechnicianSubscription?> UpdateAsync(string id, TechnicianSubscription subscription)
        {
            subscription.UpdatedAt = DateTime.UtcNow;
            var filter = Builders<TechnicianSubscription>.Filter.Eq(s => s.Id, id);
            
            var update = Builders<TechnicianSubscription>.Update
                .Set(s => s.Status, subscription.Status)
                .Set(s => s.EndDate, subscription.EndDate)
                // Bỏ logic set Amount vì không cần thiết
                .Set(s => s.PaymentStatus, subscription.PaymentStatus)
                .Set(s => s.PaymentMethod, subscription.PaymentMethod)
                .Set(s => s.TransactionId, subscription.TransactionId)
                .Set(s => s.AutoRenew, subscription.AutoRenew)
                .Set(s => s.CancelledAt, subscription.CancelledAt)
                .Set(s => s.CancellationReason, subscription.CancellationReason)
                .Set(s => s.UpdatedAt, subscription.UpdatedAt);
            
            var options = new FindOneAndUpdateOptions<TechnicianSubscription> { ReturnDocument = ReturnDocument.After };
            return await _collection.FindOneAndUpdateAsync(filter, update, options);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _collection.DeleteOneAsync(s => s.Id == id);
            return result.DeletedCount > 0;
        }

        public async Task<List<TechnicianSubscription>> GetActiveSubscriptionsAsync()
        {
            var filter = Builders<TechnicianSubscription>.Filter.Eq(s => s.Status, SubscriptionStatus.ACTIVE);
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<List<TechnicianSubscription>> GetExpiredSubscriptionsAsync()
        {
            var filter = Builders<TechnicianSubscription>.Filter.Eq(s => s.Status, SubscriptionStatus.EXPIRED);
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<List<TechnicianSubscription>> GetSubscriptionsByPackageAsync(string packageId)
        {
            var filter = Builders<TechnicianSubscription>.Filter.Eq(s => s.PackageId, packageId);
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<int> CountActiveSubscriptionsAsync()
        {
            var filter = Builders<TechnicianSubscription>.Filter.Eq(s => s.Status, SubscriptionStatus.ACTIVE);
            return (int)await _collection.CountDocumentsAsync(filter);
        }

        public async Task<double> GetTotalRevenueAsync()
        {
            // Tính doanh thu từ tất cả subscriptions (không phân biệt payment status)
            var subscriptions = await _collection.Find(_ => true).ToListAsync();
            
            // Debug logging
            Console.WriteLine($"🔍 Total subscriptions found: {subscriptions.Count}");
            foreach (var sub in subscriptions)
            {
                Console.WriteLine($"📊 Subscription {sub.Id}: Amount = {sub.Amount}, Status = {sub.Status}, PaymentStatus = {sub.PaymentStatus}");
            }
            
            // Tính doanh thu từ paymentHistory.amount thay vì amount
            var totalRevenue = subscriptions.Sum(s => 
                s.PaymentHistory?.Sum(ph => ph.Amount) ?? 0
            );
            
            Console.WriteLine($"💰 Total Revenue calculated: {totalRevenue}");
            
            return totalRevenue;
        }

        public async Task<double> GetMonthlyRevenueAsync(int year, int month)
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1);
            
            // Tính doanh thu theo tháng từ tất cả subscriptions (không phân biệt payment status)
            var filter = Builders<TechnicianSubscription>.Filter.And(
                Builders<TechnicianSubscription>.Filter.Gte(s => s.CreatedAt, startDate),
                Builders<TechnicianSubscription>.Filter.Lt(s => s.CreatedAt, endDate)
            );
            
            var subscriptions = await _collection.Find(filter).ToListAsync();
            
            // Tính doanh thu từ paymentHistory.amount thay vì amount
            return subscriptions.Sum(s => 
                s.PaymentHistory?.Sum(ph => ph.Amount) ?? 0
            );
        }

        public async Task<List<TechnicianSubscription>> GetSubscriptionsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var filter = Builders<TechnicianSubscription>.Filter.And(
                Builders<TechnicianSubscription>.Filter.Gte(s => s.CreatedAt, startDate),
                Builders<TechnicianSubscription>.Filter.Lt(s => s.CreatedAt, endDate)
            );
            
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<List<TechnicianSubscription>> GetSubscriptionsByYearAsync(int year)
        {
            var startDate = new DateTime(year, 1, 1);
            var endDate = startDate.AddYears(1);
            
            var filter = Builders<TechnicianSubscription>.Filter.And(
                Builders<TechnicianSubscription>.Filter.Gte(s => s.CreatedAt, startDate),
                Builders<TechnicianSubscription>.Filter.Lt(s => s.CreatedAt, endDate)
            );
            
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<bool> UpdatePaymentStatusAsync(string id, string paymentStatus, string? transactionId = null)
        {
            var filter = Builders<TechnicianSubscription>.Filter.Eq(s => s.Id, id);
            var update = Builders<TechnicianSubscription>.Update
                .Set(s => s.PaymentStatus, (SubscriptionPaymentStatus)Enum.Parse(typeof(SubscriptionPaymentStatus), paymentStatus))
                .Set(s => s.UpdatedAt, DateTime.UtcNow);
            
            if (!string.IsNullOrEmpty(transactionId))
            {
                update = update.Set(s => s.TransactionId, transactionId);
            }
            
            var result = await _collection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> CancelSubscriptionAsync(string id, string reason)
        {
            var filter = Builders<TechnicianSubscription>.Filter.Eq(s => s.Id, id);
            var update = Builders<TechnicianSubscription>.Update
                .Set(s => s.Status, SubscriptionStatus.CANCELLED)
                .Set(s => s.CancelledAt, DateTime.UtcNow)
                .Set(s => s.CancellationReason, reason)
                .Set(s => s.UpdatedAt, DateTime.UtcNow);
            
            var result = await _collection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> RenewSubscriptionAsync(string id, DateTime newEndDate)
        {
            var filter = Builders<TechnicianSubscription>.Filter.Eq(s => s.Id, id);
            var update = Builders<TechnicianSubscription>.Update
                .Set(s => s.EndDate, newEndDate)
                .Set(s => s.Status, SubscriptionStatus.ACTIVE)
                .Set(s => s.UpdatedAt, DateTime.UtcNow);
            
            var result = await _collection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        public async Task<Technician?> GetTechnicianByIdAsync(string technicianId)
        {
            var filter = Builders<Technician>.Filter.Eq(t => t.Id, technicianId);
            return await _technicianCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<Package?> GetPackageByIdAsync(string packageId)
        {
            var filter = Builders<Package>.Filter.Eq(p => p.Id, packageId);
            return await _packageCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<User?> GetUserByIdAsync(string userId)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, userId);
            return await _userCollection.Find(filter).FirstOrDefaultAsync();
        }
    }
}