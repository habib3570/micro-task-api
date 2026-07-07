using MicroTaskAPI.Application.DTOs.User;

namespace MicroTaskAPI.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<List<UserResponseDto>> GetAllUsersAsync();
        Task<UserResponseDto> GetByEmailAsync(string email);
        Task UpdateRoleAsync(string email, UpdateRoleDto dto);
        Task DeleteAsync(string email);
        Task<int> GetCoinAsync(string email);
        Task UpdateCoinAsync(string email, UpdateCoinDto dto);
        Task<List<UserResponseDto>> GetTopWorkersAsync();
    }
}