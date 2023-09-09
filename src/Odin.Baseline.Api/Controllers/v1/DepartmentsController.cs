using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Odin.Baseline.Api.Helpers;
using Odin.Baseline.Api.Models;
using Odin.Baseline.Application.Departments.ChangeStatusDepartment;
using Odin.Baseline.Application.Departments.Common;
using Odin.Baseline.Application.Departments.CreateDepartment;
using Odin.Baseline.Application.Departments.GetDepartmentById;
using Odin.Baseline.Application.Departments.UpdateDepartment;
using Odin.Baseline.Application.Employees.Common;
using Odin.Baseline.Application.Employees.GetEmployees;
using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Enums;

namespace Odin.Baseline.Api.Controllers.v1
{
    [ApiController]
    [Authorize]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/departments")]
    public class DepartmentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DepartmentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(DepartmentOutput), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
                throw new BadRequestException("Invalid request");

            var department = await _mediator.Send(new GetDepartmentByIdInput { Id = id }, cancellationToken);

            return Ok(department);
        }

        [HttpPost]
        [ProducesResponseType(typeof(DepartmentOutput), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Create([FromBody] CreateDepartmentInput input, CancellationToken cancellationToken)
        {
            //TODO: Alterar quando auth estiver implementado
            input.LoggedUsername = "ricardo.goes";

            var departmentCreated = await _mediator.Send(input, cancellationToken);

            return CreatedAtAction(
                actionName: nameof(GetById),
                routeValues: new { id = departmentCreated.Id },
                value: departmentCreated);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(DepartmentOutput), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateDepartmentInput input, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty || id != input.Id)
                throw new BadRequestException("Invalid request");

            //TODO: Alterar quando auth estiver implementado
            input.LoggedUsername = "ricardo.goes";

            var departmentUpdated = await _mediator.Send(input, cancellationToken);

            return Ok(departmentUpdated);
        }

        [HttpPut("{id:guid}/status")]
        [ProducesResponseType(typeof(DepartmentOutput), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ChangeStatus([FromRoute] Guid id, [FromQuery] string action, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty || string.IsNullOrWhiteSpace(action))
                throw new BadRequestException("Invalid request");

            if (action.ToUpper() != "ACTIVATE" && action.ToUpper() != "DEACTIVATE")
                throw new BadRequestException("Invalid action. Only ACTIVATE or DEACTIVATE values are allowed");

            var departmentUpdated = await _mediator.Send(new ChangeStatusDepartmentInput
            {
                Id = id,
                Action = (ChangeStatusAction)Enum.Parse(typeof(ChangeStatusAction), action, true),
                LoggedUsername = "ricardo.goes" // TODO: Alterar quando auth estiver implementado
            }, cancellationToken);

            return Ok(departmentUpdated);
        }

        [HttpGet("{id:guid}/employees")]
        [ProducesResponseType(typeof(DepartmentOutput), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetEmployeesByDepartment([FromRoute] Guid id,
            CancellationToken cancellationToken,
            [FromQuery(Name = "page_number")] int? pageNumber = null,
            [FromQuery(Name = "page_size")] int? pageSize = null,
            [FromQuery(Name = "sort")] string? sort = null,
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
            if (id == Guid.Empty)
                throw new BadRequestException("Invalid request");

            var input = new GetEmployeesInput
             (
                 page: pageNumber ?? 1,
                 pageSize: pageSize ?? 5,
                 sort: Utils.GetSortParam(sort),
                 customerId: null,
                 departmentId: id,
                 firstName: firstName ?? "",
                 lastName: lastName ?? "",
                 document: document ?? "",
                 email: email ?? "",
                 isActive: isActive,
                 createdBy: createdBy,
                 createdAtStart: createdAtStart,
                 createdAtEnd: createdAtEnd,
                 lastUpdatedBy: lastUpdatedBy,
                 lastUpdatedAtStart: LastUpdatedAtStart,
                 lastUpdatedAtEnd: LastUpdatedAtEnd
             ); ;

            var paginatedEmployees = await _mediator.Send(input, cancellationToken);
            return Ok(new PaginatedApiResponse<EmployeeOutput>(paginatedEmployees));
        }
    }
}
