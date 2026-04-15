using System.ComponentModel.DataAnnotations;

namespace FinanceTracker.Dtos.IncomeSourceDto
{
    public class CreateIncomeSourceDto
    {

        [Required]
        [MaxLength(50)]
        public string IncomeSourceName { get; set; } = null!;


    }
}
