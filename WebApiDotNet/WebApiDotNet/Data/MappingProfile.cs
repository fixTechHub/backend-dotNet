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
            CreateMap<Coupon, CouponDto>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();
            // DTO -> Entity
            CreateMap<CreateCouponDto, Coupon>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UsedCount, opt => opt.Ignore())
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type)); ;
            CreateMap<UpdateCouponDto, Coupon>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UsedCount, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type)); ;
            CreateMap<CouponUsage, CouponUsageDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<Address, AddressDto>().ReverseMap();
            CreateMap<Booking, BookingDto>().ReverseMap();
            CreateMap<BookingLocation, BookingLocationDto>().ReverseMap();
            CreateMap<GeoJson, GeoJsonDto>().ReverseMap();
            CreateMap<Technician, TechnicianDto>().ReverseMap();
            CreateMap<GeoJsonPoint, GeoJsonPointDto>().ReverseMap();
            CreateMap<BankAccount, BankAccountDto>().ReverseMap();
            CreateMap<TechnicianRates, TechnicianRatesDto>().ReverseMap();
            CreateMap<LaborTiers, LaborTiersDto>().ReverseMap();
            CreateMap<UpdateUserDto, User>().ReverseMap();
            CreateMap<Role, RoleDto>().ReverseMap();
            CreateMap<SystemReport, SystemReportDto>().ReverseMap();            
            CreateMap<CreateCategoryDto, Category>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
            CreateMap<UpdateCategoryDto, Category>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
            CreateMap<Warranty, WarrantyDto>().ReverseMap();
            CreateMap<Service, ServiceDto>()
                .ForMember(dest => dest.ServiceType, opt => opt.MapFrom(src => src.ServiceType.ToString()))
                .ReverseMap()
                .ForMember(dest => dest.ServiceType, opt => opt.MapFrom(src => Enum.Parse<ServiceType>(src.ServiceType)));
            CreateMap<CreateServiceDto, Service>().ReverseMap();
            CreateMap<UpdateServiceDto, Service>().ReverseMap();
            CreateMap<Schedule, ScheduleDto>().ReverseMap();
            CreateMap<EstimatedMarketPrice, EstimatedMarketPriceDto>().ReverseMap();
        }
    }
}
