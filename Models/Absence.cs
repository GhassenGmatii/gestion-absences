using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GestionAbsences.Models
{
    public class Absence
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int StudentId { get; set; }

        [ForeignKey("StudentId")]
        [JsonIgnore]
        public User Student { get; set; } = null!;

        [Required]
        public int ClassId { get; set; }

        [ForeignKey("ClassId")]
        [JsonIgnore]
        public Class ClassEntity { get; set; } = null!;

        [Required]
        public int MarkedById { get; set; }

        [ForeignKey("MarkedById")]
        [JsonIgnore]
        public User MarkedBy { get; set; } = null!;

        [Required]
        public DateTime Date { get; set; }

        public bool IsJustified { get; set; } = false;

        public string? Justification { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
