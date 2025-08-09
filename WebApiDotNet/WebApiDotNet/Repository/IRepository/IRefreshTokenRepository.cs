using WebApiDotNet.Models;

namespace WebApiDotNet.Repository.IRepository
{
    public interface IRefreshTokenRepository
    {
        Task CreateAsync(RefreshToken token);
        Task<RefreshToken?> GetByTokenAsync(string token);
        Task<List<RefreshToken>> GetByUserIdAsync(string userId);
        Task UpdateAsync(RefreshToken token);
    }
}
