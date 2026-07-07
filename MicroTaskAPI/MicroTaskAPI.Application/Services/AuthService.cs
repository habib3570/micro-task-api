using MicroTaskAPI.Application.DTOs.Auth;
using MicroTaskAPI.Application.Interfaces.Repositories;
using MicroTaskAPI.Application.Interfaces.Services;
using MicroTaskAPI.Domain.Constants;
using MicroTaskAPI.Domain.Entities;
using MicroTaskAPI.Domain.Enums;
using MicroTaskAPI.Domain.Exceptions;

namespace MicroTaskAPI.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;

        public AuthService(IUserRepository userRepository, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
        {
            var exists = await _userRepository.ExistsByEmailAsync(request.Email);
            if (exists)
                throw new DuplicateEntityException("User", "email", request.Email);

            if (!Enum.TryParse<UserRole>(request.Role, true, out var role) ||
                (role != UserRole.Worker && role != UserRole.Buyer))
                throw new ArgumentException("Role must be either 'Worker' or 'Buyer'.");

            var defaultCoin = role == UserRole.Worker
                ? CoinConstants.DefaultWorkerCoin
                : CoinConstants.DefaultBuyerCoin;

            var user = new User
            {
                DisplayName = request.DisplayName,
                Email = request.Email,
                PhotoUrl = request.PhotoUrl,
                Role = role,
                Coin = defaultCoin,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };

            await _userRepository.AddAsync(user);

            var token = _jwtService.GenerateToken(user);

            return MapToAuthResponse(user, token);
        }

        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email)
                ?? throw new EntityNotFoundException("User", request.Email);

            if (string.IsNullOrEmpty(user.PasswordHash) ||
                !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid email or password.");

            var token = _jwtService.GenerateToken(user);
            return MapToAuthResponse(user, token);
        }

        public async Task<AuthResponseDto> GoogleLoginAsync(GoogleLoginRequestDto request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);

            if (user == null)
            {
                if (!Enum.TryParse<UserRole>(request.Role ?? "Worker", true, out var role) ||
                    (role != UserRole.Worker && role != UserRole.Buyer))
                    role = UserRole.Worker;

                var defaultCoin = role == UserRole.Worker
                    ? CoinConstants.DefaultWorkerCoin
                    : CoinConstants.DefaultBuyerCoin;

                user = new User
                {
                    DisplayName = request.DisplayName,
                    Email = request.Email,
                    PhotoUrl = request.PhotoUrl,
                    Role = role,
                    Coin = defaultCoin,
                    FirebaseUid = request.FirebaseUid
                };

                await _userRepository.AddAsync(user);
            }
            else if (string.IsNullOrEmpty(user.FirebaseUid))
            {
                user.FirebaseUid = request.FirebaseUid;
                await _userRepository.UpdateAsync(user);
            }

            var token = _jwtService.GenerateToken(user);
            return MapToAuthResponse(user, token);
        }

        public async Task<AuthResponseDto> GetCurrentUserAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email)
                ?? throw new EntityNotFoundException("User", email);

            var token = _jwtService.GenerateToken(user);
            return MapToAuthResponse(user, token);
        }

        private static AuthResponseDto MapToAuthResponse(User user, string token) => new()
        {
            Id = user.Id,
            DisplayName = user.DisplayName,
            Email = user.Email,
            PhotoUrl = user.PhotoUrl,
            Role = user.Role.ToString(),
            Coin = user.Coin,
            Token = token
        };
    }
}