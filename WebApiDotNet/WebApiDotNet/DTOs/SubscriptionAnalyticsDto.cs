using System;
using System.Collections.Generic;

namespace WebApiDotNet.DTOs
{
    public class SubscriptionAnalyticsDto
    {
        // Tổng quan
        public int TotalSubscriptions { get; set; }
        public int ActiveSubscriptions { get; set; }
        public int ExpiredSubscriptions { get; set; }
        public int PendingSubscriptions { get; set; }
        public int CancelledSubscriptions { get; set; }
        
        // Doanh thu
        public decimal TotalRevenue { get; set; }
        public decimal AvgRevenuePerSubscription { get; set; }
        public decimal RevenueGrowth { get; set; }
        
        // Tỷ lệ
        public double ConversionRate { get; set; }
        public double ChurnRate { get; set; }
        public double RetentionRate { get; set; }
        
        // Tỷ lệ rời bỏ chi tiết
        public double CustomerCancellationRate { get; set; }  // Tỷ lệ khách hàng tự hủy (CANCELLED)
        public double TotalChurnRate { get; set; }            // Tỷ lệ rời bỏ tổng hợp (tất cả trạng thái không hoạt động)
        public double ExpiredChurnRate { get; set; }          // Tỷ lệ rời bỏ do hết hạn (EXPIRED)
        public double SuspendedChurnRate { get; set; }        // Tỷ lệ rời bỏ do bị đình chỉ (SUSPENDED)
        
        // Dữ liệu theo thời gian
        public List<MonthlyMetricDto> MonthlyMetrics { get; set; } = new List<MonthlyMetricDto>();
        public List<QuarterlyMetricDto> QuarterlyMetrics { get; set; } = new List<QuarterlyMetricDto>();
        
        // Phân tích gói
        public List<PackageAnalyticsDto> PackageAnalytics { get; set; } = new List<PackageAnalyticsDto>();
        
        // Phân tích trạng thái
        public List<StatusAnalyticsDto> StatusAnalytics { get; set; } = new List<StatusAnalyticsDto>();
        
        // Chi tiết ExtraFromTechnicianEarning theo thời gian
        public List<TechnicianEarningDetailDto> TechnicianEarningDetails { get; set; } = new List<TechnicianEarningDetailDto>();
        public decimal TotalExtraFromTechnicianEarning { get; set; }
    }

    public class MonthlyMetricDto
    {
        public int Month { get; set; }
        public string MonthName { get; set; } = string.Empty;
        public int Subscriptions { get; set; }
        public int ActiveSubscriptions { get; set; }
        public decimal Revenue { get; set; }
        public double ConversionRate { get; set; }
    }

    public class QuarterlyMetricDto
    {
        public int Quarter { get; set; }
        public int Subscriptions { get; set; }
        public int ActiveSubscriptions { get; set; }
        public decimal Revenue { get; set; }
        public double ConversionRate { get; set; }
    }

    public class PackageAnalyticsDto
    {
        public string PackageId { get; set; } = string.Empty;
        public string PackageName { get; set; } = string.Empty; // Thêm tên Package
        public int TotalSubscriptions { get; set; }
        public int ActiveSubscriptions { get; set; }
        public decimal Revenue { get; set; }
        public decimal AvgPrice { get; set; }
        public double ConversionRate { get; set; }
    }

    public class StatusAnalyticsDto
    {
        public string Status { get; set; } = string.Empty;
        public int Count { get; set; }
        public decimal Revenue { get; set; }
        public decimal AvgRevenue { get; set; }
    }

    public class TechnicianEarningDetailDto
    {
        public DateTime Date { get; set; }
        public string TimeLabel { get; set; } = string.Empty; // "Jan", "Q1", etc.
        public int BookingCount { get; set; }
        public decimal TotalTechnicianEarning { get; set; }
        public decimal ExtraFromTechnicianEarning { get; set; } // 8% của TechnicianEarning
        public decimal AveragePerBooking { get; set; }
        public List<BookingEarningDetailDto> BookingDetails { get; set; } = new List<BookingEarningDetailDto>();
    }

    public class BookingEarningDetailDto
    {
        public string BookingId { get; set; } = string.Empty;
        public string BookingCode { get; set; } = string.Empty;
        public DateTime BookingDate { get; set; }
        public string TechnicianId { get; set; } = string.Empty;
        public string TechnicianName { get; set; } = string.Empty;
        public string ServiceName { get; set; } = string.Empty;
        public decimal TechnicianEarning { get; set; }
        public decimal ExtraFromBooking { get; set; } // 8% của TechnicianEarning
        public string PaymentStatus { get; set; } = string.Empty;
    }
}
