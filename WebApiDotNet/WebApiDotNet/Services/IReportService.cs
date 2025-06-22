using WebApiDotNet.Models;

namespace WebApiDotNet.Services
{
    public interface IReportService
    {
        Task<List<Report>> GetAllAsync();
        Task<Report> GetByIdAsync(string id);
    }
}
