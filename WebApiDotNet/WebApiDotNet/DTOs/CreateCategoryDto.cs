using System;
using System.ComponentModel.DataAnnotations;

namespace WebApiDotNet.DTOs
{
    public class CreateCategoryDto
    {
        [Required(ErrorMessage = "CategoryName is required")]
        public string CategoryName { get; set; }
        [Required(ErrorMessage = "Icon is required")]
        public string Icon { get; set; }
        public bool IsActive { get; set; } = true;
    }
} 