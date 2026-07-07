namespace MicroTaskAPI.Application.DTOs.Auth
{
    public class GoogleLoginRequestDto
    {
        public string Email { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string? PhotoUrl { get; set; }
        public string FirebaseUid { get; set; } = string.Empty;
        public string? Role { get; set; } // Only used on first-time registration via Google
    }
}