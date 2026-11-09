using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace RaceDay.Web.Models
{
    public class LoginViewModel
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterViewModel
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string Role { get; set; } = "Participant"; // Organiser or Participant
        public string? OrganisationName { get; set; }
    }

    public class EventViewModel
    {
        public int EventID { get; set; }
        public int OrganiserID { get; set; }
        public string EventName { get; set; } = string.Empty;
        public string? EventDescription { get; set; }
        public DateTime EventDate { get; set; }
        public string Location { get; set; } = string.Empty;
        public TimeSpan StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public decimal? RouteDistance { get; set; }
        public string EventType { get; set; } = string.Empty; // Run, Walk, Cycle
        public string? PosterUrl { get; set; }
        public IFormFile? PosterImage { get; set; } // For image upload to Azure via API
        public List<CategoryViewModel> Categories { get; set; } = new();
    }

    public class CategoryViewModel
    {
        public int CategoryID { get; set; }
        public int EventID { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string? CategoryDescription { get; set; }
        public int? MinAge { get; set; }
        public int? MaxAge { get; set; }
        public string? Gender { get; set; }
        public decimal EntryFee { get; set; }
    }

    public class EnrolmentViewModel
    {
        public int EnrolmentID { get; set; }
        public int UserID { get; set; }
        public string? ParticipantName { get; set; }
        public int EventID { get; set; }
        public string? EventName { get; set; }
        public int CategoryID { get; set; }
        public string? CategoryName { get; set; }
        public DateTime EnrolmentDate { get; set; }
        public string Status { get; set; } = "Pending";
        public string PaymentStatus { get; set; } = "Unpaid";
        public decimal AmountPaid { get; set; }
        public string? ReferenceNumber { get; set; }
    }

    public class ResultViewModel
    {
        public int ResultID { get; set; }
        public int EnrolmentID { get; set; }
        public string? ParticipantName { get; set; }
        public string? EventName { get; set; }
        public string? CategoryName { get; set; }
        public TimeSpan FinishTime { get; set; }
        public TimeSpan? ChipTime { get; set; }
        public int Position { get; set; }
        public decimal? Pace { get; set; }
        public DateTime RecordedAt { get; set; }
    }
}
