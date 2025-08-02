using Iyaspark.Application.Modules.Users.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Iyaspark.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
        {
            var token = await _mediator.Send(command);
            if (string.IsNullOrWhiteSpace(token))
                return Unauthorized("Geçersiz e-posta veya şifre.");

            return Ok(new { token });
        }
        [HttpPost("create-anon")]
        public async Task<IActionResult> CreateAnonymousUser(CreateUserCommand command)
        {
            var userId = await _mediator.Send(command);
            return Ok(new { message = "Kullanıcı oluşturuldu", userId });
        }

    }
}
