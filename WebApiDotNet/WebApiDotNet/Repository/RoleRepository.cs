using MongoDB.Driver;
using WebApiDotNet.Models;
using WebApiDotNet.Repository.IRepository;
using WebApiDotNet.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApiDotNet.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly IMongoCollection<Role> _collection;

        public RoleRepository(MongoDbContext context)
        {
            _collection = context.Roles;
        }

        public async Task<List<Role>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<Role?> GetByIdAsync(string id)
        {
            return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }
    }
} 