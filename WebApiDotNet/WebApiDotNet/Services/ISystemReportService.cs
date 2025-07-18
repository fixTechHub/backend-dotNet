using WebApiDotNet.DTOs;
using WebApiDotNet.Models;

namespace WebApiDotNet.Services
{
    public interface ISystemReportService
    {
        Task<List<SystemReportDto>> GetAllAsync();
        Task<SystemReportDto?> UpdateStatusAsync(string id, string status, string? resolutionNote = null, string? resolvedBy = null);
    }
}