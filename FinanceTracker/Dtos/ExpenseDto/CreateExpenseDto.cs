using System.ComponentModel.DataAnnotations;

namespace FinanceTracker.Dtos.ExpenseDto
{
    public class CreateExpenseDto
    {


        [Required]

        public int CategoryId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]

        public decimal Amount { get; set; }

        [StringLength(300)]
        public string? Description { get; set; }

    }
}
