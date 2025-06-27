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
            return await _repository.GetAllAsync();
        }

        public async Task<SystemReportDto?> UpdateStatusAsync(string id, string status)
        {
            var updated = await _repository.UpdateStatusAsync(id, status);
            return updated == null ? null : _mapper.Map<SystemReportDto>(updated);
        }
    }
}