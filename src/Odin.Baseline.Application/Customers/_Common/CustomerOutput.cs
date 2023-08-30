using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.ValueObjects;

namespace Odin.Baseline.Application.Customers.Common
{
    public class CustomerOutput
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Document { get; set; }
        public Address Address { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        public string LastUpdatedBy { get; set; }

        public static CustomerOutput FromCustomer(Customer customer)
        {
            return new CustomerOutput
            {
                Id = customer.Id,
                Name = customer.Name,
                Document = customer.Document,
                Address = customer.Address,
                IsActive = customer.IsActive,
                CreatedAt = customer.CreatedAt,
                CreatedBy = customer.CreatedBy,
                LastUpdatedAt = customer.LastUpdatedAt,
                LastUpdatedBy = customer.LastUpdatedBy

            };
        }

        public static IEnumerable<CustomerOutput> FromCustomer(IEnumerable<Customer> customers)
            => customers.Select(FromCustomer);
    }
}
