using MongoDB.Bson.Serialization.Attributes;

namespace WebApiDotNet.DTOs
{
    public class CouponDto
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public double Value { get; set; }
        public string Type { get; set; }
        public double MaxDiscount { get; set; }
        public double MinOrderValue { get; set; }
        public int TotalUsageLimit { get; set; }
        public int UsedCount { get; set; }
        public string Audience { get; set; }
        public List<string> UserIds { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
