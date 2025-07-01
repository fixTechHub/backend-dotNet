using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using WebApiDotNet.DTOs;
using WebApiDotNet.Models;
using WebApiDotNet.Repository.IRepository;

namespace WebApiDotNet.Services
{
    public class ServiceService : IServiceService
    {
        private readonly IServiceRepository _repo;
        private readonly IMapper _mapper;
        public ServiceService(IServiceRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        public async Task<IEnumerable<ServiceDto>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<ServiceDto>>(list);
        }
        public async Task<ServiceDto> GetByIdAsync(string id)
        {
            var s = await _repo.GetByIdAsync(id);
            return s == null ? null : _mapper.Map<ServiceDto>(s);
        }
    }
} 