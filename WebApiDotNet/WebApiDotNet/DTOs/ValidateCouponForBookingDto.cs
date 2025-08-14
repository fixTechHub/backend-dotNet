namespace WebApiDotNet.DTOs
{
    public class ValidateCouponForBookingDto
    {
        public string CouponCode { get; set; }
        public string UserId { get; set; }
        public double OrderAmount { get; set; }
    }
}