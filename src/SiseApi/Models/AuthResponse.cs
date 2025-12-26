namespace SiseApi.Models
{
    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public string TokenType { get; set; } = "Bearer";
        public DateTime ExpiresAt { get; set; }
        public string? UserName { get; set; }
        public int UserId { get; set; }
        public string Role { get; set; } = string.Empty;
    }
}
