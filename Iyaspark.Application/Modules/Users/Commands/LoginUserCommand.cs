using MediatR;

namespace Iyaspark.Application.Modules.Users.Commands
{
    public class LoginUserCommand : IRequest<string> // Giriş başarılıysa JWT Token dönecek
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
