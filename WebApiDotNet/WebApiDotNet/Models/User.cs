using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApiDotNet.Models
{
    [BsonIgnoreExtraElements]
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("userCode")]
        public string UserCode { get; set; }

        [BsonElement("fullName")]
        public string FullName { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("phone")]
        public string Phone { get; set; }

        [BsonElement("googleId")]
        public string? GoogleId { get; set; }

        [BsonElement("address")]
        public Address Address { get; set; }

        [BsonElement("avatar")]
        public string Avatar { get; set; }

        [BsonElement("passwordHash")]
        public string PasswordHash { get; set; }

        [BsonElement("phoneVerified")]
        public bool PhoneVerified { get; set; }

        [BsonElement("emailVerified")]
        public bool EmailVerified { get; set; }

        [BsonElement("lockedReason")]
        public string? LockedReason { get; set; }

        [BsonElement("faceScanImage")]
        public string? FaceScanImage { get; set; }

        [BsonElement("role")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Role { get; set; }

        [BsonElement("status")]
        [BsonRepresentation(BsonType.String)]
        public UserStatus Status { get; set; } = UserStatus.PENDING;

        [BsonElement("deletedAt")]
        public DateTime? DeletedAt { get; set; }

        [BsonElement("createdAt")] 
        public DateTime CreatedAt { get; set; } 

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [BsonElement("verificationOTP")]
        public string? VerificationOTP { get; set; }

        [BsonElement("otpExpires")]
        public DateTime? OtpExpires { get; set; }

        [BsonElement("verificationCode")]
        public string? VerificationCode { get; set; }

        [BsonElement("verificationCodeExpires")]
        public DateTime? VerificationCodeExpires { get; set; }

        [BsonElement("pendingDeletionAt")]
        public DateTime? PendingDeletionAt { get; set; }

        [BsonElement("lastDeletionReminderSent")]
        public DateTime? LastDeletionReminderSent { get; set; }
    }

    public enum UserStatus
    {
        PENDING,
        ACTIVE,
        INACTIVE,
        INACTIVE_USER,
        INACTIVE_ADMIN,
        BLOCKED,
        DELETED,
        PENDING_DELETION
    }

    public class Address
    {
        [BsonElement("street")]
        public string Street { get; set; }
        [BsonElement("city")]
        public string City { get; set; }
        [BsonElement("district")]
        public string District { get; set; }
    }
} 