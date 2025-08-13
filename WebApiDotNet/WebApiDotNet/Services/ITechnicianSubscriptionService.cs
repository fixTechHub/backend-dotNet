using WebApiDotNet.DTOs;

namespace WebApiDotNet.Services
{
    public interface ITechnicianSubscriptionService
    {
        Task<List<TechnicianSubscriptionDto>> GetAllAsync();
        Task<TechnicianSubscriptionDto?> GetByIdAsync(string id);
        Task<TechnicianSubscriptionDto?> GetByTechnicianIdAsync(string technicianId);
        Task<TechnicianSubscriptionDto> CreateAsync(CreateTechnicianSubscriptionDto createDto);
        Task<TechnicianSubscriptionDto?> UpdateAsync(string id, UpdateTechnicianSubscriptionDto updateDto);
        Task<bool> DeleteAsync(string id);
        
        // Business logic methods
        Task<List<TechnicianSubscriptionDto>> GetActiveSubscriptionsAsync();
        Task<List<TechnicianSubscriptionDto>> GetExpiredSubscriptionsAsync();
        Task<List<TechnicianSubscriptionDto>> GetSubscriptionsByPackageAsync(string packageId);
        Task<int> CountActiveSubscriptionsAsync();
        Task<double> GetTotalRevenueAsync();
        Task<double> GetMonthlyRevenueAsync(int year, int month);
        Task<List<TechnicianSubscriptionDto>> GetSubscriptionsByDateRangeAsync(DateTime startDate, DateTime endDate);
        
        // Payment related
        Task<bool> UpdatePaymentStatusAsync(string id, string paymentStatus, string? transactionId = null);
        Task<bool> CancelSubscriptionAsync(string id, string reason);
        Task<bool> RenewSubscriptionAsync(string id, DateTime newEndDate);
        
        // Summary methods for reporting
        Task<List<TechnicianSubscriptionSummaryDto>> GetSubscriptionsSummaryAsync();
        Task<object> GetRevenueStatisticsAsync(int year);
    }
}