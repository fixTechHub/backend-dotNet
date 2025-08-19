using System.Threading.Tasks;
using WebApiDotNet.DTOs;

namespace WebApiDotNet.Services
{
    public interface IAnalyticsService
    {
        Task<SubscriptionAnalyticsDto> GetSubscriptionAnalyticsAsync(int year, string timeRange);
    }
}
