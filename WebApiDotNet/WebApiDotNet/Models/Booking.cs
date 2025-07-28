using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace WebApiDotNet.Models
{
    [BsonIgnoreExtraElements]
    public class Booking
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("bookingCode")]
        [BsonRequired]
        public string BookingCode { get; set; }

        [BsonElement("customerId")]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonRequired]
        public string CustomerId { get; set; }

        [BsonElement("technicianId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string TechnicianId { get; set; }

        [BsonElement("serviceId")]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonRequired]
        public string ServiceId { get; set; }

        [BsonElement("location")]
        public BookingLocation Location { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("images")]
        public List<string> Images { get; set; }

        [BsonElement("schedule")]
        public Schedule Schedule { get; set; }

        [BsonElement("isUrgent")]
        public bool IsUrgent { get; set; } = false;

        [BsonElement("quote")]
        public Quote Quote { get; set; }

        [BsonElement("discountCode")]
        public string DiscountCode { get; set; }

        [BsonElement("discountValue")]
        public double DiscountValue { get; set; } = 0;

        [BsonElement("technicianEarning")]
        public double? TechnicianEarning { get; set; }

        [BsonElement("commissionAmount")]
        public double? CommissionAmount { get; set; }

        [BsonElement("holdingAmount")]
        public double? HoldingAmount { get; set; }

        [BsonElement("finalPrice")]
        public double? FinalPrice { get; set; }

        [BsonElement("customerConfirmedDone")]
        public bool CustomerConfirmedDone { get; set; } = false;

        [BsonElement("technicianConfirmedDone")]
        public bool TechnicianConfirmedDone { get; set; } = false;

        [BsonElement("status")]
        [BsonRepresentation(BsonType.String)]
        public BookingStatus Status { get; set; } = BookingStatus.PENDING;

        [BsonElement("isChatAllowed")]
        public bool IsChatAllowed { get; set; } = false;

        [BsonElement("isVideoCallAllowed")]
        public bool IsVideoCallAllowed { get; set; } = false;

        [BsonElement("warrantyExpiresAt")]
        public DateTime? WarrantyExpiresAt { get; set; }

        [BsonElement("completedAt")]
        public DateTime? CompletedAt { get; set; }

        [BsonElement("cancelledBy")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string CancelledBy { get; set; }

        [BsonElement("cancellationReason")]
        public string CancellationReason { get; set; }

        [BsonElement("paymentStatus")]
        [BsonRepresentation(BsonType.String)]
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.PENDING;

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }

    public enum BookingStatus
    {
        PENDING,
        CONFIRMED,
        IN_PROGRESS,
        AWAITING_DONE,
        WAITING_CONFIRM,
        AWAITING_CONFIRM,
        CONFIRM_ADDITIONAL,
        DONE,
        CANCELLED,
        WAITING_CUSTOMER_CONFIRM_ADDITIONAL,
        WAITING_TECHNICIAN_CONFIRM_ADDITIONAL
    }

    public enum PaymentStatus
    {
        PENDING,
        PAID,
        FAILED,
        CANCELLED,
        REFUNDED
        
    }

    public enum QuoteStatus
    {
        PENDING,
        ACCEPTED,
        REJECTED
    }

    [BsonIgnoreExtraElements]
    public class BookingLocation
    {
        [BsonElement("address")]
        public string Address { get; set; }

        [BsonElement("geojson")]
        public GeoJson GeoJson { get; set; }
    }

    public class GeoJson
    {
        [BsonElement("type")]
        public string Type { get; set; } = "Point";

        [BsonElement("coordinates")]
        public double[] Coordinates { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class QuoteItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("name")]
        public string Name { get; set; }
        [BsonElement("price")]
        public double Price { get; set; }
        [BsonElement("quantity")]
        public int Quantity { get; set; }
        [BsonElement("note")]
        public string Note { get; set; }
    }

    public class Quote
    {
        [BsonElement("status")]
        [BsonRepresentation(BsonType.String)]
        public QuoteStatus Status { get; set; } = QuoteStatus.PENDING;

        [BsonElement("commissionConfigId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string CommissionConfigId { get; set; }

        [BsonElement("laborPrice")]
        public double LaborPrice { get; set; } = 0;

        [BsonElement("items")]
        public List<QuoteItem> Items { get; set; }

        [BsonElement("totalAmount")]
        public double? TotalAmount { get; set; }

        [BsonElement("warrantiesDuration")]
        public int WarrantiesDuration { get; set; } = 30;

        [BsonElement("justification")]
        public string Justification { get; set; }

        [BsonElement("quotedAt")]
        public DateTime? QuotedAt { get; set; } = DateTime.UtcNow;
    }

    [BsonIgnoreExtraElements]
    public class Schedule
    {
        [BsonElement("startTime")]
        public DateTime StartTime { get; set; }

        [BsonElement("endTime")]
        public DateTime? EndTime { get; set; }

        [BsonElement("expectedEndTime")]
        public DateTime? ExpectedEndTime { get; set; }
    }
} 