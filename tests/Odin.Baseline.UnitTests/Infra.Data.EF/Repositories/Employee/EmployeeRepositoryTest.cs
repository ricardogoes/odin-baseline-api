using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using Odin.Baseline.Application.Employees.GetEmployees;
using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.Repositories;
using Odin.Baseline.Infra.Data.EF.Mappers;
using Odin.Baseline.Infra.Data.EF.Repositories;

namespace Odin.Baseline.UnitTests.Infra.Data.EF.Repositories.Employee
{
    [Collection(nameof(EmployeeRepositoryTestFixtureCollection))]
    public class EmployeeRepositoryTest
    {
        private readonly EmployeeRepositoryTestFixture _fixture;

        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly Mock<ITenantService> _tenantServiceMock;

        public EmployeeRepositoryTest(EmployeeRepositoryTestFixture fixture)
        {
            _fixture = fixture;

            _httpContextAccessorMock = new();
            _httpContextAccessorMock.Setup(s => s.HttpContext.User.Identity!.Name).Returns("unit.testing");

            _tenantServiceMock = new();
            _tenantServiceMock.Setup(s => s.GetTenant()).Returns(_fixture.TenantId);
        }

        [Fact(DisplayName = "InsertAsync() should insert a valid employee")]
        [Trait("Infra.Data.EF", "Repositories / EmployeeRepository")]
        public async Task InsertValidEmployee()
        {
            var dbContext = _fixture.CreateDbContext();

            var exampleEmployee = _fixture.GetValidEmployee();

            var repository = new EmployeeRepository(dbContext, _httpContextAccessorMock.Object, _tenantServiceMock.Object);
            var unitOfWork = new UnitOfWork(dbContext);

            await repository.InsertAsync(exampleEmployee, CancellationToken.None);
            await unitOfWork.CommitAsync(CancellationToken.None);

            var employeeInserted = await repository.FindByIdAsync(exampleEmployee.Id, CancellationToken.None);

            employeeInserted.Should().NotBeNull();
            employeeInserted.Id.Should().Be(exampleEmployee.Id);
            employeeInserted.FirstName.Should().Be(exampleEmployee.FirstName);
            employeeInserted.LastName.Should().Be(exampleEmployee.LastName);
            employeeInserted.Document.Should().Be(exampleEmployee.Document);
            employeeInserted.Email.Should().Be(exampleEmployee.Email);            
            employeeInserted.IsActive.Should().Be(exampleEmployee.IsActive);
        }

        [Fact(DisplayName = "InsertAsync() should insert a valid employee with historic positions")]
        [Trait("Infra.Data.EF", "Repositories / EmployeeRepository")]
        public async Task InsertValidEmployeeAndHistoricPositions()
        {
            var dbContext = _fixture.CreateDbContext(true);

            var exampleEmployee = _fixture.GetValidEmployee();

            var historicPosition1 = new EmployeePositionHistory(Guid.NewGuid(), 1_000, DateTime.Now.AddMonths(-2), DateTime.Now, isActual: false);
            var historicPosition2 = new EmployeePositionHistory(Guid.NewGuid(), 2_000, DateTime.Now.AddMonths(-2), null, isActual: true);

            exampleEmployee.AddHistoricPosition(historicPosition1);
            exampleEmployee.AddHistoricPosition(historicPosition2);

            var repository = new EmployeeRepository(dbContext, _httpContextAccessorMock.Object, _tenantServiceMock.Object);
            var unitOfWork = new UnitOfWork(dbContext);

            await repository.InsertAsync(exampleEmployee, CancellationToken.None);
            await unitOfWork.CommitAsync(CancellationToken.None);
                        
            var employeeInserted = await repository.FindByIdAsync(exampleEmployee.Id, CancellationToken.None);

            employeeInserted.Should().NotBeNull();
            employeeInserted.Id.Should().Be(exampleEmployee.Id);
            employeeInserted.FirstName.Should().Be(exampleEmployee.FirstName);
            employeeInserted.LastName.Should().Be(exampleEmployee.LastName);
            employeeInserted.Document.Should().Be(exampleEmployee.Document);
            employeeInserted.Email.Should().Be(exampleEmployee.Email);
            employeeInserted.IsActive.Should().Be(exampleEmployee.IsActive);

            var dbHistoricPosition1 = employeeInserted.HistoricPositions.FirstOrDefault(x => x.PositionId == historicPosition1.PositionId);
            dbHistoricPosition1.Should().NotBeNull();
            dbHistoricPosition1!.Salary.Should().Be(historicPosition1.Salary);
            dbHistoricPosition1.StartDate.Should().Be(historicPosition1.StartDate);
            dbHistoricPosition1.FinishDate.Should().NotBeNull();
            dbHistoricPosition1.IsActual.Should().BeFalse();

            var dbHistoricPosition2 = employeeInserted.HistoricPositions.FirstOrDefault(x => x.PositionId == historicPosition2.PositionId);
            dbHistoricPosition2.Should().NotBeNull();
            dbHistoricPosition2!.Salary.Should().Be(historicPosition2.Salary);
            dbHistoricPosition2.StartDate.Should().Be(historicPosition2.StartDate);
            dbHistoricPosition2.FinishDate.Should().BeNull();
            dbHistoricPosition2.IsActual.Should().BeTrue();
        }

