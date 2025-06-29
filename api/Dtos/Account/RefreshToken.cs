using api.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Account
{
    [Serializable]
    public class RefreshToken
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Token { get; set; } = string.Empty;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime ExpiresAt { get; set; }

        public DateTime? RevokedAt { get; set; }

        public string? ReplacedByToken { get; set; }

        public string? Device { get; set; }

        public string? IpAddress { get; set; }

        [Required]
        public bool IsRevoked => RevokedAt != null || DateTime.UtcNow >= ExpiresAt;

        // 🔐 Relación con el usuario
        [Required]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey("UserId")]
        public AppUser? User { get; set; }
    }
}
