using AutoMapper;
using FluentAssertions;
using Moq;
using Odin.Baseline.Crosscutting.AutoMapper.Profiles;
using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.Repositories;
using Odin.Baseline.Domain.Models;
using Odin.Baseline.Domain.QueryModels;
using Odin.Baseline.Domain.ViewModels.Customers;
using Odin.Baseline.Service;
using System.Linq.Expressions;

namespace Odin.Baseline.UnitTests.Services
{
    public class CustomersServiceTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly IMapper _mapper;

        public CustomersServiceTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork> { DefaultValue = DefaultValue.Mock };

            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CustomersProfile>();
            });
            _mapper = mapperConfiguration.CreateMapper();
        }

        [Fact(DisplayName = "InsertAsync() should return OK with valid data")]
        [Trait("Category", "Customers Service Tests")]
        public async Task InsertAsync_Ok()
        {
            var expectedCustomerInserted = new Customer
            {
                CustomerId = 1,
                Name = "Unit Testing",
                IsActive = true,
                CreatedBy = "unit.testing",
                CreatedAt = DateTime.UtcNow,
                LastUpdatedBy = "unit.testing",
                LastUpdatedAt = DateTime.UtcNow
            };

            _unitOfWorkMock.Setup(s => s.Repository().Insert<Customer, Customer>(It.IsAny<Customer>()))
                .Returns(() => expectedCustomerInserted);

            var service = new CustomersService(_unitOfWorkMock.Object, _mapper);
            var customerInserted = await service.InsertAsync(new CustomerToInsert
            {
                Name = "Unit Testing",
                Document = "123.123.123-12"
            }, "unit.testing", new CancellationToken());

            customerInserted.CustomerId.Should().Be(expectedCustomerInserted.CustomerId);
        }

        [Fact(DisplayName = "InsertAsync() should return ArgumentNullException when customerToInsert is null")]
        [Trait("Category", "Customers Service Tests")]
        public async Task InsertAsync_should_throw_ArgumentNullException_when_customer_null()
        {
            var service = new CustomersService(_unitOfWorkMock.Object, _mapper);
            
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async() => await service.InsertAsync(null, "unit.testing", new CancellationToken()));

            ex.Message.Should().Contain("Value cannot be null");
            ex.ParamName.Should().Be("customerToInsert");
        }

        [Fact(DisplayName = "InsertAsync() should return ArgumentNullException when loggedUsername is null")]
        [Trait("Category", "Customers Service Tests")]
        public async Task InsertAsync_should_throw_ArgumentNullException_when_loggedUsername_null()
        {
            var service = new CustomersService(_unitOfWorkMock.Object, _mapper);

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () => await service.InsertAsync(new CustomerToInsert
            {
                Name = "Unit Testing",
                Document = "123.123.123-12"
            }, null, new CancellationToken()));

            ex.Message.Should().Contain("Value cannot be null");
            ex.ParamName.Should().Be("loggedUsername");
        }

        [Fact(DisplayName = "UpdateAsync() should return OK with valid data")]
        [Trait("Category", "Customers Service Tests")]
        public async Task UpdateAsync_Ok()
        {
            var expectedCustomerUpdated = new Customer
            {
                CustomerId = 1,
                Name = "Unit Testing",
                IsActive = true,
                CreatedBy = "unit.testing",
                CreatedAt = DateTime.UtcNow,
                LastUpdatedBy = "unit.testing",
                LastUpdatedAt = DateTime.UtcNow
            };

            _unitOfWorkMock.Setup(s => s.Repository().Update<Customer, Customer>(It.IsAny<Customer>()))
                .Returns(() => expectedCustomerUpdated);

            var service = new CustomersService(_unitOfWorkMock.Object, _mapper);
            var customerUpdated = await service.UpdateAsync(new CustomerToUpdate
            {
                CustomerId = 1,
                Name = "Unit Testing",
                Document = "123.123.123-12"
            }, "unit.testing", new CancellationToken());

            customerUpdated.CustomerId.Should().Be(expectedCustomerUpdated.CustomerId);
        }

        [Fact(DisplayName = "UpdateAsync() should return ArgumentNullException when customerToInsert is null")]
        [Trait("Category", "Customers Service Tests")]
        public async Task UpdateAsync_should_throw_ArgumentNullException_when_customer_null()
        {
            var service = new CustomersService(_unitOfWorkMock.Object, _mapper);

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () => await service.UpdateAsync(null, "unit.testing", new CancellationToken()));

            ex.Message.Should().Contain("Value cannot be null");
            ex.ParamName.Should().Be("customerToUpdate");
        }

        [Fact(DisplayName = "UpdateAsync() should return ArgumentNullException when loggedUsername is null")]
        [Trait("Category", "Customers Service Tests")]
        public async Task UpdateAsync_should_throw_ArgumentNullException_when_loggedUsername_null()
        {
            var service = new CustomersService(_unitOfWorkMock.Object, _mapper);

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () => await service.UpdateAsync(new CustomerToUpdate
            {
                CustomerId = 1,
                Name = "Unit Testing",
                Document = "123.123.123-12"
            }, null, new CancellationToken()));

            ex.Message.Should().Contain("Value cannot be null");
            ex.ParamName.Should().Be("loggedUsername");
        }

        [Fact(DisplayName = "UpdateAsync() should throw NotFoundException when customer does not exist")]
        [Trait("Category", "Customers Service Tests")]
        public async Task UpdateAsync_should_throw_NotFoundException_when_customer_does_not_exist()
        {
            _unitOfWorkMock.Setup(s => s.Repository().Update<Customer, Customer>(It.IsAny<Customer>()))
                .Returns(() => null);

            var service = new CustomersService(_unitOfWorkMock.Object, _mapper);
            var ex = await Assert.ThrowsAsync<NotFoundException>(async () => await service.UpdateAsync(new CustomerToUpdate
            {
                CustomerId = 1,
                Name = "Unit Testing",
                Document = "123.123.123-12"
            }, "unit.testing", new CancellationToken()));

            ex.Message.Should().Contain("Customer not found");
        }

        [Fact(DisplayName = "ChangeStatusAsync() should return OK with valid data")]
        [Trait("Category", "Customers Service Tests")]
        public async Task ChangeStatusAsync_Ok()
        {
            var expectedCustomerUpdated = new Customer
            {
                CustomerId = 1,
                Name = "Unit Testing",
                IsActive = true,
                CreatedBy = "unit.testing",
                CreatedAt = DateTime.UtcNow,
                LastUpdatedBy = "unit.testing",
                LastUpdatedAt = DateTime.UtcNow
            };

            _unitOfWorkMock.Setup(s => s.Repository().Update<Customer, Customer>(It.IsAny<Customer>()))
                .Returns(() => expectedCustomerUpdated);

            _unitOfWorkMock.Setup(s => s.Repository().GetByIdAsync<Customer, Customer>(1, new CancellationToken()))
                .Returns(() => Task.FromResult(expectedCustomerUpdated));

            var service = new CustomersService(_unitOfWorkMock.Object, _mapper);
            var customerUpdated = await service.ChangeStatusAsync(1, "unit.testing", new CancellationToken());

            customerUpdated.CustomerId.Should().Be(expectedCustomerUpdated.CustomerId);
        }

        [Fact(DisplayName = "ChangeStatusAsync() should return ArgumentException when customerId is invalid")]
        [Trait("Category", "Customers Service Tests")]
        public async Task ChangeStatusAsync_should_throw_ArgumentException_when_customerId_invalid()
        {
            var service = new CustomersService(_unitOfWorkMock.Object, _mapper);

            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await service.ChangeStatusAsync(0, "unit.testing", new CancellationToken()));

            ex.Message.Should().Contain("Invalid customerId");
            ex.ParamName.Should().Be("customerId");
        }

        [Fact(DisplayName = "ChangeStatusAsync() should return ArgumentNullException when loggedUsername is null")]
        [Trait("Category", "Customers Service Tests")]
        public async Task ChangeStatusAsync_should_throw_ArgumentNullException_when_loggedUsername_null()
        {
            var service = new CustomersService(_unitOfWorkMock.Object, _mapper);

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () => await service.ChangeStatusAsync(1, null, new CancellationToken()));

            ex.Message.Should().Contain("Value cannot be null");
            ex.ParamName.Should().Be("loggedUsername");
        }

        [Fact(DisplayName = "ChangeStatusAsync() should throw NotFoundException when customer does not exist")]
        [Trait("Category", "Customers Service Tests")]
        public async Task ChangeStatusAsync_should_throw_NotFoundException_when_customer_does_not_exist()
        {
            _unitOfWorkMock.Setup(s => s.Repository().GetByIdAsync<Customer, Customer>(1, new CancellationToken()))
                .Returns(() => null);

            var service = new CustomersService(_unitOfWorkMock.Object, _mapper);
            var ex = await Assert.ThrowsAsync<NotFoundException>(async () => await service.ChangeStatusAsync(1, "unit.testing", new CancellationToken()));

            ex.Message.Should().Contain("Customer not found");
        }

        [Fact(DisplayName = "GetByIdAsync() should return OK with valid data")]
        [Trait("Category", "Customers Service Tests")]
        public async Task GetByIdAsync_Ok()
        {
            var expectedCustomer = new Customer
            {
                CustomerId = 1,
                Name = "Unit Testing",
                IsActive = true,
                CreatedBy = "unit.testing",
                CreatedAt = DateTime.UtcNow,
                LastUpdatedBy = "unit.testing",
                LastUpdatedAt = DateTime.UtcNow
            };
                        
            _unitOfWorkMock.Setup(s => s.Repository().GetByIdAsync<Customer, Customer>(1, new CancellationToken()))
                .Returns(() => Task.FromResult(expectedCustomer));

            var service = new CustomersService(_unitOfWorkMock.Object, _mapper);
            var customer = await service.GetByIdAsync(1, new CancellationToken());

            customer.CustomerId.Should().Be(expectedCustomer.CustomerId);
        }

        [Fact(DisplayName = "GetByIdAsync() should return ArgumentException when customerId is invalid")]
        [Trait("Category", "Customers Service Tests")]
        public async Task GetByIdAsync_should_throw_ArgumentException_when_customerId_invalid()
        {
            var service = new CustomersService(_unitOfWorkMock.Object, _mapper);

            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await service.GetByIdAsync(0, new CancellationToken()));

            ex.Message.Should().Contain("Invalid customerId");
            ex.ParamName.Should().Be("customerId");
        }

        [Fact(DisplayName = "GetAllAsync() should return data filtered")]
        [Trait("Category", "Customers Service Tests")]
        public async Task GetAllAsync_should_return_data_filtered()
        {
            var expectedCustomers = new PagedList<Customer>
            {
                TotalRecords = 4,
                Items = new List<Customer>
                {
                    new Customer { CustomerId = 1, Name = "Unit Testing 1", IsActive = true, CreatedBy = "unit.testing", CreatedAt = DateTime.UtcNow, LastUpdatedBy = "unit.testing", LastUpdatedAt = DateTime.UtcNow },
                    new Customer { CustomerId = 2, Name = "Unit Testing 2", IsActive = true, CreatedBy = "unit.testing", CreatedAt = DateTime.UtcNow, LastUpdatedBy = "unit.testing", LastUpdatedAt = DateTime.UtcNow },
                    new Customer { CustomerId = 3, Name = "Unit Testing 3", IsActive = true, CreatedBy = "unit.testing", CreatedAt = DateTime.UtcNow, LastUpdatedBy = "unit.testing", LastUpdatedAt = DateTime.UtcNow },
                    new Customer { CustomerId = 4, Name = "Unit Testing 4", IsActive = true, CreatedBy = "unit.testing", CreatedAt = DateTime.UtcNow, LastUpdatedBy = "unit.testing", LastUpdatedAt = DateTime.UtcNow },
                }
            };            

            _unitOfWorkMock.Setup(s => s.Repository().FindListAsync<Customer, Customer>(It.IsAny<Expression<Func<Customer, bool>>>(), 1, 10, "name", new CancellationToken()))
                .Returns(() => Task.FromResult(expectedCustomers));

            var service = new CustomersService(_unitOfWorkMock.Object, _mapper);
            var customers = await service.GetAllAsync(new CustomersQueryModel(1, 10, "Unit Testing", "123.123.123-12", true, "name"), new CancellationToken());

            customers.TotalRecords.Should().Be(4);
        }
    }
}
