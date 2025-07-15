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
        public string Tag { get; set; } // SYSTEM, PAYMENT, UI, OTHER

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("status")]
        public string Status { get; set; } = "PENDING"; // PENDING, IN_PROGRESS, RESOLVED, REJECTED

        [BsonElement("resolvedBy")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? ResolvedBy { get; set; }

        [BsonElement("resolutionNote")]
        public string? ResolutionNote { get; set; }

        [BsonElement("resolvedAt")]
        public DateTime? ResolvedAt { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }
}