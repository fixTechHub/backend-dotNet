using AutoMapper;
using WebApiDotNet.DTOs;
using WebApiDotNet.Models;
using System;

namespace WebApiDotNet.Data
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Entity -> DTO
            CreateMap<Coupon, CouponDto>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<CouponUsage, CouponUsageDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<Address, AddressDto>().ReverseMap();
            CreateMap<Booking, BookingDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src => src.PaymentStatus.ToString()));            
            CreateMap<BookingLocation, BookingLocationDto>().ReverseMap();
            CreateMap<GeoJson, GeoJsonDto>().ReverseMap();
            CreateMap<Technician, TechnicianDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.Availability, opt => opt.MapFrom(src => src.Availability.ToString()))
                .ReverseMap()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<TechnicianStatus>(src.Status.ToString())))
                .ForMember(dest => dest.Availability, opt => opt.MapFrom(src => Enum.Parse<TechnicianAvailability>(src.Availability.ToString())));
            CreateMap<GeoJsonPoint, GeoJsonPointDto>().ReverseMap();
            CreateMap<BankAccount, BankAccountDto>().ReverseMap();
            CreateMap<TechnicianRates, TechnicianRatesDto>().ReverseMap();
            CreateMap<LaborTiers, LaborTiersDto>().ReverseMap();
            CreateMap<SystemReport, SystemReportDto>()
                .ForMember(dest => dest.Tag, opt => opt.MapFrom(src => src.Tag.ToString()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));            
            CreateMap<Role, RoleDto>().ReverseMap();
            CreateMap<CommissionConfig, CommissionConfigDto>().ReverseMap();
            CreateMap<Warranty, WarrantyDto>().ReverseMap();
            CreateMap<Service, ServiceDto>()
                .ForMember(dest => dest.ServiceType, opt => opt.MapFrom(src => src.ServiceType.ToString()))
                .ReverseMap()
                .ForMember(dest => dest.ServiceType, opt => opt.MapFrom(src => Enum.Parse<ServiceType>(src.ServiceType)));
            CreateMap<Schedule, ScheduleDto>().ReverseMap();
            CreateMap<EstimatedMarketPrice, EstimatedMarketPriceDto>().ReverseMap();
            CreateMap<Quote, QuoteDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));            
            CreateMap<QuoteItem, QuoteItemDto>().ReverseMap();
            CreateMap<Report, ReportDto>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
            
            // DTO -> Entity
            CreateMap<CreateCommissionConfigDto, CommissionConfig>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
            CreateMap<UpdateCommissionConfigDto, CommissionConfig>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
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
            CreateMap<UpdateUserDto, User>().ReverseMap();          
            CreateMap<CreateCategoryDto, Category>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
            CreateMap<UpdateCategoryDto, Category>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
            CreateMap<CreateServiceDto, Service>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.ServiceType, opt => opt.MapFrom(src => Enum.Parse<ServiceType>(src.ServiceType, true)));
            CreateMap<UpdateServiceDto, Service>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.ServiceType, opt => opt.MapFrom(src => Enum.Parse<ServiceType>(src.ServiceType, true)));
            CreateMap<ReportDto, Report>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => Enum.Parse<ReportType>(src.Type, true)))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<ReportStatus>(src.Status, true)));
            CreateMap<BookingDto, Booking>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<BookingStatus>(src.Status, true)))
                .ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src => Enum.Parse<PaymentStatus>(src.PaymentStatus, true)));
            CreateMap<QuoteDto, Quote>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<QuoteStatus>(src.Status, true)));
            CreateMap<SystemReportDto, SystemReport>()
                .ForMember(dest => dest.Tag, opt => opt.MapFrom(src => Enum.Parse<SystemReportTag>(src.Tag, true)))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<SystemReportStatus>(src.Status, true)));
        }
    }
}
