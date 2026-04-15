using System.ComponentModel.DataAnnotations;

namespace FinanceTracker.Dtos.CategoryDto
{
    public class CreateCategoryDto
    {
        [Required]
        public string CategoryName { get; set; } = null!;


        //   [RegularExpression(@"^#[0-9A-Fa-f]{6}$", ErrorMessage = "ColorHex must be a valid hex color like #FF5733")]
        //   public string ColorHex { get; set; } = null!;
        //  public string? IconName { get; set; }





    }
}
