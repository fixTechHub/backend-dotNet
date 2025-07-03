using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiDotNet.DTOs;

namespace WebApiDotNet.Services
{
    public interface IServiceService
    {
        Task<IEnumerable<ServiceDto>> GetAllAsync();
        Task<ServiceDto> GetByIdAsync(string id);
        Task<ServiceDto> CreateAsync(CreateServiceDto dto);
        Task<ServiceDto> UpdateAsync(string id, UpdateServiceDto dto);
        Task<bool> DeleteAsync(string id);
    }
} 