using WebApiDotNet.Models;
using WebApiDotNet.Repository.IRepository;

namespace WebApiDotNet.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _repository;

        public ReportService(IReportRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Report>> GetAllAsync()
        {
            var reports = await _repository.GetAllAsync();
            foreach (var r in reports)
            {
                if (string.IsNullOrEmpty(r.Type)) r.Type = "REPORT";
                if (string.IsNullOrEmpty(r.Status)) r.Status = "PENDING";
                if (r.Penalty == null) r.Penalty = null;
            }
            return reports;
        }

        public async Task<Report> GetByIdAsync(string id)
        {
            var r = await _repository.GetByIdAsync(id);
            if (r != null)
            {
                if (string.IsNullOrEmpty(r.Type)) r.Type = "REPORT";
                if (string.IsNullOrEmpty(r.Status)) r.Status = "PENDING";
                if (r.Penalty == null) r.Penalty = null;
            }
            return r;
        }
    }
}