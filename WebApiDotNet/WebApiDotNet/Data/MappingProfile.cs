using AutoMapper;
using WebApiDotNet.DTOs;
using WebApiDotNet.Models;

namespace WebApiDotNet.Data
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Entity -> DTO
            CreateMap<Coupon, CouponDto>();

            // DTO -> Entity
            CreateMap<CreateCouponDto, Coupon>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UsedCount, opt => opt.Ignore());
            CreateMap<UpdateCouponDto, Coupon>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UsedCount, opt => opt.Ignore());    
        }
    }
}
