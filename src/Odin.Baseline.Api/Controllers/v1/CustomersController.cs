using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Odin.Baseline.Api.Attributes;
using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Domain.Entities;
using Odin.Baseline.Domain.Interfaces.Services;
using Odin.Baseline.Domain.Models;
using Odin.Baseline.Domain.QueryModels;
using Odin.Baseline.Domain.ViewModels.Customers;
using Odin.Baseline.Domain.ViewModels.Departments;

namespace Odin.Baseline.Api.Controllers.v1
{
    [ApiController]
    [Authorize]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/customers")]
    public class CustomersController : BaseController
    {
        private readonly ICustomersService _customersService;
        private readonly IDepartmentsService _departmentsService;
        private readonly IEmployeesService _employeesService;
        private readonly ICompaniesPositionsService _positionsService;
        
        public CustomersController(AppSettings appSettings, ICustomersService customersService, IDepartmentsService departmentsService,
            IEmployeesService employeesService, ICompaniesPositionsService positionsService, ILogger<CustomersController> logger)
            : base(appSettings, logger)
        {
            _customersService = customersService ?? throw new ArgumentNullException(nameof(customersService));
            _departmentsService = departmentsService ?? throw new ArgumentNullException(nameof(departmentsService));
            _employeesService = employeesService ?? throw new ArgumentNullException(nameof(employeesService));
            _positionsService = positionsService ?? throw new ArgumentNullException(nameof(positionsService));
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomersAsync([FromQuery] CustomersQueryModel queryData)
        {
            try
            {
                var customers = await _customersService.GetAllAsync(queryData, _cancellationToken);

                return Ok(new PagedApiResponse<Customer>(ApiResponseState.Success, "", customers, queryData.PageNumber, queryData.PageSize));
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetCustomerByIdAsync(int customerId)
        {
            try
            {
                if (customerId <= 0)
                    throw new BadRequestException("Invalid request");

                var customer = await _customersService.GetByIdAsync(customerId, _cancellationToken);
                if (customer is null)
                    throw new NotFoundException("Customer not found");

                return Ok(new ApiResponse(ApiResponseState.Success, customer));
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
        public async Task<IActionResult> InsertCustomerAsync([FromBody] CustomerToInsert customerToInsert)
        {
            try
            {
                var customerInserted = await _customersService.InsertAsync(customerToInsert, _loggedUsername, _cancellationToken);

                return CreatedAtAction(
                    controllerName: "Customers",
                    actionName: "GetCustomerById",
                    routeValues: new { customerId = customerInserted.CustomerId },
                    value: new ApiResponse(ApiResponseState.Success, customerInserted));
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

        [HttpPut("{customerId}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateCustomerAsync(int customerId, [FromBody] CustomerToUpdate customerToUpdate)
        {
            try
            {
                if (customerId <= 0 || customerId != customerToUpdate.CustomerId)
                    throw new BadRequestException("Invalid request");

                var customerUpdated = await _customersService.UpdateAsync(customerToUpdate, _loggedUsername, _cancellationToken);

                return Ok(new ApiResponse(ApiResponseState.Success, customerUpdated));
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

        [HttpPut("{customerId}/status")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> ChangeStatusCustomerAsync(int customerId)
        {
            try
            {
                if (customerId <= 0)
                    throw new BadRequestException("Invalid request");

                var customerUpdated = await _customersService.ChangeStatusAsync(customerId, _loggedUsername, _cancellationToken);
                return Ok(new ApiResponse(ApiResponseState.Success, customerUpdated));
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

        [HttpGet("{customerId}/departments")]
        public async Task<IActionResult> GetDepartmentsByCustomerAsync(int customerId, [FromQuery] DepartmentsQueryModel queryData)
        {
            try
            {
                if (customerId <= 0)
                    throw new BadRequestException("Invalid request");

                var user = User.Identity.Name;
                var departments = await _departmentsService.GetByCustomerAsync(customerId, queryData, _cancellationToken);

                return Ok(new PagedApiResponse<DepartmentToQuery>(ApiResponseState.Success, "", departments, queryData.PageNumber, queryData.PageSize));
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

        [HttpGet("{customerId}/positions")]
        public async Task<IActionResult> GetPositionsByCustomerAsync(int customerId, [FromQuery] CompaniesPositionsQueryModel paginationData)
        {
            try
            {
                if (customerId <= 0)
                    throw new BadRequestException("Invalid request");

                var positions = await _positionsService.GetByCustomerAsync(customerId, paginationData, _cancellationToken);

                return Ok(new ApiResponse(ApiResponseState.Success, positions));
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

        [HttpGet("{customerId}/employees")]
        public async Task<IActionResult> GetEmployeesByCustomerAsync(int customerId, [FromQuery] EmployeesQueryModel paginationData)
        {
            try
            {
                if (customerId <= 0)
                    throw new BadRequestException("Invalid request");

                var positions = await _employeesService.GetByCustomerAsync(customerId, paginationData, _cancellationToken);

                return Ok(new ApiResponse(ApiResponseState.Success, positions));
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
