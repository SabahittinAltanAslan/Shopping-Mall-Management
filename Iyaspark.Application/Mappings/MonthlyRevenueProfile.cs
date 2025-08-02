using AutoMapper;
using Iyaspark.Application.Modules.MonthlyRevenues.DTOs;
using Iyaspark.Domain.Entities;

public class MonthlyRevenueProfile : Profile
{
    public MonthlyRevenueProfile()
    {
        CreateMap<MonthlyRevenue, MonthlyRevenueDto>()
            .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Tenant.CompanyName))
            .ForMember(dest => dest.TaxNumber, opt => opt.MapFrom(src => src.Tenant.TaxNumber));
        CreateMap<CreateMonthlyRevenueDto, MonthlyRevenue>();

        CreateMap<UpdateMonthlyRevenueDto, MonthlyRevenue>();
    }
}