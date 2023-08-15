using AutoMapper;
using FluentAssertions;
using Moq;
using Odin.Baseline.Crosscutting.AutoMapper.Profiles;
using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.Repositories;
using Odin.Baseline.Domain.Models;
using Odin.Baseline.Domain.QueryModels;
using Odin.Baseline.Domain.ViewModels.Employees;
using Odin.Baseline.Service;
using System.Linq.Expressions;

namespace Odin.Baseline.UnitTests.Services
{
    public class EmployeesServiceTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly IMapper _mapper;

        public EmployeesServiceTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork> { DefaultValue = DefaultValue.Mock };

            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<EmployeesProfile>();
            });
            _mapper = mapperConfiguration.CreateMapper();
        }

        [Fact(DisplayName = "InsertAsync() should return OK with valid data")]
        [Trait("Category", "Employees Service Tests")]
        public async Task InsertAsync_Ok()
        {
            var expectedEmployeeInserted = new EmployeeToQuery
            {
                EmployeeId = 1,
                CustomerId = 1,
                CustomerName = "Customer Unit Testing",
                FirstName = "Unit",
                LastName = "Testing",
                Email = "unit.testing@email.com",
                Salary = 1000,
                IsActive = true,
                CreatedBy = "unit.testing",
                CreatedAt = DateTime.UtcNow,
                LastUpdatedBy = "unit.testing",
                LastUpdatedAt = DateTime.UtcNow
            };

            _unitOfWorkMock.Setup(s => s.Repository().Insert<Employee, EmployeeToQuery>(It.IsAny<Employee>()))
                .Returns(() => expectedEmployeeInserted);

            var service = new EmployeesService(_unitOfWorkMock.Object, _mapper);
            var positionInserted = await service.InsertAsync(new EmployeeToInsert
            {
                CustomerId = 1,
                FirstName = "Unit",
                LastName = "Testing",
                Email = "unit.testing@email.com",
                Salary = 1000,
            }, "unit.testing", new CancellationToken());

            positionInserted.EmployeeId.Should().Be(expectedEmployeeInserted.EmployeeId);
        }

        [Fact(DisplayName = "InsertAsync() should return ArgumentNullException when positionToInsert is null")]
        [Trait("Category", "Employees Service Tests")]
        public async Task InsertAsync_should_throw_ArgumentNullException_when_position_null()
        {
            var service = new EmployeesService(_unitOfWorkMock.Object, _mapper);
            
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async() => await service.InsertAsync(null, "unit.testing", new CancellationToken()));

            ex.Message.Should().Contain("Value cannot be null");
            ex.ParamName.Should().Be("employeeToInsert");
        }

        [Fact(DisplayName = "InsertAsync() should return ArgumentNullException when loggedUsername is null")]
        [Trait("Category", "Employees Service Tests")]
        public async Task InsertAsync_should_throw_ArgumentNullException_when_loggedUsername_null()
        {
            var service = new EmployeesService(_unitOfWorkMock.Object, _mapper);

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () => await service.InsertAsync(new EmployeeToInsert
            {
                CustomerId = 1,
                FirstName = "Unit",
                LastName = "Testing",
                Email = "unit.testing@email.com",
                Salary = 1000
            }, null, new CancellationToken()));

            ex.Message.Should().Contain("Value cannot be null");
            ex.ParamName.Should().Be("loggedUsername");
        }

        [Fact(DisplayName = "UpdateAsync() should return OK with valid data")]
        [Trait("Category", "Employees Service Tests")]
        public async Task UpdateAsync_Ok()
        {
            var expectedEmployeeUpdated = new EmployeeToQuery
            {
                EmployeeId = 1,
                CustomerId = 1,
                CustomerName = "Customer Unit Testing",
                FirstName = "Unit",
                LastName = "Testing",
                Email = "unit.testing@email.com",
                Salary = 1000,
                IsActive = true,
                CreatedBy = "unit.testing",
                CreatedAt = DateTime.UtcNow,
                LastUpdatedBy = "unit.testing",
                LastUpdatedAt = DateTime.UtcNow
            };

            _unitOfWorkMock.Setup(s => s.Repository().Update<Employee, EmployeeToQuery>(It.IsAny<Employee>()))
                .Returns(() => expectedEmployeeUpdated);

            var service = new EmployeesService(_unitOfWorkMock.Object, _mapper);
            var positionUpdated = await service.UpdateAsync(new EmployeeToUpdate
            {
                EmployeeId = 1,
                CustomerId = 1,
                FirstName = "Unit",
                LastName = "Testing",
                Email = "unit.testing@email.com",
                Salary = 1000,
            }, "unit.testing", new CancellationToken());

            positionUpdated.EmployeeId.Should().Be(expectedEmployeeUpdated.EmployeeId);
        }

        [Fact(DisplayName = "UpdateAsync() should return ArgumentNullException when positionToInsert is null")]
        [Trait("Category", "Employees Service Tests")]
        public async Task UpdateAsync_should_throw_ArgumentNullException_when_position_null()
        {
            var service = new EmployeesService(_unitOfWorkMock.Object, _mapper);

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () => await service.UpdateAsync(null, "unit.testing", new CancellationToken()));

            ex.Message.Should().Contain("Value cannot be null");
            ex.ParamName.Should().Be("employeeToUpdate");
        }

        [Fact(DisplayName = "UpdateAsync() should return ArgumentNullException when loggedUsername is null")]
        [Trait("Category", "Employees Service Tests")]
        public async Task UpdateAsync_should_throw_ArgumentNullException_when_loggedUsername_null()
        {
            var service = new EmployeesService(_unitOfWorkMock.Object, _mapper);

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () => await service.UpdateAsync(new EmployeeToUpdate
            {
                EmployeeId = 1,
                CustomerId = 1,
                FirstName = "Unit",
                LastName = "Testing",
                Email = "unit.testing@email.com",
                Salary = 1000,
            }, null, new CancellationToken()));

            ex.Message.Should().Contain("Value cannot be null");
            ex.ParamName.Should().Be("loggedUsername");
        }

        [Fact(DisplayName = "UpdateAsync() should throw NotFoundException when position does not exist")]
        [Trait("Category", "Employees Service Tests")]
        public async Task UpdateAsync_should_throw_NotFoundException_when_position_does_not_exist()
        {
            _unitOfWorkMock.Setup(s => s.Repository().Update<Employee, EmployeeToQuery>(It.IsAny<Employee>()))
                .Returns(() => null);

            var service = new EmployeesService(_unitOfWorkMock.Object, _mapper);
            var ex = await Assert.ThrowsAsync<NotFoundException>(async () => await service.UpdateAsync(new EmployeeToUpdate
            {
                EmployeeId = 1,
                CustomerId = 1,
                FirstName = "Unit",
                LastName = "Testing",
                Email = "unit.testing@email.com",
                Salary = 1000,
            }, "unit.testing", new CancellationToken()));

            ex.Message.Should().Contain("Employee not found");
        }

        [Fact(DisplayName = "ChangeStatusAsync() should return OK with valid data")]
        [Trait("Category", "Employees Service Tests")]
        public async Task ChangeStatusAsync_Ok()
        {
            var expectedEmployeeUpdated = new EmployeeToQuery
            {
                EmployeeId = 1,
                CustomerId = 1,
                CustomerName = "Customer Unit Testing",
                FirstName = "Unit",
                LastName = "Testing",
                Email = "unit.testing@email.com",
                Salary = 1000,
                IsActive = true,
                CreatedBy = "unit.testing",
                CreatedAt = DateTime.UtcNow,
                LastUpdatedBy = "unit.testing",
                LastUpdatedAt = DateTime.UtcNow
            };

            _unitOfWorkMock.Setup(s => s.Repository().Update<Employee, EmployeeToQuery>(It.IsAny<Employee>()))
                .Returns(() => expectedEmployeeUpdated);

            _unitOfWorkMock.Setup(s => s.Repository().GetByIdAsync<Employee, EmployeeToQuery>(1, new CancellationToken()))
                .Returns(() => Task.FromResult(expectedEmployeeUpdated));

            var service = new EmployeesService(_unitOfWorkMock.Object, _mapper);
            var positionUpdated = await service.ChangeStatusAsync(1, "unit.testing", new CancellationToken());

            positionUpdated.EmployeeId.Should().Be(expectedEmployeeUpdated.EmployeeId);
        }

        [Fact(DisplayName = "ChangeStatusAsync() should return ArgumentException when positionId is invalid")]
        [Trait("Category", "Employees Service Tests")]
        public async Task ChangeStatusAsync_should_throw_ArgumentException_when_positionId_invalid()
        {
            var service = new EmployeesService(_unitOfWorkMock.Object, _mapper);

            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await service.ChangeStatusAsync(0, "unit.testing", new CancellationToken()));

            ex.Message.Should().Contain("Invalid employeeId");
            ex.ParamName.Should().Be("employeeId");
        }

        [Fact(DisplayName = "ChangeStatusAsync() should return ArgumentNullException when loggedUsername is null")]
        [Trait("Category", "Employees Service Tests")]
        public async Task ChangeStatusAsync_should_throw_ArgumentNullException_when_loggedUsername_null()
        {
            var service = new EmployeesService(_unitOfWorkMock.Object, _mapper);

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () => await service.ChangeStatusAsync(1, null, new CancellationToken()));

            ex.Message.Should().Contain("Value cannot be null");
            ex.ParamName.Should().Be("loggedUsername");
        }

        [Fact(DisplayName = "ChangeStatusAsync() should throw NotFoundException when position does not exist")]
        [Trait("Category", "Employees Service Tests")]
        public async Task ChangeStatusAsync_should_throw_NotFoundException_when_position_does_not_exist()
        {
            _unitOfWorkMock.Setup(s => s.Repository().GetByIdAsync<Employee, EmployeeToQuery>(1, new CancellationToken()))
                .Returns(() => null);

            var service = new EmployeesService(_unitOfWorkMock.Object, _mapper);
            var ex = await Assert.ThrowsAsync<NotFoundException>(async () => await service.ChangeStatusAsync(1, "unit.testing", new CancellationToken()));

            ex.Message.Should().Contain("Employee not found");
        }

        [Fact(DisplayName = "GetByIdAsync() should return OK with valid data")]
        [Trait("Category", "Employees Service Tests")]
        public async Task GetByIdAsync_Ok()
        {
            var expectedEmployee = new EmployeeToQuery
            {
                EmployeeId = 1,
                CustomerId = 1,
                CustomerName = "Customer Unit Testing",
                FirstName = "Unit",
                LastName = "Testing",
                Email = "unit.testing@email.com",
                Salary = 1000,
                IsActive = true,
                CreatedBy = "unit.testing",
                CreatedAt = DateTime.UtcNow,
                LastUpdatedBy = "unit.testing",
                LastUpdatedAt = DateTime.UtcNow
            };
                        
            _unitOfWorkMock.Setup(s => s.Repository().GetByIdAsync<Employee, EmployeeToQuery>(1, new CancellationToken()))
                .Returns(() => Task.FromResult(expectedEmployee));

            var service = new EmployeesService(_unitOfWorkMock.Object, _mapper);
            var position = await service.GetByIdAsync(1, new CancellationToken());

            position.EmployeeId.Should().Be(expectedEmployee.EmployeeId);
        }

        [Fact(DisplayName = "GetByIdAsync() should return ArgumentException when positionId is invalid")]
        [Trait("Category", "Employees Service Tests")]
        public async Task GetByIdAsync_should_throw_ArgumentException_when_positionId_invalid()
        {
            var service = new EmployeesService(_unitOfWorkMock.Object, _mapper);

            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await service.GetByIdAsync(0, new CancellationToken()));

            ex.Message.Should().Contain("Invalid employeeId");
            ex.ParamName.Should().Be("employeeId");
        }

        [Fact(DisplayName = "GetByCustomerAsync() should return data filtered")]
        [Trait("Category", "Employees Service Tests")]
        public async Task GetByCustomerAsync_should_return_data_filtered()
        {
            var expectedEmployees = new PagedList<EmployeeToQuery>
            {
                TotalRecords = 4,
                Items = new List<EmployeeToQuery>
                {
                    new EmployeeToQuery { EmployeeId = 1, CustomerId = 1, CustomerName = "Customer Unit Testing", FirstName = "Unit", LastName = "Testing 01", Email = "unit.testing01@email.com", Salary = 1000, IsActive = true, CreatedBy = "unit.testing", CreatedAt = DateTime.UtcNow, LastUpdatedBy = "unit.testing", LastUpdatedAt = DateTime.UtcNow },
                    new EmployeeToQuery { EmployeeId = 2, CustomerId = 1, CustomerName = "Customer Unit Testing", FirstName = "Unit", LastName = "Testing 02", Email = "unit.testing02@email.com", Salary = 1000, IsActive = true, CreatedBy = "unit.testing", CreatedAt = DateTime.UtcNow, LastUpdatedBy = "unit.testing", LastUpdatedAt = DateTime.UtcNow },
                    new EmployeeToQuery { EmployeeId = 3, CustomerId = 1, CustomerName = "Customer Unit Testing", FirstName = "Unit", LastName = "Testing 03", Email = "unit.testing03@email.com", Salary = 1000, IsActive = true, CreatedBy = "unit.testing", CreatedAt = DateTime.UtcNow, LastUpdatedBy = "unit.testing", LastUpdatedAt = DateTime.UtcNow },
                    new EmployeeToQuery { EmployeeId = 4, CustomerId = 1, CustomerName = "Customer Unit Testing", FirstName = "Unit", LastName = "Testing 04", Email = "unit.testing04@email.com", Salary = 1000, IsActive = true, CreatedBy = "unit.testing", CreatedAt = DateTime.UtcNow, LastUpdatedBy = "unit.testing", LastUpdatedAt = DateTime.UtcNow },
                }
            };            

            _unitOfWorkMock.Setup(s => s.Repository().FindListAsync<Employee, EmployeeToQuery>(It.IsAny<Expression<Func<Employee, bool>>>(), 1, 10, "name", new CancellationToken()))
                .Returns(() => Task.FromResult(expectedEmployees));

            var service = new EmployeesService(_unitOfWorkMock.Object, _mapper);
            var positions = await service.GetByCustomerAsync(1, new EmployeesQueryModel(1, 10, "Unit", "Testing", "unit.testing@email.com", 1000, true, "name"), new CancellationToken());

            positions.TotalRecords.Should().Be(4);
        }

        [Fact(DisplayName = "GetByDepartmentAsync() should return data filtered")]
        [Trait("Category", "Employees Service Tests")]
        public async Task GetByDepartmentAsync_should_return_data_filtered()
        {
            var expectedEmployees = new PagedList<EmployeeToQuery>
            {
                TotalRecords = 4,
                Items = new List<EmployeeToQuery>
                {
                    new EmployeeToQuery { EmployeeId = 1, CustomerId = 1, CustomerName = "Customer Unit Testing", FirstName = "Unit", LastName = "Testing 01", Email = "unit.testing01@email.com", Salary = 1000, IsActive = true, CreatedBy = "unit.testing", CreatedAt = DateTime.UtcNow, LastUpdatedBy = "unit.testing", LastUpdatedAt = DateTime.UtcNow },
                    new EmployeeToQuery { EmployeeId = 2, CustomerId = 1, CustomerName = "Customer Unit Testing", FirstName = "Unit", LastName = "Testing 02", Email = "unit.testing02@email.com", Salary = 1000, IsActive = true, CreatedBy = "unit.testing", CreatedAt = DateTime.UtcNow, LastUpdatedBy = "unit.testing", LastUpdatedAt = DateTime.UtcNow },
                    new EmployeeToQuery { EmployeeId = 3, CustomerId = 1, CustomerName = "Customer Unit Testing", FirstName = "Unit", LastName = "Testing 03", Email = "unit.testing03@email.com", Salary = 1000, IsActive = true, CreatedBy = "unit.testing", CreatedAt = DateTime.UtcNow, LastUpdatedBy = "unit.testing", LastUpdatedAt = DateTime.UtcNow },
                    new EmployeeToQuery { EmployeeId = 4, CustomerId = 1, CustomerName = "Customer Unit Testing", FirstName = "Unit", LastName = "Testing 04", Email = "unit.testing04@email.com", Salary = 1000, IsActive = true, CreatedBy = "unit.testing", CreatedAt = DateTime.UtcNow, LastUpdatedBy = "unit.testing", LastUpdatedAt = DateTime.UtcNow },
                }
            };

            _unitOfWorkMock.Setup(s => s.Repository().FindListAsync<Employee, EmployeeToQuery>(It.IsAny<Expression<Func<Employee, bool>>>(), 1, 10, "name", new CancellationToken()))
                .Returns(() => Task.FromResult(expectedEmployees));

            var service = new EmployeesService(_unitOfWorkMock.Object, _mapper);
            var positions = await service.GetByDepartmentAsync(1, new EmployeesQueryModel(1, 10, "Unit", "Testing", "unit.testing@email.com", 1000, true, "name"), new CancellationToken());

            positions.TotalRecords.Should().Be(4);
        }

        [Fact(DisplayName = "GetByCompanyPositionAsync() should return data filtered")]
        [Trait("Category", "Employees Service Tests")]
        public async Task GetByCompanyPositionAsync_should_return_data_filtered()
        {
            var expectedEmployees = new PagedList<EmployeeToQuery>
            {
                TotalRecords = 4,
                Items = new List<EmployeeToQuery>
                {
                    new EmployeeToQuery { EmployeeId = 1, CustomerId = 1, CustomerName = "Customer Unit Testing", FirstName = "Unit", LastName = "Testing 01", Email = "unit.testing01@email.com", Salary = 1000, IsActive = true, CreatedBy = "unit.testing", CreatedAt = DateTime.UtcNow, LastUpdatedBy = "unit.testing", LastUpdatedAt = DateTime.UtcNow },
                    new EmployeeToQuery { EmployeeId = 2, CustomerId = 1, CustomerName = "Customer Unit Testing", FirstName = "Unit", LastName = "Testing 02", Email = "unit.testing02@email.com", Salary = 1000, IsActive = true, CreatedBy = "unit.testing", CreatedAt = DateTime.UtcNow, LastUpdatedBy = "unit.testing", LastUpdatedAt = DateTime.UtcNow },
                    new EmployeeToQuery { EmployeeId = 3, CustomerId = 1, CustomerName = "Customer Unit Testing", FirstName = "Unit", LastName = "Testing 03", Email = "unit.testing03@email.com", Salary = 1000, IsActive = true, CreatedBy = "unit.testing", CreatedAt = DateTime.UtcNow, LastUpdatedBy = "unit.testing", LastUpdatedAt = DateTime.UtcNow },
                    new EmployeeToQuery { EmployeeId = 4, CustomerId = 1, CustomerName = "Customer Unit Testing", FirstName = "Unit", LastName = "Testing 04", Email = "unit.testing04@email.com", Salary = 1000, IsActive = true, CreatedBy = "unit.testing", CreatedAt = DateTime.UtcNow, LastUpdatedBy = "unit.testing", LastUpdatedAt = DateTime.UtcNow },
                }
            };

            _unitOfWorkMock.Setup(s => s.Repository().FindListAsync<Employee, EmployeeToQuery>(It.IsAny<Expression<Func<Employee, bool>>>(), 1, 10, "name", new CancellationToken()))
                .Returns(() => Task.FromResult(expectedEmployees));

            var service = new EmployeesService(_unitOfWorkMock.Object, _mapper);
            var positions = await service.GetByCompanyPositionAsync(1, new EmployeesQueryModel(1, 10, "Unit", "Testing", "unit.testing@email.com", 1000, true, "name"), new CancellationToken());

            positions.TotalRecords.Should().Be(4);
        }
    }
}
