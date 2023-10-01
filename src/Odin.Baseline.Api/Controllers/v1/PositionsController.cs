using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Odin.Baseline.Api.Helpers;
using Odin.Baseline.Api.Models;
using Odin.Baseline.Application.Positions;
using Odin.Baseline.Application.Positions.ChangeStatusPosition;
using Odin.Baseline.Application.Positions.CreatePosition;
using Odin.Baseline.Application.Positions.GetPositionById;
using Odin.Baseline.Application.Positions.GetPositions;
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

        [HttpGet()]
        [ProducesResponseType(typeof(PositionOutput), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll(
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
            var input = new GetPositionsInput(
                page: pageNumber ?? 1,
                pageSize: pageSize ?? 5,
                sort: Utils.GetSortParam(sort),
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
                (ChangeStatusAction)Enum.Parse(typeof(ChangeStatusAction), action, true)
            ), cancellationToken);

            return Ok(positionUpdated);
        }
    }
}
