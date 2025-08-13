using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApiDotNet.Models
{
    [BsonIgnoreExtraElements]
    public class Package
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("price")]
        public double Price { get; set; }

        [BsonElement("description")]
        public string? Description { get; set; }

        [BsonElement("benefits")]
        public List<string> Benefits { get; set; } = new();

        [BsonElement("isActive")]
        public bool IsActive { get; set; } = true;

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }
}