using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiDotNet.DTOs;

namespace WebApiDotNet.Services
{
    public interface IWarrantyService
    {
        Task<List<WarrantyDto>> GetAllAsync();
        Task<WarrantyDto?> GetByIdAsync(string id);
        Task<WarrantyDto?> UpdateStatusAsync(string id, string status, bool isReviewedByAdmin);
        Task<WarrantyDto?> UpdateDetailsAsync(string id, UpdateWarrantyDetailsDto dto);
    }
} 