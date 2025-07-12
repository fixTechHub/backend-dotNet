using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace WebApiDotNet.Models
{
    [BsonIgnoreExtraElements]
    public class Coupon
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("code")]
        [BsonRequired]
        public string Code { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("type")]
        [BsonRepresentation(BsonType.String)]
        [BsonRequired]
        public CouponType Type { get; set; }

        [BsonElement("value")]
        [BsonRequired]
        public double Value { get; set; }

        [BsonElement("maxDiscount")]
        public double? MaxDiscount { get; set; }

        [BsonElement("minOrderValue")]
        public double MinOrderValue { get; set; } = 0;

        [BsonElement("totalUsageLimit")]
        public int TotalUsageLimit { get; set; } = 1;

        [BsonElement("usedCount")]
        public int UsedCount { get; set; } = 0;

        [BsonElement("audience")]
        [BsonRepresentation(BsonType.String)]
        public CouponAudience Audience { get; set; } = CouponAudience.ALL;

        [BsonElement("userIds")]
        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> UserIds { get; set; }

        [BsonElement("isActive")]
        public bool IsActive { get; set; } = true;

        [BsonElement("startDate")]
        [BsonRequired]
        public DateTime StartDate { get; set; }

        [BsonElement("endDate")]
        [BsonRequired]
        public DateTime EndDate { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("isDeleted")]
        public bool IsDeleted { get; set; } = false;

        [BsonElement("deletedAt")]
        public DateTime? DeletedAt { get; set; }
    }

    public enum CouponType
    {
        PERCENT,
        FIXED
    }

    public enum CouponAudience
    {
        ALL,
        NEW_USER,
        EXISTING_USER,
        SPECIFIC_USERS
    }
}
