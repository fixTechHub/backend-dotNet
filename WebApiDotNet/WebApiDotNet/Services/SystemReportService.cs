using WebApiDotNet.Models;
using WebApiDotNet.Repository.IRepository;

namespace WebApiDotNet.Services
{
    public class SystemReportService : ISystemReportService
    {
        private readonly ISystemReportRepository _repository;

        public SystemReportService(ISystemReportRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<SystemReport>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<SystemReport?> UpdateStatusAsync(string id, string status)
        {
            return await _repository.UpdateStatusAsync(id, status);
        }
    }
}