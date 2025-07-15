using AutoMapper;
using WebApiDotNet.DTOs;
using WebApiDotNet.Models;
using WebApiDotNet.Repository.IRepository;

namespace WebApiDotNet.Services
{
    public class SystemReportService : ISystemReportService
    {
        private readonly ISystemReportRepository _repository;
        private readonly IMapper _mapper;
        public SystemReportService(ISystemReportRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<SystemReport>> GetAllAsync()
        {
            var reports = await _repository.GetAllAsync();
            foreach (var r in reports)
            {
                if (string.IsNullOrEmpty(r.Status)) r.Status = "PENDING";
                if (r.ResolvedBy == null) r.ResolvedBy = null;
                if (r.ResolutionNote == null) r.ResolutionNote = null;
                if (r.ResolvedAt == null) r.ResolvedAt = null;
            }
            return reports;
        }

        public async Task<SystemReportDto?> UpdateStatusAsync(string id, string status)
        {
            var updated = await _repository.UpdateStatusAsync(id, status);
            if (updated != null)
            {
                if (string.IsNullOrEmpty(updated.Status)) updated.Status = "PENDING";
                if (updated.ResolvedBy == null) updated.ResolvedBy = null;
                if (updated.ResolutionNote == null) updated.ResolutionNote = null;
                if (updated.ResolvedAt == null) updated.ResolvedAt = null;
            }
            return updated == null ? null : _mapper.Map<SystemReportDto>(updated);
        }
    }
}