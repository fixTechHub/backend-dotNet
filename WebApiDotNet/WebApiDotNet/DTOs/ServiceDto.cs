namespace WebApiDotNet.DTOs
{
    public class ServiceDto
    {
        public string Id { get; set; }
        public string ServiceName { get; set; }
        public string CategoryId { get; set; }
        public string Icon { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
} 