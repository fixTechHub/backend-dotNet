namespace WebApiDotNet.DTOs
{
    public class CreateCouponDto
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public string Type { get; set; } // PERCENT, FIXED
        public double Value { get; set; }
        public double MaxDiscount { get; set; }
        public double MinOrderValue { get; set; }
        public int TotalUsageLimit { get; set; }
        public string Audience { get; set; }
        public List<string> UserIds { get; set; }
        public bool IsActive { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
