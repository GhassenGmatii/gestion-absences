using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using GestionAbsences.Models.Enums;

namespace GestionAbsences.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [JsonIgnore]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public UserRole Role { get; set; }

        [JsonIgnore]
        public string? RefreshToken { get; set; }

        [JsonIgnore]
        public DateTime? RefreshTokenExpiryTime { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [JsonIgnore]
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

        [JsonIgnore]
        public ICollection<Absence> Absences { get; set; } = new List<Absence>();

        [JsonIgnore]
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();

        [JsonIgnore]
        public ICollection<Class> TaughtClasses { get; set; } = new List<Class>();
    }
}
