namespace WebApiDotNet.DTOs
{
    public class CouponUsageDto
    {
        public string Id { get; set; }
        public string CouponId { get; set; }
        public string UserId { get; set; }
        public string BookingId { get; set; }
        public DateTime UsedAt { get; set; }
    }
}
