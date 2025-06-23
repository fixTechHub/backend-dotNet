using AutoMapper;
using WebApiDotNet.DTOs;
using WebApiDotNet.Models;
using WebApiDotNet.Repository.IRepository;

namespace WebApiDotNet.Services
{
    public class CouponService : ICouponService
    {
        private readonly ICouponRepository _repository;
        private readonly IMapper _mapper;

        public CouponService(ICouponRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<CouponDto>> GetAllCouponsAsync()
        {
            var coupons = await _repository.GetAllAsync();
            var now = DateTime.UtcNow;
            var tasks = new List<Task>();
            foreach (var coupon in coupons)
            {
                if (coupon.IsActive && coupon.EndDate < now)
                {
                    coupon.IsActive = false;
                    tasks.Add(_repository.UpdateAsync(coupon.Id, coupon));
                }
            }
            if (tasks.Count > 0)
                await Task.WhenAll(tasks);
            return _mapper.Map<List<CouponDto>>(coupons);
        }

        public async Task<List<CouponDto>> GetDeletedCouponsAsync()
        {
            var coupons = await _repository.GetDeletedAsync();
            return _mapper.Map<List<CouponDto>>(coupons);
        }

        public async Task<CouponDto> GetCouponByIdAsync(string id)
        {
            var coupon = await _repository.GetByIdAsync(id);
            var now = DateTime.UtcNow;
            if (coupon != null && coupon.IsActive && coupon.EndDate < now)
            {
                coupon.IsActive = false;
                await _repository.UpdateAsync(coupon.Id, coupon);
            }
            return _mapper.Map<CouponDto>(coupon);
        }

        public async Task CreateCouponAsync(CreateCouponDto dto)
        {
            if (await _repository.ExistsByCodeAsync(dto.Code))
                throw new Exception("Mã coupon đã tồn tại");

            var coupon = _mapper.Map<Coupon>(dto);
            await _repository.CreateAsync(coupon);
        }

        public async Task UpdateCouponAsync(string id, UpdateCouponDto dto)
        {
            if (!await _repository.ExistsAsync(id))
                throw new Exception("Không tìm thấy coupon");

            var coupon = await _repository.GetByIdAsync(id);
            _mapper.Map(dto, coupon);
            await _repository.UpdateAsync(id, coupon);
        }

        public async Task DeleteCouponAsync(string id)
        {
            if (!await _repository.ExistsAsync(id))
                throw new Exception("Không tìm thấy coupon");

            await _repository.DeleteAsync(id);
        }

        public async Task RestoreCouponAsync(string id)
        {
            try
            {
                await _repository.RestoreAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Không tìm thấy coupon đã xóa");
            }
        }
    }
}