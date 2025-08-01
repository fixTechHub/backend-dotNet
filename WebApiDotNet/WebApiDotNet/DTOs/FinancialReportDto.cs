using System.Collections.Generic;

namespace WebApiDotNet.DTOs
{
    public class FinancialReportDto
    {
        public double TotalRevenue { get; set; } // Tổng FinalPrice từ tất cả booking
        public double TotalHoldingAmount { get; set; } // Tổng HoldingAmount từ tất cả booking
        public double TotalCommissionAmount { get; set; } // Tổng CommissionAmount từ tất cả booking
        public double TotalTechnicianEarning { get; set; } // Tổng TechnicianEarning từ tất cả booking
        public double TotalWithdrawn { get; set; } // Tổng TotalWithdrawn từ tất cả technician
        public List<BookingFinancialDto> Bookings { get; set; } = new();
        public List<TechnicianFinancialDto> Technicians { get; set; } = new();
    }

    public class BookingFinancialDto
    {
        public string Id { get; set; }
        public string BookingCode { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string TechnicianId { get; set; }
        public string TechnicianName { get; set; }
        public string ServiceId { get; set; }
        public string ServiceName { get; set; }
        public double? FinalPrice { get; set; }
        public double? HoldingAmount { get; set; }
        public double? CommissionAmount { get; set; }
        public double? TechnicianEarning { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; }
        public string PaymentStatus { get; set; }
    }

    public class TechnicianFinancialDto
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public double TotalEarning { get; set; }
        public double TotalCommissionPaid { get; set; }
        public double TotalHoldingAmount { get; set; }
        public double TotalWithdrawn { get; set; }
        public List<BookingFinancialDto> Bookings { get; set; } = new();
    }

    public class FinancialSummaryDto
    {
        public double TotalRevenue { get; set; }
        public double TotalHoldingAmount { get; set; }
        public double TotalCommissionAmount { get; set; }
        public double TotalTechnicianEarning { get; set; }
        public double TotalWithdrawn { get; set; }
    }

    public class TechnicianFinancialSummaryDto
    {
        public string TechnicianId { get; set; }
        public string TechnicianName { get; set; }
        public double TotalEarning { get; set; }
        public double TotalCommissionPaid { get; set; }
        public double TotalHoldingAmount { get; set; }
        public double TotalWithdrawn { get; set; }
        public int TotalBookings { get; set; }
    }
} 