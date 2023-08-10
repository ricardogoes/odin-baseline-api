using AutoMapper;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.ViewModels.Customers;

namespace Odin.Baseline.Crosscutting.AutoMapper.Profiles
{
    public class CustomersProfile : Profile
    {
        public CustomersProfile() 
        {
            CreateMap<Customer, CustomerToInsert>();
            CreateMap<CustomerToInsert, Customer>();

            CreateMap<Customer, CustomerToUpdate>();
            CreateMap<CustomerToUpdate, Customer>();
        }
    }
}
