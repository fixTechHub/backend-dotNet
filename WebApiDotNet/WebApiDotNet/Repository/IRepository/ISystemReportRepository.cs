using WebApiDotNet.Models;

namespace WebApiDotNet.Repository.IRepository
{
    public interface ISystemReportRepository
    {
        Task<List<SystemReport>> GetAllAsync();
        Task<SystemReport?> UpdateStatusAsync(string id, string status, string? resolutionNote = null, string? resolvedBy = null);
    }
}