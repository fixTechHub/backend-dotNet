using System.ComponentModel;

namespace WebApiDotNet.DTOs
{
    public class UpdateWarrantyStatusDto
    {
        public string Status { get; set; }
        public bool IsReviewedByAdmin { get; set; }
    }
} 