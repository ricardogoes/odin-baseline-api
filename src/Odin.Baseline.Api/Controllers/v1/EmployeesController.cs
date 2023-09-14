using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Odin.Baseline.Application.Employees.AddPosition;
using Odin.Baseline.Application.Employees.ChangeAddressEmployee;
using Odin.Baseline.Application.Employees.ChangeStatusEmployee;
using Odin.Baseline.Application.Employees.Common;
using Odin.Baseline.Application.Employees.CreateEmployee;
using Odin.Baseline.Application.Employees.GetEmployeeById;
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
            //TODO: Alterar quando auth estiver implementado
            input.ChangeLoggedUsername("ricardo.goes");

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

            //TODO: Alterar quando auth estiver implementado
            input.ChangeLoggedUsername("ricardo.goes");

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
                (ChangeStatusAction)Enum.Parse(typeof(ChangeStatusAction), action, true),
                "ricardo.goes" // TODO: Alterar quando auth estiver implementado
            ), cancellationToken);

            return Ok(employeeUpdated);
        }

        [HttpPut("{id:guid}/addresses")]
        [ProducesResponseType(typeof(EmployeeOutput), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ChangeAddress([FromRoute] Guid id, [FromBody] ChangeAddressEmployeeInput input, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty || id != input.EmployeeId)
                throw new BadRequestException("Invalid request");

            var employeeUpdated = await _mediator.Send(input, cancellationToken);

            return Ok(employeeUpdated);
        }

        [HttpPost("{id:guid}/positions")]
        [ProducesResponseType(typeof(EmployeeOutput), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddPosition([FromRoute] Guid id, [FromBody] AddPositionInput input, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty || id != input.EmployeeId)
                throw new BadRequestException("Invalid request");

            var employeeUpdated = await _mediator.Send(input, cancellationToken);

            return Ok(employeeUpdated);
        }
    }
}
