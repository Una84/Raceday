using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDay.Core.Entities
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }

        [Required]
        public int EventID { get; set; }

        [ForeignKey("EventID")]
        public Event Event { get; set; } = null!;

        [Required, MaxLength(100)]
        public string CategoryName { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? CategoryDescription { get; set; }

        public int? MinAge { get; set; }
        public int? MaxAge { get; set; }

        [MaxLength(20)]
        public string? Gender { get; set; } // 'Open', 'Male', 'Female'

        [Required, Column(TypeName = "decimal(10,2)")]
        public decimal EntryFee { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation properties
        public ICollection<Enrolment> Enrolments { get; set; } = new List<Enrolment>();
    }
}
