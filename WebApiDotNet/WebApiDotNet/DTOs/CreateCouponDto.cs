using System.ComponentModel.DataAnnotations;

namespace WebApiDotNet.DTOs
{
    public class CreateCouponDto : IValidatableObject
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
                if (MaxDiscount == null)
                {
                    yield return new ValidationResult("MaxDiscount is required when Type is PERCENT.", new[] { nameof(MaxDiscount) });
                }
                if (Value != null)
                {
                    yield return new ValidationResult("Value should not be set when Type is PERCENT.", new[] { nameof(Value) });
                }
            }
            else if (Type == "FIXED")
            {
                if (Value == null)
                {
                    yield return new ValidationResult("Value is required when Type is FIXED.", new[] { nameof(Value) });
                }
                // MaxDiscount có thể không cần thiết
            }
        }
    }
}
