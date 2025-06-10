using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace WebApiDotNet.Models
{
    public class Coupon
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("code")]
        public string Code { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("type")]
        public string Type { get; set; } // PERCENT, FIXED

        [BsonElement("value")]
        public double Value { get; set; }

        [BsonElement("maxDiscount")]
        public double MaxDiscount { get; set; }

        [BsonElement("minOrderValue")]
        public double MinOrderValue { get; set; }

        [BsonElement("totalUsageLimit")]
        public int TotalUsageLimit { get; set; }

        [BsonElement("usedCount")]
        public int UsedCount { get; set; }

        [BsonElement("audience")]
        public string Audience { get; set; }

        [BsonElement("userIds")]
        public List<string> UserIds { get; set; }

        [BsonElement("isActive")]
        public bool IsActive { get; set; }

        [BsonElement("startDate")]
        public DateTime StartDate { get; set; }

        [BsonElement("endDate")]
        public DateTime EndDate { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }
    
}
