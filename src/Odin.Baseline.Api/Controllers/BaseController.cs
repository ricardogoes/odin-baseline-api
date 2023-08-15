using Microsoft.AspNetCore.Mvc;
using Odin.Baseline.Domain.Models;

namespace Odin.Baseline.Api.Controllers
{
    [Produces("application/json")]
    public abstract class BaseController : ControllerBase
    {
        public readonly ILogger _logger; 
        public readonly CancellationToken _cancellationToken;
        public readonly string _loggedUsername;



        public BaseController(AppSettings appSettings, ILogger logger)
        {
            _logger = logger;
            _loggedUsername = User.Claims.Where(x => x.Type == "cognito:username").FirstOrDefault().Value;

            var tokenSource = new CancellationTokenSource();
            tokenSource.CancelAfter(appSettings.CancellationTokenTimeout);

            _cancellationToken = tokenSource.Token;
        }

        protected IActionResult HandleError(Exception ex)
        {
            return StatusCode(500, new ApiResponse(ApiResponseState.Failed, "An error ocurred, please try again. If the error persists contact the System Administrator"));
        }
    }
}
