using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace WebApiDotNet.Models
{
    [BsonIgnoreExtraElements]
    public class Warranty
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("bookingId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string BookingId { get; set; }

        [BsonElement("customerId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string CustomerId { get; set; }

        [BsonElement("technicianId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string TechnicianId { get; set; }

        [BsonElement("requestDate")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime RequestDate { get; set; }

        [BsonElement("reportedIssue")]
        public string ReportedIssue { get; set; }

        [BsonElement("isUnderWarranty")]
        public bool IsUnderWarranty { get; set; } = true;

        [BsonElement("expireAt")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime? ExpireAt { get; set; }

        [BsonElement("status")]
        [BsonRepresentation(BsonType.String)]
        public WarrantyStatus Status { get; set; } = WarrantyStatus.PENDING;

        [BsonElement("resolutionNote")]
        public string? ResolutionNote { get; set; }

        [BsonElement("rejectionReason")]
        public string? RejectionReason { get; set; }

        [BsonElement("isReviewedByAdmin")]
        public bool IsReviewedByAdmin { get; set; } = false;

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

    public enum WarrantyStatus
    {
        PENDING,
        CONFIRMED,
        RESOLVED,
        DENIED,
        EXPIRED,
        DONE
    }
} 