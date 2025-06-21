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
        public string Type { get; set; } // REPORT

        [BsonElement("reportedUserId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ReportedUserId { get; set; }

        [BsonElement("reporterId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ReporterId { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("status")]
        public string Status { get; set; } // pending, resolved, rejected,...

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }
}