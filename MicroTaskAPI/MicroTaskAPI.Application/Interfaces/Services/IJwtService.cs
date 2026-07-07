using MicroTaskAPI.Domain.Entities;

namespace MicroTaskAPI.Application.Interfaces.Services
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}