        [Fact(DisplayName = "FindByIdAsync() should get a employee by a valid Id")]
        [Trait("Infra.Data.EF", "Repositories / EmployeeRepository")]
        public async Task Get()
        {
            var dbContext = _fixture.CreateDbContext();

            var exampleEmployee = _fixture.GetValidEmployeeModel();
            var exampleEmployeesList = _fixture.GetValidEmployeesModelList(length: 15);
            exampleEmployeesList.Add(exampleEmployee);

            await dbContext.AddRangeAsync(exampleEmployeesList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var employeeRepository = new EmployeeRepository(_fixture.CreateDbContext(true), _httpContextAccessorMock.Object, _tenantServiceMock.Object);

            var dbEmployee = await employeeRepository.FindByIdAsync(exampleEmployee.Id, CancellationToken.None);

            dbEmployee.Should().NotBeNull();            
            dbEmployee!.Id.Should().Be(exampleEmployee.Id);
            dbEmployee.FirstName.Should().Be(exampleEmployee.FirstName);
            dbEmployee.LastName.Should().Be(exampleEmployee.LastName);
            dbEmployee.Document.Should().Be(exampleEmployee.Document);
            dbEmployee.Email.Should().Be(exampleEmployee.Email);
            dbEmployee.IsActive.Should().Be(exampleEmployee.IsActive);
            dbEmployee.CreatedAt.Should().Be(exampleEmployee.CreatedAt);
        }

        [Fact(DisplayName = "FindByIdAsync() should throw an error on FindByIdAsync when employee not found")]
        [Trait("Infra.Data.EF", "Repositories / EmployeeRepository")]
        public async Task GetThrowIfNotFound()
        {
            var dbContext = _fixture.CreateDbContext();
            var exampleId = Guid.NewGuid();

            await dbContext.AddRangeAsync(_fixture.GetValidEmployeesModelList(length: 15));
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var employeeRepository = new EmployeeRepository(dbContext, _httpContextAccessorMock.Object, _tenantServiceMock.Object);

            var task = async () => await employeeRepository.FindByIdAsync(exampleId, CancellationToken.None);

            await task.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Employee with Id '{exampleId}' not found.");
        }

        [Fact(DisplayName = "UpdateAsync() should update a employee")]
        [Trait("Infra.Data.EF", "Repositories / EmployeeRepository")]
        public async Task Update()
        {
            var dbContext = _fixture.CreateDbContext();

            var exampleEmployee = _fixture.GetValidEmployee();
            var newEmployeeValues = _fixture.GetValidEmployee();

            var exampleEmployeesList = _fixture.GetValidEmployeesModelList(length: 15);
            exampleEmployeesList.Add(exampleEmployee.ToEmployeeModel(Guid.NewGuid()));

            await dbContext.AddRangeAsync(exampleEmployeesList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            exampleEmployee.Update(newEmployeeValues.FirstName, newEmployeeValues.LastName, newEmployeeValues.Document, newEmployeeValues.Email);

            dbContext = _fixture.CreateDbContext(true);
            var employeeRepository = new EmployeeRepository(dbContext, _httpContextAccessorMock.Object, _tenantServiceMock.Object);
            var unitOfWork = new UnitOfWork(dbContext);

            await employeeRepository.UpdateAsync(exampleEmployee, CancellationToken.None);
            await unitOfWork.CommitAsync(CancellationToken.None);

            var dbEmployee = await employeeRepository.FindByIdAsync(exampleEmployee.Id, CancellationToken.None);

            dbEmployee.Should().NotBeNull();
            dbEmployee.FirstName.Should().Be(exampleEmployee.FirstName);
            dbEmployee.LastName.Should().Be(exampleEmployee.LastName);
            dbEmployee.Document.Should().Be(exampleEmployee.Document);
            dbEmployee.Email.Should().Be(exampleEmployee.Email);
            dbEmployee.IsActive.Should().Be(newEmployeeValues.IsActive);
        }

        [Fact(DisplayName = "UpdateAsync() should update a valid employee with historic positions")]
        [Trait("Infra.Data.EF", "Repositories / EmployeeRepository")]
        public async Task UpdateValidEmployeeAndHistoricPositions()
        {
            var dbContext = _fixture.CreateDbContext();

            var exampleEmployee = _fixture.GetValidEmployee();

            var historicPosition = new EmployeePositionHistory(Guid.NewGuid(), 2_000, DateTime.Now.AddMonths(-2), DateTime.Now, false);
            exampleEmployee.AddHistoricPosition(historicPosition);

            var exampleEmployeesList = _fixture.GetValidEmployeesModelList(length: 15);
            exampleEmployeesList.Add(exampleEmployee.ToEmployeeModel(Guid.NewGuid()));

            await dbContext.AddRangeAsync(exampleEmployeesList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var newEmployeeValues = _fixture.GetValidEmployee();
            exampleEmployee.Update(newEmployeeValues.FirstName, newEmployeeValues.LastName, newEmployeeValues.Document, newEmployeeValues.Email);

            var newHistoricPosition = new EmployeePositionHistory(Guid.NewGuid(), 2_500, DateTime.Now, null, true);
            exampleEmployee.AddHistoricPosition(newHistoricPosition);

            dbContext = _fixture.CreateDbContext(true);
            var employeeRepository = new EmployeeRepository(dbContext, _httpContextAccessorMock.Object, _tenantServiceMock.Object);
            var unitOfWork = new UnitOfWork(dbContext);

            await employeeRepository.UpdateAsync(exampleEmployee, CancellationToken.None);
            await unitOfWork.CommitAsync(CancellationToken.None);

            var dbEmployee = await employeeRepository.FindByIdAsync(exampleEmployee.Id, CancellationToken.None);

            dbEmployee.Should().NotBeNull();
            dbEmployee.FirstName.Should().Be(exampleEmployee.FirstName);
            dbEmployee.LastName.Should().Be(exampleEmployee.LastName);
            dbEmployee.Document.Should().Be(exampleEmployee.Document);
            dbEmployee.Email.Should().Be(exampleEmployee.Email);
            dbEmployee.IsActive.Should().Be(newEmployeeValues.IsActive);

            var dbHistoricPosition1 = dbEmployee.HistoricPositions.FirstOrDefault(x => x.PositionId == historicPosition.PositionId);
            dbHistoricPosition1.Should().NotBeNull();
            dbHistoricPosition1!.Salary.Should().Be(historicPosition.Salary);
            dbHistoricPosition1.StartDate.Should().Be(historicPosition.StartDate);
            dbHistoricPosition1.FinishDate.Should().NotBeNull();
            dbHistoricPosition1.IsActual.Should().BeFalse();

            var dbHistoricPosition2 = dbEmployee.HistoricPositions.FirstOrDefault(x => x.PositionId == newHistoricPosition.PositionId);
            dbHistoricPosition2.Should().NotBeNull();
            dbHistoricPosition2!.Salary.Should().Be(newHistoricPosition.Salary);
            dbHistoricPosition2.StartDate.Should().Be(newHistoricPosition.StartDate);
            dbHistoricPosition2.FinishDate.Should().BeNull();
            dbHistoricPosition2.IsActual.Should().BeTrue();
        }

        [Fact(DisplayName = "DeleteAsync() should delete a employee")]
        [Trait("Infra.Data.EF", "Repositories / EmployeeRepository")]
        public async Task Delete()
        {
            var dbContext = _fixture.CreateDbContext(true);
            var exampleEmployee = _fixture.GetValidEmployee();
            var exampleEmployeesList = _fixture.GetValidEmployeesModelList(length: 15);
            exampleEmployeesList.Add(exampleEmployee.ToEmployeeModel(Guid.NewGuid()));

            await dbContext.AddRangeAsync(exampleEmployeesList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            dbContext = _fixture.CreateDbContext(true);
            var employeeRepository = new EmployeeRepository(dbContext, _httpContextAccessorMock.Object, _tenantServiceMock.Object);
            var unitOfWork = new UnitOfWork(dbContext);

            await employeeRepository.DeleteAsync(exampleEmployee);
            await unitOfWork.CommitAsync(CancellationToken.None);

            var action = async () => await employeeRepository.FindByIdAsync(exampleEmployee.Id, CancellationToken.None);

            await action.Should().ThrowAsync<NotFoundException>().WithMessage($"Employee with Id '{exampleEmployee.Id}' not found.");
        }

        [Fact(DisplayName = "FindPaginatedListAsync() should get paginated list of employees with filtered data")]
        [Trait("Infra.Data.EF", "Repositories / EmployeeRepository")]
        public async Task SearchRetursListAndTotalFiltered()
        {
            var dbContext = _fixture.CreateDbContext();

            var exampleEmployeesList = _fixture.GetValidEmployeesModelList(length: 15);

            await dbContext.AddRangeAsync(exampleEmployeesList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var searchInput = new GetEmployeesInput(1, 20, sort: "firstname", departmentId: null, firstName: "", lastName: "", document: "", email: "", isActive: true, "", null, null, "", null, null);
            var filters = new Dictionary<string, object?>
            {
                { "DepartmentId", searchInput.DepartmentId },
                { "FirstName", searchInput.FirstName },
                { "LastName", searchInput.LastName },
                { "Document", searchInput.Document },
                { "Email", searchInput.Email },
                { "IsActive", searchInput.IsActive },
            };

            var employeeRepository = new EmployeeRepository(dbContext, _httpContextAccessorMock.Object, _tenantServiceMock.Object);
            var output = await employeeRepository.FindPaginatedListAsync(filters, searchInput.PageNumber, searchInput.PageSize, searchInput.Sort!, CancellationToken.None);

            output.Should().NotBeNull();
            output.Items.Should().NotBeNull();
            output.TotalItems.Should().Be(exampleEmployeesList.Where(x => x.IsActive).Count());
            output.Items.Should().HaveCount(exampleEmployeesList.Where(x => x.IsActive).Count());
            foreach (var outputItem in output.Items)
            {
                var exampleItem = exampleEmployeesList.Find(
                    employee => employee.Id == outputItem.Id
                );
                exampleItem.Should().NotBeNull();
                outputItem.FirstName.Should().Be(exampleItem!.FirstName);
                outputItem.LastName.Should().Be(exampleItem.LastName);
                outputItem.Document.Should().Be(exampleItem.Document);
                outputItem.Email.Should().Be(exampleItem.Email);
                outputItem.IsActive.Should().Be(exampleItem.IsActive);
                outputItem.CreatedAt.Should().Be(exampleItem.CreatedAt);
            }
        }

        [Fact(DisplayName = "FindPaginatedListAsync() should get paginated list of employees with historic positions and filtered data")]
        [Trait("Infra.Data.EF", "Repositories / EmployeeRepository")]
        public async Task SearchRetursCompleteAndTotalFiltered()
        {
            var dbContext = _fixture.CreateDbContext();

            var exampleEmployeesList = _fixture.GetValidEmployeesModelList(length: 15);

            var employeeToAddPositionHistory = exampleEmployeesList.FirstOrDefault(x => x.IsActive)!;
            var examplePositionsHistoryList = _fixture.GetValidEmployeesPositionsHistoryModelList(employeeToAddPositionHistory.Id);

            await dbContext.AddRangeAsync(exampleEmployeesList);
            await dbContext.AddRangeAsync(examplePositionsHistoryList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var searchInput = new GetEmployeesInput(1, 20, sort: "firstname", departmentId: null, firstName: "", lastName: "", document: "", email: "", isActive: true, "", null, null, "", null, null);
            var filters = new Dictionary<string, object?>
            {
                { "DepartmentId", searchInput.DepartmentId },
                { "FirstName", searchInput.FirstName },
                { "LastName", searchInput.LastName },
                { "Document", searchInput.Document },
                { "Email", searchInput.Email },
                { "IsActive", searchInput.IsActive },
            };

            var employeeRepository = new EmployeeRepository(dbContext, _httpContextAccessorMock.Object, _tenantServiceMock.Object);
            var output = await employeeRepository.FindPaginatedListAsync(filters, searchInput.PageNumber, searchInput.PageSize, searchInput.Sort!, CancellationToken.None);

            output.Should().NotBeNull();
            output.Items.Should().NotBeNull();
            output.TotalItems.Should().Be(exampleEmployeesList.Where(x => x.IsActive).Count());
            output.Items.Should().HaveCount(exampleEmployeesList.Where(x => x.IsActive).Count());
            foreach (var outputItem in output.Items)
            {
                var exampleItem = exampleEmployeesList.Find(
                    employee => employee.Id == outputItem.Id
                );
                exampleItem.Should().NotBeNull();
                outputItem.FirstName.Should().Be(exampleItem!.FirstName);
                outputItem.LastName.Should().Be(exampleItem.LastName);
                outputItem.Document.Should().Be(exampleItem.Document);
                outputItem.Email.Should().Be(exampleItem.Email);
                outputItem.IsActive.Should().Be(exampleItem.IsActive);
                outputItem.CreatedAt.Should().Be(exampleItem.CreatedAt);

                outputItem.HistoricPositions.Should().NotBeNull();
                foreach (var positionHistory in outputItem.HistoricPositions)
                {
                    var examplePositionHistory = examplePositionsHistoryList.FirstOrDefault(x => x.EmployeeId == exampleItem.Id && x.PositionId == positionHistory.PositionId);
                    examplePositionHistory.Should().NotBeNull();
                    examplePositionHistory!.Salary.Should().Be(positionHistory.Salary);
                    examplePositionHistory.StartDate.Should().Be(positionHistory.StartDate);
                    examplePositionHistory.FinishDate.Should().Be(positionHistory.FinishDate);
                    examplePositionHistory.IsActual.Should().Be(positionHistory.IsActual);
                }
            }
        }

        [Fact(DisplayName = "FindPaginatedListAsync() should get paginated list of employees with no filtered data")]
        [Trait("Infra.Data.EF", "Repositories / EmployeeRepository")]
        public async Task SearchRetursListAndTotal()
        {
            var dbContext = _fixture.CreateDbContext();

            var exampleEmployeesList = _fixture.GetValidEmployeesModelList(length: 15);

            await dbContext.AddRangeAsync(exampleEmployeesList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var searchInput = new GetEmployeesInput(1, 20, sort: "firstname", departmentId: null, firstName: "", lastName: "", document: "", email: "", isActive: null, "", null, null, "", null, null);
            var filters = new Dictionary<string, object?>
            {
                { "DepartmentId", searchInput.DepartmentId },
                { "FirstName", searchInput.FirstName },
                { "LastName", searchInput.LastName },
                { "Document", searchInput.Document },
                { "Email", searchInput.Email },
                { "IsActive", searchInput.IsActive },
            };

            var employeeRepository = new EmployeeRepository(dbContext, _httpContextAccessorMock.Object, _tenantServiceMock.Object);
            var output = await employeeRepository.FindPaginatedListAsync(filters, searchInput.PageNumber, searchInput.PageSize, searchInput.Sort!, CancellationToken.None);

            output.Should().NotBeNull();
            output.Items.Should().NotBeNull();
            output.TotalItems.Should().Be(exampleEmployeesList.Count());
            output.Items.Should().HaveCount(exampleEmployeesList.Count());
            foreach (var outputItem in output.Items)
            {
                var exampleItem = exampleEmployeesList.Find(employee => employee.Id == outputItem.Id);
                exampleItem.Should().NotBeNull();
                outputItem.DepartmentId.Should().Be(exampleItem!.DepartmentId);
                outputItem.FirstName.Should().Be(exampleItem.FirstName);
                outputItem.LastName.Should().Be(exampleItem.LastName);
                outputItem.Document.Should().Be(exampleItem.Document);
                outputItem.Email.Should().Be(exampleItem.Email);
                outputItem.IsActive.Should().Be(exampleItem.IsActive);
                outputItem.CreatedAt.Should().Be(exampleItem.CreatedAt);
            }
        }

        [Fact(DisplayName = "FindPaginatedListAsync() should return a empty list when database is clean")]
        [Trait("Infra.Data.EF", "Repositories / EmployeeRepository")]
        public async Task SearchRetursEmptyWhenPersistenceIsEmpty()
        {
            var dbContext = _fixture.CreateDbContext();
            var employeeRepository = new EmployeeRepository(dbContext, _httpContextAccessorMock.Object, _tenantServiceMock.Object);

            var searchInput = new GetEmployeesInput(1, 20, sort: "name", departmentId: null, firstName: "", lastName: "", document: "", email: "", isActive: null, "", null, null, "", null, null);
            var filters = new Dictionary<string, object?>
            {
                { "DepartmentId", searchInput.DepartmentId },
                { "FirstName", searchInput.FirstName },
                { "LastName", searchInput.LastName },
                { "Document", searchInput.Document },
                { "Email", searchInput.Email },
                { "IsActive", searchInput.IsActive },
            };

            var output = await employeeRepository.FindPaginatedListAsync(filters, searchInput.PageNumber, searchInput.PageSize, searchInput.Sort!, CancellationToken.None);

            output.Should().NotBeNull();
            output.Items.Should().NotBeNull();
            output.TotalItems.Should().Be(0);
            output.Items.Should().HaveCount(0);
        }

        [Fact(DisplayName = "FindByDocumentAsync() should get a employee by a valid document")]
        [Trait("Infra.Data.EF", "Repositories / EmployeeRepository")]
        public async Task GetByDocument()
        {
            var dbContext = _fixture.CreateDbContext();

            var exampleEmployee = _fixture.GetValidEmployeeModel();
            var exampleEmployeesList = _fixture.GetValidEmployeesModelList(length: 15);
            exampleEmployeesList.Add(exampleEmployee);

            await dbContext.AddRangeAsync(exampleEmployeesList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var employeeRepository = new EmployeeRepository(_fixture.CreateDbContext(true), _httpContextAccessorMock.Object, _tenantServiceMock.Object);

            var dbEmployee = await employeeRepository.FindByDocumentAsync(exampleEmployee.Document, CancellationToken.None);

            dbEmployee.Should().NotBeNull();
            dbEmployee!.FirstName.Should().Be(exampleEmployee.FirstName);
            dbEmployee.Id.Should().Be(exampleEmployee.Id);
            dbEmployee.Document.Should().Be(exampleEmployee.Document);
            dbEmployee.IsActive.Should().Be(exampleEmployee.IsActive);
            dbEmployee.CreatedAt.Should().Be(exampleEmployee.CreatedAt);
        }

        [Fact(DisplayName = "FindByDocumentAsync() should throw an error when employee not found")]
        [Trait("Infra.Data.EF", "Repositories / EmployeeRepository")]
        public async Task GetThrowIfDocumentNotFound()
        {
            var dbContext = _fixture.CreateDbContext();
            var document = _fixture.GetValidEmployeeDocument();

            await dbContext.AddRangeAsync(_fixture.GetValidEmployeesModelList(new List<Guid> { Guid.NewGuid() }, length: 15));
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var employeeRepository = new EmployeeRepository(dbContext, _httpContextAccessorMock.Object, _tenantServiceMock.Object);

            var task = async () => await employeeRepository.FindByDocumentAsync(document, CancellationToken.None);

            await task.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Employee with Document '{document}' not found.");
        }
    }
}

