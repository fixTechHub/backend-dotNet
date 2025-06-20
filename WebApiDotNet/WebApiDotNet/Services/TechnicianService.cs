using AutoMapper;
using WebApiDotNet.DTOs;
using WebApiDotNet.Models;
using WebApiDotNet.Repository.IRepository;

namespace WebApiDotNet.Services
{
    public class TechnicianService : ITechnicianService
    {
        private readonly ITechnicianRepository _repository;
        private readonly IMapper _mapper;

        public TechnicianService(ITechnicianRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<TechnicianDto>> GetAllAsync()
        {
            var technicians = await _repository.GetAllAsync();
            return _mapper.Map<List<TechnicianDto>>(technicians);
        }

        public async Task<TechnicianDto?> GetByIdAsync(string id)
        {
            var technician = await _repository.GetByIdAsync(id);
            return technician == null ? null : _mapper.Map<TechnicianDto>(technician);
        }

        public async Task<TechnicianDto?> UpdateStatusAsync(string id, string status, string? note = null)
        {
            var updated = await _repository.UpdateStatusAsync(id, status, note);
            return updated == null ? null : _mapper.Map<TechnicianDto>(updated);
        }
    }
} 