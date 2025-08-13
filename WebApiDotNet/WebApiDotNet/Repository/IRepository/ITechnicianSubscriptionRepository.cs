using WebApiDotNet.Models;

namespace WebApiDotNet.Repository.IRepository
{
    public interface ITechnicianSubscriptionRepository
    {
        Task<List<TechnicianSubscription>> GetAllAsync();
        Task<TechnicianSubscription?> GetByIdAsync(string id);
        Task<TechnicianSubscription?> GetByTechnicianIdAsync(string technicianId);
        Task<TechnicianSubscription> CreateAsync(TechnicianSubscription subscription);
        Task<TechnicianSubscription?> UpdateAsync(string id, TechnicianSubscription subscription);
        Task<bool> DeleteAsync(string id);
        
        // Business logic methods
        Task<List<TechnicianSubscription>> GetActiveSubscriptionsAsync();
        Task<List<TechnicianSubscription>> GetExpiredSubscriptionsAsync();
        Task<List<TechnicianSubscription>> GetSubscriptionsByPackageAsync(string packageId);
        Task<int> CountActiveSubscriptionsAsync();
        Task<double> GetTotalRevenueAsync();
        Task<double> GetMonthlyRevenueAsync(int year, int month);
        Task<List<TechnicianSubscription>> GetSubscriptionsByDateRangeAsync(DateTime startDate, DateTime endDate);
        
        // Payment related
        Task<bool> UpdatePaymentStatusAsync(string id, string paymentStatus, string? transactionId = null);
        Task<bool> CancelSubscriptionAsync(string id, string reason);
        Task<bool> RenewSubscriptionAsync(string id, DateTime newEndDate);
        
        // Methods to get related data
        Task<Technician?> GetTechnicianByIdAsync(string technicianId);
        Task<Package?> GetPackageByIdAsync(string packageId);
        Task<User?> GetUserByIdAsync(string userId);
    }
}