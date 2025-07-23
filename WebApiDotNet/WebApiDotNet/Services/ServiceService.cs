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
        public async Task<List<ServiceDto>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync();
            return _mapper.Map<List<ServiceDto>>(list);
        }
        public async Task<List<ServiceDto>> GetDeletedAsync()
        {
            var list = await _repo.GetDeletedAsync();
            return _mapper.Map<List<ServiceDto>>(list);
        }
        public async Task<ServiceDto> GetByIdAsync(string id)
        {
            var s = await _repo.GetByIdAsync(id);
            return s == null ? null : _mapper.Map<ServiceDto>(s);
        }
        public async Task<ServiceDto> CreateAsync(CreateServiceDto dto)
        {
            // Kiểm tra trùng tên, icon
            if (await _repo.ExistsByNameAsync(dto.ServiceName))
                throw new Exception("Tên dịch vụ đã tồn tại");
            if (await _repo.ExistsByIconAsync(dto.Icon))
                throw new Exception("Icon đã tồn tại");
            // Logic nghiệp vụ: FIXED thì không nhập giá, COMPLEX thì bắt buộc nhập giá min/max
            if (dto.ServiceType == "FIXED")
            {
                if (dto.EstimatedMarketPrice != null && (dto.EstimatedMarketPrice.Min != null || dto.EstimatedMarketPrice.Max != null))
                {
                    throw new Exception("Dịch vụ loại FIXED không được nhập giá thị trường!");
                }
            }
            else if (dto.ServiceType == "COMPLEX")
            {
                if (dto.EstimatedMarketPrice == null || dto.EstimatedMarketPrice.Min == null || dto.EstimatedMarketPrice.Max == null)
                {
                    throw new Exception("Dịch vụ loại COMPLEX phải nhập đủ giá min và max!");
                }
                if (dto.EstimatedMarketPrice.Min < 1000 || dto.EstimatedMarketPrice.Max < 1000)
                {
                    throw new Exception("Giá min và max phải lớn hơn hoặc bằng 1,000 VND!");
                }
                if (dto.EstimatedMarketPrice.Min > dto.EstimatedMarketPrice.Max)
                {
                    throw new Exception("Giá min không được lớn hơn giá max!");
                }
            }
            var service = _mapper.Map<Service>(dto);
            service.CreatedAt = DateTime.UtcNow;
            service.UpdatedAt = DateTime.UtcNow;
            var created = await _repo.CreateAsync(service);
            return _mapper.Map<ServiceDto>(created);
        }
        public async Task<ServiceDto> UpdateAsync(string id, UpdateServiceDto dto)
        {
            var service = await _repo.GetByIdAsync(id);
            if (service == null) throw new Exception("Không tìm thấy dịch vụ");
            // Kiểm tra trùng tên, icon (trừ chính bản ghi đang sửa)
            if (service.ServiceName != dto.ServiceName && await _repo.ExistsByNameAsync(dto.ServiceName))
                throw new Exception("Tên dịch vụ đã tồn tại");
            if (service.Icon != dto.Icon && await _repo.ExistsByIconAsync(dto.Icon))
                throw new Exception("Icon đã tồn tại");
            // Logic nghiệp vụ: FIXED thì không nhập giá, COMPLEX thì bắt buộc nhập giá min/max
            if (dto.ServiceType == "FIXED")
            {
                if (dto.EstimatedMarketPrice != null && (dto.EstimatedMarketPrice.Min != null || dto.EstimatedMarketPrice.Max != null))
                {
                    throw new Exception("Dịch vụ loại FIXED không được nhập giá thị trường!");
                }
            }
            else if (dto.ServiceType == "COMPLEX")
            {
                if (dto.EstimatedMarketPrice == null || dto.EstimatedMarketPrice.Min == null || dto.EstimatedMarketPrice.Max == null)
                {
                    throw new Exception("Dịch vụ loại COMPLEX phải nhập đủ giá min và max!");
                }
                if (dto.EstimatedMarketPrice.Min < 1000 || dto.EstimatedMarketPrice.Max < 1000)
                {
                    throw new Exception("Giá min và max phải lớn hơn hoặc bằng 1,000 VND!");
                }
                if (dto.EstimatedMarketPrice.Min > dto.EstimatedMarketPrice.Max)
                {
                    throw new Exception("Giá min không được lớn hơn giá max!");
                }
            }
            _mapper.Map(dto, service);
            var updated = await _repo.UpdateAsync(id, service);
            return _mapper.Map<ServiceDto>(updated);
        }
        public async Task DeleteAsync(string id)
        {
            if (!await _repo.ExistsAsync(id))
                throw new Exception("Không tìm thấy dịch vụ");
            await _repo.DeleteAsync(id);
        }
        public async Task RestoreAsync(string id)
        {
            if (!await _repo.ExistsDeletedAsync(id))
                throw new Exception("Không tìm thấy dịch vụ đã xóa");
            await _repo.RestoreAsync(id);
        }
        public async Task<bool> ExistsAsync(string id)
        {
            return await _repo.ExistsAsync(id);
        }
    }
} 