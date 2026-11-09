using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDay.Core.Entities
{
    public class Enrolment
    {
        [Key]
        public int EnrolmentID { get; set; }

        [Required]
        public int UserID { get; set; }
        [ForeignKey("UserID")]
        public User User { get; set; } = null!;

        [Required]
        public int EventID { get; set; }
        [ForeignKey("EventID")]
        public Event Event { get; set; } = null!;

        [Required]
        public int CategoryID { get; set; }
        [ForeignKey("CategoryID")]
        public Category Category { get; set; } = null!;

        public DateTime EnrolmentDate { get; set; } = DateTime.UtcNow;

        [Required, MaxLength(20)]
        public string Status { get; set; } = "Pending"; // 'Pending', 'Confirmed', 'Cancelled'

        [Required, MaxLength(20)]
        public string PaymentStatus { get; set; } = "Unpaid"; // 'Unpaid', 'Paid', 'Refunded'

        [Column(TypeName = "decimal(10,2)")]
        public decimal AmountPaid { get; set; } = 0;

        [MaxLength(100)]
        public string? ReferenceNumber { get; set; }

        // Navigation property
        public Result? Result { get; set; }
    }
}
