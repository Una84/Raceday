using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDay.Core.Entities
{
    public class Result
    {
        [Key]
        public int ResultID { get; set; }

        [Required]
        public int EnrolmentID { get; set; }
        [ForeignKey("EnrolmentID")]
        public Enrolment Enrolment { get; set; } = null!;

        [Required]
        public TimeSpan FinishTime { get; set; }

        public TimeSpan? ChipTime { get; set; }

        [Required]
        public int Position { get; set; }

        [Column(TypeName = "decimal(6,2)")]
        public decimal? Pace { get; set; }

        public DateTime RecordedAt { get; set; } = DateTime.UtcNow;
    }
}
