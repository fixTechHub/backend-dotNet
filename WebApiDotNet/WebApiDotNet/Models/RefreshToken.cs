using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApiDotNet.Models
{
    public class RefreshToken
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("userId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; } = string.Empty;

        [BsonElement("token")]
        public string Token { get; set; } = string.Empty; // Could be hashed in production

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("expiresAt")]
        public DateTime ExpiresAt { get; set; }

        [BsonElement("revokedAt")]
        public DateTime? RevokedAt { get; set; }

        [BsonElement("replacedByToken")]
        public string? ReplacedByToken { get; set; }

        [BsonElement("createdByIp")]
        public string? CreatedByIp { get; set; }

        [BsonElement("revokedByIp")]
        public string? RevokedByIp { get; set; }

        [BsonIgnore]
        public bool IsActive => RevokedAt == null && DateTime.UtcNow <= ExpiresAt;
    }
}
