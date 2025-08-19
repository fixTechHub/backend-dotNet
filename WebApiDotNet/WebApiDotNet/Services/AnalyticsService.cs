using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiDotNet.DTOs;
using WebApiDotNet.Models;
using WebApiDotNet.Repository.IRepository;

namespace WebApiDotNet.Services
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly ITechnicianSubscriptionRepository _subscriptionRepository;
        private readonly IPackageRepository _packageRepository;

        public AnalyticsService(ITechnicianSubscriptionRepository subscriptionRepository, IPackageRepository packageRepository)
        {
            _subscriptionRepository = subscriptionRepository;
            _packageRepository = packageRepository;
        }

        public async Task<SubscriptionAnalyticsDto> GetSubscriptionAnalyticsAsync(int year, string timeRange)
        {
            var subscriptions = await _subscriptionRepository.GetSubscriptionsByYearAsync(year);
            
            // Lọc dữ liệu theo timeRange
            var filteredSubscriptions = FilterSubscriptionsByTimeRange(subscriptions, year, timeRange);
            
            var result = new SubscriptionAnalyticsDto
            {
                TotalSubscriptions = filteredSubscriptions.Count(),
                ActiveSubscriptions = filteredSubscriptions.Count(s => s.Status == SubscriptionStatus.ACTIVE),
                ExpiredSubscriptions = filteredSubscriptions.Count(s => s.Status == SubscriptionStatus.EXPIRED),
                PendingSubscriptions = filteredSubscriptions.Count(s => s.Status == SubscriptionStatus.PENDING_ACTIVATION),
                CancelledSubscriptions = filteredSubscriptions.Count(s => s.Status == SubscriptionStatus.CANCELLED),
                
                TotalRevenue = (decimal)filteredSubscriptions.Sum(s => s.Amount),
                AvgRevenuePerSubscription = filteredSubscriptions.Any() ? 
                    (decimal)(filteredSubscriptions.Sum(s => s.Amount) / (double)filteredSubscriptions.Count()) : 0m,
                
                RevenueGrowth = CalculateRevenueGrowth(filteredSubscriptions, year),
                ConversionRate = CalculateConversionRate(filteredSubscriptions),
                ChurnRate = CalculateChurnRate(filteredSubscriptions),
                RetentionRate = CalculateRetentionRate(filteredSubscriptions),
                
                MonthlyMetrics = CalculateMonthlyMetrics(filteredSubscriptions, year, timeRange),
                QuarterlyMetrics = CalculateQuarterlyMetrics(filteredSubscriptions, year, timeRange),
                PackageAnalytics = await CalculatePackageAnalyticsAsync(filteredSubscriptions),
                StatusAnalytics = CalculateStatusAnalytics(filteredSubscriptions)
            };

            return result;
        }

        private double CalculateConversionRate(IEnumerable<TechnicianSubscription> subscriptions)
        {
            var total = subscriptions.Count();
            var active = subscriptions.Count(s => s.Status == SubscriptionStatus.ACTIVE);
            return total > 0 ? (double)active / total * 100 : 0;
        }

        private double CalculateChurnRate(IEnumerable<TechnicianSubscription> subscriptions)
        {
            var total = subscriptions.Count();
            var cancelled = subscriptions.Count(s => s.Status == SubscriptionStatus.CANCELLED);
            return total > 0 ? (double)cancelled / total * 100 : 0;
        }

        private double CalculateRetentionRate(IEnumerable<TechnicianSubscription> subscriptions)
        {
            var total = subscriptions.Count();
            var retained = subscriptions.Count(s => s.Status == SubscriptionStatus.ACTIVE || s.Status == SubscriptionStatus.PENDING_ACTIVATION);
            return total > 0 ? (double)retained / total * 100 : 0;
        }

        private decimal CalculateRevenueGrowth(IEnumerable<TechnicianSubscription> subscriptions, int year)
        {
            // Tính doanh thu năm hiện tại
            var currentYearRevenue = subscriptions.Sum(s => s.Amount);
            
            // TODO: Implement logic tính doanh thu năm trước
            // Hiện tại return 0, có thể mở rộng sau
            return 0m;
        }

        private IEnumerable<TechnicianSubscription> FilterSubscriptionsByTimeRange(
            IEnumerable<TechnicianSubscription> subscriptions, int year, string timeRange)
        {
            var currentDate = DateTime.Now;
            var currentYear = currentDate.Year;
            var currentMonth = currentDate.Month;
            
            switch (timeRange?.ToLower())
            {
                case "month":
                    // Chỉ lấy dữ liệu của tháng hiện tại
                    if (year == currentYear)
                    {
                        return subscriptions.Where(s => s.CreatedAt.Month == currentMonth);
                    }
                    else
                    {
                        // Nếu không phải năm hiện tại, trả về tháng đầu tiên của năm đó
                        return subscriptions.Where(s => s.CreatedAt.Month == 1);
                    }
                    
                case "quarter":
                case "year":
                default:
                    // Trả về dữ liệu của cả năm (quarter và year đều hiển thị đầy đủ)
                    return subscriptions;
            }
        }

        private List<MonthlyMetricDto> CalculateMonthlyMetrics(IEnumerable<TechnicianSubscription> subscriptions, int year, string timeRange)
        {
            var months = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", 
                                "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            
            var result = new List<MonthlyMetricDto>();
            
            switch (timeRange?.ToLower())
            {
                case "month":
                    // Chỉ hiển thị tháng hiện tại
                    var currentMonth = DateTime.Now.Month;
                    if (year == DateTime.Now.Year)
                    {
                        var monthSubscriptions = subscriptions.Where(s => s.CreatedAt.Month == currentMonth).ToList();
                        var monthActive = monthSubscriptions.Count(s => s.Status == SubscriptionStatus.ACTIVE);
                        var monthRevenue = monthSubscriptions.Sum(s => s.Amount);
                        var conversionRate = monthSubscriptions.Count > 0 ? 
                            (double)monthActive / monthSubscriptions.Count * 100 : 0;
                        
                        result.Add(new MonthlyMetricDto
                        {
                            Month = currentMonth,
                            MonthName = months[currentMonth - 1],
                            Subscriptions = monthSubscriptions.Count,
                            ActiveSubscriptions = monthActive,
                            Revenue = (decimal)monthRevenue,
                            ConversionRate = conversionRate
                        });
                    }
                    else
                    {
                        // Nếu không phải năm hiện tại, hiển thị tháng đầu tiên
                        var monthSubscriptions = subscriptions.Where(s => s.CreatedAt.Month == 1).ToList();
                        var monthActive = monthSubscriptions.Count(s => s.Status == SubscriptionStatus.ACTIVE);
                        var monthRevenue = monthSubscriptions.Sum(s => s.Amount);
                        var conversionRate = monthSubscriptions.Count > 0 ? 
                            (double)monthActive / monthSubscriptions.Count * 100 : 0;
                        
                        result.Add(new MonthlyMetricDto
                        {
                            Month = 1,
                            MonthName = months[0],
                            Subscriptions = monthSubscriptions.Count,
                            ActiveSubscriptions = monthActive,
                            Revenue = (decimal)monthRevenue,
                            ConversionRate = conversionRate
                        });
                    }
                    break;
                    
                case "quarter":
                    // Chỉ hiển thị 3 tháng của quý hiện tại
                    var currentQuarter = (DateTime.Now.Month - 1) / 3 + 1;
                    var startMonth = (currentQuarter - 1) * 3 + 1;
                    var endMonth = currentQuarter * 3;
                    
                    if (year == DateTime.Now.Year)
                    {
                        for (int i = startMonth; i <= endMonth; i++)
                        {
                            var monthSubscriptions = subscriptions.Where(s => s.CreatedAt.Month == i).ToList();
                            var monthActive = monthSubscriptions.Count(s => s.Status == SubscriptionStatus.ACTIVE);
                            var monthRevenue = monthSubscriptions.Sum(s => s.Amount);
                            var conversionRate = monthSubscriptions.Count > 0 ? 
                                (double)monthActive / monthSubscriptions.Count * 100 : 0;
                            
                            result.Add(new MonthlyMetricDto
                            {
                                Month = i,
                                MonthName = months[i - 1],
                                Subscriptions = monthSubscriptions.Count,
                                ActiveSubscriptions = monthActive,
                                Revenue = (decimal)monthRevenue,
                                ConversionRate = conversionRate
                            });
                        }
                    }
                    else
                    {
                        // Nếu không phải năm hiện tại, hiển thị quý đầu tiên
                        for (int i = 1; i <= 3; i++)
                        {
                            var monthSubscriptions = subscriptions.Where(s => s.CreatedAt.Month == i).ToList();
                            var monthActive = monthSubscriptions.Count(s => s.Status == SubscriptionStatus.ACTIVE);
                            var monthRevenue = monthSubscriptions.Sum(s => s.Amount);
                            var conversionRate = monthSubscriptions.Count > 0 ? 
                                (double)monthActive / monthSubscriptions.Count * 100 : 0;
                            
                            result.Add(new MonthlyMetricDto
                            {
                                Month = i,
                                MonthName = months[i - 1],
                                Subscriptions = monthSubscriptions.Count,
                                ActiveSubscriptions = monthActive,
                                Revenue = (decimal)monthRevenue,
                                ConversionRate = conversionRate
                            });
                        }
                    }
                    break;
                    
                case "year":
                default:
                    // Hiển thị tất cả 12 tháng
                    for (int i = 0; i < 12; i++)
                    {
                        var monthSubscriptions = subscriptions.Where(s => 
                            s.CreatedAt.Month == i + 1).ToList();
                        
                        var monthActive = monthSubscriptions.Count(s => s.Status == SubscriptionStatus.ACTIVE);
                        var monthRevenue = monthSubscriptions.Sum(s => s.Amount);
                        var conversionRate = monthSubscriptions.Count > 0 ? 
                            (double)monthActive / monthSubscriptions.Count * 100 : 0;
                        
                        result.Add(new MonthlyMetricDto
                        {
                            Month = i + 1,
                            MonthName = months[i],
                            Subscriptions = monthSubscriptions.Count,
                            ActiveSubscriptions = monthActive,
                            Revenue = (decimal)monthRevenue,
                            ConversionRate = conversionRate
                        });
                    }
                    break;
            }
            
            return result;
        }

        private List<QuarterlyMetricDto> CalculateQuarterlyMetrics(IEnumerable<TechnicianSubscription> subscriptions, int year, string timeRange)
        {
            var result = new List<QuarterlyMetricDto>();
            
            switch (timeRange?.ToLower())
            {
                case "month":
                    // Nếu chọn month, không hiển thị quarterly metrics
                    return result;
                    
                case "quarter":
                case "year":
                default:
                    // Hiển thị tất cả 4 quý của năm được chọn
                    for (int quarter = 1; quarter <= 4; quarter++)
                    {
                        var startMonth = (quarter - 1) * 3 + 1;
                        var endMonth = quarter * 3;
                        
                        var quarterSubscriptions = subscriptions.Where(s => 
                            s.CreatedAt.Month >= startMonth && s.CreatedAt.Month <= endMonth).ToList();
                        
                        var quarterActive = quarterSubscriptions.Count(s => s.Status == SubscriptionStatus.ACTIVE);
                        var quarterRevenue = quarterSubscriptions.Sum(s => s.Amount);
                        var conversionRate = quarterSubscriptions.Count > 0 ? 
                            (double)quarterActive / quarterSubscriptions.Count * 100 : 0;
                        
                        result.Add(new QuarterlyMetricDto
                        {
                            Quarter = quarter,
                            Subscriptions = quarterSubscriptions.Count,
                            ActiveSubscriptions = quarterActive,
                            Revenue = (decimal)quarterRevenue,
                            ConversionRate = conversionRate
                        });
                    }
                    break;
            }
            
            return result;
        }

        private async Task<List<PackageAnalyticsDto>> CalculatePackageAnalyticsAsync(IEnumerable<TechnicianSubscription> subscriptions)
        {
            var packageGroups = subscriptions
                .GroupBy(s => s.PackageId)
                .Select(g => new { PackageId = g.Key, Group = g })
                .ToList();

            var result = new List<PackageAnalyticsDto>();
            
            foreach (var packageGroup in packageGroups)
            {
                var package = await _packageRepository.GetByIdAsync(packageGroup.PackageId);
                var packageName = package?.Name ?? "Gói không xác định";
                
                var group = packageGroup.Group;
                result.Add(new PackageAnalyticsDto
                {
                    PackageId = packageGroup.PackageId,
                    PackageName = packageName,
                    TotalSubscriptions = group.Count(),
                    ActiveSubscriptions = group.Count(s => s.Status == SubscriptionStatus.ACTIVE),
                    Revenue = (decimal)group.Sum(s => s.Amount),
                    AvgPrice = (decimal)group.Average(s => s.Amount),
                    ConversionRate = group.Count() > 0 ? 
                        (double)group.Count(s => s.Status == SubscriptionStatus.ACTIVE) / group.Count() * 100 : 0
                });
            }
            
            return result;
        }

        private List<StatusAnalyticsDto> CalculateStatusAnalytics(IEnumerable<TechnicianSubscription> subscriptions)
        {
            var statusGroups = subscriptions
                .GroupBy(s => s.Status)
                .Select(g => new StatusAnalyticsDto
                {
                    Status = g.Key.ToString(),
                    Count = g.Count(),
                    Revenue = (decimal)g.Sum(s => s.Amount),
                    AvgRevenue = g.Count() > 0 ? (decimal)(g.Sum(s => s.Amount) / g.Count()) : 0
                })
                .ToList();
            
            return statusGroups;
        }
    }
}
