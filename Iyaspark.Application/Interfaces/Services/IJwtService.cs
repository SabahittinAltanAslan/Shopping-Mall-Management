using Iyaspark.Domain.Entities;

namespace Iyaspark.Application.Interfaces.Services
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}
