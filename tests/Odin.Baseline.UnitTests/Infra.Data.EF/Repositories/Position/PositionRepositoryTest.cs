using FluentAssertions;
using Odin.Baseline.Application.Positions.GetPositions;
using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Infra.Data.EF.Mappers;
using Odin.Baseline.Infra.Data.EF.Models;
using Odin.Baseline.Infra.Data.EF.Repositories;

namespace Odin.Baseline.UnitTests.Infra.Data.EF.Repositories.Position
{
    [Collection(nameof(PositionRepositoryTestFixtureCollection))]
    public class PositionRepositoryTest
    {
        private readonly PositionRepositoryTestFixture _fixture;

        public PositionRepositoryTest(PositionRepositoryTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = "InsertAsync() should insert a valid position")]
        [Trait("Infra.Data.EF", "Repositories / PositionRepository")]
        public async Task InsertValidPosition()
        {
            var dbContext = _fixture.CreateDbContext();

            var exampleCustomer = _fixture.GetValidCustomerModel();
            await dbContext.AddAsync(exampleCustomer);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var examplePosition = _fixture.GetValidPosition(exampleCustomer.Id);

            var repository = new PositionRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext);

            await repository.InsertAsync(examplePosition, CancellationToken.None);
            await unitOfWork.CommitAsync(CancellationToken.None);

            var positionInserted = await repository.FindByIdAsync(examplePosition.Id, CancellationToken.None);

