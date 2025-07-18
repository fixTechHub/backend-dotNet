using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApiDotNet.DTOs
{
    public class UpdateServiceDto
    {
        [Required(ErrorMessage = "ServiceName is required")]
        public string ServiceName { get; set; }
        [Required(ErrorMessage = "CategoryId is required")]
        public string CategoryId { get; set; }
        [Required(ErrorMessage = "ServiceType is required")]
        public string ServiceType { get; set; }
        public EstimatedMarketPriceDto? EstimatedMarketPrice { get; set; }
        [Required(ErrorMessage = "Icon is required")]
        public string Icon { get; set; }
        [Required(ErrorMessage = "Description is required")]
        [StringLength(5000, ErrorMessage = "Description cannot exceed 5000 characters")]
        public string Description { get; set; }
        public bool IsActive { get; set; } = true;
    }
} 