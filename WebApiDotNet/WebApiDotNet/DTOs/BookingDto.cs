using System.Collections.Generic;

namespace WebApiDotNet.DTOs
{
    public class BookingDto
    {
        public string Id { get; set; }
        public string BookingCode { get; set; }
        public string CustomerId { get; set; }
        public string TechnicianId { get; set; }
        public string ServiceId { get; set; }
        public BookingLocationDto Location { get; set; }
        public string Description { get; set; }
        public List<string> Images { get; set; }
        public ScheduleDto Schedule { get; set; }
        public bool CustomerConfirmedDone { get; set; }
        public bool TechnicianConfirmedDone { get; set; }
        public string Status { get; set; } // BookingStatus: PENDING, CONFIRMED, IN_PROGRESS, AWAITING_DONE, DONE, CANCELLED
        public bool IsChatAllowed { get; set; }
        public bool IsVideoCallAllowed { get; set; }
        public string PaymentStatus { get; set; } // PaymentStatus: PENDING, PAID, FAILED, CANCELLED, REFUNDED
        public bool IsUrgent { get; set; }
        public QuoteDto Quote { get; set; }
        public string DiscountCode { get; set; }
        public double DiscountValue { get; set; }
        public double? TechnicianEarning { get; set; }
        public double? CommissionAmount { get; set; }
        public double? HoldingAmount { get; set; }
        public double? FinalPrice { get; set; }
        public DateTime? WarrantyExpiresAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string CancelledBy { get; set; }
        public string CancellationReason { get; set; }
    }

    public class BookingLocationDto
    {
        public string Address { get; set; }
        public GeoJsonDto GeoJson { get; set; }
    }

    public class GeoJsonDto
    {
        public string Type { get; set; }
        public double[] Coordinates { get; set; }
    }

    public class ScheduleDto
    {
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime? ExpectedEndTime { get; set; }
    }

    public class QuoteDto
    {
        public string Status { get; set; } // QuoteStatus: PENDING, ACCEPTED, REJECTED
        public string CommissionConfigId { get; set; }
        public double LaborPrice { get; set; }
        public List<QuoteItemDto> Items { get; set; }
        public double? TotalAmount { get; set; }
        public int WarrantiesDuration { get; set; }
        public string Justification { get; set; }
        public DateTime? QuotedAt { get; set; }
    }
    public class QuoteItemDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string Note { get; set; }
    }
} 