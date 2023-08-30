using FluentAssertions;
using Odin.Baseline.Application.Departments.GetDepartments;
using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Infra.Data.EF.Mappers;
using Odin.Baseline.Infra.Data.EF.Repositories;

namespace Odin.Baseline.IntegrationTests.Infra.Data.EF.Repositories.Department
{
    [Collection(nameof(DepartmentRepositoryTestFixtureCollection))]
    public class DepartmentRepositoryTest
    {
        private readonly DepartmentRepositoryTestFixture _fixture;

        public DepartmentRepositoryTest(DepartmentRepositoryTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = "InsertAsync() should insert a valid department")]
        [Trait("Integration/Infra.Data.EF", "Repositories / DepartmentRepository")]
        public async Task InsertValidDepartment()
        {
            var dbContext = _fixture.CreateDbContext();
            var exampleDepartment = _fixture.GetValidDepartment();

            var repository = new DepartmentRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext);

            await repository.InsertAsync(exampleDepartment, CancellationToken.None);
            await unitOfWork.CommitAsync(CancellationToken.None);

            var departmentInserted = await repository.FindByIdAsync(exampleDepartment.Id, CancellationToken.None);

            departmentInserted.Should().NotBeNull();
            departmentInserted.Id.Should().Be(exampleDepartment.Id);
            departmentInserted.Name.Should().Be(exampleDepartment.Name);
            departmentInserted.IsActive.Should().Be(exampleDepartment.IsActive);
        }



        [Fact(DisplayName = "FindByIdAsync() should get a department by a valid Id")]
        [Trait("Integration/Infra.Data.EF", "Repositories / DepartmentRepository")]
        public async Task Get()
        {
            var dbContext = _fixture.CreateDbContext();

            var exampleDepartment = _fixture.GetValidDepartmentModel();
            var exampleDepartmentsList = _fixture.GetValidDepartmentsModelList(new List<Guid> { Guid.NewGuid() }, 15);
            exampleDepartmentsList.Add(exampleDepartment);

            await dbContext.AddRangeAsync(exampleDepartmentsList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var departmentRepository = new DepartmentRepository(_fixture.CreateDbContext(true));

            var dbDepartment = await departmentRepository.FindByIdAsync(exampleDepartment.Id, CancellationToken.None);

            dbDepartment.Should().NotBeNull();
            dbDepartment!.Name.Should().Be(exampleDepartment.Name);
            dbDepartment.Id.Should().Be(exampleDepartment.Id);
            dbDepartment.IsActive.Should().Be(exampleDepartment.IsActive);
            dbDepartment.CreatedAt.Should().Be(exampleDepartment.CreatedAt);
        }

        [Fact(DisplayName = "FindByIdAsync() should throw an error on FindByIdAsync when department not found")]
        [Trait("Integration/Infra.Data.EF", "Repositories / DepartmentRepository")]
        public async Task GetThrowIfNotFound()
        {
            var dbContext = _fixture.CreateDbContext();
            var exampleId = Guid.NewGuid();

            await dbContext.AddRangeAsync(_fixture.GetValidDepartmentsModelList(new List<Guid> { Guid.NewGuid() }, 15));
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var departmentRepository = new DepartmentRepository(dbContext);

            var task = async () => await departmentRepository.FindByIdAsync(exampleId, CancellationToken.None);

            await task.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Department with Id '{exampleId}' not found.");
        }

        [Fact(DisplayName = "UpdateAsync() should update a department")]
        [Trait("Integration/Infra.Data.EF", "Repositories / DepartmentRepository")]
        public async Task Update()
        {
            var dbContext = _fixture.CreateDbContext();
            var exampleDepartment = _fixture.GetValidDepartment();
            var newDepartmentValues = _fixture.GetValidDepartment();

            var exampleDepartmentsList = _fixture.GetValidDepartmentsModelList(new List<Guid> { Guid.NewGuid() }, 15);
            exampleDepartmentsList.Add(exampleDepartment.ToDepartmentModel());

            await dbContext.AddRangeAsync(exampleDepartmentsList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            exampleDepartment.Update(newDepartmentValues.Name);

            dbContext = _fixture.CreateDbContext(true);
            var departmentRepository = new DepartmentRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext);

            await departmentRepository.UpdateAsync(exampleDepartment);
            await unitOfWork.CommitAsync(CancellationToken.None);

            var dbDepartment = await departmentRepository.FindByIdAsync(exampleDepartment.Id, CancellationToken.None);

            dbDepartment.Should().NotBeNull();
            dbDepartment!.Name.Should().Be(newDepartmentValues.Name);   
            dbDepartment.IsActive.Should().Be(newDepartmentValues.IsActive);
        }

        [Fact(DisplayName = "DeleteAsync() should delete a department")]
        [Trait("Integration/Infra.Data.EF", "Repositories / DepartmentRepository")]
        public async Task Delete()
        {
            var dbContext = _fixture.CreateDbContext(true);
            var exampleDepartment = _fixture.GetValidDepartment();
            var exampleDepartmentsList = _fixture.GetValidDepartmentsModelList(new List<Guid> { Guid.NewGuid() }, 15);
            exampleDepartmentsList.Add(exampleDepartment.ToDepartmentModel());

            await dbContext.AddRangeAsync(exampleDepartmentsList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            dbContext = _fixture.CreateDbContext(true);
            var departmentRepository = new DepartmentRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext);

            await departmentRepository.DeleteAsync(exampleDepartment);
            await unitOfWork.CommitAsync(CancellationToken.None);

            var action = async () => await departmentRepository.FindByIdAsync(exampleDepartment.Id, CancellationToken.None);

            await action.Should().ThrowAsync<NotFoundException>().WithMessage($"Department with Id '{exampleDepartment.Id}' not found.");
        }

        [Fact(DisplayName = "FindPaginatedListAsync() should get paginated list of departments with filtered data")]
        [Trait("Integration/Infra.Data.EF", "Repositories / DepartmentRepository")]
        public async Task SearchRetursListAndTotalFiltered()
        {
            var dbContext = _fixture.CreateDbContext();

            var customerId1 = Guid.NewGuid();
            var customerId2 = Guid.NewGuid();

            var exampleDepartmentsList = _fixture.GetValidDepartmentsModelList(new List<Guid> { customerId1, customerId2 }, 15);

            await dbContext.AddRangeAsync(exampleDepartmentsList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var searchInput = new GetDepartmentsInput(1, 20, sort: "name", customerId: customerId1, name: "", isActive: true);
            var filters = new Dictionary<string, object>
            {
                { "CustomerId", searchInput.CustomerId },
                { "Name", searchInput.Name },
                { "IsActive", searchInput.IsActive },
            };

            var departmentRepository = new DepartmentRepository(dbContext);
            var output = await departmentRepository.FindPaginatedListAsync(filters, searchInput.PageNumber, searchInput.PageSize, searchInput.Sort, CancellationToken.None);

            output.Should().NotBeNull();
            output.Items.Should().NotBeNull();
            output.TotalItems.Should().Be(exampleDepartmentsList.Where(x => x.CustomerId == customerId1 && x.IsActive).Count());
            output.Items.Should().HaveCount(exampleDepartmentsList.Where(x => x.CustomerId == customerId1 && x.IsActive).Count());
            foreach (var outputItem in output.Items)
            {
                var exampleItem = exampleDepartmentsList.Find(
                    department => department.Id == outputItem.Id
                );
                exampleItem.Should().NotBeNull();
                outputItem.CustomerId.Should().Be(customerId1);
                outputItem.Name.Should().Be(exampleItem!.Name);
                outputItem.IsActive.Should().Be(exampleItem.IsActive);
                outputItem.CreatedAt.Should().Be(exampleItem.CreatedAt);
            }
        }

        [Fact(DisplayName = "FindPaginatedListAsync() should get paginated list of departments with no filtered data")]
        [Trait("Integration/Infra.Data.EF", "Repositories / DepartmentRepository")]
        public async Task SearchRetursListAndTotal()
        {
            var dbContext = _fixture.CreateDbContext();
            var exampleDepartmentsList = _fixture.GetValidDepartmentsModelList(new List<Guid> { Guid.NewGuid() },15);

            await dbContext.AddRangeAsync(exampleDepartmentsList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var searchInput = new GetDepartmentsInput(1, 20, customerId: Guid.Empty, name: "", isActive: null, sort: "name");
            var filters = new Dictionary<string, object>
            {
                { "CustomerId", searchInput.CustomerId },
                { "Name", searchInput.Name },
                { "IsActive", searchInput.IsActive },
            };

            var departmentRepository = new DepartmentRepository(dbContext);
            var output = await departmentRepository.FindPaginatedListAsync(filters, searchInput.PageNumber, searchInput.PageSize, searchInput.Sort, CancellationToken.None);

            output.Should().NotBeNull();
            output.Items.Should().NotBeNull();
            output.TotalItems.Should().Be(exampleDepartmentsList.Count());
            output.Items.Should().HaveCount(exampleDepartmentsList.Count());
            foreach (var outputItem in output.Items)
            {
                var exampleItem = exampleDepartmentsList.Find(department => department.Id == outputItem.Id);
                exampleItem.Should().NotBeNull();
                outputItem.CustomerId.Should().Be(exampleItem!.CustomerId);
                outputItem.Name.Should().Be(exampleItem.Name);
                outputItem.IsActive.Should().Be(exampleItem.IsActive);
                outputItem.CreatedAt.Should().Be(exampleItem.CreatedAt);
            }
        }

        [Fact(DisplayName = "FindPaginatedListAsync() should return a empty list when database is clean")]
        [Trait("Integration/Infra.Data.EF", "Repositories / DepartmentRepository")]
        public async Task SearchRetursEmptyWhenPersistenceIsEmpty()
        {
            var dbContext = _fixture.CreateDbContext();
            var departmentRepository = new DepartmentRepository(dbContext);
            var searchInput = new GetDepartmentsInput(1, 20, customerId: Guid.Empty, name: "", isActive: true, sort: "");
            var filters = new Dictionary<string, object>
            {
                { "CustomerId", searchInput.CustomerId },
                { "Name", searchInput.Name },                
                { "IsActive", searchInput.IsActive },
            };

            var output = await departmentRepository.FindPaginatedListAsync(filters, searchInput.PageNumber, searchInput.PageSize, searchInput.Sort, CancellationToken.None);

            output.Should().NotBeNull();
            output.Items.Should().NotBeNull();
            output.TotalItems.Should().Be(0);
            output.Items.Should().HaveCount(0);
        }
    }
}

