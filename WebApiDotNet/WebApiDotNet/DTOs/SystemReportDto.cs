namespace WebApiDotNet.DTOs
{
    public class SystemReportDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Tag { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string SubmittedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
