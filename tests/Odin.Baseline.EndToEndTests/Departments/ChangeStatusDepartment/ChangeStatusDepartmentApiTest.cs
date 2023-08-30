using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Odin.Baseline.Application.Departments.Common;
using System.Net;

namespace Odin.Baseline.EndToEndTests.Departments.ChangeStatusDepartment
{

    [Collection(nameof(ChangeStatusDepartmentApiTestCollection))]
    public class ChangeStatusDepartmentApiTest
    {
        private readonly ChangeStatusDepartmentApiTestFixture _fixture;

        public ChangeStatusDepartmentApiTest(ChangeStatusDepartmentApiTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = "Should activate a department")]
        [Trait("E2E/Controllers", "Departments / [v1]ChangeStatusDepartment")]
        public async Task ActivateDepartment()
        {
            var departmentsList = _fixture.GetValidDepartmentsModelList(20);

            var dbContext = _fixture.CreateDbContext(preserveData: true);
            await dbContext.AddRangeAsync(departmentsList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var departmentToChangeStatus = departmentsList.Where(x => !x.IsActive).FirstOrDefault();

            var input = _fixture.GetValidInputToActivate(departmentToChangeStatus.Id) ;

            var (response, output) = await _fixture.ApiClient.PutAsync<DepartmentOutput>($"/v1/departments/{departmentToChangeStatus.Id}/status?action=ACTIVATE", input);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            output.Should().NotBeNull();
            output.Name.Should().Be(departmentToChangeStatus.Name);
            output.IsActive.Should().BeTrue();
            output.LastUpdatedAt.Should().NotBeSameDateAs(default);
        }

        [Fact(DisplayName = "Should deactivate a department")]
        [Trait("E2E/Controllers", "Departments / [v1]ChangeStatusDepartment")]
        public async Task DeactivateDepartment()
        {
            var departmentsList = _fixture.GetValidDepartmentsModelList(20);

            var dbContext = _fixture.CreateDbContext(preserveData: true);
            await dbContext.AddRangeAsync(departmentsList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var departmentToChangeStatus = departmentsList.Where(x => x.IsActive).FirstOrDefault();

            var input = _fixture.GetValidInputToDeactivate(departmentToChangeStatus.Id);

            var (response, output) = await _fixture.ApiClient.PutAsync<DepartmentOutput>($"/v1/departments/{departmentToChangeStatus.Id}/status?action=DEACTIVATE", input);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            output.Should().NotBeNull();
            output.Name.Should().Be(departmentToChangeStatus.Name);
            output.IsActive.Should().BeFalse();
            output.LastUpdatedAt.Should().NotBeSameDateAs(default);
        }

        [Fact(DisplayName = "Should throw an error when Id is empty")]
        [Trait("E2E/Controllers", "Departments / [v1]ChangeStatusDepartment")]
        public async Task ErrorWhenInvalidIds()
        {            
            var (response, output) = await _fixture.ApiClient.PutAsync<ProblemDetails>($"/v1/departments/{Guid.Empty}/status?action=ACTIVATE", null);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            output.Should().NotBeNull();
            output.Title.Should().Be("Bad request");
            output.Type.Should().Be("BadRequest");
            output.Status.Should().Be(StatusCodes.Status400BadRequest);
            output.Detail.Should().Be("Invalid request");
        }

        [Fact(DisplayName = "Should throw an error when action is invalid")]
        [Trait("E2E/Controllers", "Departments / [v1]ChangeStatusDepartment")]
        public async Task ErrorWhenInvalidAction()
        {

            var departmentToChangeStatus = _fixture.GetValidDepartment();

            var input = _fixture.GetInputWithInvalidAction(departmentToChangeStatus.Id);

            var (response, output) = await _fixture.ApiClient.PutAsync<ProblemDetails>($"/v1/departments/{Guid.NewGuid()}/status?action=INVALID", input);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            output.Should().NotBeNull();
            output.Title.Should().Be("Bad request");
            output.Type.Should().Be("BadRequest");
            output.Status.Should().Be(StatusCodes.Status400BadRequest);
            output.Detail.Should().Be("Invalid action. Only ACTIVATE or DEACTIVATE values are allowed");
        }


        [Fact(DisplayName = "Should throw an error when department not found")]
        [Trait("E2E/Controllers", "Departments / [v1]ChangeStatusDepartment")]
        public async Task ErrorWhenNotFound()
        {
            var departmentsList = _fixture.GetValidDepartmentsModelList(20);

            var dbContext = _fixture.CreateDbContext(preserveData: true);
            await dbContext.AddRangeAsync(departmentsList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var idToQuery = Guid.NewGuid();
            var input = _fixture.GetValidInputToActivate(idToQuery);

            var (response, output) = await _fixture.ApiClient.PutAsync<ProblemDetails>($"/v1/departments/{idToQuery}/status?action=ACTIVATE", input);

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);
            output.Should().NotBeNull();
            output!.Status.Should().Be((int)StatusCodes.Status404NotFound);
            output.Type.Should().Be("NotFound");
            output.Title.Should().Be("Not found");
            output.Detail.Should().Be($"Department with Id '{idToQuery}' not found.");
        }
    }
}
