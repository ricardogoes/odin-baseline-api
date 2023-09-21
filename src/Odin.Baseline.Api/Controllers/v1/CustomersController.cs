using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Odin.Baseline.Api.Helpers;
using Odin.Baseline.Api.Models;
using Odin.Baseline.Api.Models.Customers;
using Odin.Baseline.Application.Customers.ChangeAddressCustomer;
using Odin.Baseline.Application.Customers.ChangeStatusCustomer;
using Odin.Baseline.Application.Customers.Common;
using Odin.Baseline.Application.Customers.CreateCustomer;
using Odin.Baseline.Application.Customers.GetCustomerById;
using Odin.Baseline.Application.Customers.GetCustomers;
using Odin.Baseline.Application.Customers.UpdateCustomer;
using Odin.Baseline.Application.Departments.Common;
using Odin.Baseline.Application.Departments.GetDepartments;
using Odin.Baseline.Application.Employees.Common;
using Odin.Baseline.Application.Employees.GetEmployees;
using Odin.Baseline.Application.Positions.Common;
using Odin.Baseline.Application.Positions.GetPositions;
using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Domain.Enums;

namespace Odin.Baseline.Api.Controllers.v1
{
    [ApiController]
    [Authorize]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/customers")]
    public class CustomersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(CustomerOutput), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(
            CancellationToken cancellationToken,
            [FromQuery(Name = "page_number")] int? PageNumber = null,
            [FromQuery(Name = "page_size")] int? PageSize = null,
            [FromQuery(Name = "sort")] string? Sort = null,
            [FromQuery(Name = "name")] string? Name = null,
            [FromQuery(Name = "document")] string? Document = null,
            [FromQuery(Name = "is_active")] bool? IsActive = null)
        {
            var input = new GetCustomersInput
            (
                page: PageNumber ?? 1,
                pageSize: PageSize ?? 10,
                sort: Utils.GetSortParam(Sort),
                name: !string.IsNullOrWhiteSpace(Name) ? Name : "",
                document: !string.IsNullOrWhiteSpace(Document) ? Document : "",
                isActive: IsActive
            );

            var paginatedCustomers = await _mediator.Send(input, cancellationToken);

            return Ok(new PaginatedApiResponse<CustomerOutput>(paginatedCustomers));
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(CustomerOutput), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
                throw new BadRequestException("Invalid request");

            var customer = await _mediator.Send(new GetCustomerByIdInput { Id = id }, cancellationToken);

            return Ok(customer);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CustomerOutput), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Create([FromBody] CreateCustomerApiRequest request, CancellationToken cancellationToken)
        {

            var loggedUsername = User.Identity!.Name!;
            var input = new CreateCustomerInput(request.Name, request.Document, loggedUsername);

            var customerCreated = await _mediator.Send(input, cancellationToken);

            return CreatedAtAction(
                actionName: nameof(GetById),
                routeValues: new { id = customerCreated.Id },
                value: customerCreated);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(CustomerOutput), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateCustomerApiRequest request, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty || id != request.Id)
                throw new BadRequestException("Invalid request");

            var loggedUsername = User.Identity!.Name!;
            var input = new UpdateCustomerInput(request.Id, request.Name, request.Document, loggedUsername);

            var customerUpdated = await _mediator.Send(input, cancellationToken);

