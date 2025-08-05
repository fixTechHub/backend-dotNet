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
        public GeoJsonPoint CurrentLocation { get; set; }

        [BsonElement("identification")]
        public string Identification { get; set; }

        [BsonElement("frontIdImage")]
        public string FrontIdImage { get; set; }

        [BsonElement("backIdImage")]
        public string BackIdImage { get; set; }

        [BsonElement("status")]
        [BsonRepresentation(BsonType.String)]
        public TechnicianStatus Status { get; set; } = TechnicianStatus.PENDING;

        [BsonElement("pendingDeletionAt")]
        public DateTime? PendingDeletionAt { get; set; }

        [BsonElement("deletedAt")]
        public DateTime? DeletedAt { get; set; }

        [BsonElement("ratingAverage")]
        public double RatingAverage { get; set; } = 0;

        [BsonElement("jobCompleted")]
        public int JobCompleted { get; set; } = 0;

        [BsonElement("experienceYears")]
        public int ExperienceYears { get; set; } = 0;

        [BsonElement("specialtiesCategories")]
        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> SpecialtiesCategories { get; set; } = new();

        [BsonElement("availability")]
        [BsonRepresentation(BsonType.String)]
        public TechnicianAvailability Availability { get; set; } = TechnicianAvailability.FREE;

        [BsonElement("balance")]
        public double Balance { get; set; } = 0;

        [BsonElement("certificate")]
        public List<string> Certificate { get; set; } = new();

        [BsonElement("bankAccount")]
        public BankAccount BankAccount { get; set; }

        [BsonElement("totalEarning")]
        public double TotalEarning { get; set; } = 0;

        [BsonElement("totalCommissionPaid")]
        public double TotalCommissionPaid { get; set; } = 0;

        [BsonElement("totalHoldingAmount")]
        public double TotalHoldingAmount { get; set; } = 0;

        [BsonElement("totalWithdrawn")]
        public double TotalWithdrawn { get; set; } = 0;

        [BsonElement("inspectionFee")]
        public double InspectionFee { get; set; }

        [BsonElement("pricesLastUpdatedAt")]
        public DateTime? PricesLastUpdatedAt { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [BsonElement("note")]
        public string? Note { get; set; }
    }

    public class GeoJsonPoint
    {
        [BsonElement("type")]
        public string Type { get; set; } = "Point";

        [BsonElement("coordinates")]
        public List<double> Coordinates { get; set; }
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

    public enum TechnicianStatus
    {
        PENDING,
        APPROVED,
        REJECTED,
        INACTIVE,
        PENDING_DELETION,
        DELETED
    }

    public enum TechnicianAvailability
    {
        ONJOB,
        FREE,
        BUSY
    }
} 