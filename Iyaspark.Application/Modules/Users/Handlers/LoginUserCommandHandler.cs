using Iyaspark.Application.Interfaces.Services;
using Iyaspark.Application.Modules.Users.Commands;
using Iyaspark.Domain.Interfaces;
using MediatR;
using BCrypt.Net;

namespace Iyaspark.Application.Modules.Users.Handlers
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, string>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;

        public LoginUserCommandHandler(IUserRepository userRepository, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        public async Task<string> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Geçersiz e-posta veya şifre.");
            }

            var token = _jwtService.GenerateToken(user);
            return token;
        }
    }
}
