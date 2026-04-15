using FinanceTracker.Dtos.CategoryDto;
using FinanceTracker.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;

namespace FinanceTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [EnableRateLimiting("GeneralLimiter")]

    public class CategoriesController : ControllerBase
    {
        private readonly ICategoriesService _categoriesService;

        public CategoriesController(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }

        [HttpGet("{walletId}/by-walletId")]

        public async Task<IActionResult> GetCategoriesByWalletId(int walletId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var categoriesDto = await _categoriesService.GetCategoriesByWalletId(userId, walletId);

            return Ok(categoriesDto);
        }

        [HttpPost("{walletId}")]

        public async Task<IActionResult> CreateCategory(int walletId, CreateCategoryDto createCategoryDto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            await _categoriesService.CreateCategory(userId, walletId, createCategoryDto);

            return Ok("Created Category Succesfully!");
        }


        [HttpDelete("{walletId}/{categoryId}")]


        public async Task<IActionResult> DeleteCategory(int walletId, int categoryId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);


            await _categoriesService.DeleteCategory(userId, walletId, categoryId);


            return NoContent();
        }
    }
}
