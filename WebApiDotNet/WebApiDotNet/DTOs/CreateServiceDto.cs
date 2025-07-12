using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApiDotNet.DTOs
{
    public class CreateServiceDto : IValidatableObject
    {
        public string ServiceName { get; set; }
        public string CategoryId { get; set; }
        public string ServiceType { get; set; }
        public EstimatedMarketPriceDto EstimatedMarketPrice { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; } = true;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ServiceType == "COMPLEX")
            {
                if (EstimatedMarketPrice == null || EstimatedMarketPrice.Min == null || EstimatedMarketPrice.Max == null)
                {
                    yield return new ValidationResult("The EstimatedMarketPrice field is required.", new[] { nameof(EstimatedMarketPrice) });
                }
            }
            // Nếu là FIXED thì không required EstimatedMarketPrice
        }
    }
}
