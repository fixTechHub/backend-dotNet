using WebApiDotNet.DTOs;

namespace WebApiDotNet.Services
{
    public interface ITechnicianService
    {
        Task<List<TechnicianDto>> GetAllAsync();
        Task<TechnicianDto?> GetByIdAsync(string id);
        Task<TechnicianDto?> UpdateStatusAsync(string id, string status, string? note = null);
        Task<int> CountByMonthAsync(int year, int month);
    }
} 