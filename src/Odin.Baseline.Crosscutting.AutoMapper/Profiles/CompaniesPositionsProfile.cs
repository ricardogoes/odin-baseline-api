using AutoMapper;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.ViewModels.CompaniesPositions;

namespace Odin.Baseline.Crosscutting.AutoMapper.Profiles
{
    public class CompaniesPositionsProfile : Profile
    {
        public CompaniesPositionsProfile() 
        {
            CreateMap<CompanyPosition, CompanyPositionToInsert>();
            CreateMap<CompanyPositionToInsert, CompanyPosition>();

            CreateMap<CompanyPosition, CompanyPositionToUpdate>();
            CreateMap<CompanyPositionToUpdate, CompanyPosition>();

            CreateMap<CompanyPosition, CompanyPositionToQuery>()
                .ForMember(x => x.PositionId, o => o.MapFrom(t => t.PositionId))
                .ForMember(x => x.CustomerId, o => o.MapFrom(t => t.CustomerId))
                .ForMember(x => x.CustomerName, o => o.MapFrom(t => t.Customer.Name))
                .ForMember(x => x.Name, o => o.MapFrom(t => t.Name))
                .ForMember(x => x.BaseSalary, o => o.MapFrom(t => t.BaseSalary))
                .ForMember(x => x.IsActive, o => o.MapFrom(t => t.IsActive))
                .ForMember(x => x.CreatedBy, o => o.MapFrom(t => t.CreatedBy))
                .ForMember(x => x.CreatedAt, o => o.MapFrom(t => t.CreatedAt))
                .ForMember(x => x.LastUpdatedBy, o => o.MapFrom(t => t.LastUpdatedBy))
                .ForMember(x => x.LastUpdatedAt, o => o.MapFrom(t => t.LastUpdatedAt))
                .ReverseMap();
        }
    }
}
