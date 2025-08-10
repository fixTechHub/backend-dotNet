using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiDotNet.Models;
using WebApiDotNet.Repository.IRepository;
using Microsoft.Extensions.Configuration;
using WebApiDotNet.Data;
using System;

namespace WebApiDotNet.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IMongoCollection<Category> _categories;

        public CategoryRepository(MongoDbContext context)
        {
            _categories = context.Categories;
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _categories.Find(c => !c.IsDeleted).ToListAsync();
        }

        public async Task<List<Category>> GetDeletedAsync()
        {
            return await _categories.Find(c => c.IsDeleted).ToListAsync();
        }

        public async Task<Category> GetByIdAsync(string id)
        {
            return await _categories.Find(c => c.Id == id && !c.IsDeleted).FirstOrDefaultAsync();
        }

        public async Task<Category> CreateAsync(Category category)
        {
            category.CreatedAt = DateTime.UtcNow;
            category.UpdatedAt = DateTime.UtcNow;
            await _categories.InsertOneAsync(category);
            return category;
        }

        public async Task<Category> UpdateAsync(string id, Category category)
        {
            category.UpdatedAt = DateTime.UtcNow;
            await _categories.ReplaceOneAsync(c => c.Id == id && !c.IsDeleted, category);
            return category;
        }

        public async Task DeleteAsync(string id)
        {
            var update = Builders<Category>.Update
                .Set(c => c.IsDeleted, true)
                .Set(c => c.IsActive, false)
                .Set(c => c.DeletedAt, DateTime.UtcNow)
                .Set(c => c.UpdatedAt, DateTime.UtcNow);
            await _categories.UpdateOneAsync(c => c.Id == id && !c.IsDeleted, update);
        }

        public async Task RestoreAsync(string id)
        {
            var update = Builders<Category>.Update
                .Set(c => c.IsDeleted, false)
                .Set(c => c.DeletedAt, null)
                .Set(c => c.UpdatedAt, DateTime.UtcNow);
            await _categories.UpdateOneAsync(c => c.Id == id && c.IsDeleted, update);
        }

        public async Task<bool> ExistsAsync(string id)
        {
            return await _categories.Find(c => c.Id == id && !c.IsDeleted).AnyAsync();
        }

        public async Task<bool> ExistsDeletedAsync(string id)
        {
            return await _categories.Find(c => c.Id == id && c.IsDeleted).AnyAsync();
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _categories.Find(c => c.CategoryName == name && !c.IsDeleted).AnyAsync();
        }
    }
}
