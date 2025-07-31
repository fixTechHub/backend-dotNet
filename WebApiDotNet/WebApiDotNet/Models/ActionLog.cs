using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace WebApiDotNet.Models
{
    [BsonIgnoreExtraElements]
    public class ActionLog
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("userId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }

        [BsonElement("actionType")]
        public string ActionType { get; set; }

        [BsonElement("method")]
        public string Method { get; set; }

        [BsonElement("route")]
        public string Route { get; set; }

        [BsonElement("params")]
        public object Params { get; set; }

        [BsonElement("query")]
        public object Query { get; set; }

        [BsonElement("body")]
        public object Body { get; set; }

        [BsonElement("statusCode")]
        public int StatusCode { get; set; }

        [BsonElement("ip")]
        public string Ip { get; set; }

        [BsonElement("userAgent")]
        public string UserAgent { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("createdAt")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
} 