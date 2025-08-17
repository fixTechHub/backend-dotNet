namespace WebApiDotNet.DTOs
{
    public class UpdateReportStatusDto
    {
        public string Status { get; set; } // PENDING, AWAITING_RESPONSE, REJECTED, RESOLVED
        public string ResolvedBy { get; set; } // ID của admin xử lý
        public string? ResolutionNote { get; set; } // Ghi chú khi xử lý
    }
}
