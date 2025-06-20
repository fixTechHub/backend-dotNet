using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace WebApiDotNet.Models
{
    public class CouponUsage
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("couponId")]
        public ObjectId CouponId { get; set; }

        [BsonElement("userId")]
        public ObjectId UserId { get; set; }

        [BsonElement("bookingId")]
        public ObjectId BookingId { get; set; }

        [BsonElement("usedAt")]
        public DateTime UsedAt { get; set; }
    }
}
