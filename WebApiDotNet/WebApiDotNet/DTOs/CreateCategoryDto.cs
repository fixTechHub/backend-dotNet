using System;

namespace WebApiDotNet.DTOs
{
    public class CreateCategoryDto
    {
        public string CategoryName { get; set; }
        public string Icon { get; set; }
        public bool IsActive { get; set; } = true;
    }
} 