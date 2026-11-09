using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDay.Core.Entities
{
    public class Organiser
    {
        [Key]
        public int OrganiserID { get; set; }

        [Required]
        public int UserID { get; set; }

        [ForeignKey("UserID")]
        public User User { get; set; } = null!;

        [Required, MaxLength(150)]
        public string OrganisationName { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        [MaxLength(150)]
        public string? Website { get; set; }

        [MaxLength(255)]
        public string? LogoUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public ICollection<Event> Events { get; set; } = new List<Event>();
    }
}
