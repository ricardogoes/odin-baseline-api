using AutoMapper;
using FluentAssertions;
using Moq;
using Odin.Baseline.Crosscutting.AutoMapper.Profiles;
using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.Repositories;
using Odin.Baseline.Domain.Models;
using Odin.Baseline.Domain.QueryModels;
using Odin.Baseline.Domain.ViewModels.CompaniesPositions;
using Odin.Baseline.Service;
using System.Linq.Expressions;

namespace Odin.Baseline.UnitTests.Services
{
    public class CompaniesPositionsServiceTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly IMapper _mapper;

        public CompaniesPositionsServiceTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork> { DefaultValue = DefaultValue.Mock };

            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CompaniesPositionsProfile>();
            });
            _mapper = mapperConfiguration.CreateMapper();
        }

        [Fact(DisplayName = "InsertAsync() should return OK with valid data")]
        [Trait("Category", "CompaniesPositions Service Tests")]
        public async Task InsertAsync_Ok()
        {
            var expectedCompanyPositionInserted = new CompanyPositionToQuery
            {
                PositionId = 1,
                CustomerId = 1,
                CustomerName = "Customer Unit Testing",
                Name = "Unit Testing",
                BaseSalary = 1000,
                IsActive = true,
                CreatedBy = "unit.testing",
                CreatedAt = DateTime.UtcNow,
                LastUpdatedBy = "unit.testing",
                LastUpdatedAt = DateTime.UtcNow
            };

            _unitOfWorkMock.Setup(s => s.Repository().Insert<CompanyPosition, CompanyPositionToQuery>(It.IsAny<CompanyPosition>()))
                .Returns(() => expectedCompanyPositionInserted);

            var service = new CompaniesPositionsService(_unitOfWorkMock.Object, _mapper);
            var positionInserted = await service.InsertAsync(new CompanyPositionToInsert
            {
                CustomerId = 1,
                Name = "Unit Testing",
                BaseSalary = 1000
            }, "unit.testing", new CancellationToken());

            positionInserted.PositionId.Should().Be(expectedCompanyPositionInserted.PositionId);
        }

        [Fact(DisplayName = "InsertAsync() should return ArgumentNullException when positionToInsert is null")]
        [Trait("Category", "CompaniesPositions Service Tests")]
        public async Task InsertAsync_should_throw_ArgumentNullException_when_position_null()
        {
            var service = new CompaniesPositionsService(_unitOfWorkMock.Object, _mapper);
            
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async() => await service.InsertAsync(null, "unit.testing", new CancellationToken()));

            ex.Message.Should().Contain("Value cannot be null");
            ex.ParamName.Should().Be("positionToInsert");
        }

        [Fact(DisplayName = "InsertAsync() should return ArgumentNullException when loggedUsername is null")]
        [Trait("Category", "CompaniesPositions Service Tests")]
        public async Task InsertAsync_should_throw_ArgumentNullException_when_loggedUsername_null()
        {
            var service = new CompaniesPositionsService(_unitOfWorkMock.Object, _mapper);

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () => await service.InsertAsync(new CompanyPositionToInsert
            {
                CustomerId = 1,
                Name = "Unit Testing",
                BaseSalary = 1000
            }, null, new CancellationToken()));

            ex.Message.Should().Contain("Value cannot be null");
            ex.ParamName.Should().Be("loggedUsername");
        }

        [Fact(DisplayName = "UpdateAsync() should return OK with valid data")]
        [Trait("Category", "CompaniesPositions Service Tests")]
        public async Task UpdateAsync_Ok()
        {
            var expectedCompanyPositionUpdated = new CompanyPositionToQuery
            {
                PositionId = 1,
                CustomerId = 1,
                CustomerName = "Customer Unit Testing",
                Name = "Unit Testing",
                BaseSalary = 1000,
                IsActive = true,
                CreatedBy = "unit.testing",
                CreatedAt = DateTime.UtcNow,
                LastUpdatedBy = "unit.testing",
                LastUpdatedAt = DateTime.UtcNow
            };

            _unitOfWorkMock.Setup(s => s.Repository().Update<CompanyPosition, CompanyPositionToQuery>(It.IsAny<CompanyPosition>()))
                .Returns(() => expectedCompanyPositionUpdated);

            var service = new CompaniesPositionsService(_unitOfWorkMock.Object, _mapper);
            var positionUpdated = await service.UpdateAsync(new CompanyPositionToUpdate
            {
                PositionId = 1,
                CustomerId = 1,
                Name = "Unit Testing",
                BaseSalary = 1000
            }, "unit.testing", new CancellationToken());

            positionUpdated.PositionId.Should().Be(expectedCompanyPositionUpdated.PositionId);
        }

        [Fact(DisplayName = "UpdateAsync() should return ArgumentNullException when positionToInsert is null")]
        [Trait("Category", "CompaniesPositions Service Tests")]
        public async Task UpdateAsync_should_throw_ArgumentNullException_when_position_null()
        {
            var service = new CompaniesPositionsService(_unitOfWorkMock.Object, _mapper);

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () => await service.UpdateAsync(null, "unit.testing", new CancellationToken()));

            ex.Message.Should().Contain("Value cannot be null");
            ex.ParamName.Should().Be("positionToUpdate");
        }

        [Fact(DisplayName = "UpdateAsync() should return ArgumentNullException when loggedUsername is null")]
        [Trait("Category", "CompaniesPositions Service Tests")]
        public async Task UpdateAsync_should_throw_ArgumentNullException_when_loggedUsername_null()
        {
            var service = new CompaniesPositionsService(_unitOfWorkMock.Object, _mapper);

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () => await service.UpdateAsync(new CompanyPositionToUpdate
            {
                PositionId = 1,
                CustomerId = 1,
                Name = "Unit Testing",
                BaseSalary = 1000
            }, null, new CancellationToken()));

            ex.Message.Should().Contain("Value cannot be null");
            ex.ParamName.Should().Be("loggedUsername");
        }

        [Fact(DisplayName = "UpdateAsync() should throw NotFoundException when position does not exist")]
        [Trait("Category", "CompaniesPositions Service Tests")]
        public async Task UpdateAsync_should_throw_NotFoundException_when_position_does_not_exist()
        {
            _unitOfWorkMock.Setup(s => s.Repository().Update<CompanyPosition, CompanyPositionToQuery>(It.IsAny<CompanyPosition>()))
                .Returns(() => null);

            var service = new CompaniesPositionsService(_unitOfWorkMock.Object, _mapper);
            var ex = await Assert.ThrowsAsync<NotFoundException>(async () => await service.UpdateAsync(new CompanyPositionToUpdate
            {
                PositionId = 1,
                CustomerId = 1,
                Name = "Unit Testing",
                BaseSalary = 1000
            }, "unit.testing", new CancellationToken()));

            ex.Message.Should().Contain("CompanyPosition not found");
        }

        [Fact(DisplayName = "ChangeStatusAsync() should return OK with valid data")]
        [Trait("Category", "CompaniesPositions Service Tests")]
        public async Task ChangeStatusAsync_Ok()
        {
            var expectedCompanyPositionUpdated = new CompanyPositionToQuery
            {
                PositionId = 1,
                CustomerId = 1,
                CustomerName = "Customer Unit Testing",
                Name = "Unit Testing",
                BaseSalary = 1000,
                IsActive = true,
                CreatedBy = "unit.testing",
                CreatedAt = DateTime.UtcNow,
                LastUpdatedBy = "unit.testing",
                LastUpdatedAt = DateTime.UtcNow
            };

            _unitOfWorkMock.Setup(s => s.Repository().Update<CompanyPosition, CompanyPositionToQuery>(It.IsAny<CompanyPosition>()))
                .Returns(() => expectedCompanyPositionUpdated);

            _unitOfWorkMock.Setup(s => s.Repository().GetByIdAsync<CompanyPosition, CompanyPositionToQuery>(1, new CancellationToken()))
                .Returns(() => Task.FromResult(expectedCompanyPositionUpdated));

            var service = new CompaniesPositionsService(_unitOfWorkMock.Object, _mapper);
            var positionUpdated = await service.ChangeStatusAsync(1, "unit.testing", new CancellationToken());

            positionUpdated.PositionId.Should().Be(expectedCompanyPositionUpdated.PositionId);
        }

        [Fact(DisplayName = "ChangeStatusAsync() should return ArgumentException when positionId is invalid")]
        [Trait("Category", "CompaniesPositions Service Tests")]
        public async Task ChangeStatusAsync_should_throw_ArgumentException_when_positionId_invalid()
        {
            var service = new CompaniesPositionsService(_unitOfWorkMock.Object, _mapper);

            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await service.ChangeStatusAsync(0, "unit.testing", new CancellationToken()));

            ex.Message.Should().Contain("Invalid positionId");
            ex.ParamName.Should().Be("positionId");
        }

        [Fact(DisplayName = "ChangeStatusAsync() should return ArgumentNullException when loggedUsername is null")]
        [Trait("Category", "CompaniesPositions Service Tests")]
        public async Task ChangeStatusAsync_should_throw_ArgumentNullException_when_loggedUsername_null()
        {
            var service = new CompaniesPositionsService(_unitOfWorkMock.Object, _mapper);

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () => await service.ChangeStatusAsync(1, null, new CancellationToken()));

            ex.Message.Should().Contain("Value cannot be null");
            ex.ParamName.Should().Be("loggedUsername");
        }

        [Fact(DisplayName = "ChangeStatusAsync() should throw NotFoundException when position does not exist")]
        [Trait("Category", "CompaniesPositions Service Tests")]
        public async Task ChangeStatusAsync_should_throw_NotFoundException_when_position_does_not_exist()
        {
            _unitOfWorkMock.Setup(s => s.Repository().GetByIdAsync<CompanyPosition, CompanyPositionToQuery>(1, new CancellationToken()))
                .Returns(() => null);

            var service = new CompaniesPositionsService(_unitOfWorkMock.Object, _mapper);
            var ex = await Assert.ThrowsAsync<NotFoundException>(async () => await service.ChangeStatusAsync(1, "unit.testing", new CancellationToken()));

            ex.Message.Should().Contain("CompanyPosition not found");
        }

        [Fact(DisplayName = "GetByIdAsync() should return OK with valid data")]
        [Trait("Category", "CompaniesPositions Service Tests")]
        public async Task GetByIdAsync_Ok()
        {
            var expectedCompanyPosition = new CompanyPositionToQuery
            {
                PositionId = 1,
                CustomerId = 1,
                CustomerName = "Customer Unit Testing",
                Name = "Unit Testing",
                BaseSalary = 1000,
                IsActive = true,
                CreatedBy = "unit.testing",
                CreatedAt = DateTime.UtcNow,
                LastUpdatedBy = "unit.testing",
                LastUpdatedAt = DateTime.UtcNow
            };
                        
            _unitOfWorkMock.Setup(s => s.Repository().GetByIdAsync<CompanyPosition, CompanyPositionToQuery>(1, new CancellationToken()))
                .Returns(() => Task.FromResult(expectedCompanyPosition));

            var service = new CompaniesPositionsService(_unitOfWorkMock.Object, _mapper);
            var position = await service.GetByIdAsync(1, new CancellationToken());

            position.PositionId.Should().Be(expectedCompanyPosition.PositionId);
        }

        [Fact(DisplayName = "GetByIdAsync() should return ArgumentException when positionId is invalid")]
        [Trait("Category", "CompaniesPositions Service Tests")]
        public async Task GetByIdAsync_should_throw_ArgumentException_when_positionId_invalid()
        {
            var service = new CompaniesPositionsService(_unitOfWorkMock.Object, _mapper);

            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await service.GetByIdAsync(0, new CancellationToken()));

            ex.Message.Should().Contain("Invalid positionId");
            ex.ParamName.Should().Be("positionId");
        }

        [Fact(DisplayName = "GetByCustomerAsync() should return data filtered")]
        [Trait("Category", "CompaniesPositions Service Tests")]
        public async Task GetByCustomerAsync_should_return_data_filtered()
        {
            var expectedCompaniesPositions = new PagedList<CompanyPositionToQuery>
            {
                TotalRecords = 4,
                Items = new List<CompanyPositionToQuery>
                {
                    new CompanyPositionToQuery { PositionId = 1, CustomerId = 1, CustomerName = "Customer Unit Testing", Name = "Unit Testing 1", BaseSalary = 1000, IsActive = true, CreatedBy = "unit.testing", CreatedAt = DateTime.UtcNow, LastUpdatedBy = "unit.testing", LastUpdatedAt = DateTime.UtcNow },
                    new CompanyPositionToQuery { PositionId = 2, CustomerId = 1, CustomerName = "Customer Unit Testing", Name = "Unit Testing 2", BaseSalary = 1000, IsActive = true, CreatedBy = "unit.testing", CreatedAt = DateTime.UtcNow, LastUpdatedBy = "unit.testing", LastUpdatedAt = DateTime.UtcNow },
                    new CompanyPositionToQuery { PositionId = 3, CustomerId = 1, CustomerName = "Customer Unit Testing", Name = "Unit Testing 3", BaseSalary = 1000, IsActive = true, CreatedBy = "unit.testing", CreatedAt = DateTime.UtcNow, LastUpdatedBy = "unit.testing", LastUpdatedAt = DateTime.UtcNow },
                    new CompanyPositionToQuery { PositionId = 4, CustomerId = 1, CustomerName = "Customer Unit Testing", Name = "Unit Testing 4", BaseSalary = 1000, IsActive = true, CreatedBy = "unit.testing", CreatedAt = DateTime.UtcNow, LastUpdatedBy = "unit.testing", LastUpdatedAt = DateTime.UtcNow },
                }
            };            

            _unitOfWorkMock.Setup(s => s.Repository().FindListAsync<CompanyPosition, CompanyPositionToQuery>(It.IsAny<Expression<Func<CompanyPosition, bool>>>(), 1, 10, "name", new CancellationToken()))
                .Returns(() => Task.FromResult(expectedCompaniesPositions));

            var service = new CompaniesPositionsService(_unitOfWorkMock.Object, _mapper);
            var positions = await service.GetByCustomerAsync(1, new CompaniesPositionsQueryModel(1, 10, "Unit Testing", 1000, true, "name"), new CancellationToken());

            positions.TotalRecords.Should().Be(4);
        }
    }
}
