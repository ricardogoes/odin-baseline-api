using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Odin.Baseline.Application.Positions.ChangeStatusPosition;
using Odin.Baseline.Application.Positions.Common;
using Odin.Baseline.Application.Positions.CreatePosition;
using Odin.Baseline.Application.Positions.GetPositionById;
using Odin.Baseline.Application.Positions.UpdatePosition;
using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Domain.Enums;

namespace Odin.Baseline.Api.Controllers.v1
{
    [ApiController]
    [Authorize]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/positions")]
    public class PositionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PositionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(PositionOutput), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
                throw new BadRequestException("Invalid request");

            var position = await _mediator.Send(new GetPositionByIdInput { Id = id }, cancellationToken);

            return Ok(position);
        }

        [HttpPost]
        [ProducesResponseType(typeof(PositionOutput), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Create([FromBody] CreatePositionInput input, CancellationToken cancellationToken)
        {
            //TODO: Alterar quando auth estiver implementado
            input.ChangeLoggedUsername("ricardo.goes");

            var positionCreated = await _mediator.Send(input, cancellationToken);

            return CreatedAtAction(
                actionName: nameof(GetById),
                routeValues: new { id = positionCreated.Id },
                value: positionCreated);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(PositionOutput), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdatePositionInput input, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty || id != input.Id)
                throw new BadRequestException("Invalid request");

            //TODO: Alterar quando auth estiver implementado
            input.ChangeLoggedUsername("ricardo.goes");

            var positionUpdated = await _mediator.Send(input, cancellationToken);

            return Ok(positionUpdated);
        }

        [HttpPut("{id:guid}/status")]
        [ProducesResponseType(typeof(PositionOutput), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ChangeStatus([FromRoute] Guid id, [FromQuery] string action, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty || string.IsNullOrWhiteSpace(action))
                throw new BadRequestException("Invalid request");

            if (action.ToUpper() != "ACTIVATE" && action.ToUpper() != "DEACTIVATE")
                throw new BadRequestException("Invalid action. Only ACTIVATE or DEACTIVATE values are allowed");

            var positionUpdated = await _mediator.Send(new ChangeStatusPositionInput
            (
                id,
                (ChangeStatusAction)Enum.Parse(typeof(ChangeStatusAction), action, true),
                "ricardo.goes" // TODO: Alterar quando auth estiver implementado
            ), cancellationToken);

            return Ok(positionUpdated);
        }
    }
}
