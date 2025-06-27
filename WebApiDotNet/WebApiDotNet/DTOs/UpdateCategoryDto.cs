using System;

namespace WebApiDotNet.DTOs
{
    public class UpdateCategoryDto
    {
        public string CategoryName { get; set; }
        public string Icon { get; set; }
        public bool IsActive { get; set; }
    }
} 