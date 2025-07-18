using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApiDotNet.Models
{
    [BsonIgnoreExtraElements]
    public class SystemReport
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("submittedBy")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string SubmittedBy { get; set; }

        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("tag")]
        [BsonRepresentation(BsonType.String)]
        public SystemReportTag Tag { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("status")]
        [BsonRepresentation(BsonType.String)]
        public SystemReportStatus Status { get; set; } = SystemReportStatus.PENDING;

        [BsonElement("resolvedBy")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? ResolvedBy { get; set; }

        [BsonElement("resolutionNote")]
        public string? ResolutionNote { get; set; }

        [BsonElement("resolvedAt")]
        public DateTime? ResolvedAt { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
    public enum SystemReportTag
    {
        SYSTEM,
        PAYMENT,
        UI,
        OTHER
    }

    public enum SystemReportStatus
    {
        PENDING,
        IN_PROGRESS,
        RESOLVED,
        REJECTED
    }

}