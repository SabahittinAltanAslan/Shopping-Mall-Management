using MediatR;
using System;

namespace Iyaspark.Application.Modules.Users.Commands
{
    public class CreateUserCommand : IRequest<Guid>
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
    }
}
