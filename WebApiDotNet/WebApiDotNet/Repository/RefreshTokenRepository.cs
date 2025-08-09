using MongoDB.Driver;
using WebApiDotNet.Data;
using WebApiDotNet.Models;
using WebApiDotNet.Repository.IRepository;

namespace WebApiDotNet.Repository
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly IMongoCollection<RefreshToken> _refreshTokens;

        public RefreshTokenRepository(MongoDbContext context)
        {
            _refreshTokens = context.RefreshTokens;
        }

        public async Task CreateAsync(RefreshToken token)
        {
            await _refreshTokens.InsertOneAsync(token);
        }

        public async Task<RefreshToken?> GetByTokenAsync(string token)
        {
            return await _refreshTokens.Find(t => t.Token == token).FirstOrDefaultAsync();
        }

        public async Task<List<RefreshToken>> GetByUserIdAsync(string userId)
        {
            return await _refreshTokens.Find(t => t.UserId == userId).ToListAsync();
        }

        public async Task UpdateAsync(RefreshToken token)
        {
            await _refreshTokens.ReplaceOneAsync(t => t.Id == token.Id, token);
        }
    }
}
