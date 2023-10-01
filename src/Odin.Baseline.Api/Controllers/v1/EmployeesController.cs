using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Odin.Baseline.Api.Helpers;
using Odin.Baseline.Api.Models;
using Odin.Baseline.Api.Models.Employees;
using Odin.Baseline.Application.Employees;
using Odin.Baseline.Application.Employees.AddPosition;
using Odin.Baseline.Application.Employees.ChangeAddressEmployee;
using Odin.Baseline.Application.Employees.ChangeStatusEmployee;
using Odin.Baseline.Application.Employees.CreateEmployee;
using Odin.Baseline.Application.Employees.GetEmployeeById;
using Odin.Baseline.Application.Employees.GetEmployees;
using Odin.Baseline.Application.Employees.UpdateEmployee;
using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Domain.Enums;

namespace Odin.Baseline.Api.Controllers.v1
{
    [ApiController]
    [Authorize]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/employees")]
    public class EmployeesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EmployeesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(EmployeeOutput), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(
            CancellationToken cancellationToken,
            [FromQuery(Name = "page_number")] int? pageNumber = null,
            [FromQuery(Name = "page_size")] int? pageSize = null,
            [FromQuery(Name = "sort")] string? sort = null,
            [FromQuery(Name = "department_id")] Guid? departmentId = null,
            [FromQuery(Name = "first_name")] string? firstName = null,
            [FromQuery(Name = "last_name")] string? lastName = null,
            [FromQuery(Name = "document")] string? document = null,
            [FromQuery(Name = "email")] string? email = null,
            [FromQuery(Name = "is_active")] bool? isActive = null,
            [FromQuery(Name = "created_by")] string? createdBy = null,
            [FromQuery(Name = "last_updated_by")] string? lastUpdatedBy = null,
            [FromQuery(Name = "created_at_start")] DateTime? createdAtStart = null,
            [FromQuery(Name = "created_at_end")] DateTime? createdAtEnd = null,
            [FromQuery(Name = "last_updated_at_start")] DateTime? LastUpdatedAtStart = null,
            [FromQuery(Name = "last_updated_at_end")] DateTime? LastUpdatedAtEnd = null)
        {
            var input = new GetEmployeesInput
            (
                page: pageNumber ?? 1,
                pageSize: pageSize ?? 5,
                sort: Utils.GetSortParam(sort),
                departmentId: departmentId,
                firstName: firstName,
                lastName: lastName,
                document: document,
                email: email,
                isActive: isActive,
                createdBy: createdBy,
                createdAtStart: createdAtStart,
                createdAtEnd: createdAtEnd,
                lastUpdatedBy: lastUpdatedBy,
                lastUpdatedAtStart: LastUpdatedAtStart,
                lastUpdatedAtEnd: LastUpdatedAtEnd
            );

            var paginatedCustomers = await _mediator.Send(input, cancellationToken);

            return Ok(new PaginatedApiResponse<EmployeeOutput>(paginatedCustomers));
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(EmployeeOutput), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
                throw new BadRequestException("Invalid request");

            var employee = await _mediator.Send(new GetEmployeeByIdInput { Id = id }, cancellationToken);

            return Ok(employee);
        }

        [HttpPost]
        [ProducesResponseType(typeof(EmployeeOutput), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Create([FromBody] CreateEmployeeInput input, CancellationToken cancellationToken)
        {
            var employeeCreated = await _mediator.Send(input, cancellationToken);

            return CreatedAtAction(
                actionName: nameof(GetById),
                routeValues: new { id = employeeCreated.Id },
                value: employeeCreated);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(EmployeeOutput), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateEmployeeInput input, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty || id != input.Id)
                throw new BadRequestException("Invalid request");

            var employeeUpdated = await _mediator.Send(input, cancellationToken);

            return Ok(employeeUpdated);
        }

        [HttpPut("{id:guid}/status")]
        [ProducesResponseType(typeof(EmployeeOutput), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ChangeStatus([FromRoute] Guid id, [FromQuery] string action, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty || string.IsNullOrWhiteSpace(action))
                throw new BadRequestException("Invalid request");

            if (action.ToUpper() != "ACTIVATE" && action.ToUpper() != "DEACTIVATE")
                throw new BadRequestException("Invalid action. Only ACTIVATE or DEACTIVATE values are allowed");

            var employeeUpdated = await _mediator.Send(new ChangeStatusEmployeeInput
            (
                id,
                (ChangeStatusAction)Enum.Parse(typeof(ChangeStatusAction), action, true)
            ), cancellationToken);

            return Ok(employeeUpdated);
        }

        [HttpPut("{id:guid}/addresses")]
        [ProducesResponseType(typeof(EmployeeOutput), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ChangeAddress([FromRoute] Guid id, [FromBody] ChangeAddressEmployeeApiRequest request, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
                throw new BadRequestException("Invalid request");

            var input = new ChangeAddressEmployeeInput(id, request.StreetName, request.StreetNumber, request.Neighborhood, request.ZipCode, request.City, request.State, request.Complement);
            var employeeUpdated = await _mediator.Send(input, cancellationToken);

            return Ok(employeeUpdated);
        }

        [HttpPost("{id:guid}/positions")]
        [ProducesResponseType(typeof(EmployeeOutput), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddPosition([FromRoute] Guid id, [FromBody] AddPositionApiRequest request, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
                throw new BadRequestException("Invalid request");

            var input = new AddPositionInput(id, request.PositionId, request.Salary, request.StartDate, request.FinishDate);
            var employeeUpdated = await _mediator.Send(input, cancellationToken);

            return Ok(employeeUpdated);
        }
    }
}
