using System.ComponentModel.DataAnnotations;

namespace FinanceTracker.Dtos.IncomeDto
{
    public class CreateIncomeDto
    {
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]

        [Required]
        public decimal Amount { get; set; }


        [Required]

        public int IncomeSourceId { get; set; }
        public string? Description { get; set; }
    }
}
