using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiDotNet.Models;
using WebApiDotNet.DTOs;

namespace WebApiDotNet.Repository.IRepository
{
    public interface IWarrantyRepository
    {
        Task<List<Warranty>> GetAllAsync();
        Task<Warranty?> GetByIdAsync(string id);
        Task<Warranty?> UpdateStatusAsync(string id, string status);
        Task<Warranty?> UpdateDetailsAsync(string id, UpdateWarrantyDetailsDto dto);
    }
} 