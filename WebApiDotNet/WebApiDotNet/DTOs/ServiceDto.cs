using System;

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
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }

    public class EstimatedMarketPriceDto
    {
        public double? Min { get; set; }
        public double? Max { get; set; }
    }

    public enum ServiceTypeDto
    {
        FIXED,
        COMPLEX
    }
} 