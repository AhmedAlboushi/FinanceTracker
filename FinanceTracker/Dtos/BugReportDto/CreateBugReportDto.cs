using System.ComponentModel.DataAnnotations;

namespace FinanceTracker.Dtos.BugReportDto
{
    public class CreateBugReportDto
    {
        [Required]

        [StringLength(500)]

        public string Description { get; set; } = null!;

    }
}
