using System;
using System.Collections.Generic;

namespace WebApiDotNet.DTOs
{
    public class ReportDto
    {
        public string Id { get; set; }
        public string Type { get; set; } // BOOKING, WARRANTY, VIOLATION
        public string? BookingId { get; set; }
        public string? WarrantyId { get; set; }
        public string ReporterId { get; set; }
        public string ReportedUserId { get; set; }
        public string Title { get; set; }
        public string Tag { get; set; } // NO_SHOW, LATE, RUDE, ISSUE, OTHER, WARRANTY_DENIED, WARRANTY_DELAY, POOR_FIX
        public string Description { get; set; }
        public List<string> Evidences { get; set; } = new List<string>();
        public string Status { get; set; } // PENDING, AWAITING_RESPONSE, REJECTED, RESOLVED
        public DateTime? ResponseDeadline { get; set; }
        public bool ResponseLocked { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
} 