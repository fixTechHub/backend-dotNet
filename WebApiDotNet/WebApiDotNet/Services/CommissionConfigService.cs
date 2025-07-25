using WebApiDotNet.Models;
using WebApiDotNet.Repository.IRepository;
using AutoMapper;
using WebApiDotNet.DTOs;

namespace WebApiDotNet.Services
{
    public class CommissionConfigService : ICommissionConfigService
    {
        private readonly ICommissionConfigRepository _repository;
        private readonly IMapper _mapper;

        public CommissionConfigService(ICommissionConfigRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<CommissionConfigDto>> GetAllAsync()
        {
            var cfs = await _repository.GetAllAsync();
            return _mapper.Map<List<CommissionConfigDto>>(cfs);
        }

        public async Task<List<CommissionConfigDto>> GetDeletedAsync()
        {
            var cfs = await _repository.GetDeletedAsync();
            return _mapper.Map<List<CommissionConfigDto>>(cfs);
        }

        public async Task<CommissionConfigDto> GetByIdAsync(string id)
        {
            var cf = await _repository.GetByIdAsync(id);
            return cf == null ? null : _mapper.Map<CommissionConfigDto>(cf);
        }

        public async Task<CommissionConfigDto> CreateAsync(CreateCommissionConfigDto dto)
        {
            var config = _mapper.Map<CommissionConfig>(dto);
            config.CreatedAt = DateTime.UtcNow;
            config.UpdatedAt = DateTime.UtcNow;

            var created = await _repository.CreateAsync(config);

            if (config.IsApplied)
            {
                // Sau khi tạo mới xong, nếu là IsApplied = true thì disable tất cả cái khác
                await _repository.DisableAllIsAppliedExceptAsync(created.Id);
            }

            return _mapper.Map<CommissionConfigDto>(created);
        }

        public async Task<CommissionConfigDto> UpdateAsync(string id, UpdateCommissionConfigDto dto)
        {
            var config = await _repository.GetByIdAsync(id);
            if (config == null)
                throw new Exception("Không tìm thấy cấu hình hoa hồng.");

            bool wasApplied = config.IsApplied;
            bool willBeApplied = dto.IsApplied;

            _mapper.Map(dto, config);
            config.UpdatedAt = DateTime.UtcNow;

            if (willBeApplied)
            {
                // Nếu chuyển sang true → disable tất cả các config khác
                await _repository.DisableAllIsAppliedExceptAsync(id);
            }
            else if (wasApplied && !willBeApplied)
            {
                // Nếu bỏ áp dụng bản ghi đang được áp dụng
                var others = await _repository.GetAllAppliedExceptAsync(id);
                if (others == null || !others.Any())
                {
                    throw new Exception("Phải có ít nhất một CommissionConfig đang được áp dụng.");
                }
            }

            var updated = await _repository.UpdateAsync(id, config);
            return _mapper.Map<CommissionConfigDto>(updated);
        }

        public async Task DeleteAsync(string id)
        {
            if (!await _repository.ExistsAsync(id))
                throw new Exception("Không tìm thấy cấu hình hoa hồng");
            await _repository.DeleteAsync(id);
        }

        public async Task RestoreAsync(string id)
        {
            if (!await _repository.ExistsDeletedAsync(id))
                throw new Exception("Không tìm thấy cấu hình đã xóa");

            await _repository.RestoreAsync(id);
        }

        public async Task<bool> ExistsAsync(string id)
        {
            return await _repository.ExistsAsync(id);
        }
        
    }
}