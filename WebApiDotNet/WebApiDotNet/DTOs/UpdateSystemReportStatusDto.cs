namespace WebApiDotNet.DTOs
{
    public class UpdateSystemReportStatusDto
    {
        public string Status { get; set; } // "PENDING", "RESOLVED", "REJECTED"
        public string? ResolutionNote { get; set; }
        public string? ResolvedBy { get; set; }
    }
} 