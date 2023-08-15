using Microsoft.AspNetCore.Mvc;
using Odin.Baseline.Api.Attributes;
using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Domain.Interfaces.Services;
using Odin.Baseline.Domain.Models;
using Odin.Baseline.Domain.QueryModels;
using Odin.Baseline.Domain.ViewModels.Departments;

namespace Odin.Baseline.Api.Controllers.v1
{
    [ApiController]
    //[Authorize]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/departments")]
    public class DepartmentsController : BaseController
    {
        private readonly IDepartmentsService _departmentsService;
        private readonly IEmployeesService _employeesService;

        public DepartmentsController(AppSettings appSettings, IDepartmentsService departmentsService, IEmployeesService employeesService,
            ILogger<DepartmentsController> logger)
            : base(appSettings, logger)
        {
            _departmentsService = departmentsService ?? throw new ArgumentNullException(nameof(departmentsService));
            _employeesService = employeesService ?? throw new ArgumentNullException(nameof(employeesService));
        }

        [HttpGet("{departmentId}")]
        public async Task<IActionResult> GetDepartmentByIdAsync(int departmentId)
        {
            try
            {
                if (departmentId <= 0)
                    throw new BadRequestException("Invalid request");

                var department = await _departmentsService.GetByIdAsync(departmentId, _cancellationToken);
                if (department is null)
                    throw new NotFoundException("Department not found");

                return Ok(new ApiResponse(ApiResponseState.Success, department));
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
        public async Task<IActionResult> InsertDepartmentAsync([FromBody] DepartmentToInsert departmentToInsert)
        {
            try
            {
                var departmentInserted = await _departmentsService.InsertAsync(departmentToInsert, _loggedUsername, _cancellationToken);

                return CreatedAtAction(
                    controllerName: "Departments",
                    actionName: "GetDepartmentById",
                    routeValues: new { departmentId = departmentInserted.DepartmentId },
                    value: new ApiResponse(ApiResponseState.Success, departmentInserted));
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

        [HttpPut("{departmentId}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateDepartmentAsync(int departmentId, [FromBody] DepartmentToUpdate departmentToUpdate)
        {
            try
            {
                if (departmentId <= 0 || departmentId != departmentToUpdate.DepartmentId)
                    throw new BadRequestException("Invalid request");

                var departmentUpdated = await _departmentsService.UpdateAsync(departmentToUpdate, _loggedUsername, _cancellationToken);

                return Ok(new ApiResponse(ApiResponseState.Success, departmentUpdated));
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

        [HttpPut("{departmentId}/status")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> ChangeStatusDepartmentAsync(int departmentId)
        {
            try
            {
                if (departmentId <= 0)
                    throw new BadRequestException("Invalid request");

                var departmentUpdated = await _departmentsService.ChangeStatusAsync(departmentId, _loggedUsername, _cancellationToken);
                return Ok(new ApiResponse(ApiResponseState.Success, departmentUpdated));
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

        [HttpGet("{departmentId}/employees")]
        public async Task<IActionResult> GetEmployeesByDepartmentAsync(int departmentId, [FromQuery] EmployeesQueryModel queryData)
        {
            try
            {
                if (departmentId <= 0)
                    throw new BadRequestException("Invalid request");

                var employees = await _employeesService.GetByDepartmentAsync(departmentId, queryData, _cancellationToken);

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
