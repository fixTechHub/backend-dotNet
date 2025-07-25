using WebApiDotNet.Models;
using WebApiDotNet.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApiDotNet.Services
{
    public interface ICommissionConfigService
    {
        Task<List<CommissionConfigDto>> GetAllAsync();
        Task<List<CommissionConfigDto>> GetDeletedAsync();
        Task<CommissionConfigDto> GetByIdAsync(string id);
        Task<CommissionConfigDto> CreateAsync(CreateCommissionConfigDto dto);
        Task<CommissionConfigDto> UpdateAsync(string id, UpdateCommissionConfigDto dto);
        Task DeleteAsync(string id);
        Task RestoreAsync(string id);
        Task<bool> ExistsAsync(string id);
    }
}
