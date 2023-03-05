using DocStorage.Application.Adapters;
using FluentMediator;
using Microsoft.AspNetCore.Mvc;

namespace DocStorage.Api.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post(LoginCommand request)
        {
            var response = await _mediator.SendAsync<AuthenticatedUser>(request);
            return Ok(response);
        }
    }
}
