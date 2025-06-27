namespace WebApiDotNet.DTOs
{
    public class UpdateSystemReportStatusDto
    {
        public string Status { get; set; } // "PENDING", "RESOLVED", "REJECTED"
        // public string? Note { get; set; } // Nếu muốn lưu ghi chú khi cập nhật status
    }
} 