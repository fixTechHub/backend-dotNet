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
        public ReportType Type { get; set; } = ReportType.REPORT;

        [BsonElement("reportedUserId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ReportedUserId { get; set; }

        [BsonElement("reporterId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ReporterId { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("status")]
        [BsonRepresentation(BsonType.String)]
        public ReportStatus Status { get; set; } = ReportStatus.PENDING;

        [BsonElement("penalty")]
        public string? Penalty { get; set; } = null;

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

    public enum ReportType
    {
        REPORT,
        VIOLATION
    }

    public enum ReportStatus
    {
        PENDING,
        CONFIRMED,
        REJECTED,
        RESOLVED,
        CLOSED
    }
}