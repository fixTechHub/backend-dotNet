using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace WebApiDotNet.DTOs
{
    public class ServiceDto
    {
        public string Id { get; set; }
        public string ServiceName { get; set; }
        public string CategoryId { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
    }
} 