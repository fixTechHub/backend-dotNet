using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiDotNet.DTOs;

namespace WebApiDotNet.Services
{
    public interface IServiceService
    {
        Task<List<ServiceDto>> GetAllAsync();
        Task<List<ServiceDto>> GetDeletedAsync();
        Task<ServiceDto> GetByIdAsync(string id);
        Task<ServiceDto> CreateAsync(CreateServiceDto dto);
        Task<ServiceDto> UpdateAsync(string id, UpdateServiceDto dto);
        Task DeleteAsync(string id);
        Task RestoreAsync(string id);
        Task<bool> ExistsAsync(string id);
    }
} 