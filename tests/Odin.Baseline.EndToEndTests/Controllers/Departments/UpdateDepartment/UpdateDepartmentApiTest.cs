﻿using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Odin.Baseline.Application.Departments;
using Odin.Baseline.Application.Departments.UpdateDepartment;
using System.Net;
using System.Text.Json;

namespace Odin.Baseline.EndToEndTests.Controllers.Departments.UpdateDepartment
{

    [Collection(nameof(UpdateDepartmentApiTestCollection))]
    public class UpdateDepartmentApiTest
    {
        private readonly UpdateDepartmentApiTestFixture _fixture;

        public UpdateDepartmentApiTest(UpdateDepartmentApiTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = "Should update a valid department")]
        [Trait("E2E/Controllers", "Departments / [v1]UpdateDepartment")]
        public async Task UpdateValidDepartment()
        {
            var departmentsList = _fixture.GetValidDepartmentsModelList(length: 20);

            var dbContext = _fixture.CreateDbContext(preserveData: true);
            await dbContext.AddRangeAsync(departmentsList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var departmentToUpdate = departmentsList[10];

            var input = _fixture.GetValidInput(id: departmentToUpdate.Id);

            var (response, output) = await _fixture.ApiClient.PutAsync<DepartmentOutput>($"/v1/departments/{departmentToUpdate.Id}", input);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            output.Should().NotBeNull();
            output.Name.Should().Be(input.Name);
            output.IsActive.Should().Be(departmentToUpdate.IsActive);
            output.LastUpdatedAt.Should().NotBeSameDateAs(default);
        }

        [Fact(DisplayName = "Should throw an error when Id from route is different of Id from department")]
        [Trait("E2E/Controllers", "Departments / [v1]UpdateDepartment")]
        public async Task ErrorWhenInvalidIds()
        {

            var departmentsList = _fixture.GetValidDepartmentsModelList(length: 20);

            var dbContext = _fixture.CreateDbContext(preserveData: true);
            await dbContext.AddRangeAsync(departmentsList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var departmentToUpdate = departmentsList[10];

            var input = _fixture.GetValidInput(departmentToUpdate.Id);

            var (response, output) = await _fixture.ApiClient.PutAsync<ProblemDetails>($"/v1/departments/{Guid.NewGuid()}", input);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            output.Should().NotBeNull();
            output.Title.Should().Be("Bad request");
            output.Type.Should().Be("BadRequest");
            output.Status.Should().Be(StatusCodes.Status400BadRequest);
            output.Detail.Should().Be("Invalid request");
        }

        [Theory(DisplayName = "Should throw an error with invalid data")]
        [Trait("E2E/Controllers", "Departments / [v1]UpdateDepartment")]
        [MemberData(
            nameof(UpdateDepartmentApiTestDataGenerator.GetInvalidInputs),
            MemberType = typeof(UpdateDepartmentApiTestDataGenerator)
        )]
        public async Task ErrorWhenCantInstantiateDepartment(UpdateDepartmentInput input, string property, string expectedDetail)
        {
            var departmentsList = _fixture.GetValidDepartmentsModelList(length: 20);

            var dbContext = _fixture.CreateDbContext(preserveData: true);
            await dbContext.AddRangeAsync(departmentsList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var departmentToUpdate = departmentsList[10];
            input.ChangeId(departmentToUpdate.Id);

            var (response, output) = await _fixture.ApiClient.PutAsync<ProblemDetails>($"/v1/departments/{input.Id}", input);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);

            output.Should().NotBeNull();
            output.Title.Should().Be("Unprocessable entity");
            output.Status.Should().Be(StatusCodes.Status422UnprocessableEntity);

            output.Extensions["errors"].Should().NotBeNull();
            var errors = JsonSerializer.Deserialize<Dictionary<string, string[]>>(JsonSerializer.Serialize(output.Extensions["errors"]))!;
            errors.ContainsKey(property).Should().BeTrue();
            errors[property].First().Should().Be(expectedDetail);
        }

        [Fact(DisplayName = "Should throw an error when department not found")]
        [Trait("E2E/Controllers", "Departments / [v1]UpdateDepartment")]
        public async Task ErrorWhenNotFound()
        {
            var departmentsList = _fixture.GetValidDepartmentsModelList(length: 20);

            var dbContext = _fixture.CreateDbContext(preserveData: true);
            await dbContext.AddRangeAsync(departmentsList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var idToQuery = Guid.NewGuid();
            var input = _fixture.GetValidInput(idToQuery);

            var (response, output) = await _fixture.ApiClient.PutAsync<ProblemDetails>($"/v1/departments/{idToQuery}", input);

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);
            output.Should().NotBeNull();
            output!.Status.Should().Be(StatusCodes.Status404NotFound);
            output.Type.Should().Be("NotFound");
            output.Title.Should().Be("Not found");
            output.Detail.Should().Be($"Department with Id '{idToQuery}' not found.");
        }
    }
}
