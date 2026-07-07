namespace MicroTaskAPI.Application.DTOs.User
{
    public class UserResponseDto
    {
        public int Id { get; set; }
        public string DisplayName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhotoUrl { get; set; }
        public string Role { get; set; } = string.Empty;
        public int Coin { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}