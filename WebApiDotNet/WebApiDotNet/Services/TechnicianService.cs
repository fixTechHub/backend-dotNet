using AutoMapper;
using WebApiDotNet.DTOs;
using WebApiDotNet.Models;
using WebApiDotNet.Repository.IRepository;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace WebApiDotNet.Services
{
    public class TechnicianService : ITechnicianService
    {
        private readonly ITechnicianRepository _repository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public TechnicianService(ITechnicianRepository repository, IUserRepository userRepository, IMapper mapper)
        {
            _repository = repository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<List<TechnicianDto>> GetAllAsync()
        {
            var technicians = await _repository.GetAllAsync();
            var technicianDtos = new List<TechnicianDto>();
            foreach (var technician in technicians)
            {
                var user = await _userRepository.GetByIdAsync(technician.UserId);
                var dto = _mapper.Map<TechnicianDto>(technician);
                if (user != null)
                {
                    dto.FullName = user.FullName;
                    dto.Email = user.Email;
                    dto.Phone = user.Phone;
                }
                technicianDtos.Add(dto);
            }
            return technicianDtos;
        }

        public async Task<TechnicianDto?> GetByIdAsync(string id)
        {
            var technician = await _repository.GetByIdAsync(id);
            return technician == null ? null : _mapper.Map<TechnicianDto>(technician);
        }

        public async Task<TechnicianDto?> UpdateStatusAsync(string id, string status, string? note = null)
        {
            var technician = await _repository.GetByIdAsync(id);
            if (technician == null)
                return null;
            if (technician.Status != TechnicianStatus.PENDING)
                throw new InvalidOperationException("Chỉ được duyệt kỹ thuật viên khi trạng thái là PENDING!");
            var updated = await _repository.UpdateStatusAsync(id, status, note);
            return updated == null ? null : _mapper.Map<TechnicianDto>(updated);
        }

        public async Task<int> CountByMonthAsync(int year, int month)
        {
            return await _repository.CountByMonthAsync(year, month);
        }
    }
} 