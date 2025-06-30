using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiDotNet.DTOs;
using WebApiDotNet.Models;
using WebApiDotNet.Repository.IRepository;
using AutoMapper;

namespace WebApiDotNet.Services
{
    public class WarrantyService : IWarrantyService
    {
        private readonly IWarrantyRepository _repository;
        private readonly IMapper _mapper;

        public WarrantyService(IWarrantyRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<WarrantyDto>> GetAllAsync()
        {
            var warranties = await _repository.GetAllAsync();
            return _mapper.Map<List<WarrantyDto>>(warranties);
        }

        public async Task<WarrantyDto?> GetByIdAsync(string id)
        {
            var warranty = await _repository.GetByIdAsync(id);
            return warranty == null ? null : _mapper.Map<WarrantyDto>(warranty);
        }

        public async Task<WarrantyDto?> UpdateStatusAsync(string id, string status, bool isReviewedByAdmin)
        {
            var updated = await _repository.UpdateStatusAsync(id, status, isReviewedByAdmin);
            return updated == null ? null : _mapper.Map<WarrantyDto>(updated);
        }
    }
} 