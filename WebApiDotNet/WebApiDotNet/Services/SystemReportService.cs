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

        public async Task<List<SystemReportDto>> GetAllAsync()
        {
            var reports = await _repository.GetAllAsync();
            return _mapper.Map<List<SystemReportDto>>(reports);
        }

        public async Task<SystemReportDto?> UpdateStatusAsync(string id, string status, string? resolutionNote = null, string? resolvedBy = null)
        {
            var updated = await _repository.UpdateStatusAsync(id, status, resolutionNote, resolvedBy);
            return updated == null ? null : _mapper.Map<SystemReportDto>(updated);
        }
    }
}