using AutoMapper;
using WebApiDotNet.DTOs;
using WebApiDotNet.Repository.IRepository;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiDotNet.Models;

namespace WebApiDotNet.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _repository;
        private readonly IMapper _mapper;

        public RoleService(IRoleRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<RoleDto>> GetAllAsync()
        {
            var roles = await _repository.GetAllAsync();
            return _mapper.Map<List<RoleDto>>(roles);
        }

        public async Task<RoleDto?> GetByIdAsync(string id)
        {
            var role = await _repository.GetByIdAsync(id);
            return role == null ? null : _mapper.Map<RoleDto>(role);
        }
    }
} 