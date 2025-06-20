using WebApiDotNet.Models;

namespace WebApiDotNet.Repository.IRepository
{
    public interface ITechnicianRepository
    {
        Task<List<Technician>> GetAllAsync();
        Task<Technician?> GetByIdAsync(string id);
        Task<Technician?> UpdateStatusAsync(string id, string status, string? note = null);
    }
} 