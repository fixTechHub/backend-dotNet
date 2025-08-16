using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace WebApiDotNet.Models
{
    [BsonIgnoreExtraElements]
    public class BookingStatusLog
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [Required]
        [BsonElement("bookingId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string BookingId { get; set; } = string.Empty;

        [Required]
        [BsonElement("fromStatus")]
        public string FromStatus { get; set; } = string.Empty;

        [Required]
        [BsonElement("toStatus")]
        public string ToStatus { get; set; } = string.Empty;

        [Required]
        [BsonElement("changedBy")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ChangedBy { get; set; } = string.Empty;

        [Required]
        [BsonElement("role")]
        public string Role { get; set; } = string.Empty;

        [BsonElement("note")]
        public string? Note { get; set; }

        [BsonElement("createdAt")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties (optional, for reference)
        [BsonIgnore]
        public Booking? Booking { get; set; }

        [BsonIgnore]
        public User? User { get; set; }
    }

    public enum UserRole
    {
        CUSTOMER,
        TECHNICIAN,
        SYSTEM,
        ADMIN
    }
}
