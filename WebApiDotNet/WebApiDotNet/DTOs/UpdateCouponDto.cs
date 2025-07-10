using System.ComponentModel.DataAnnotations;

namespace WebApiDotNet.DTOs
{
    public class UpdateCouponDto : IValidatableObject
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public string Type { get; set; } // PERCENT, FIXED
        public double? Value { get; set; }
        public double? MaxDiscount { get; set; }
        public double MinOrderValue { get; set; }
        public int TotalUsageLimit { get; set; }
        public string Audience { get; set; }
        public List<string> UserIds { get; set; }
        public bool IsActive { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Type == "PERCENT")
            {
                if (Value == null || Value <= 0)
                {
                    yield return new ValidationResult("Value must be > 0 when Type is PERCENT.", new[] { nameof(Value) });
                }
                if (MaxDiscount == null || MaxDiscount <= 0)
                {
                    yield return new ValidationResult("MaxDiscount must be > 0 when Type is PERCENT.", new[] { nameof(MaxDiscount) });
                }
            }
            else if (Type == "FIXED")
            {
                if (Value == null || Value <= 0)
                {
                    yield return new ValidationResult("Value must be > 0 when Type is FIXED.", new[] { nameof(Value) });
                }
                // MaxDiscount có thể không cần thiết
            }
        }
    }
}
