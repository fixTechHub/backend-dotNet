namespace WebApiDotNet.DTOs
{
    public class UpdateServiceDto
    {
        public string ServiceName { get; set; }
        public string CategoryId { get; set; }
        public ServiceTypeDto ServiceType { get; set; }
        public EstimatedMarketPriceDto EstimatedMarketPrice { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; } = true;
    }
} 