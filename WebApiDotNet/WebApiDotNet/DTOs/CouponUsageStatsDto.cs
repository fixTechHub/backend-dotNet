namespace WebApiDotNet.DTOs
{
    public class CouponUsageStatsDto
    {
        public int TotalCoupons { get; set; }
        public int ActiveCoupons { get; set; }
        public int InactiveCoupons { get; set; }
        public int CouponsNearLimit { get; set; }
        public int CouponsAtLimit { get; set; }
        public int TotalUsageCount { get; set; }
        public double AverageUsageRate { get; set; }
    }
}
