using System;
using System.ComponentModel.DataAnnotations;
namespace WebApiDotNet.DTOs
{
    public class ServiceDto
    {
        public string Id { get; set; }
        public string ServiceName { get; set; }
        public string CategoryId { get; set; }
        public string ServiceType { get; set; }
        public EstimatedMarketPriceDto EstimatedMarketPrice { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class EstimatedMarketPriceDto
    {
        [Range(1000, int.MaxValue, ErrorMessage = "Value must be at least 1,000 VND")]
        public double? Min { get; set; }
        [Range(1000, int.MaxValue, ErrorMessage = "Value must be at least 1,000 VND")]
        public double? Max { get; set; }
    }
} 