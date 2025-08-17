using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApiDotNet.Models
{
    [BsonIgnoreExtraElements]
    public class Report
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("type")]
        [BsonRepresentation(BsonType.String)]
        public ReportType Type { get; set; } = ReportType.BOOKING;

        [BsonElement("bookingId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? BookingId { get; set; }

        [BsonElement("warrantyId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? WarrantyId { get; set; }

        [BsonElement("reporterId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ReporterId { get; set; }

        [BsonElement("reportedUserId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ReportedUserId { get; set; }

        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("tag")]
        [BsonRepresentation(BsonType.String)]
        public ReportTag Tag { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("evidences")]
        public List<string> Evidences { get; set; } = new List<string>();

        [BsonElement("status")]
        [BsonRepresentation(BsonType.String)]
        public ReportStatus Status { get; set; } = ReportStatus.PENDING;

        [BsonElement("responseDeadline")]
        public DateTime? ResponseDeadline { get; set; }

        [BsonElement("responseLocked")]
        public bool ResponseLocked { get; set; } = false;

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

    public enum ReportType
    {
        BOOKING,
        WARRANTY,
        VIOLATION
    }

    public enum ReportTag
    {
        NO_SHOW,
        LATE,
        RUDE,
        ISSUE,
        OTHER,
        WARRANTY_DENIED,
        WARRANTY_DELAY,
        POOR_FIX
    }

    public enum ReportStatus
    {
        PENDING,
        AWAITING_RESPONSE,
        REJECTED,
        RESOLVED
    }
}