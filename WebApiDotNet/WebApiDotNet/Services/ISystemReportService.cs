using WebApiDotNet.Models;

namespace WebApiDotNet.Services
{
    public interface ISystemReportService
    {
        Task<List<SystemReport>> GetAllAsync();
        Task<SystemReport?> UpdateStatusAsync(string id, string status);
    }
}