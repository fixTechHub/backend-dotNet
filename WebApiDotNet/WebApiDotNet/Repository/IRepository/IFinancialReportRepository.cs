using WebApiDotNet.DTOs;
using WebApiDotNet.Models;

namespace WebApiDotNet.Repository.IRepository
{
    public interface IFinancialReportRepository
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