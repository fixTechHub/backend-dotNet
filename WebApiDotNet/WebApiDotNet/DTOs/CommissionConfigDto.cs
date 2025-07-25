using System;

namespace WebApiDotNet.DTOs
{
    public class CommissionConfigDto
    {
        public string Id { get; set; }
        public double CommissionPercent { get; set; }
        public double HoldingPercent { get; set; }
        public double CommissionMinAmount { get; set; }
        public string CommissionType { get; set; } // "PERCENT" | "MIN_AMOUNT"
        public DateTime StartDate { get; set; }
        public bool IsApplied { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
    }
} 