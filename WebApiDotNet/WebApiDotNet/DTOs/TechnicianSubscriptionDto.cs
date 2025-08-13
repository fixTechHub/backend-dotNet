using System;

namespace WebApiDotNet.DTOs
{
    public class TechnicianSubscriptionDto
    {
        public string Id { get; set; }
        public string TechnicianId { get; set; }
        public string PackageId { get; set; }
        public string Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public double Amount { get; set; }
        public string PaymentStatus { get; set; }
        public string? PaymentMethod { get; set; }
        public string? TransactionId { get; set; }
        public bool AutoRenew { get; set; }
        public DateTime? CancelledAt { get; set; }
        public string? CancellationReason { get; set; }
        public List<PaymentHistoryItemDto> PaymentHistory { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        
    }

    public class CreateTechnicianSubscriptionDto
    {
        public string TechnicianId { get; set; }
        public string PackageId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public double Amount { get; set; }
        public string? PaymentMethod { get; set; }
        public bool AutoRenew { get; set; } = false;
    }

    public class UpdateTechnicianSubscriptionDto
    {
        public string? Status { get; set; }
        public DateTime? EndDate { get; set; }
        public string? PaymentStatus { get; set; }
        public string? PaymentMethod { get; set; }
        public string? TransactionId { get; set; }
        public bool? AutoRenew { get; set; }
        public DateTime? CancelledAt { get; set; }
        public string? CancellationReason { get; set; }
    }

    public class TechnicianSubscriptionSummaryDto
    {
        public string Id { get; set; }
        public string TechnicianName { get; set; }
        public string PackageName { get; set; }
        public string Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public double Amount { get; set; }
        public string PaymentStatus { get; set; }
        public bool AutoRenew { get; set; }
    }

    public class PaymentHistoryItemDto
    {
        public double Amount { get; set; }
        public string PaymentStatus { get; set; }
        public string? PaymentMethod { get; set; }
        public string? TransactionId { get; set; }
        public DateTime? PaidAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}