using Microsoft.AspNetCore.Mvc;
using Odin.Baseline.Api.Attributes;
using Odin.Baseline.Domain.CustomExceptions;
using Odin.Baseline.Domain.Interfaces.Services;
using Odin.Baseline.Domain.Models;
using Odin.Baseline.Domain.ViewModels.Customers;

namespace Odin.Baseline.Api.Controllers.v1
{
    [ApiController]
    //[Authorize]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/customers")]
    public class CustomersController : BaseController
    {
        private readonly ICustomersService _customersService;

        public CustomersController(ICustomersService customersService,
            ILogger<CustomersController> logger)
            : base(logger)
        {
            _customersService = customersService ?? throw new ArgumentNullException(nameof(customersService));
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomersAsync()
        {
            try
            {
                var customers = await _customersService.GetAllAsync();
                return Ok(new ApiResponse(ApiResponseState.Success, customers));
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

                var customer = await _customersService.GetByIdAsync(customerId);
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
                var customerInserted = await _customersService.InsertAsync(customerToInsert);

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

                var customerUpdated = await _customersService.UpdateAsync(customerToUpdate);

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

                var customerUpdated = await _customersService.ChangeStatusAsync(customerId);
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
    }
}
