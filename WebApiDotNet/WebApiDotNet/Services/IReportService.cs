using WebApiDotNet.Models;
using WebApiDotNet.DTOs;

namespace WebApiDotNet.Services
{
    public interface IReportService
    {
        Task<List<ReportDto>> GetAllAsync();
        Task<ReportDto> GetByIdAsync(string id);
        Task<List<ReportDto>> GetByTypeAsync(ReportType type);
        Task<List<ReportDto>> GetByStatusAsync(ReportStatus status);
        Task<Dictionary<string, int>> GetUserReportCountsAsync();
        Task<int> GetUserReportCountAsync(string userId);
        Task<Dictionary<string, int>> GetUserReportCountsByTypeAsync(ReportType type);
        Task<ReportDto?> UpdateStatusAsync(string reportId, ReportStatus newStatus, string resolvedBy);
        Task<bool> CheckAndAutoLockUserAsync(string reportedUserId);
    }
}
