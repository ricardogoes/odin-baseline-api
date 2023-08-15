using AutoMapper;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.ViewModels.Departments;

namespace Odin.Baseline.Crosscutting.AutoMapper.Profiles
{
    public class DepartmentsProfile : Profile
    {
        public DepartmentsProfile() 
        {
            CreateMap<Department, DepartmentToInsert>();
            CreateMap<DepartmentToInsert, Department>();

            CreateMap<Department, DepartmentToUpdate>();
            CreateMap<DepartmentToUpdate, Department>();

            CreateMap<Department, DepartmentToQuery>()
                .ForMember(x => x.DepartmentId, o => o.MapFrom(t => t.DepartmentId))
                .ForMember(x => x.CustomerId, o => o.MapFrom(t => t.CustomerId))
                .ForMember(x => x.CustomerName, o => o.MapFrom(t => t.Customer.Name))
                .ForMember(x => x.Name, o => o.MapFrom(t => t.Name))
                .ForMember(x => x.IsActive, o => o.MapFrom(t => t.IsActive))
                .ForMember(x => x.CreatedBy, o => o.MapFrom(t => t.CreatedBy))
                .ForMember(x => x.CreatedAt, o => o.MapFrom(t => t.CreatedAt))
                .ForMember(x => x.LastUpdatedBy, o => o.MapFrom(t => t.LastUpdatedBy))
                .ForMember(x => x.LastUpdatedAt, o => o.MapFrom(t => t.LastUpdatedAt))
                .ReverseMap();

        }
    }
}
