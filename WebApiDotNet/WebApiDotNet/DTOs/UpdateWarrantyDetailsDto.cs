using System.ComponentModel;

namespace WebApiDotNet.DTOs
{
    public class UpdateWarrantyDetailsDto
    {
        public string? ResolutionNote { get; set; }
        public string? RejectionReason { get; set; }
        public string Status { get; set; }
        public bool IsReviewedByAdmin { get; set; }
    }
} 