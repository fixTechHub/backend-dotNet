using WebApiDotNet.Models;
using WebApiDotNet.Repository.IRepository;
using AutoMapper;
using WebApiDotNet.DTOs;
using WebApiDotNet.Services;

namespace WebApiDotNet.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public ReportService(IReportRepository repository, IMapper mapper, IUserService userService)
        {
            _repository = repository;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<List<ReportDto>> GetAllAsync()
        {
            var reports = await _repository.GetAllAsync();
            return _mapper.Map<List<ReportDto>>(reports);
        }

        public async Task<ReportDto> GetByIdAsync(string id)
        {
            var r = await _repository.GetByIdAsync(id);
            return r == null ? null : _mapper.Map<ReportDto>(r);
        }

        public async Task<List<ReportDto>> GetByTypeAsync(ReportType type)
        {
            var reports = await _repository.GetAllAsync();
            var filteredReports = reports.Where(r => r.Type == type).ToList();
            return _mapper.Map<List<ReportDto>>(filteredReports);
        }

        public async Task<List<ReportDto>> GetByStatusAsync(ReportStatus status)
        {
            var reports = await _repository.GetAllAsync();
            var filteredReports = reports.Where(r => r.Status == status).ToList();
            return _mapper.Map<List<ReportDto>>(filteredReports);
        }

        public async Task<Dictionary<string, int>> GetUserReportCountsAsync()
        {
            var reports = await _repository.GetAllAsync();
            
            // Nhóm theo reportedUserId và đếm số lần
            var reportCounts = reports
                .Where(r => !string.IsNullOrEmpty(r.ReportedUserId))
                .GroupBy(r => r.ReportedUserId)
                .ToDictionary(g => g.Key, g => g.Count());
            
            return reportCounts;
        }

        public async Task<int> GetUserReportCountAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return 0;
                
            var reports = await _repository.GetAllAsync();
            
            // Đếm số lần user này bị report
            var count = reports.Count(r => r.ReportedUserId == userId);
            
            return count;
        }

        public async Task<Dictionary<string, int>> GetUserReportCountsByTypeAsync(ReportType type)
        {
            var reports = await _repository.GetAllAsync();
            
            // Lọc theo type và nhóm theo reportedUserId
            var reportCounts = reports
                .Where(r => r.Type == type && !string.IsNullOrEmpty(r.ReportedUserId))
                .GroupBy(r => r.ReportedUserId)
                .ToDictionary(g => g.Key, g => g.Count());
            
            return reportCounts;
        }

        public async Task<ReportDto?> UpdateStatusAsync(string reportId, ReportStatus newStatus, string resolvedBy)
        {
            var report = await _repository.GetByIdAsync(reportId);
            if (report == null)
                return null;

            // Update status
            report.Status = newStatus;
            report.UpdatedAt = DateTime.UtcNow;

            // Nếu status là RESOLVED, kiểm tra và tự động khóa user nếu cần
            if (newStatus == ReportStatus.RESOLVED)
            {
                await CheckAndAutoLockUserAsync(report.ReportedUserId);
            }

            var updatedReport = await _repository.UpdateAsync(report);
            return updatedReport != null ? _mapper.Map<ReportDto>(updatedReport) : null;
        }

        public async Task<bool> CheckAndAutoLockUserAsync(string reportedUserId)
        {
            if (string.IsNullOrEmpty(reportedUserId))
                return false;

            var reports = await _repository.GetAllAsync();
            
            // Lấy tất cả reports của user này (đã RESOLVED)
            var userReports = reports
                .Where(r => r.ReportedUserId == reportedUserId && r.Status == ReportStatus.RESOLVED)
                .ToList();

            if (userReports.Count < 3)
                return false;

            // Kiểm tra điều kiện 1: 3 báo cáo từ 3 user khác nhau
            var uniqueReporters = userReports
                .Select(r => r.ReporterId)
                .Distinct()
                .Count();

            // Kiểm tra điều kiện 2: 3 báo cáo từ 3 bookingID khác nhau (nếu có)
            var uniqueBookings = userReports
                .Where(r => !string.IsNullOrEmpty(r.BookingId))
                .Select(r => r.BookingId)
                .Distinct()
                .Count();

            // Nếu đủ điều kiện, khóa user
            if (uniqueReporters >= 3 || uniqueBookings >= 3)
            {
                try
                {
                    // Khóa user với lý do tự động
                    var lockReason = $"Tự động khóa do vi phạm nhiều lần: {uniqueReporters} báo cáo từ {uniqueReporters} user khác nhau, {uniqueBookings} booking khác nhau";
                    var lockUserDto = new LockUserDto { LockedReason = lockReason };
                    await _userService.LockUserAsync(reportedUserId, lockUserDto);
                    return true;
                }
                catch (Exception ex)
                {
                    // Log lỗi nhưng không throw exception
                    Console.WriteLine($"Lỗi khi khóa user {reportedUserId}: {ex.Message}");
                    return false;
                }
            }

            return false;
        }
    }
}