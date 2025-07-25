using WebApiDotNet.Models;

namespace WebApiDotNet.Repository.IRepository
{
    public interface ICommissionConfigRepository
    {
        Task<List<CommissionConfig>> GetAllAsync();
        Task<List<CommissionConfig>> GetDeletedAsync();
        Task<CommissionConfig> GetByIdAsync(string id);
        Task<CommissionConfig> CreateAsync(CommissionConfig config);
        Task<CommissionConfig> UpdateAsync(string id, CommissionConfig config);
        Task DeleteAsync(string id);
        Task RestoreAsync(string id);
        Task<bool> ExistsAsync(string id);
        Task<bool> ExistsDeletedAsync(string id);
        Task DisableAllIsAppliedExceptAsync(string exceptId);
        Task<List<CommissionConfig>> GetAllAppliedExceptAsync(string exceptId);
        Task<CommissionConfig?> GetAppliedConfigAsync();
    }
}