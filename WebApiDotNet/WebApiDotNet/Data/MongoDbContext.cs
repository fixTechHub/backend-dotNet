using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Data;
using WebApiDotNet.Models;

namespace WebApiDotNet.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IOptions<MongoDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.DatabaseName);
        }

        public IMongoCollection<Coupon> Coupons => _database.GetCollection<Coupon>("Coupons");

        // Ví dụ tạo các collection (collection tương ứng với bảng trong SQL)
        // Thêm các collection khác tương tự
    }
}
