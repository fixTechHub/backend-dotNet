using WebApiDotNet.DTOs;
using WebApiDotNet.Repository.IRepository;

namespace WebApiDotNet.Services
{
    public class FinancialReportService : IFinancialReportService
    {
        private readonly IFinancialReportRepository _financialReportRepository;

        public FinancialReportService(IFinancialReportRepository financialReportRepository)
        {
            _financialReportRepository = financialReportRepository;
        }

        public async Task<FinancialSummaryDto> GetFinancialSummaryAsync()
        {
            return await _financialReportRepository.GetFinancialSummaryAsync();
        }

        public async Task<List<BookingFinancialDto>> GetAllBookingsFinancialAsync()
        {
            return await _financialReportRepository.GetAllBookingsFinancialAsync();
        }

        public async Task<List<TechnicianFinancialSummaryDto>> GetAllTechniciansFinancialSummaryAsync()
        {
            return await _financialReportRepository.GetAllTechniciansFinancialSummaryAsync();
        }

        public async Task<TechnicianFinancialDto> GetTechnicianFinancialDetailsAsync(string technicianId)
        {
            if (string.IsNullOrEmpty(technicianId))
                throw new ArgumentException("TechnicianId cannot be null or empty");

            return await _financialReportRepository.GetTechnicianFinancialDetailsAsync(technicianId);
        }

        public async Task<List<BookingFinancialDto>> GetBookingsByTechnicianIdAsync(string technicianId)
        {
            if (string.IsNullOrEmpty(technicianId))
                throw new ArgumentException("TechnicianId cannot be null or empty");

            return await _financialReportRepository.GetBookingsByTechnicianIdAsync(technicianId);
        }

        public async Task<double> GetTotalRevenueAsync()
        {
            return await _financialReportRepository.GetTotalRevenueAsync();
        }

        public async Task<double> GetTotalHoldingAmountAsync()
        {
            return await _financialReportRepository.GetTotalHoldingAmountAsync();
        }

        public async Task<double> GetTotalCommissionAmountAsync()
        {
            return await _financialReportRepository.GetTotalCommissionAmountAsync();
        }

        public async Task<double> GetTotalTechnicianEarningAsync()
        {
            return await _financialReportRepository.GetTotalTechnicianEarningAsync();
        }

        public async Task<double> GetTotalWithdrawnAsync()
        {
            return await _financialReportRepository.GetTotalWithdrawnAsync();
        }
    }
} 