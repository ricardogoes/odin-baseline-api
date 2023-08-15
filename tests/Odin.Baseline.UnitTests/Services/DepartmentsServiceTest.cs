using AutoMapper;
using FluentAssertions;
using Moq;
using Odin.Baseline.Crosscutting.AutoMapper.Profiles;
using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.Repositories;
using Odin.Baseline.Domain.Models;
using Odin.Baseline.Domain.QueryModels;
using Odin.Baseline.Domain.ViewModels.Departments;
using Odin.Baseline.Service;
using System.Linq.Expressions;

namespace Odin.Baseline.UnitTests.Services
{
    public class DepartmentsServiceTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly IMapper _mapper;

        public DepartmentsServiceTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork> { DefaultValue = DefaultValue.Mock };

            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<DepartmentsProfile>();
            });
            _mapper = mapperConfiguration.CreateMapper();
        }

        [Fact(DisplayName = "InsertAsync() should return OK with valid data")]
        [Trait("Category", "Departments Service Tests")]
        public async Task InsertAsync_Ok()
        {
            var expectedDepartmentInserted = new DepartmentToQuery
            {
                DepartmentId = 1,
                CustomerId = 1,
                CustomerName = "Customer Unit Testing",
                Name = "Unit Testing",
                IsActive = true,
                CreatedBy = "unit.testing",
                CreatedAt = DateTime.UtcNow,
                LastUpdatedBy = "unit.testing",
                LastUpdatedAt = DateTime.UtcNow
            };

            _unitOfWorkMock.Setup(s => s.Repository().Insert<Department, DepartmentToQuery>(It.IsAny<Department>()))
                .Returns(() => expectedDepartmentInserted);

            var service = new DepartmentsService(_unitOfWorkMock.Object, _mapper);
            var positionInserted = await service.InsertAsync(new DepartmentToInsert
            {
                CustomerId = 1,
                Name = "Unit Testing",
            }, "unit.testing", new CancellationToken());

            positionInserted.DepartmentId.Should().Be(expectedDepartmentInserted.DepartmentId);
        }

        [Fact(DisplayName = "InsertAsync() should return ArgumentNullException when positionToInsert is null")]
        [Trait("Category", "Departments Service Tests")]
        public async Task InsertAsync_should_throw_ArgumentNullException_when_position_null()
        {
            var service = new DepartmentsService(_unitOfWorkMock.Object, _mapper);
            
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async() => await service.InsertAsync(null, "unit.testing", new CancellationToken()));

            ex.Message.Should().Contain("Value cannot be null");
            ex.ParamName.Should().Be("departmentToInsert");
        }

        [Fact(DisplayName = "InsertAsync() should return ArgumentNullException when loggedUsername is null")]
        [Trait("Category", "Departments Service Tests")]
        public async Task InsertAsync_should_throw_ArgumentNullException_when_loggedUsername_null()
        {
            var service = new DepartmentsService(_unitOfWorkMock.Object, _mapper);

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () => await service.InsertAsync(new DepartmentToInsert
            {
                CustomerId = 1,
                Name = "Unit Testing",
            }, null, new CancellationToken()));

            ex.Message.Should().Contain("Value cannot be null");
            ex.ParamName.Should().Be("loggedUsername");
        }

        [Fact(DisplayName = "UpdateAsync() should return OK with valid data")]
        [Trait("Category", "Departments Service Tests")]
        public async Task UpdateAsync_Ok()
        {
            var expectedDepartmentUpdated = new DepartmentToQuery
            {
                DepartmentId = 1,
                CustomerId = 1,
                CustomerName = "Customer Unit Testing",
                Name = "Unit Testing",
                IsActive = true,
                CreatedBy = "unit.testing",
                CreatedAt = DateTime.UtcNow,
                LastUpdatedBy = "unit.testing",
                LastUpdatedAt = DateTime.UtcNow
            };

            _unitOfWorkMock.Setup(s => s.Repository().Update<Department, DepartmentToQuery>(It.IsAny<Department>()))
                .Returns(() => expectedDepartmentUpdated);

            var service = new DepartmentsService(_unitOfWorkMock.Object, _mapper);
            var positionUpdated = await service.UpdateAsync(new DepartmentToUpdate
            {
                DepartmentId = 1,
                CustomerId = 1,
                Name = "Unit Testing",
            }, "unit.testing", new CancellationToken());

            positionUpdated.DepartmentId.Should().Be(expectedDepartmentUpdated.DepartmentId);
        }

        [Fact(DisplayName = "UpdateAsync() should return ArgumentNullException when positionToInsert is null")]
        [Trait("Category", "Departments Service Tests")]
        public async Task UpdateAsync_should_throw_ArgumentNullException_when_position_null()
        {
            var service = new DepartmentsService(_unitOfWorkMock.Object, _mapper);

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () => await service.UpdateAsync(null, "unit.testing", new CancellationToken()));

            ex.Message.Should().Contain("Value cannot be null");
            ex.ParamName.Should().Be("departmentToUpdate");
        }

        [Fact(DisplayName = "UpdateAsync() should return ArgumentNullException when loggedUsername is null")]
        [Trait("Category", "Departments Service Tests")]
        public async Task UpdateAsync_should_throw_ArgumentNullException_when_loggedUsername_null()
        {
            var service = new DepartmentsService(_unitOfWorkMock.Object, _mapper);

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () => await service.UpdateAsync(new DepartmentToUpdate
            {
                DepartmentId = 1,
                CustomerId = 1,
                Name = "Unit Testing",
            }, null, new CancellationToken()));

            ex.Message.Should().Contain("Value cannot be null");
            ex.ParamName.Should().Be("loggedUsername");
        }

        [Fact(DisplayName = "UpdateAsync() should throw NotFoundException when position does not exist")]
        [Trait("Category", "Departments Service Tests")]
        public async Task UpdateAsync_should_throw_NotFoundException_when_position_does_not_exist()
        {
            _unitOfWorkMock.Setup(s => s.Repository().Update<Department, DepartmentToQuery>(It.IsAny<Department>()))
                .Returns(() => null);

            var service = new DepartmentsService(_unitOfWorkMock.Object, _mapper);
            var ex = await Assert.ThrowsAsync<NotFoundException>(async () => await service.UpdateAsync(new DepartmentToUpdate
            {
                DepartmentId = 1,
                CustomerId = 1,
                Name = "Unit Testing",
            }, "unit.testing", new CancellationToken()));

            ex.Message.Should().Contain("Department not found");
        }

        [Fact(DisplayName = "ChangeStatusAsync() should return OK with valid data")]
        [Trait("Category", "Departments Service Tests")]
        public async Task ChangeStatusAsync_Ok()
        {
            var expectedDepartmentUpdated = new DepartmentToQuery
            {
                DepartmentId = 1,
                CustomerId = 1,
                CustomerName = "Customer Unit Testing",
                Name = "Unit Testing",
                IsActive = true,
                CreatedBy = "unit.testing",
                CreatedAt = DateTime.UtcNow,
                LastUpdatedBy = "unit.testing",
                LastUpdatedAt = DateTime.UtcNow
            };

            _unitOfWorkMock.Setup(s => s.Repository().Update<Department, DepartmentToQuery>(It.IsAny<Department>()))
                .Returns(() => expectedDepartmentUpdated);

            _unitOfWorkMock.Setup(s => s.Repository().GetByIdAsync<Department, DepartmentToQuery>(1, new CancellationToken()))
                .Returns(() => Task.FromResult(expectedDepartmentUpdated));

            var service = new DepartmentsService(_unitOfWorkMock.Object, _mapper);
            var positionUpdated = await service.ChangeStatusAsync(1, "unit.testing", new CancellationToken());

            positionUpdated.DepartmentId.Should().Be(expectedDepartmentUpdated.DepartmentId);
        }

        [Fact(DisplayName = "ChangeStatusAsync() should return ArgumentException when positionId is invalid")]
        [Trait("Category", "Departments Service Tests")]
        public async Task ChangeStatusAsync_should_throw_ArgumentException_when_positionId_invalid()
        {
            var service = new DepartmentsService(_unitOfWorkMock.Object, _mapper);

            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await service.ChangeStatusAsync(0, "unit.testing", new CancellationToken()));

            ex.Message.Should().Contain("Invalid departmentId");
            ex.ParamName.Should().Be("departmentId");
        }

        [Fact(DisplayName = "ChangeStatusAsync() should return ArgumentNullException when loggedUsername is null")]
        [Trait("Category", "Departments Service Tests")]
        public async Task ChangeStatusAsync_should_throw_ArgumentNullException_when_loggedUsername_null()
        {
            var service = new DepartmentsService(_unitOfWorkMock.Object, _mapper);

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () => await service.ChangeStatusAsync(1, null, new CancellationToken()));

            ex.Message.Should().Contain("Value cannot be null");
            ex.ParamName.Should().Be("loggedUsername");
        }

        [Fact(DisplayName = "ChangeStatusAsync() should throw NotFoundException when position does not exist")]
        [Trait("Category", "Departments Service Tests")]
        public async Task ChangeStatusAsync_should_throw_NotFoundException_when_position_does_not_exist()
        {
            _unitOfWorkMock.Setup(s => s.Repository().GetByIdAsync<Department, DepartmentToQuery>(1, new CancellationToken()))
                .Returns(() => null);

            var service = new DepartmentsService(_unitOfWorkMock.Object, _mapper);
            var ex = await Assert.ThrowsAsync<NotFoundException>(async () => await service.ChangeStatusAsync(1, "unit.testing", new CancellationToken()));

            ex.Message.Should().Contain("Department not found");
        }

        [Fact(DisplayName = "GetByIdAsync() should return OK with valid data")]
        [Trait("Category", "Departments Service Tests")]
        public async Task GetByIdAsync_Ok()
        {
            var expectedDepartment = new DepartmentToQuery
            {
                DepartmentId = 1,
                CustomerId = 1,
                CustomerName = "Customer Unit Testing",
                Name = "Unit Testing",
                IsActive = true,
                CreatedBy = "unit.testing",
                CreatedAt = DateTime.UtcNow,
                LastUpdatedBy = "unit.testing",
                LastUpdatedAt = DateTime.UtcNow
            };
                        
            _unitOfWorkMock.Setup(s => s.Repository().GetByIdAsync<Department, DepartmentToQuery>(1, new CancellationToken()))
                .Returns(() => Task.FromResult(expectedDepartment));

            var service = new DepartmentsService(_unitOfWorkMock.Object, _mapper);
            var position = await service.GetByIdAsync(1, new CancellationToken());

            position.DepartmentId.Should().Be(expectedDepartment.DepartmentId);
        }

        [Fact(DisplayName = "GetByIdAsync() should return ArgumentException when positionId is invalid")]
        [Trait("Category", "Departments Service Tests")]
        public async Task GetByIdAsync_should_throw_ArgumentException_when_positionId_invalid()
        {
            var service = new DepartmentsService(_unitOfWorkMock.Object, _mapper);

            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await service.GetByIdAsync(0, new CancellationToken()));

            ex.Message.Should().Contain("Invalid departmentId");
            ex.ParamName.Should().Be("departmentId");
        }

        [Fact(DisplayName = "GetByCustomerAsync() should return data filtered")]
        [Trait("Category", "Departments Service Tests")]
        public async Task GetByCustomerAsync_should_return_data_filtered()
        {
            var expectedDepartments = new PagedList<DepartmentToQuery>
            {
                TotalRecords = 4,
                Items = new List<DepartmentToQuery>
                {
                    new DepartmentToQuery { DepartmentId = 1, CustomerId = 1, CustomerName = "Customer Unit Testing", Name = "Unit Testing 1", IsActive = true, CreatedBy = "unit.testing", CreatedAt = DateTime.UtcNow, LastUpdatedBy = "unit.testing", LastUpdatedAt = DateTime.UtcNow },
                    new DepartmentToQuery { DepartmentId = 2, CustomerId = 1, CustomerName = "Customer Unit Testing", Name = "Unit Testing 2", IsActive = true, CreatedBy = "unit.testing", CreatedAt = DateTime.UtcNow, LastUpdatedBy = "unit.testing", LastUpdatedAt = DateTime.UtcNow },
                    new DepartmentToQuery { DepartmentId = 3, CustomerId = 1, CustomerName = "Customer Unit Testing", Name = "Unit Testing 3", IsActive = true, CreatedBy = "unit.testing", CreatedAt = DateTime.UtcNow, LastUpdatedBy = "unit.testing", LastUpdatedAt = DateTime.UtcNow },
                    new DepartmentToQuery { DepartmentId = 4, CustomerId = 1, CustomerName = "Customer Unit Testing", Name = "Unit Testing 4", IsActive = true, CreatedBy = "unit.testing", CreatedAt = DateTime.UtcNow, LastUpdatedBy = "unit.testing", LastUpdatedAt = DateTime.UtcNow },
                }
            };            

            _unitOfWorkMock.Setup(s => s.Repository().FindListAsync<Department, DepartmentToQuery>(It.IsAny<Expression<Func<Department, bool>>>(), 1, 10, "name", new CancellationToken()))
                .Returns(() => Task.FromResult(expectedDepartments));

            var service = new DepartmentsService(_unitOfWorkMock.Object, _mapper);
            var positions = await service.GetByCustomerAsync(1, new DepartmentsQueryModel(1, 10, "Unit Testing", true, "name"), new CancellationToken());

            positions.TotalRecords.Should().Be(4);
        }
    }
}
