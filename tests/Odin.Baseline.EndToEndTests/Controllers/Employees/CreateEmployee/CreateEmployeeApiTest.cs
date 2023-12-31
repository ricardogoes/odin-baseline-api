﻿using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Odin.Baseline.Application.Employees;
using Odin.Baseline.Application.Employees.CreateEmployee;
using System.Net;
using System.Text.Json;

namespace Odin.Baseline.EndToEndTests.Controllers.Employees.CreateEmployee
{

    [Collection(nameof(CreateEmployeeApiTestCollection))]
    public class CreateEmployeeApiTest
    {
        private readonly CreateEmployeeApiTestFixture _fixture;

        public CreateEmployeeApiTest(CreateEmployeeApiTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = "Should insert a valid employee")]
        [Trait("E2E/Controllers", "Employees / [v1]CreateEmployee")]
        public async Task InsertValidEmployee()
        {
            var dbContext = _fixture.CreateDbContext();

            var department = _fixture.GetValidDepartmentModel();

            await dbContext.AddAsync(department);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var input = _fixture.GetValidCreateEmployeeInput(department.Id);

            var (response, output) = await _fixture.ApiClient.PostAsync<EmployeeOutput>("/v1/employees", input);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            output.FirstName.Should().Be(input.FirstName);
            output.LastName.Should().Be(input.LastName);
            output.Email.Should().Be(input.Email);
            output.Document.Should().Be(input.Document);
            output.IsActive.Should().BeTrue();
            output.CreatedAt.Should().NotBeSameDateAs(default);
        }

        [Fact(DisplayName = "Should insert a valid employee without department")]
        [Trait("E2E/Controllers", "Employees / [v1]CreateEmployee")]
        public async Task InsertValidEmployeeWithoutDepartment()
        {
            var input = _fixture.GetValidCreateEmployeeInput();

            var (response, output) = await _fixture.ApiClient.PostAsync<EmployeeOutput>("/v1/employees", input);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            output.FirstName.Should().Be(input.FirstName);
            output.LastName.Should().Be(input.LastName);
            output.Email.Should().Be(input.Email);
            output.Document.Should().Be(input.Document);
            output.IsActive.Should().BeTrue();
            output.CreatedAt.Should().NotBeSameDateAs(default);
        }

        [Fact(DisplayName = "Should throw an error when document is duplicated")]
        [Trait("E2E/Controllers", "Employees / [v1]CreateEmployee")]
        public async Task ThrowErrorWithDuplicatedDocument()
        {
            var department = _fixture.GetValidDepartmentModel();
            var employeesList = _fixture.GetValidEmployeesModelList(new List<Guid> { department.Id }, 20);

            var dbContext = _fixture.CreateDbContext(preserveData: true);
            await dbContext.AddAsync(department);
            await dbContext.AddRangeAsync(employeesList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var input = _fixture.GetValidCreateEmployeeInput();
            input.ChangeDocument(employeesList.First().Document);

            var (response, output) = await _fixture.ApiClient.PostAsync<ProblemDetails>($"/v1/employees", input);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
            output.Should().NotBeNull();
            output.Title.Should().Be("Unprocessable entity");
            output.Type.Should().Be("UnprocessableEntity");
            output.Status.Should().Be(StatusCodes.Status422UnprocessableEntity);
            output.Detail.Should().Be("Document must be unique");
        }

        [Theory(DisplayName = "Should throw an error with invalid data")]
        [Trait("E2E/Controllers", "Employees / [v1]CreateEmployee")]
        [MemberData(
            nameof(CreateEmployeeApiTestDataGenerator.GetInvalidInputs),
            MemberType = typeof(CreateEmployeeApiTestDataGenerator)
        )]
        public async Task ErrorWhenCantInstantiateEmployee(CreateEmployeeInput input, string property, string expectedDetail)
        {
            var (response, output) = await _fixture.ApiClient.PostAsync<ProblemDetails>("/v1/employees", input);

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
    }
}
