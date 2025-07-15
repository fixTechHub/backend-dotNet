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
        public DateTime RequestDate { get; set; }

        [BsonElement("reportedIssue")]
        public string ReportedIssue { get; set; }

        [BsonElement("isUnderWarranty")]
        public bool IsUnderWarranty { get; set; } = true;

        [BsonElement("expireAt")]
        public DateTime? ExpireAt { get; set; }

        [BsonElement("status")]
        public string Status { get; set; } = "PENDING"; // PENDING, CONFIRMED, RESOLVED, DENIED, EXPIRED

        [BsonElement("resolutionNote")]
        public string? ResolutionNote { get; set; }

        [BsonElement("rejectionReason")]
        public string? RejectionReason { get; set; }

        [BsonElement("isReviewedByAdmin")]
        public bool IsReviewedByAdmin { get; set; } = false;

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }
} 