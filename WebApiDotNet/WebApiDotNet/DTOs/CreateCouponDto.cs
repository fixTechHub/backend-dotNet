using System.ComponentModel.DataAnnotations;

namespace WebApiDotNet.DTOs
{
    public class CreateCouponDto : IValidatableObject
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public string Type { get; set; } // PERCENT, FIXED
        public double Value { get; set; }
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
            var validAudiences = new[] { "ALL", "NEW_USER", "EXISTING_USER", "SPECIFIC_USERS" };
            if (!string.IsNullOrEmpty(Audience) && !validAudiences.Contains(Audience))
            {
                yield return new ValidationResult($"Audience must be one of: {string.Join(", ", validAudiences)}.", new[] { nameof(Audience) });
            }
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

            // Validate UserIds chỉ khi Audience là SPECIFIC_USERS
            if (Audience == "SPECIFIC_USERS")
            {
                if (UserIds == null || !UserIds.Any())
                {
                    yield return new ValidationResult("UserIds is required when Audience is SPECIFIC_USERS.", new[] { nameof(UserIds) });
                }
            }
        }
    }
}
