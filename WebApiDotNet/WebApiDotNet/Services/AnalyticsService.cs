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
        private readonly IBookingRepository _bookingRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITechnicianRepository _technicianRepository;
        private readonly IServiceRepository _serviceRepository;

        public AnalyticsService(
            ITechnicianSubscriptionRepository subscriptionRepository, 
            IPackageRepository packageRepository, 
            IBookingRepository bookingRepository,
            IUserRepository userRepository,
            ITechnicianRepository technicianRepository,
            IServiceRepository serviceRepository)
        {
            _subscriptionRepository = subscriptionRepository;
            _packageRepository = packageRepository;
            _bookingRepository = bookingRepository;
            _userRepository = userRepository;
            _technicianRepository = technicianRepository;
            _serviceRepository = serviceRepository;
        }

        public async Task<SubscriptionAnalyticsDto> GetSubscriptionAnalyticsAsync(int year, string timeRange)
        {
            var subscriptions = await _subscriptionRepository.GetSubscriptionsByYearAsync(year);
            
            // Lọc dữ liệu theo timeRange
            var filteredSubscriptions = FilterSubscriptionsByTimeRange(subscriptions, year, timeRange);
            
            // Doanh thu từ bán Package
            var packageRevenue = (decimal)filteredSubscriptions.Sum(s => s.PaymentHistory?.Sum(ph => ph.Amount) ?? 0);

            // Lấy bookings để cộng thêm 8% từ TechnicianEarning của đơn đã PAID
            var allBookings = await _bookingRepository.GetAllAsync();
            var filteredBookings = FilterBookingsByTimeRange(allBookings, year, timeRange)
                .Where(b => b.PaymentStatus == PaymentStatus.PAID);
            var extraFromTechnicianEarning = (decimal)filteredBookings.Sum(b => (b.TechnicianEarning ?? 0) * 0.08);

            // Tính chi tiết ExtraFromTechnicianEarning theo thời gian
            var technicianEarningDetails = await CalculateTechnicianEarningDetailsByTimeAsync(allBookings, year, timeRange);

            var result = new SubscriptionAnalyticsDto
            {
                TotalSubscriptions = filteredSubscriptions.Count(),
                ActiveSubscriptions = filteredSubscriptions.Count(s => s.Status == SubscriptionStatus.ACTIVE),
                ExpiredSubscriptions = filteredSubscriptions.Count(s => s.Status == SubscriptionStatus.EXPIRED),
                PendingSubscriptions = filteredSubscriptions.Count(s => s.Status == SubscriptionStatus.PENDING_ACTIVATION),
                CancelledSubscriptions = filteredSubscriptions.Count(s => s.Status == SubscriptionStatus.CANCELLED),
                
                TotalRevenue = packageRevenue + extraFromTechnicianEarning,
                AvgRevenuePerSubscription = filteredSubscriptions.Any() ? 
                    (decimal)filteredSubscriptions.Average(s => s.PaymentHistory?.Sum(ph => ph.Amount) ?? 0) : 0m,
                RevenueGrowth = CalculateRevenueGrowth(filteredSubscriptions, year),
                ConversionRate = CalculateConversionRate(filteredSubscriptions),
                ChurnRate = CalculateChurnRate(filteredSubscriptions),
                RetentionRate = CalculateRetentionRate(filteredSubscriptions),
                
                // Tỷ lệ rời bỏ chi tiết
                CustomerCancellationRate = CalculateCustomerCancellationRate(filteredSubscriptions),
                TotalChurnRate = CalculateTotalChurnRate(filteredSubscriptions),
                ExpiredChurnRate = CalculateExpiredChurnRate(filteredSubscriptions),
                SuspendedChurnRate = CalculateSuspendedChurnRate(filteredSubscriptions),
                
                MonthlyMetrics = CalculateMonthlyMetrics(filteredSubscriptions, year, timeRange, allBookings),
                QuarterlyMetrics = CalculateQuarterlyMetrics(filteredSubscriptions, year, timeRange, allBookings),
                PackageAnalytics = await CalculatePackageAnalyticsAsync(filteredSubscriptions),
                StatusAnalytics = CalculateStatusAnalytics(filteredSubscriptions),
                
                // Chi tiết ExtraFromTechnicianEarning
                TechnicianEarningDetails = technicianEarningDetails,
                TotalExtraFromTechnicianEarning = extraFromTechnicianEarning
            };

            return result;
        }



        private IEnumerable<Booking> FilterBookingsByTimeRange(
            IEnumerable<Booking> bookings, int year, string timeRange)
        {
            var currentDate = DateTime.Now;
            var currentYear = currentDate.Year;
            var currentMonth = currentDate.Month;

            switch (timeRange?.ToLower())
            {
                case "month":
                    if (year == currentYear)
                    {
                        return bookings.Where(b => b.CreatedAt.Year == currentYear && b.CreatedAt.Month == currentMonth);
                    }
                    else
                    {
                        return bookings.Where(b => b.CreatedAt.Year == year && b.CreatedAt.Month == 1);
                    }

                case "quarter":
                    var currentQuarter = (currentMonth - 1) / 3 + 1;
                    var startMonth = (currentQuarter - 1) * 3 + 1;
                    var endMonth = currentQuarter * 3;
                    if (year == currentYear)
                    {
                        return bookings.Where(b => b.CreatedAt.Year == currentYear && b.CreatedAt.Month >= startMonth && b.CreatedAt.Month <= endMonth);
                    }
                    else
                    {
                        return bookings.Where(b => b.CreatedAt.Year == year && b.CreatedAt.Month >= 1 && b.CreatedAt.Month <= 3);
                    }

                case "year":
                default:
                    return bookings.Where(b => b.CreatedAt.Year == year);
            }
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

        /// <summary>
        /// Tính tỷ lệ rời bỏ khách hàng tự hủy (CANCELLED)
        /// </summary>
        /// <param name="subscriptions">Danh sách gói đăng ký</param>
        /// <returns>Tỷ lệ rời bỏ khách hàng tự hủy (%)</returns>
        private double CalculateCustomerCancellationRate(IEnumerable<TechnicianSubscription> subscriptions)
        {
            var total = subscriptions.Count();
            var customerCancelled = subscriptions.Count(s => s.Status == SubscriptionStatus.CANCELLED);
            return total > 0 ? (double)customerCancelled / total * 100 : 0;
        }

        /// <summary>
        /// Tính tỷ lệ rời bỏ tổng hợp (bao gồm tất cả trạng thái không hoạt động)
        /// </summary>
        /// <param name="subscriptions">Danh sách gói đăng ký</param>
        /// <returns>Tỷ lệ rời bỏ tổng hợp (%)</returns>
        private double CalculateTotalChurnRate(IEnumerable<TechnicianSubscription> subscriptions)
        {
            var total = subscriptions.Count();
            var totalChurned = subscriptions.Count(s => 
                s.Status == SubscriptionStatus.CANCELLED || 
                s.Status == SubscriptionStatus.EXPIRED || 
                s.Status == SubscriptionStatus.SUSPENDED);
            return total > 0 ? (double)totalChurned / total * 100 : 0;
        }

        /// <summary>
        /// Tính tỷ lệ rời bỏ do hết hạn (EXPIRED)
        /// </summary>
        /// <param name="subscriptions">Danh sách gói đăng ký</param>
        /// <returns>Tỷ lệ rời bỏ do hết hạn (%)</returns>
        private double CalculateExpiredChurnRate(IEnumerable<TechnicianSubscription> subscriptions)
        {
            var total = subscriptions.Count();
            var expired = subscriptions.Count(s => s.Status == SubscriptionStatus.EXPIRED);
            return total > 0 ? (double)expired / total * 100 : 0;
        }

        /// <summary>
        /// Tính tỷ lệ rời bỏ do bị đình chỉ (SUSPENDED)
        /// </summary>
        /// <param name="subscriptions">Danh sách gói đăng ký</param>
        /// <returns>Tỷ lệ rời bỏ do bị đình chỉ (%)</returns>
        private double CalculateSuspendedChurnRate(IEnumerable<TechnicianSubscription> subscriptions)
        {
            var total = subscriptions.Count();
            var suspended = subscriptions.Count(s => s.Status == SubscriptionStatus.SUSPENDED);
            return total > 0 ? (double)suspended / total * 100 : 0;
        }

        private double CalculateRetentionRate(IEnumerable<TechnicianSubscription> subscriptions)
        {
            var total = subscriptions.Count();
            var retained = subscriptions.Count(s => s.Status == SubscriptionStatus.ACTIVE || s.Status == SubscriptionStatus.PENDING_ACTIVATION);
            return total > 0 ? (double)retained / total * 100 : 0;
        }

        private decimal CalculateRevenueGrowth(IEnumerable<TechnicianSubscription> subscriptions, int year)
        {
            var currentDate = DateTime.Now;
            var currentMonth = currentDate.Month;
            var currentYear = currentDate.Year;
            
            // Tính doanh thu tháng hiện tại
            var currentMonthRevenue = subscriptions
                .Where(s => s.PaymentHistory != null && s.PaymentHistory.Any(ph => 
                    ph.CreatedAt.Month == currentMonth && ph.CreatedAt.Year == currentYear))
                .Sum(s => s.PaymentHistory?.Where(ph => 
                    ph.CreatedAt.Month == currentMonth && ph.CreatedAt.Year == currentYear)
                    .Sum(ph => ph.Amount) ?? 0);
            
            // Tính doanh thu cùng tháng năm ngoái
            var lastYear = currentYear - 1;
            var lastYearSameMonthRevenue = subscriptions
                .Where(s => s.PaymentHistory != null && s.PaymentHistory.Any(ph => 
                    ph.CreatedAt.Month == currentMonth && ph.CreatedAt.Year == lastYear))
                .Sum(s => s.PaymentHistory?.Where(ph => 
                    ph.CreatedAt.Month == currentMonth && ph.CreatedAt.Year == lastYear)
                    .Sum(ph => ph.Amount) ?? 0);
            
            // Tính tỷ lệ tăng trưởng so với cùng tháng năm ngoái
            if (lastYearSameMonthRevenue == 0)
            {
                return currentMonthRevenue > 0 ? 100m : 0m;
            }
            
            return Math.Round(((decimal)(currentMonthRevenue - lastYearSameMonthRevenue) / (decimal)lastYearSameMonthRevenue) * 100m, 2);
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

        private List<MonthlyMetricDto> CalculateMonthlyMetrics(IEnumerable<TechnicianSubscription> subscriptions, int year, string timeRange, IEnumerable<Booking> allBookings)
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
                        var monthRevenue = monthSubscriptions.Sum(s => s.PaymentHistory?.Sum(ph => ph.Amount) ?? 0);
                        var monthBookings = allBookings.Where(b => b.CreatedAt.Year == DateTime.Now.Year && b.CreatedAt.Month == currentMonth && b.PaymentStatus == PaymentStatus.PAID);
                        var monthExtra = monthBookings.Sum(b => (b.TechnicianEarning ?? 0) * 0.08);
                        var conversionRate = monthSubscriptions.Count > 0 ? 
                            (double)monthActive / monthSubscriptions.Count * 100 : 0;
                        
                        result.Add(new MonthlyMetricDto
                        {
                            Month = currentMonth,
                            MonthName = months[currentMonth - 1],
                            Subscriptions = monthSubscriptions.Count,
                            ActiveSubscriptions = monthActive,
                            Revenue = (decimal)(monthRevenue + monthExtra),
                            ConversionRate = conversionRate
                        });
                    }
                    else
                    {
                        // Nếu không phải năm hiện tại, hiển thị tháng đầu tiên
                        var monthSubscriptions = subscriptions.Where(s => s.CreatedAt.Month == 1).ToList();
                        var monthActive = monthSubscriptions.Count(s => s.Status == SubscriptionStatus.ACTIVE);
                        var monthRevenue = monthSubscriptions.Sum(s => s.PaymentHistory?.Sum(ph => ph.Amount) ?? 0);
                        var monthBookings = allBookings.Where(b => b.CreatedAt.Year == year && b.CreatedAt.Month == 1 && b.PaymentStatus == PaymentStatus.PAID);
                        var monthExtra = monthBookings.Sum(b => (b.TechnicianEarning ?? 0) * 0.08);
                        var conversionRate = monthSubscriptions.Count > 0 ? 
                            (double)monthActive / monthSubscriptions.Count * 100 : 0;
                        
                        result.Add(new MonthlyMetricDto
                        {
                            Month = 1,
                            MonthName = months[0],
                            Subscriptions = monthSubscriptions.Count,
                            ActiveSubscriptions = monthActive,
                            Revenue = (decimal)(monthRevenue + monthExtra),
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
                            var monthRevenue = monthSubscriptions.Sum(s => s.PaymentHistory?.Sum(ph => ph.Amount) ?? 0);
                            var monthBookings = allBookings.Where(b => b.CreatedAt.Year == DateTime.Now.Year && b.CreatedAt.Month == i && b.PaymentStatus == PaymentStatus.PAID);
                            var monthExtra = monthBookings.Sum(b => (b.TechnicianEarning ?? 0) * 0.08);
                            var conversionRate = monthSubscriptions.Count > 0 ? 
                                (double)monthActive / monthSubscriptions.Count * 100 : 0;
                            
                            result.Add(new MonthlyMetricDto
                            {
                                Month = i,
                                MonthName = months[i - 1],
                                Subscriptions = monthSubscriptions.Count,
                                ActiveSubscriptions = monthActive,
                                Revenue = (decimal)(monthRevenue + monthExtra),
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
                            var monthRevenue = monthSubscriptions.Sum(s => s.PaymentHistory?.Sum(ph => ph.Amount) ?? 0);
                            var monthBookings = allBookings.Where(b => b.CreatedAt.Year == year && b.CreatedAt.Month == i && b.PaymentStatus == PaymentStatus.PAID);
                            var monthExtra = monthBookings.Sum(b => (b.TechnicianEarning ?? 0) * 0.08);
                            var conversionRate = monthSubscriptions.Count > 0 ? 
                                (double)monthActive / monthSubscriptions.Count * 100 : 0;
                            
                            result.Add(new MonthlyMetricDto
                            {
                                Month = i,
                                MonthName = months[i - 1],
                                Subscriptions = monthSubscriptions.Count,
                                ActiveSubscriptions = monthActive,
                                Revenue = (decimal)(monthRevenue + monthExtra),
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
                        var monthRevenue = monthSubscriptions.Sum(s => s.PaymentHistory?.Sum(ph => ph.Amount) ?? 0);
                        var monthBookings = allBookings.Where(b => b.CreatedAt.Year == year && b.CreatedAt.Month == i + 1 && b.PaymentStatus == PaymentStatus.PAID);
                        var monthExtra = monthBookings.Sum(b => (b.TechnicianEarning ?? 0) * 0.08);
                        var conversionRate = monthSubscriptions.Count > 0 ? 
                            (double)monthActive / monthSubscriptions.Count * 100 : 0;
                        
                        result.Add(new MonthlyMetricDto
                        {
                            Month = i + 1,
                            MonthName = months[i],
                            Subscriptions = monthSubscriptions.Count,
                            ActiveSubscriptions = monthActive,
                            Revenue = (decimal)(monthRevenue + monthExtra),
                            ConversionRate = conversionRate
                        });
                    }
                    break;
            }
            
            return result;
        }

        private List<QuarterlyMetricDto> CalculateQuarterlyMetrics(IEnumerable<TechnicianSubscription> subscriptions, int year, string timeRange, IEnumerable<Booking> allBookings)
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
                        var quarterRevenue = quarterSubscriptions.Sum(s => s.PaymentHistory?.Sum(ph => ph.Amount) ?? 0);
                        var quarterBookings = allBookings.Where(b => b.CreatedAt.Year == year && b.CreatedAt.Month >= startMonth && b.CreatedAt.Month <= endMonth && b.PaymentStatus == PaymentStatus.PAID);
                        var quarterExtra = quarterBookings.Sum(b => (b.TechnicianEarning ?? 0) * 0.08);
                        var conversionRate = quarterSubscriptions.Count > 0 ? 
                            (double)quarterActive / quarterSubscriptions.Count * 100 : 0;
                        
                        result.Add(new QuarterlyMetricDto
                        {
                            Quarter = quarter,
                            Subscriptions = quarterSubscriptions.Count,
                            ActiveSubscriptions = quarterActive,
                            Revenue = (decimal)(quarterRevenue + quarterExtra),
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
                    Revenue = (decimal)group.Sum(s => s.PaymentHistory?.Sum(ph => ph.Amount) ?? 0),
                    AvgPrice = (decimal)group.Average(s => s.PaymentHistory?.Sum(ph => ph.Amount) ?? 0),
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
                    Revenue = (decimal)g.Sum(s => s.PaymentHistory?.Sum(ph => ph.Amount) ?? 0),
                    AvgRevenue = g.Count() > 0 ? (decimal)(g.Sum(s => s.PaymentHistory?.Sum(ph => ph.Amount) ?? 0) / g.Count()) : 0
                })
                .ToList();
            
            return statusGroups;
        }

        private async Task<List<TechnicianEarningDetailDto>> CalculateTechnicianEarningByMonthAsync(List<Booking> bookings, int year)
        {
            var months = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", 
                                "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            
            var result = new List<TechnicianEarningDetailDto>();
            
            for (int month = 1; month <= 12; month++)
            {
                var monthBookings = bookings.Where(b => b.CreatedAt.Month == month).ToList();
                var totalTechnicianEarning = monthBookings.Sum(b => b.TechnicianEarning ?? 0);
                var extraEarning = (decimal)(totalTechnicianEarning * 0.08);
                
                var bookingDetails = new List<BookingEarningDetailDto>();
                
                foreach (var booking in monthBookings)
                {
                    var technicianName = "N/A";
                    var serviceName = "N/A";
                    
                    // Lấy tên technician
                    if (!string.IsNullOrEmpty(booking.TechnicianId))
                    {
                        var technician = await _technicianRepository.GetByIdAsync(booking.TechnicianId);
                        if (technician != null)
                        {
                            var user = await _userRepository.GetByIdAsync(technician.UserId);
                            technicianName = user?.FullName ?? "N/A";
                        }
                    }
                    
                    // Lấy tên service
                    if (!string.IsNullOrEmpty(booking.ServiceId))
                    {
                        var service = await _serviceRepository.GetByIdAsync(booking.ServiceId);
                        serviceName = service?.ServiceName ?? "N/A";
                    }
                    
                    bookingDetails.Add(new BookingEarningDetailDto
                    {
                        BookingId = booking.Id,
                        BookingDate = booking.CreatedAt,
                        TechnicianId = booking.TechnicianId ?? "",
                        TechnicianName = technicianName,
                        ServiceName = serviceName,
                        TechnicianEarning = (decimal)(booking.TechnicianEarning ?? 0),
                        ExtraFromBooking = (decimal)((booking.TechnicianEarning ?? 0) * 0.08),
                        PaymentStatus = booking.PaymentStatus.ToString()
                    });
                }
                
                result.Add(new TechnicianEarningDetailDto
                {
                    Date = new DateTime(year, month, 1),
                    TimeLabel = months[month - 1],
                    BookingCount = monthBookings.Count,
                    TotalTechnicianEarning = (decimal)totalTechnicianEarning,
                    ExtraFromTechnicianEarning = extraEarning,
                    AveragePerBooking = monthBookings.Count > 0 ? extraEarning / monthBookings.Count : 0m,
                    BookingDetails = bookingDetails
                });
            }
            
            return result;
        }

        private async Task<List<TechnicianEarningDetailDto>> CalculateTechnicianEarningByQuarterAsync(List<Booking> bookings, int year)
        {
            var result = new List<TechnicianEarningDetailDto>();
            
            for (int quarter = 1; quarter <= 4; quarter++)
            {
                var startMonth = (quarter - 1) * 3 + 1;
                var endMonth = quarter * 3;
                
                var quarterBookings = bookings.Where(b => 
                    b.CreatedAt.Month >= startMonth && b.CreatedAt.Month <= endMonth).ToList();
                
                var totalTechnicianEarning = quarterBookings.Sum(b => b.TechnicianEarning ?? 0);
                var extraEarning = (decimal)(totalTechnicianEarning * 0.08);
                
                var bookingDetails = new List<BookingEarningDetailDto>();
                
                foreach (var booking in quarterBookings)
                {
                    var technicianName = "N/A";
                    var serviceName = "N/A";
                    
                    // Lấy tên technician
                    if (!string.IsNullOrEmpty(booking.TechnicianId))
                    {
                        var technician = await _technicianRepository.GetByIdAsync(booking.TechnicianId);
                        if (technician != null)
                        {
                            var user = await _userRepository.GetByIdAsync(technician.UserId);
                            technicianName = user?.FullName ?? "N/A";
                        }
                    }
                    
                    // Lấy tên service
                    if (!string.IsNullOrEmpty(booking.ServiceId))
                    {
                        var service = await _serviceRepository.GetByIdAsync(booking.ServiceId);
                        serviceName = service?.ServiceName ?? "N/A";
                    }
                    
                    bookingDetails.Add(new BookingEarningDetailDto
                    {
                        BookingId = booking.Id,
                        BookingDate = booking.CreatedAt,
                        TechnicianId = booking.TechnicianId ?? "",
                        TechnicianName = technicianName,
                        ServiceName = serviceName,
                        TechnicianEarning = (decimal)(booking.TechnicianEarning ?? 0),
                        ExtraFromBooking = (decimal)((booking.TechnicianEarning ?? 0) * 0.08),
                        PaymentStatus = booking.PaymentStatus.ToString()
                    });
                }
                
                result.Add(new TechnicianEarningDetailDto
                {
                    Date = new DateTime(year, startMonth, 1),
                    TimeLabel = $"Q{quarter}",
                    BookingCount = quarterBookings.Count,
                    TotalTechnicianEarning = (decimal)totalTechnicianEarning,
                    ExtraFromTechnicianEarning = extraEarning,
                    AveragePerBooking = quarterBookings.Count > 0 ? extraEarning / quarterBookings.Count : 0m,
                    BookingDetails = bookingDetails
                });
            }
            
            return result;
        }

        private async Task<List<TechnicianEarningDetailDto>> CalculateTechnicianEarningDetailsByTimeAsync(IEnumerable<Booking> allBookings, int year, string timeRange)
        {
            var yearBookings = allBookings.Where(b => b.CreatedAt.Year == year && b.PaymentStatus == PaymentStatus.PAID).ToList();
            
            switch (timeRange?.ToLower())
            {
                case "month":
                    return await CalculateTechnicianEarningByMonthAsync(yearBookings, year);
                    
                case "quarter":
                    return await CalculateTechnicianEarningByQuarterAsync(yearBookings, year);
                    
                case "year":
                default:
                    return await CalculateTechnicianEarningByMonthAsync(yearBookings, year);
            }
        }
    }
}
