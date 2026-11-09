using System;
using System.ComponentModel.DataAnnotations;

namespace RaceDay.Api.Dtos
{
    public class CreateEventDto
    {
        [Required, MaxLength(150)]
        public string EventName { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? EventDescription { get; set; }

        [Required]
        public DateTime EventDate { get; set; }

        [Required, MaxLength(200)]
        public string Location { get; set; } = string.Empty;

        [Required]
        public TimeSpan StartTime { get; set; }

        public TimeSpan? EndTime { get; set; }

        public decimal? RouteDistance { get; set; }

        [Required, MaxLength(20)]
        public string EventType { get; set; } = string.Empty; // 'Run', 'Walk', 'Cycle'

        [MaxLength(255)]
        public string? PosterUrl { get; set; }
    }

    public class UpdateEventDto : CreateEventDto
    {
        public bool IsActive { get; set; } = true;
    }

    public class CreateCategoryDto
    {
        [Required, MaxLength(100)]
        public string CategoryName { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? CategoryDescription { get; set; }

        public int? MinAge { get; set; }
        public int? MaxAge { get; set; }

        [MaxLength(20)]
        public string? Gender { get; set; } // 'Open', 'Male', 'Female'

        [Required]
        public decimal EntryFee { get; set; }
    }

    public class UpdateCategoryDto : CreateCategoryDto
    {
        public bool IsActive { get; set; } = true;
    }
}
