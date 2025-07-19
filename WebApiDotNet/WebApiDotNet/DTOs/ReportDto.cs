using System;

namespace WebApiDotNet.DTOs
{
    public class ReportDto
    {
        public string Id { get; set; }
        public string Type { get; set; } // REPORT, VIOLATION
        public string ReportedUserId { get; set; }
        public string ReporterId { get; set; }
        public string Description { get; set; }
        public string Status { get; set; } // PENDING, CONFIRMED, REJECTED, RESOLVED, CLOSED
        public string? Penalty { get; set; }
        public DateTime CreatedAt { get; set; }
    }
} 