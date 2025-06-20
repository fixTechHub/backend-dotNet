namespace WebApiDotNet.DTOs
{
    public class UpdateTechnicianStatusDto
    {
        public string Status { get; set; } // "APPROVED" hoặc "REJECTED"
        public string? Note { get; set; } // Nếu muốn lưu lý do từ chối
    }
} 