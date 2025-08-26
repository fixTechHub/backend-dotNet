using AutoMapper;
using WebApiDotNet.DTOs;
using WebApiDotNet.Models;
using WebApiDotNet.Repository.IRepository;

namespace WebApiDotNet.Services
{
    public class TechnicianSubscriptionService : ITechnicianSubscriptionService
    {
        private readonly ITechnicianSubscriptionRepository _repository;
        private readonly IMapper _mapper;

        public TechnicianSubscriptionService(ITechnicianSubscriptionRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<TechnicianSubscriptionDto>> GetAllAsync()
        {
            var subscriptions = await _repository.GetAllAsync();
            return _mapper.Map<List<TechnicianSubscriptionDto>>(subscriptions);
        }

        public async Task<TechnicianSubscriptionDto?> GetByIdAsync(string id)
        {
            var subscription = await _repository.GetByIdAsync(id);
            return subscription == null ? null : _mapper.Map<TechnicianSubscriptionDto>(subscription);
        }

        public async Task<TechnicianSubscriptionDto?> GetByTechnicianIdAsync(string technicianId)
        {
            var subscription = await _repository.GetByTechnicianIdAsync(technicianId);
            return subscription == null ? null : _mapper.Map<TechnicianSubscriptionDto>(subscription);
        }

        public async Task<TechnicianSubscriptionDto> CreateAsync(CreateTechnicianSubscriptionDto createDto)
        {
            var subscription = _mapper.Map<TechnicianSubscription>(createDto);
            var createdSubscription = await _repository.CreateAsync(subscription);
            return _mapper.Map<TechnicianSubscriptionDto>(createdSubscription);
        }

        public async Task<TechnicianSubscriptionDto?> UpdateAsync(string id, UpdateTechnicianSubscriptionDto updateDto)
        {
            var existingSubscription = await _repository.GetByIdAsync(id);
            if (existingSubscription == null)
                return null;

            // Update only non-null properties
            if (updateDto.Status != null)
                existingSubscription.Status = (SubscriptionStatus)Enum.Parse(typeof(SubscriptionStatus), updateDto.Status);
            if (updateDto.EndDate.HasValue)
                existingSubscription.EndDate = updateDto.EndDate.Value;
            if (updateDto.PaymentStatus != null)
                existingSubscription.PaymentStatus = (SubscriptionPaymentStatus)Enum.Parse(typeof(SubscriptionPaymentStatus), updateDto.PaymentStatus);
            if (updateDto.PaymentMethod != null)
                existingSubscription.PaymentMethod = updateDto.PaymentMethod;
            if (updateDto.TransactionId != null)
                existingSubscription.TransactionId = updateDto.TransactionId;
            if (updateDto.AutoRenew.HasValue)
                existingSubscription.AutoRenew = updateDto.AutoRenew.Value;
            if (updateDto.CancelledAt.HasValue)
                existingSubscription.CancelledAt = updateDto.CancelledAt.Value;
            if (updateDto.CancellationReason != null)
                existingSubscription.CancellationReason = updateDto.CancellationReason;

            var updatedSubscription = await _repository.UpdateAsync(id, existingSubscription);
            return updatedSubscription == null ? null : _mapper.Map<TechnicianSubscriptionDto>(updatedSubscription);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<List<TechnicianSubscriptionDto>> GetActiveSubscriptionsAsync()
        {
            var subscriptions = await _repository.GetActiveSubscriptionsAsync();
            return _mapper.Map<List<TechnicianSubscriptionDto>>(subscriptions);
        }

        public async Task<List<TechnicianSubscriptionDto>> GetExpiredSubscriptionsAsync()
        {
            var subscriptions = await _repository.GetExpiredSubscriptionsAsync();
            return _mapper.Map<List<TechnicianSubscriptionDto>>(subscriptions);
        }

        public async Task<List<TechnicianSubscriptionDto>> GetSubscriptionsByPackageAsync(string packageId)
        {
            var subscriptions = await _repository.GetSubscriptionsByPackageAsync(packageId);
            return _mapper.Map<List<TechnicianSubscriptionDto>>(subscriptions);
        }

        public async Task<int> CountActiveSubscriptionsAsync()
        {
            return await _repository.CountActiveSubscriptionsAsync();
        }

        public async Task<double> GetTotalRevenueAsync()
        {
            return await _repository.GetTotalRevenueAsync();
        }

        public async Task<double> GetMonthlyRevenueAsync(int year, int month)
        {
            return await _repository.GetMonthlyRevenueAsync(year, month);
        }

        public async Task<List<TechnicianSubscriptionDto>> GetSubscriptionsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var subscriptions = await _repository.GetSubscriptionsByDateRangeAsync(startDate, endDate);
            return _mapper.Map<List<TechnicianSubscriptionDto>>(subscriptions);
        }

        public async Task<bool> UpdatePaymentStatusAsync(string id, string paymentStatus, string? transactionId = null)
        {
            return await _repository.UpdatePaymentStatusAsync(id, paymentStatus, transactionId);
        }

        public async Task<bool> CancelSubscriptionAsync(string id, string reason)
        {
            return await _repository.CancelSubscriptionAsync(id, reason);
        }

        public async Task<bool> RenewSubscriptionAsync(string id, DateTime newEndDate)
        {
            return await _repository.RenewSubscriptionAsync(id, newEndDate);
        }

        public async Task<List<TechnicianSubscriptionSummaryDto>> GetSubscriptionsSummaryAsync()
        {
            var subscriptions = await _repository.GetAllAsync();
            var summaryList = new List<TechnicianSubscriptionSummaryDto>();

            foreach (var subscription in subscriptions)
            {
                // Get technician name from User collection
                var technician = await _repository.GetTechnicianByIdAsync(subscription.TechnicianId);
                var technicianName = "Unknown Technician";
                if (technician != null)
                {
                    var user = await _repository.GetUserByIdAsync(technician.UserId);
                    technicianName = user?.FullName ?? "Unknown Technician";
                }

                // Get package name
                var package = await _repository.GetPackageByIdAsync(subscription.PackageId);
                var packageName = package?.Name ?? "Unknown Package";

                summaryList.Add(new TechnicianSubscriptionSummaryDto
                {
                    Id = subscription.Id,
                    TechnicianName = technicianName,
                    PackageName = packageName,
                    Status = subscription.Status.ToString(),
                    StartDate = subscription.StartDate,
                    EndDate = subscription.EndDate,
                    Amount = subscription.PaymentHistory?.Sum(ph => ph.Amount) ?? 0,
                    PaymentStatus = subscription.PaymentStatus.ToString(),
                    AutoRenew = subscription.AutoRenew
                });
            }

            return summaryList;
        }

        public async Task<object> GetRevenueStatisticsAsync(int year)
        {
            var monthlyRevenue = new List<object>();
            
            for (int month = 1; month <= 12; month++)
            {
                var revenue = await _repository.GetMonthlyRevenueAsync(year, month);
                monthlyRevenue.Add(new { month, revenue });
            }

            var totalRevenue = await _repository.GetTotalRevenueAsync();
            var activeSubscriptions = await _repository.CountActiveSubscriptionsAsync();

            return new
            {
                year,
                totalRevenue,
                activeSubscriptions,
                monthlyRevenue
            };
        }
    }
}