using System;
using System.Threading.Tasks;
using Identity.Core.Interfaces;
using Identity.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Identity.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IAuthenticationService _authenticationService;

        public UsersController(ILogger<UsersController> logger,
            IAuthenticationService authenticationService)
        {
            _logger = logger;
            _authenticationService = authenticationService;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(AuthenticateRequestDto model)
        {
            try
            {
                if (model == null)
                {
                    throw new ArgumentNullException(nameof(model));
                }

                var response = await _authenticationService.Authenticate(model);

                if (response == null)
                {
                    _logger.LogInformation($"Unable to authenticate user {model.Username}");
                    return BadRequest();
                }

                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception happened during authentication request");
                return BadRequest();
            }
        }
    }
}
