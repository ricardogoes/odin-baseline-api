using AutoMapper;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.ViewModels.Employees;

namespace Odin.Baseline.Crosscutting.AutoMapper.Profiles
{
    public class EmployeesProfile : Profile
    {
        public EmployeesProfile() 
        {
            CreateMap<Employee, EmployeeToInsert>();
            CreateMap<EmployeeToInsert, Employee>();

            CreateMap<Employee, EmployeeToUpdate>();
            CreateMap<EmployeeToUpdate, Employee>();

            CreateMap<Employee, EmployeeToQuery>()
                .ForMember(x => x.EmployeeId, o => o.MapFrom(t => t.EmployeeId))
                .ForMember(x => x.CustomerId, o => o.MapFrom(t => t.CustomerId))
                .ForMember(x => x.CustomerName, o => o.MapFrom(t => t.Customer.Name))
                .ForMember(x => x.DepartmentId, o => o.MapFrom(t => t.DepartmentId))
                .ForMember(x => x.DepartmentName, o => o.MapFrom(t => t.Department.Name))
                .ForMember(x => x.CompanyPositionId, o => o.MapFrom(t => t.CompanyPositionId))
                .ForMember(x => x.CompanyPositionName, o => o.MapFrom(t => t.CompanyPosition.Name))
                .ForMember(x => x.FirstName, o => o.MapFrom(t => t.FirstName))
                .ForMember(x => x.LastName, o => o.MapFrom(t => t.LastName))
                .ForMember(x => x.Email, o => o.MapFrom(t => t.Email))
                .ForMember(x => x.Salary, o => o.MapFrom(t => t.Salary))
                .ForMember(x => x.IsActive, o => o.MapFrom(t => t.IsActive))
                .ForMember(x => x.CreatedBy, o => o.MapFrom(t => t.CreatedBy))
                .ForMember(x => x.CreatedAt, o => o.MapFrom(t => t.CreatedAt))
                .ForMember(x => x.LastUpdatedBy, o => o.MapFrom(t => t.LastUpdatedBy))
                .ForMember(x => x.LastUpdatedAt, o => o.MapFrom(t => t.LastUpdatedAt))
                .ReverseMap();
        }
    }
}
