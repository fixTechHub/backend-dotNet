using System;

namespace WebApiDotNet.DTOs
{
    public class SystemReportDto
    {
        public string Id { get; set; }
        public string SubmittedBy { get; set; }
        public string Title { get; set; }
        public string Tag { get; set; } // SYSTEM, PAYMENT, UI, OTHER
        public string Description { get; set; }
        public string Status { get; set; } // PENDING, IN_PROGRESS, RESOLVED, REJECTED
        public string? ResolvedBy { get; set; }
        public string? ResolutionNote { get; set; }
        public DateTime? ResolvedAt { get; set; }
    }
}
