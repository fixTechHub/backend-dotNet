using System;

namespace WebApiDotNet.DTOs
{
    public class UserFilterCriteria
    {
        public bool? IsNewUser { get; set; }
        public bool? IsIntermissionUser { get; set; }
        public double? MinTotalBookingValue { get; set; }
        public double? MaxTotalBookingValue { get; set; }
        public string? BookingTimeFrom { get; set; }
        public string? BookingTimeTo { get; set; }
        public int? MinBookingCountInMonth { get; set; }
        public int? MaxBookingCountInMonth { get; set; }
        public string? Rank { get; set; } // Silver, Gold, Diamond, VIP
    }
} 