            positionInserted.Should().NotBeNull();
            positionInserted.Id.Should().Be(examplePosition.Id);
            positionInserted.Name.Should().Be(examplePosition.Name);
            positionInserted.BaseSalary.Should().Be(examplePosition.BaseSalary);
            positionInserted.IsActive.Should().Be(examplePosition.IsActive);
        }



        [Fact(DisplayName = "FindByIdAsync() should get a position by a valid Id")]
        [Trait("Infra.Data.EF", "Repositories / PositionRepository")]
        public async Task Get()
        {
            var dbContext = _fixture.CreateDbContext();

            var exampleCustomer = _fixture.GetValidCustomerModel();
            await dbContext.AddAsync(exampleCustomer);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var examplePosition = _fixture.GetValidPositionModel(exampleCustomer.Id);

            var examplePositionsList = _fixture.GetValidPositionsModelList(new List<Guid> { exampleCustomer.Id }, 15);
            examplePositionsList.Add(examplePosition);

            await dbContext.AddRangeAsync(examplePositionsList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var positionRepository = new PositionRepository(_fixture.CreateDbContext(true));

            var dbPosition = await positionRepository.FindByIdAsync(examplePosition.Id, CancellationToken.None);

            dbPosition.Should().NotBeNull();            
            dbPosition!.Id.Should().Be(examplePosition.Id);
            dbPosition.Name.Should().Be(examplePosition.Name);
            dbPosition.BaseSalary.Should().Be(examplePosition.BaseSalary);
            dbPosition.IsActive.Should().Be(examplePosition.IsActive);
            dbPosition.CreatedAt.Should().Be(examplePosition.CreatedAt);
        }

        [Fact(DisplayName = "FindByIdAsync() should throw an error on FindByIdAsync when position not found")]
        [Trait("Infra.Data.EF", "Repositories / PositionRepository")]
        public async Task GetThrowIfNotFound()
        {
            var dbContext = _fixture.CreateDbContext();
            var exampleId = Guid.NewGuid();

            await dbContext.AddRangeAsync(_fixture.GetValidPositionsModelList(new List<Guid> { Guid.NewGuid() }, 15));
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var positionRepository = new PositionRepository(dbContext);

            var task = async () => await positionRepository.FindByIdAsync(exampleId, CancellationToken.None);

            await task.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Position with Id '{exampleId}' not found.");
        }

        [Fact(DisplayName = "UpdateAsync() should update a position")]
        [Trait("Infra.Data.EF", "Repositories / PositionRepository")]
        public async Task Update()
        {
            var dbContext = _fixture.CreateDbContext();
            
            var exampleCustomer = _fixture.GetValidCustomerModel();
            await dbContext.AddAsync(exampleCustomer);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var examplePosition = _fixture.GetValidPosition(exampleCustomer.Id);
            var newPositionValues = _fixture.GetValidPosition(exampleCustomer.Id);

            var examplePositionsList = _fixture.GetValidPositionsModelList(new List<Guid> { exampleCustomer.Id }, 15);
            examplePositionsList.Add(examplePosition.ToPositionModel());

            await dbContext.AddRangeAsync(examplePositionsList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            examplePosition.Update(newPositionValues.Name);

            dbContext = _fixture.CreateDbContext(true);
            var positionRepository = new PositionRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext);

            await positionRepository.UpdateAsync(examplePosition, CancellationToken.None);
            await unitOfWork.CommitAsync(CancellationToken.None);

            var dbPosition = await positionRepository.FindByIdAsync(examplePosition.Id, CancellationToken.None);

            dbPosition.Should().NotBeNull();
            dbPosition!.Name.Should().Be(newPositionValues.Name);
            dbPosition.BaseSalary.Should().Be(examplePosition.BaseSalary);
            dbPosition.IsActive.Should().Be(newPositionValues.IsActive);
        }

        [Fact(DisplayName = "DeleteAsync() should delete a position")]
        [Trait("Infra.Data.EF", "Repositories / PositionRepository")]
        public async Task Delete()
        {
            var dbContext = _fixture.CreateDbContext(true);
            var examplePosition = _fixture.GetValidPosition();
            var examplePositionsList = _fixture.GetValidPositionsModelList(new List<Guid> { Guid.NewGuid() }, 15);
            examplePositionsList.Add(examplePosition.ToPositionModel());

            await dbContext.AddRangeAsync(examplePositionsList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            dbContext = _fixture.CreateDbContext(true);
            var positionRepository = new PositionRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext);

            await positionRepository.DeleteAsync(examplePosition);
            await unitOfWork.CommitAsync(CancellationToken.None);

            var action = async () => await positionRepository.FindByIdAsync(examplePosition.Id, CancellationToken.None);

            await action.Should().ThrowAsync<NotFoundException>().WithMessage($"Position with Id '{examplePosition.Id}' not found.");
        }

        [Fact(DisplayName = "FindPaginatedListAsync() should get paginated list of positions with filtered data")]
        [Trait("Infra.Data.EF", "Repositories / PositionRepository")]
        public async Task SearchRetursListAndTotalFiltered()
        {
            var dbContext = _fixture.CreateDbContext();

            var customer1 = _fixture.GetValidCustomerModel();
            var customer2 = _fixture.GetValidCustomerModel();

            await dbContext.AddRangeAsync(new List<CustomerModel> { customer1, customer2 });


            var examplePositionsList = _fixture.GetValidPositionsModelList(new List<Guid> { customer1.Id, customer2.Id }, 15);

            await dbContext.AddRangeAsync(examplePositionsList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var searchInput = new GetPositionsInput(1, 20, customerId: customer1.Id, sort: "name", name: "", isActive: true, "", null, null, "", null, null);
            var filters = new Dictionary<string, object?>
            {
                { "CustomerId", searchInput.CustomerId },
                { "Name", searchInput.Name },
                { "IsActive", searchInput.IsActive },
            };

            var positionRepository = new PositionRepository(dbContext);
            var output = await positionRepository.FindPaginatedListAsync(filters, searchInput.PageNumber, searchInput.PageSize, searchInput.Sort!, CancellationToken.None);

            output.Should().NotBeNull();
            output.Items.Should().NotBeNull();
            output.TotalItems.Should().Be(examplePositionsList.Where(x => x.CustomerId == customer1.Id && x.IsActive).Count());
            output.Items.Should().HaveCount(examplePositionsList.Where(x => x.CustomerId == customer1.Id && x.IsActive).Count());
            foreach (var outputItem in output.Items)
            {
                var exampleItem = examplePositionsList.Find(
                    position => position.Id == outputItem.Id
                );
                exampleItem.Should().NotBeNull();
                outputItem.CustomerId.Should().Be(customer1.Id);
                outputItem.Name.Should().Be(exampleItem!.Name);
                outputItem.BaseSalary.Should().Be(exampleItem.BaseSalary);
                outputItem.IsActive.Should().Be(exampleItem.IsActive);
                outputItem.CreatedAt.Should().Be(exampleItem.CreatedAt);
            }
        }

        [Fact(DisplayName = "FindPaginatedListAsync() should get paginated list of positions with no filtered data")]
        [Trait("Infra.Data.EF", "Repositories / PositionRepository")]
        public async Task SearchRetursListAndTotal()
        {
            var dbContext = _fixture.CreateDbContext();

            var exampleCustomer = _fixture.GetValidCustomerModel();
            await dbContext.AddAsync(exampleCustomer);

            var examplePositionsList = _fixture.GetValidPositionsModelList(new List<Guid> { exampleCustomer.Id },15);

            await dbContext.AddRangeAsync(examplePositionsList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var searchInput = new GetPositionsInput(1, 20, customerId: Guid.Empty, sort: "name", name: "", isActive: null, "", null, null, "", null, null);
            var filters = new Dictionary<string, object?>
            {
                { "CustomerId", searchInput.CustomerId },
                { "Name", searchInput.Name },
                { "IsActive", searchInput.IsActive },
            };

            var positionRepository = new PositionRepository(dbContext);
            var output = await positionRepository.FindPaginatedListAsync(filters, searchInput.PageNumber, searchInput.PageSize, searchInput.Sort!, CancellationToken.None);

            output.Should().NotBeNull();
            output.Items.Should().NotBeNull();
            output.TotalItems.Should().Be(examplePositionsList.Count());
            output.Items.Should().HaveCount(examplePositionsList.Count());
            foreach (var outputItem in output.Items)
            {
                var exampleItem = examplePositionsList.Find(position => position.Id == outputItem.Id);
                exampleItem.Should().NotBeNull();
                outputItem.CustomerId.Should().Be(exampleItem!.CustomerId);
                outputItem.Name.Should().Be(exampleItem.Name);
                outputItem.BaseSalary.Should().Be(exampleItem.BaseSalary);
                outputItem.IsActive.Should().Be(exampleItem.IsActive);
                outputItem.CreatedAt.Should().Be(exampleItem.CreatedAt);
            }
        }

        [Fact(DisplayName = "FindPaginatedListAsync() should return a empty list when database is clean")]
        [Trait("Infra.Data.EF", "Repositories / PositionRepository")]
        public async Task SearchRetursEmptyWhenPersistenceIsEmpty()
        {
            var dbContext = _fixture.CreateDbContext();
            var positionRepository = new PositionRepository(dbContext);
            var searchInput = new GetPositionsInput(1, 20, customerId: Guid.Empty, sort: "", name: "", isActive: true, "", null, null, "", null, null);
            var filters = new Dictionary<string, object?>
            {
                { "CustomerId", searchInput.CustomerId },
                { "Name", searchInput.Name },                
                { "IsActive", searchInput.IsActive },
            };

            var output = await positionRepository.FindPaginatedListAsync(filters, searchInput.PageNumber, searchInput.PageSize, searchInput.Sort!, CancellationToken.None);

            output.Should().NotBeNull();
            output.Items.Should().NotBeNull();
            output.TotalItems.Should().Be(0);
            output.Items.Should().HaveCount(0);
        }
    }
}