            return Ok(customerUpdated);
        }

        [HttpPut("{id:guid}/status")]
        [ProducesResponseType(typeof(CustomerOutput), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ChangeStatus([FromRoute] Guid id, [FromQuery] string action, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty || string.IsNullOrWhiteSpace(action))
                throw new BadRequestException("Invalid request");

            if (action.ToUpper() != "ACTIVATE" && action.ToUpper() != "DEACTIVATE")
                throw new BadRequestException("Invalid action. Only ACTIVATE or DEACTIVATE values are allowed");

            var loggedUsername = User.Identity!.Name!;

            var customerUpdated = await _mediator.Send(new ChangeStatusCustomerInput
            (
                id,
                (ChangeStatusAction)Enum.Parse(typeof(ChangeStatusAction), action, true),
                loggedUsername
            ), cancellationToken);

            return Ok(customerUpdated);
        }

        [HttpPut("{id:guid}/addresses")]
        [ProducesResponseType(typeof(EmployeeOutput), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ChangeAddress([FromRoute] Guid id, [FromBody] ChangeAddressCustomerApiRequest request, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty || id != request.CustomerId)
                throw new BadRequestException("Invalid request");

            var loggedUsername = User.Identity!.Name!;
            var input = new ChangeAddressCustomerInput(id, request.StreetName, request.StreetNumber, request.Neighborhood, 
                request.ZipCode, request.City, request.State, loggedUsername, request.Complement);

            var customerUpdated = await _mediator.Send(input, cancellationToken);

            return Ok(customerUpdated);
        }

        [HttpGet("{id:guid}/departments")]
        [ProducesResponseType(typeof(CustomerOutput), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetDepartmentsByCustomer([FromRoute] Guid id,
            CancellationToken cancellationToken,
            [FromQuery(Name = "page_number")] int? pageNumber = null,
            [FromQuery(Name = "page_size")] int? pageSize = null,
            [FromQuery(Name = "sort")] string? sort = null,
            [FromQuery(Name = "name")] string? name = null,
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

            var input = new GetDepartmentsInput(
                page: pageNumber ?? 1,
                pageSize: pageSize ?? 5,
                sort: Utils.GetSortParam(sort),
                customerId: id,
                name: name,
                isActive: isActive,
                createdBy: createdBy,
                createdAtStart: createdAtStart,
                createdAtEnd: createdAtEnd,
                lastUpdatedBy: lastUpdatedBy,
                lastUpdatedAtStart: LastUpdatedAtStart,
                lastUpdatedAtEnd: LastUpdatedAtEnd);            

            var paginatedDepartments = await _mediator.Send(input, cancellationToken);
            return Ok(new PaginatedApiResponse<DepartmentOutput>(paginatedDepartments));

        }
        
        [HttpGet("{id:guid}/positions")]
        [ProducesResponseType(typeof(CustomerOutput), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPositionsByCustomer([FromRoute] Guid id,
            CancellationToken cancellationToken,
            [FromQuery(Name = "page_number")] int? pageNumber = null,
            [FromQuery(Name = "page_size")] int? pageSize = null,
            [FromQuery(Name = "sort")] string? sort = null,
            [FromQuery(Name = "name")] string? name = null,
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

            var input = new GetPositionsInput(
                page: pageNumber ?? 1,
                pageSize: pageSize ?? 5,
                sort: Utils.GetSortParam(sort),
                customerId: id,
                name: name,
                isActive: isActive,
                createdBy: createdBy,
                createdAtStart: createdAtStart,
                createdAtEnd: createdAtEnd,
                lastUpdatedBy: lastUpdatedBy,
                lastUpdatedAtStart: LastUpdatedAtStart,
                lastUpdatedAtEnd: LastUpdatedAtEnd);

            var paginatedPositions = await _mediator.Send(input, cancellationToken);
            return Ok(new PaginatedApiResponse<PositionOutput>(paginatedPositions));
        }

        [HttpGet("{id:guid}/employees")]
        [ProducesResponseType(typeof(CustomerOutput), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetEmployeesByCustomer([FromRoute] Guid id,
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
            if (id == Guid.Empty)
                throw new BadRequestException("Invalid request");

            var input = new GetEmployeesInput
            (
                page: pageNumber ?? 1,
                pageSize: pageSize ?? 5,
                sort: Utils.GetSortParam(sort),
                customerId: id,
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
            );;

            var paginatedEmployees = await _mediator.Send(input, cancellationToken);
            return Ok(new PaginatedApiResponse<EmployeeOutput>(paginatedEmployees));
        }
    }
}
