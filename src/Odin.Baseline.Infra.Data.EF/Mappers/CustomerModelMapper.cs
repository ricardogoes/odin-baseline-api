using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.ValueObjects;
using Odin.Baseline.Infra.Data.EF.Models;

namespace Odin.Baseline.Infra.Data.EF.Mappers
{
    public static class CustomerModelMapper
    {
        public static CustomerModel ToCustomerModel(this Customer customer)
        {
            return new CustomerModel
            {
                Id = customer.Id,
                Name = customer.Name,
                Document = customer.Document,
                StreetName = customer.Address?.StreetName,
                StreetNumber = customer.Address?.StreetNumber,
                Complement = customer.Address?.Complement,
                Neighborhood = customer.Address?.Neighborhood,
                ZipCode = customer.Address?.ZipCode,
                City = customer.Address?.City,
                State = customer.Address?.State,
                IsActive = customer.IsActive,
                CreatedAt = customer.CreatedAt,
                CreatedBy = customer.CreatedBy,
                LastUpdatedAt = customer.LastUpdatedAt,
                LastUpdatedBy = customer.LastUpdatedBy
            };
        }

        public static IEnumerable<CustomerModel> ToCustomerModel(this IEnumerable<Customer> customers)
            => customers.Select(ToCustomerModel);

        public static Customer ToCustomer(this CustomerModel model)
        {
            var customer = new Customer(model.Id, model.Name, model.Document, isActive: model.IsActive);
            
            if(!string.IsNullOrWhiteSpace(model.StreetName))
            { 
                var address = new Address(model.StreetName, model.StreetNumber ?? 0, model.Complement, model.Neighborhood, model.ZipCode, model.City, model.State);
                customer.ChangeAddress(address);
            }

            customer.SetAuditLog(model.CreatedAt, model.CreatedBy, model.LastUpdatedAt, model.LastUpdatedBy);

            return customer;
        }

        public static IEnumerable<Customer> ToCustomer(this IEnumerable<CustomerModel> models)
            => models.Select(ToCustomer);
    }
}
