namespace WebApiDotNet.DTOs
{
    public class UpdateUserDto
    {
        public string? Role { get; set; }
        public string? Status { get; set; }
        public string? LockedReason { get; set; }
    }
} 