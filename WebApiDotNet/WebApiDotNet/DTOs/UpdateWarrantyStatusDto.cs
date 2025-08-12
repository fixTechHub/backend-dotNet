using System.ComponentModel;

namespace WebApiDotNet.DTOs
{
    public class UpdateWarrantyStatusDto
    {
        public string Status { get; set; }
        // 🔄 IsReviewedByAdmin sẽ tự động được set thành true khi admin thay đổi
    }
} 