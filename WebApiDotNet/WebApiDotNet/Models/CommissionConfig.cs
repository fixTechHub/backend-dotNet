using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace WebApiDotNet.Models
{
    [BsonIgnoreExtraElements]
    public class CommissionConfig
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("commissionPercent")]
        public double CommissionPercent { get; set; }

        [BsonElement("holdingPercent")]
        public double HoldingPercent { get; set; }

        [BsonElement("commissionMinAmount")]
        public double CommissionMinAmount { get; set; } = 0;

        [BsonElement("commissionType")]
        [BsonRepresentation(BsonType.String)]
        public CommissionType CommissionType { get; set; } = CommissionType.PERCENT;

        [BsonElement("startDate")]
        public DateTime StartDate { get; set; }

        [BsonElement("isApplied")]
        public bool IsApplied { get; set; } = true;

        [BsonElement("isDeleted")]
        public bool IsDeleted { get; set; } = false;
        
        [BsonElement("deletedAt")]
        public DateTime? DeletedAt { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }= DateTime.UtcNow;

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; }= DateTime.UtcNow;
    }

    public enum CommissionType
    {
        PERCENT,
        MIN_AMOUNT
    }
}