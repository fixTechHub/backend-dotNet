using System;

namespace WebApiDotNet.DTOs
{
    public class CategoryDto
    {
        public string Id { get; set; }
        public string CategoryName { get; set; }
        public string Icon { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
} 