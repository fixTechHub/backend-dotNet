using WebApiDotNet.Models;
using WebApiDotNet.Repository.IRepository;
using AutoMapper;
using WebApiDotNet.DTOs;

namespace WebApiDotNet.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _repository;
        private readonly IMapper _mapper;

        public ReportService(IReportRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<ReportDto>> GetAllAsync()
        {
            var reports = await _repository.GetAllAsync();
            return _mapper.Map<List<ReportDto>>(reports);
        }

        public async Task<ReportDto> GetByIdAsync(string id)
        {
            var r = await _repository.GetByIdAsync(id);
            return r == null ? null : _mapper.Map<ReportDto>(r);
        }
    }
}