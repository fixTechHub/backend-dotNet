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

        public GeoJsonPointDto CurrentLocation { get; set; }
        public string Identification { get; set; }
        public string FrontIdImage { get; set; }
        public string BackIdImage { get; set; }
        public string Status { get; set; }
        public DateTime? PendingDeletionAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public double RatingAverage { get; set; }
        public int JobCompleted { get; set; }
        public int ExperienceYears { get; set; }
        public List<string> SpecialtiesCategories { get; set; }
        public string Availability { get; set; }
        public double Balance { get; set; }
        public List<string> Certificate { get; set; }
        public BankAccountDto BankAccount { get; set; }
        public double TotalEarning { get; set; }
        public double TotalCommissionPaid { get; set; }
        public double TotalHoldingAmount { get; set; }
        public double TotalWithdrawn { get; set; }
        public TechnicianRatesDto Rates { get; set; }
        public DateTime? PricesLastUpdatedAt { get; set; }
        public string? Note { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class GeoJsonPointDto
    {
        public string Type { get; set; }
        public List<double> Coordinates { get; set; }
    }

    public class BankAccountDto
    {
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public string AccountHolder { get; set; }
        public string Branch { get; set; }
    }

    public class TechnicianRatesDto
    {
        public double InspectionFee { get; set; }
        public LaborTiersDto LaborTiers { get; set; }
    }

    public class LaborTiersDto
    {
        public double? Tier1 { get; set; }
        public double? Tier2 { get; set; }
        public double? Tier3 { get; set; }
    }

    
} 