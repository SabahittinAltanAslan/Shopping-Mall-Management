using AutoMapper;
using Iyaspark.Application.DTOs.Contract;
using Iyaspark.Application.DTOs.Tenant;
using Iyaspark.Application.Modules.Contracts.DTOs;
using Iyaspark.Application.Modules.Guarantees.DTOs;
using Iyaspark.Application.Modules.MonthlyRevenues.DTOs;
using Iyaspark.Domain.Entities;

namespace Iyaspark.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Tenant
            CreateMap<CreateTenantDto, Tenant>();
            CreateMap<Tenant, TenantDto>();

            // Contract
            // Contract
            CreateMap<CreateContractDto, Contract>();
            CreateMap<UpdateContractDto, Contract>();
            CreateMap<Contract, ContractDto>()
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Tenant.CompanyName));
            CreateMap<UpdateContractDto, Contract>()
                .ForMember(dest => dest.PdfFilePath, opt => opt.MapFrom(src => src.ContractFilePath));
            CreateMap<CreateContractDto, Contract>()
                .ForMember(dest => dest.PdfFilePath, opt => opt.MapFrom(src => src.ContractFilePath));





            CreateMap<CreateGuaranteeDto, Guarantee>();
            CreateMap<Guarantee, GuaranteeDto>()
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Tenant.CompanyName));

            CreateMap<Tenant, MonthlyRentDto>()
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.CompanyName))
                .ForMember(dest => dest.TaxNumber, opt => opt.MapFrom(src => src.TaxNumber))
                .ForMember(dest => dest.FloorLabel, opt => opt.MapFrom(src => src.FloorLabel))
                .ForMember(dest => dest.FacadeDirection, opt => opt.MapFrom(src => src.FacadeDirection.ToString()))
                .ForMember(dest => dest.Sector, opt => opt.MapFrom(src => src.Sector))
                .ForMember(dest => dest.TenantType, opt => opt.MapFrom(src => src.TenantType.ToString()))
                .ForMember(dest => dest.RentType, opt => opt.Ignore())
                .ForMember(dest => dest.RentAmount, opt => opt.Ignore())
                .ForMember(dest => dest.Month, opt => opt.Ignore())
                .ForMember(dest => dest.Year, opt => opt.Ignore());



        }
    }
}
