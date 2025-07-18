using System.ComponentModel.DataAnnotations;

namespace WebApiDotNet.DTOs
{
    public class CreateCouponDto
    {
        [Required(ErrorMessage = "Code is required")]
        [StringLength(100, ErrorMessage = "Code cannot exceed 100 characters")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Description is required")]
        [StringLength(5000, ErrorMessage = "Description cannot exceed 5000 characters")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Type is required")]
        public string Type { get; set; } // PERCENT, FIXED
        [Required(ErrorMessage = "Value is required")]
        [Range(1000, int.MaxValue, ErrorMessage = "Value must be at least 1,000 VND")]
        public double Value { get; set; }
        public double? MaxDiscount { get; set; }
        [Required(ErrorMessage = "MinOrderValue is required")]
        [Range(1000, int.MaxValue, ErrorMessage = "Min Order Value must be at least 1,000 VND")]
        public double MinOrderValue { get; set; }
        public int TotalUsageLimit { get; set; }
        public string Audience { get; set; }
        public List<string> UserIds { get; set; }
        public bool IsActive { get; set; }
        [Required(ErrorMessage = "StartDate is required")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "EndDate is required")]
        public DateTime EndDate { get; set; }

        // public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        // {
        //     var validAudiences = new[] { "ALL", "NEW_USER", "EXISTING_USER", "SPECIFIC_USERS" };
        //     if (!string.IsNullOrEmpty(Audience) && !validAudiences.Contains(Audience))
        //     {
        //         yield return new ValidationResult($"Audience must be one of: {string.Join(", ", validAudiences)}.", new[] { nameof(Audience) });
        //     }
        //     if (Type == "PERCENT")
        //     {
        //         if (Value == null || Value <= 0)
        //         {
        //             yield return new ValidationResult("Value must be > 0 when Type is PERCENT.", new[] { nameof(Value) });
        //         }
        //         if (MaxDiscount == null || MaxDiscount <= 0)
        //         {
        //             yield return new ValidationResult("MaxDiscount must be > 0 when Type is PERCENT.", new[] { nameof(MaxDiscount) });
        //         }
        //     }
        //     else if (Type == "FIXED")
        //     {
        //         if (Value == null || Value <= 0)
        //         {
        //             yield return new ValidationResult("Value must be > 0 when Type is FIXED.", new[] { nameof(Value) });
        //         }
        //         // MaxDiscount có thể không cần thiết
        //     }

        //     // Validate UserIds chỉ khi Audience là SPECIFIC_USERS
        //     if (Audience == "SPECIFIC_USERS")
        //     {
        //         if (UserIds == null || !UserIds.Any())
        //         {
        //             yield return new ValidationResult("UserIds is required when Audience is SPECIFIC_USERS.", new[] { nameof(UserIds) });
        //         }
        //     }
        // }
    }
}
