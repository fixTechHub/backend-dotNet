using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApiDotNet.Models
{
    [BsonIgnoreExtraElements]
    public class Booking
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("bookingCode")]
        public string BookingCode { get; set; }

        [BsonElement("customerId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string CustomerId { get; set; }

        [BsonElement("technicianId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string TechnicianId { get; set; }

        [BsonElement("serviceId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ServiceId { get; set; }

        [BsonElement("location")]
        public BookingLocation Location { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("images")]
        public List<string> Images { get; set; }

        [BsonElement("schedule")]
        public Schedule Schedule { get; set; }

        [BsonElement("customerConfirmedDone")]
        public bool CustomerConfirmedDone { get; set; }

        [BsonElement("technicianConfirmedDone")]
        public bool TechnicianConfirmedDone { get; set; }

        [BsonElement("status")]
        public string Status { get; set; }

        [BsonElement("isChatAllowed")]
        public bool IsChatAllowed { get; set; }

        [BsonElement("isVideoCallAllowed")]
        public bool IsVideoCallAllowed { get; set; }

        [BsonElement("paymentStatus")]
        public string PaymentStatus { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class BookingLocation
    {
        [BsonElement("address")]
        public string Address { get; set; }
        [BsonElement("geojson")]
        public GeoJson GeoJson { get; set; }
    }

    public class GeoJson
    {
        [BsonElement("type")]
        public string Type { get; set; }
        [BsonElement("coordinates")]
        public List<double> Coordinates { get; set; }
    }

    public class Schedule
    {
        [BsonElement("startTime")]
        public DateTime StartTime { get; set; }

        [BsonElement("endTime")]
        public DateTime EndTime { get; set; }
    }
} 