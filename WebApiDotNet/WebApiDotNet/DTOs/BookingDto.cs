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
        public DateTime Schedule { get; set; }
        public bool CustomerConfirmedDone { get; set; }
        public bool TechnicianConfirmedDone { get; set; }
        public string Status { get; set; }
        public bool IsChatAllowed { get; set; }
        public bool IsVideoCallAllowed { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class BookingLocationDto
    {
        public string Address { get; set; }
        public GeoJsonDto GeoJson { get; set; }
    }

    public class GeoJsonDto
    {
        public string Type { get; set; }
        public List<double> Coordinates { get; set; }
    }
} 