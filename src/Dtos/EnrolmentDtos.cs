using System.ComponentModel.DataAnnotations;

namespace RaceDay.Api.Dtos
{
    public class CreateEnrolmentDto
    {
        [Required]
        public int CategoryID { get; set; }
    }

    public class UpdateEnrolmentStatusDto
    {
        [Required, MaxLength(20)]
        public string Status { get; set; } = string.Empty; // 'Pending', 'Confirmed', 'Cancelled'

        [Required, MaxLength(20)]
        public string PaymentStatus { get; set; } = string.Empty; // 'Unpaid', 'Paid', 'Refunded'
    }
}
