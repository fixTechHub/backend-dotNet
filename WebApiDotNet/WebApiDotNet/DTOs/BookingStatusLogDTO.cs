using System.ComponentModel.DataAnnotations;
using WebApiDotNet.Models;

namespace WebApiDotNet.DTOs
{
    public class CreateBookingStatusLogDTO
    {
        [Required]
        public string BookingId { get; set; } = string.Empty;

        [Required]
        [EnumDataType(typeof(BookingStatus))]
        public string FromStatus { get; set; } = string.Empty;

        [Required]
        [EnumDataType(typeof(BookingStatus))]
        public string ToStatus { get; set; } = string.Empty;

        [Required]
        public string ChangedBy { get; set; } = string.Empty;

        [Required]
        [EnumDataType(typeof(UserRole))]
        public string Role { get; set; } = string.Empty;

        public string? Note { get; set; }
    }

    public class UpdateBookingStatusLogDTO
    {
        public string? Note { get; set; }
    }

    public class BookingStatusLogResponseDTO
    {
        public string Id { get; set; } = string.Empty;
        public string BookingId { get; set; } = string.Empty;
        public string FromStatus { get; set; } = string.Empty;
        public string ToStatus { get; set; } = string.Empty;
        public string ChangedBy { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string? Note { get; set; }
        public DateTime CreatedAt { get; set; }
        
        // Additional info
        public string? ChangedByUserName { get; set; }
        public string? ChangedByUserEmail { get; set; }
        
        // Booking info (optional)
        public string? BookingCode { get; set; }
        public string? BookingDescription { get; set; }
    }

    public class BookingStatusLogFilterDTO
    {
        public string? BookingId { get; set; }
        public string? ChangedBy { get; set; }
        public string? Role { get; set; }
        public string? FromStatus { get; set; }
        public string? ToStatus { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string? SortBy { get; set; } = "CreatedAt";
        public bool SortDescending { get; set; } = true;
    }

    public class BookingStatusChangeDTO
    {
        [Required]
        public string BookingId { get; set; } = string.Empty;

        [Required]
        [EnumDataType(typeof(BookingStatus))]
        public string NewStatus { get; set; } = string.Empty;

        [Required]
        public string ChangedBy { get; set; } = string.Empty;

        [Required]
        [EnumDataType(typeof(UserRole))]
        public string Role { get; set; } = string.Empty;

        public string? Note { get; set; }
    }
}
