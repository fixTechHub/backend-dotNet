using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace WebApiDotNet.Models
{
    public class Service
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("serviceName")]
        public string ServiceName { get; set; }

        [BsonElement("categoryId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string CategoryId { get; set; }

        [BsonElement("serviceType")]
        public ServiceType ServiceType { get; set; }

        [BsonElement("estimatedMarketPrice")]
        public EstimatedMarketPrice EstimatedMarketPrice { get; set; }

        [BsonElement("icon")]
        public string Icon { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("isActive")]
        public bool IsActive { get; set; } = true;

        [BsonElement("isDeleted")]
        public bool IsDeleted { get; set; } = false;

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; }
        [BsonElement("deletedAt")]
        public DateTime? DeletedAt { get; set; }
    }

    public class EstimatedMarketPrice
    {
        [BsonElement("min")]
        public double? Min { get; set; }

        [BsonElement("max")]
        public double? Max { get; set; }
    }

    public enum ServiceType
    {
        FIXED,
        COMPLEX
    }
}
