using System.Collections.Generic;

namespace WebApiDotNet.DTOs
{
    public class TechnicianDto
    {
        public string Id { get; set; }
        public string UserId { get; set; }

        // User Info
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public GeoJsonDto CurrentLocation { get; set; }
        public string Identification { get; set; }
        public double RatingAverage { get; set; }
        public int JobCompleted { get; set; }
        public int ExperienceYears { get; set; }
        public List<string> SpecialtiesCategories { get; set; }
        public string Availability { get; set; }
        public int Balance { get; set; }
        public BankAccountDto BankAccount { get; set; }
        public int TotalEarning { get; set; }
        public int TotalCommissionPaid { get; set; }
        public int TotalHoldingAmount { get; set; }
        public int TotalWithdrawn { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Status { get; set; }
        public string? Note { get; set; }
    }

    public class BankAccountDto
    {
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public string AccountHolder { get; set; }
        public string Branch { get; set; }
    }
} 