using WebApiDotNet.DTOs;

namespace WebApiDotNet.Services
{
    public interface IFinancialReportService
    {
        Task<FinancialSummaryDto> GetFinancialSummaryAsync();
        Task<List<BookingFinancialDto>> GetAllBookingsFinancialAsync();
        Task<List<TechnicianFinancialSummaryDto>> GetAllTechniciansFinancialSummaryAsync();
        Task<TechnicianFinancialDto> GetTechnicianFinancialDetailsAsync(string technicianId);
        Task<List<BookingFinancialDto>> GetBookingsByTechnicianIdAsync(string technicianId);
        Task<double> GetTotalRevenueAsync();
        Task<double> GetTotalHoldingAmountAsync();
        Task<double> GetTotalCommissionAmountAsync();
        Task<double> GetTotalTechnicianEarningAsync();
        Task<double> GetTotalWithdrawnAsync();
    }
} 