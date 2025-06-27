using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiDotNet.Models;
using WebApiDotNet.Repository.IRepository;
using Microsoft.Extensions.Configuration;

namespace WebApiDotNet.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IMongoCollection<Category> _categories;

        public CategoryRepository(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetConnectionString("MongoDb"));
            var database = client.GetDatabase(configuration["MongoDbSettings:DatabaseName"]);
            _categories = database.GetCollection<Category>("Category");
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _categories.Find(_ => true).ToListAsync();
        }

        public async Task<Category> GetByIdAsync(string id)
        {
            return await _categories.Find(c => c.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Category> CreateAsync(Category category)
        {
            await _categories.InsertOneAsync(category);
            return category;
        }

        public async Task<bool> UpdateAsync(string id, Category category)
        {
            var result = await _categories.ReplaceOneAsync(c => c.Id == id, category);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _categories.DeleteOneAsync(c => c.Id == id);
            return result.DeletedCount > 0;
        }
    }
}
