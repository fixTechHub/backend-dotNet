using System.ComponentModel.DataAnnotations;

namespace WebApiDotNet.DTOs
{
    public class UpdateCommissionConfigDto
    {
        [Required(ErrorMessage = "Commission Percent is required")]
        public double CommissionPercent { get; set; }
        [Required(ErrorMessage = "Holding Percent is required")]
        public double HoldingPercent { get; set; }
        [Required(ErrorMessage = "Commission Min Amount is required")]
        public double CommissionMinAmount { get; set; }
        [Required(ErrorMessage = "Commission Type is required")]
        public string CommissionType { get; set; } // "PERCENT" | "MIN_AMOUNT"
        [Required(ErrorMessage = "Start Date is required")]
        public DateTime StartDate { get; set; }
        public bool IsApplied { get; set; } = true;
    }
}