using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace WebApiDotNet.Models
{
    
    public class BookingPrice
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("bookingId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string BookingId { get; set; }

        [BsonElement("technicianId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string TechnicianId { get; set; }

        [BsonElement("commissionConfigId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string CommissionConfigId { get; set; }

        [BsonElement("laborPrice")]
        public decimal LaborPrice { get; set; }

        [BsonElement("status")]
        public string Status { get; set; }

        [BsonElement("quotedAt")]
        public DateTime? QuotedAt { get; set; }

        [BsonElement("expiresAt")]
        public DateTime? ExpiresAt { get; set; }

        [BsonElement("finalPrice")]
        public decimal? FinalPrice { get; set; }

        [BsonElement("warrantiesDuration")]
        public int? WarrantiesDuration { get; set; }

        [BsonElement("discountValue")]
        public decimal? DiscountValue { get; set; }

        [BsonElement("discountCode")]
        public string DiscountCode { get; set; }

        [BsonElement("extraDescription")]
        public string ExtraDescription { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }
} 