using WebApiDotNet.Models;
using WebApiDotNet.DTOs;

namespace WebApiDotNet.Services
{
    public interface IReportService
    {
        Task<List<ReportDto>> GetAllAsync();
        Task<ReportDto> GetByIdAsync(string id);
    }
}
