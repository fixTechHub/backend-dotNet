using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApiDotNet.Models
{
    [BsonIgnoreExtraElements]
    public class TechnicianSubscription
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("technician")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string TechnicianId { get; set; }

        [BsonElement("package")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string PackageId { get; set; }

        [BsonElement("status")]
        [BsonRepresentation(BsonType.String)]
        public SubscriptionStatus Status { get; set; } = SubscriptionStatus.ACTIVE;

        [BsonElement("startDate")]
        public DateTime StartDate { get; set; }

        [BsonElement("endDate")]
        public DateTime? EndDate { get; set; }

        [BsonElement("amount")]
        public double Amount { get; set; }

        [BsonElement("paymentStatus")]
        [BsonRepresentation(BsonType.String)]
        public SubscriptionPaymentStatus PaymentStatus { get; set; } = SubscriptionPaymentStatus.PENDING;

        [BsonElement("paymentMethod")]
        public string? PaymentMethod { get; set; }

        [BsonElement("transactionId")]
        public string? TransactionId { get; set; }

        [BsonElement("autoRenew")]
        public bool AutoRenew { get; set; } = false;

        [BsonElement("cancelledAt")]
        public DateTime? CancelledAt { get; set; }

        [BsonElement("cancellationReason")]
        public string? CancellationReason { get; set; }

        [BsonElement("paymentHistory")]
        public List<PaymentHistoryItem> PaymentHistory { get; set; } = new();

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

    public enum SubscriptionStatus
    {
        ACTIVE,
        EXPIRED,
        CANCELLED,
        SUSPENDED,
        PENDING_ACTIVATION
    }

    public enum SubscriptionPaymentStatus
    {
        PENDING,
        PAID,
        FAILED,
        CANCELLED,
        REFUNDED
    }

    [BsonIgnoreExtraElements]
    public class PaymentHistoryItem
    {
        [BsonElement("amount")]
        public double Amount { get; set; }

        [BsonElement("paymentStatus")]
        [BsonRepresentation(BsonType.String)]
        public SubscriptionPaymentStatus PaymentStatus { get; set; }

        [BsonElement("paymentMethod")]
        public string? PaymentMethod { get; set; }

        [BsonElement("transactionId")]
        public string? TransactionId { get; set; }

        [BsonElement("paidAt")]
        public DateTime? PaidAt { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}