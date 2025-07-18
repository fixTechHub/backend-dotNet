using System;

namespace WebApiDotNet.DTOs
{
    public class WarrantyDto
    {
        public string Id { get; set; }
        public string BookingId { get; set; }
        public string CustomerId { get; set; }
        public string TechnicianId { get; set; }
        public DateTime RequestDate { get; set; }
        public string ReportedIssue { get; set; }
        public bool IsUnderWarranty { get; set; }
        public DateTime ExpireAt { get; set; }
        public string Status { get; set; }
        public bool IsReviewedByAdmin { get; set; }
    }
} 