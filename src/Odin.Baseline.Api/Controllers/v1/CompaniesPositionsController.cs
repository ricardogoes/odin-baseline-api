using Microsoft.AspNetCore.Mvc;
using Odin.Baseline.Api.Attributes;
using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Domain.Interfaces.Services;
using Odin.Baseline.Domain.Models;
using Odin.Baseline.Domain.QueryModels;
using Odin.Baseline.Domain.ViewModels.CompaniesPositions;

namespace Odin.Baseline.Api.Controllers.v1
{
    [ApiController]
    //[Authorize]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/positions")]
    public class CompaniesPositionsController : BaseController
    {
        private readonly ICompaniesPositionsService _positionsService;
        private readonly IEmployeesService _employeesService;

        public CompaniesPositionsController(AppSettings appSettings, ICompaniesPositionsService positionsService, IEmployeesService employessService,
            ILogger<CompaniesPositionsController> logger)
            : base(appSettings, logger)
        {
            _positionsService = positionsService ?? throw new ArgumentNullException(nameof(positionsService));
            _employeesService = employessService ?? throw new ArgumentNullException(nameof(employessService));
        }

        [HttpGet("{positionId}")]
        public async Task<IActionResult> GetCompanyPositionByIdAsync(int positionId)
        {
            try
            {
                if (positionId <= 0)
                    throw new BadRequestException("Invalid request");

                var position = await _positionsService.GetByIdAsync(positionId, _cancellationToken);
                if (position is null)
                    throw new NotFoundException("CompanyPosition not found");

                return Ok(new ApiResponse(ApiResponseState.Success, position));
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new ApiResponse(ApiResponseState.Failed, ex.Message));
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ApiResponse(ApiResponseState.Failed, ex.Message));
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> InsertCompanyPositionAsync([FromBody] CompanyPositionToInsert positionToInsert)
        {
            try
            {
                var positionInserted = await _positionsService.InsertAsync(positionToInsert, _loggedUsername, _cancellationToken);

                return CreatedAtAction(
                    controllerName: "CompaniesPositions",
                    actionName: "GetCompanyPositionById",
                    routeValues: new { positionId = positionInserted.PositionId },
                    value: new ApiResponse(ApiResponseState.Success, positionInserted));
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new ApiResponse(ApiResponseState.Failed, ex.Message));
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPut("{positionId}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateCompanyPositionAsync(int positionId, [FromBody] CompanyPositionToUpdate positionToUpdate)
        {
            try
            {
                if (positionId <= 0 || positionId != positionToUpdate.PositionId)
                    throw new BadRequestException("Invalid request");

                var positionUpdated = await _positionsService.UpdateAsync(positionToUpdate, _loggedUsername, _cancellationToken);

                return Ok(new ApiResponse(ApiResponseState.Success, positionUpdated));
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new ApiResponse(ApiResponseState.Failed, ex.Message));
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPut("{positionId}/status")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> ChangeStatusCompanyPositionAsync(int positionId)
        {
            try
            {
                if (positionId <= 0)
                    throw new BadRequestException("Invalid request");

                var positionUpdated = await _positionsService.ChangeStatusAsync(positionId, _loggedUsername, _cancellationToken);
                return Ok(new ApiResponse(ApiResponseState.Success, positionUpdated));
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new ApiResponse(ApiResponseState.Failed, ex.Message));
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet("{positionId}/employees")]
        public async Task<IActionResult> GetEmployeesByPositionAsync(int positionId, [FromQuery] EmployeesQueryModel queryData)
        {
            try
            {
                if (positionId <= 0)
                    throw new BadRequestException("Invalid request");

                var employees = await _employeesService.GetByCompanyPositionAsync(positionId, queryData, _cancellationToken);

                return Ok(new ApiResponse(ApiResponseState.Success, employees));
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new ApiResponse(ApiResponseState.Failed, ex.Message));
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }
    }
}
