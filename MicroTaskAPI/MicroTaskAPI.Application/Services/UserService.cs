using MicroTaskAPI.Application.DTOs.User;
using MicroTaskAPI.Application.Interfaces.Repositories;
using MicroTaskAPI.Application.Interfaces.Services;
using MicroTaskAPI.Domain.Enums;
using MicroTaskAPI.Domain.Exceptions;

namespace MicroTaskAPI.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<UserResponseDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(MapToDto).ToList();
        }

        public async Task<UserResponseDto> GetByEmailAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email)
                ?? throw new EntityNotFoundException("User", email);
            return MapToDto(user);
        }

        public async Task UpdateRoleAsync(string email, UpdateRoleDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(email)
                ?? throw new EntityNotFoundException("User", email);

            if (!Enum.TryParse<UserRole>(dto.Role, true, out var role))
                throw new ArgumentException("Invalid role value.");

            user.Role = role;
            await _userRepository.UpdateAsync(user);
        }

        public async Task DeleteAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email)
                ?? throw new EntityNotFoundException("User", email);

            await _userRepository.DeleteAsync(user);
        }

        public async Task<int> GetCoinAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email)
                ?? throw new EntityNotFoundException("User", email);
            return user.Coin;
        }

        public async Task UpdateCoinAsync(string email, UpdateCoinDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(email)
                ?? throw new EntityNotFoundException("User", email);

            user.Coin = dto.Coin;
            await _userRepository.UpdateAsync(user);
        }

        public async Task<List<UserResponseDto>> GetTopWorkersAsync()
        {
            var users = await _userRepository.GetTopWorkersAsync(6);
            return users.Select(MapToDto).ToList();
        }

        private static UserResponseDto MapToDto(Domain.Entities.User user) => new()
        {
            Id = user.Id,
            DisplayName = user.DisplayName,
            Email = user.Email,
            PhotoUrl = user.PhotoUrl,
            Role = user.Role.ToString(),
            Coin = user.Coin,
            CreatedAt = user.CreatedAt
        };
    }
}