using Microsoft.AspNetCore.Mvc;
using Odin.Baseline.Api.Attributes;
using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Domain.Interfaces.Services;
using Odin.Baseline.Domain.Models;
using Odin.Baseline.Domain.ViewModels.Employees;

namespace Odin.Baseline.Api.Controllers.v1
{
    [ApiController]
    //[Authorize]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/employees")]
    public class EmployeesController : BaseController
    {
        private readonly IEmployeesService _employeesService;

        public EmployeesController(AppSettings appSettings, IEmployeesService employeesService,
            ILogger<EmployeesController> logger)
            : base(appSettings, logger)
        {
            _employeesService = employeesService ?? throw new ArgumentNullException(nameof(employeesService));
        }

        [HttpGet("{employeeId}")]
        public async Task<IActionResult> GetEmployeeByIdAsync(int employeeId)
        {
            try
            {
                if (employeeId <= 0)
                    throw new BadRequestException("Invalid request");

                var employee = await _employeesService.GetByIdAsync(employeeId, _cancellationToken);
                if (employee is null)
                    throw new NotFoundException("Employee not found");

                return Ok(new ApiResponse(ApiResponseState.Success, employee));
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
        public async Task<IActionResult> InsertEmployeeAsync([FromBody] EmployeeToInsert employeeToInsert)
        {
            try
            {
                var employeeInserted = await _employeesService.InsertAsync(employeeToInsert, _loggedUsername, _cancellationToken);

                return CreatedAtAction(
                    controllerName: "Employees",
                    actionName: "GetEmployeeById",
                    routeValues: new { employeeId = employeeInserted.EmployeeId },
                    value: new ApiResponse(ApiResponseState.Success, employeeInserted));
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

        [HttpPut("{employeeId}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateEmployeeAsync(int employeeId, [FromBody] EmployeeToUpdate employeeToUpdate)
        {
            try
            {
                if (employeeId <= 0 || employeeId != employeeToUpdate.EmployeeId)
                    throw new BadRequestException("Invalid request");

                var employeeUpdated = await _employeesService.UpdateAsync(employeeToUpdate, _loggedUsername, _cancellationToken);

                return Ok(new ApiResponse(ApiResponseState.Success, employeeUpdated));
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

        [HttpPut("{employeeId}/status")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> ChangeStatusEmployeeAsync(int employeeId)
        {
            try
            {
                if (employeeId <= 0)
                    throw new BadRequestException("Invalid request");

                var employeeUpdated = await _employeesService.ChangeStatusAsync(employeeId, _loggedUsername, _cancellationToken);
                return Ok(new ApiResponse(ApiResponseState.Success, employeeUpdated));
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
