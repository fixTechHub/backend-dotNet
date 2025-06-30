using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApiDotNet.Models
{
    [BsonIgnoreExtraElements]
    public class Technician
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("userId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }

        [BsonElement("currentLocation")]
        public GeoJson CurrentLocation { get; set; }

        [BsonElement("identification")]
        public string Identification { get; set; }

        [BsonElement("ratingAverage")]
        public double RatingAverage { get; set; }

        [BsonElement("jobCompleted")]
        public int JobCompleted { get; set; }

        [BsonElement("experienceYears")]
        public int ExperienceYears { get; set; }

        [BsonElement("specialtiesCategories")]
        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> SpecialtiesCategories { get; set; }
        

        [BsonElement("availability")]
        public string Availability { get; set; }

        [BsonElement("balance")]
        public int Balance { get; set; }

        [BsonElement("bankAccount")]
        public BankAccount BankAccount { get; set; }

        [BsonElement("totalEarning")]
        public int TotalEarning { get; set; }

        [BsonElement("totalCommissionPaid")]
        public int TotalCommissionPaid { get; set; }

        [BsonElement("totalHoldingAmount")]
        public int TotalHoldingAmount { get; set; }

        [BsonElement("totalWithdrawn")]
        public int TotalWithdrawn { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [BsonElement("status")]
        public string Status { get; set; } = "PENDING";
        
        public string? Note { get; set; }
    }

    public class BankAccount
    {
        [BsonElement("bankName")]
        public string BankName { get; set; }
        [BsonElement("accountNumber")]
        public string AccountNumber { get; set; }
        [BsonElement("accountHolder")]
        public string AccountHolder { get; set; }
        [BsonElement("branch")]
        public string Branch { get; set; }
    }
} 