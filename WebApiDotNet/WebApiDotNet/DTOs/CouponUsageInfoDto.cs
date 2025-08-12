namespace WebApiDotNet.DTOs
{
    public class CouponUsageInfoDto
    {
        public string CouponId { get; set; }
        public string Code { get; set; }
        public bool IsActive { get; set; }
        public int UsedCount { get; set; }
        public int TotalUsageLimit { get; set; }
        public int RemainingUses { get; set; }
        public double UsagePercentage { get; set; }
        public bool IsAtLimit { get; set; }
        public bool IsNearLimit { get; set; }
        public string StatusMessage { get; set; }
    }
}
