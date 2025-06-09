using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace WebApiDotNet.Models
{
    public class Coupon
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public string Type { get; set; } // PERCENT, FIXED

        public double Value { get; set; }

        public double MaxDiscount { get; set; }

        public double MinOrderValue { get; set; }

        public int TotalUsageLimit { get; set; }

        public int UsedCount { get; set; }

        public string Audience { get; set; } // ALL, NEW_USER, EXISTING_USER, SPECIFIC_USERS

        public List<string> UserIds { get; set; } // List ObjectId dưới dạng string

        public bool IsActive { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
    
}
