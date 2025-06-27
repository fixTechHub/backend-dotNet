using WebApiDotNet.DTOs;
using WebApiDotNet.Models;

namespace WebApiDotNet.Services
{
    public interface ISystemReportService
    {
        Task<List<SystemReport>> GetAllAsync();
        Task<SystemReportDto?> UpdateStatusAsync(string id, string status);
    }
}