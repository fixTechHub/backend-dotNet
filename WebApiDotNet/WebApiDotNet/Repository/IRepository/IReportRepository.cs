using WebApiDotNet.Models;

namespace WebApiDotNet.Repository.IRepository
{
    public interface IReportRepository
    {
        Task<List<Report>> GetAllAsync();
        Task<Report> GetByIdAsync(string id);
    }
}