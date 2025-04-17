using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce_Web_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_Web_API.Controllers
{
    [ApiController]
    [Route("api/categories/")]
    public class CategoryController : ControllerBase
    {
        private static List<Category> categories = new List<Category>();

        //GET: api/categories => Read Categories
        [HttpGet]
        public IActionResult GetCategories([FromQuery] string searchValue = "")
        {
            if (!string.IsNullOrEmpty(searchValue))
            {
                var searchedCategories = categories
                    .Where(c =>
                        !string.IsNullOrEmpty(c.Name)
                        && c.Name.Contains(searchValue, StringComparison.OrdinalIgnoreCase)
                    )
                    .ToList();
                return Ok(searchedCategories);
            }
            return Ok(categories);
        }

        //POST: api/categories => Create Categories
        [HttpPost]
        public IActionResult CreateCategories([FromBody] Category categoryData)
        {
            if (string.IsNullOrEmpty(categoryData.Name))
            {
                return BadRequest("Category Name required");
            }

            if (categoryData.Name.Length > 2)
            {
                // Console.WriteLine($"{categoryData}");
                var newCategory = new Category
                {
                    CategoryId = Guid.NewGuid(),
                    Name = categoryData.Name,
                    Description = categoryData.Description,
                    CreatedAt = DateTime.UtcNow,
                };
                categories.Add(newCategory);
                return Created($"/api/categories/{newCategory.CategoryId}", newCategory);
            }
            else
            {
                return BadRequest("Minimum name value 3");
            }
        }

        //PUT: api/categories => Update Categories
        [HttpPut("{categoryId:guid}")]
        public IActionResult UpdateCategoryByID([FromRoute] Guid categoryId, [FromBody] Category categoryData)
        {
            if (categoryData == null)
            {
                return BadRequest("Missing Category");
            }
            var foundCategory = categories.FirstOrDefault(Category =>
                Category.CategoryId == categoryId
            );
            if (foundCategory == null)
            {
                return BadRequest("Categories ID not found");
            }
            if (!string.IsNullOrWhiteSpace(categoryData.Name))
            {
                if (categoryData.Name.Length >= 2)
                {
                    foundCategory.Name = categoryData.Name;
                }
                else
                {
                    return BadRequest("Add more character");
                }
            }
            if (!string.IsNullOrEmpty(categoryData.Description))
            {
                foundCategory.Description = categoryData.Description;
            }
            // Simulate delete logic
            Console.WriteLine($"Category with ID {categoryData.CategoryId} updated.");

            return Ok(new { message = "Updated successfully" });
            // return Results.NoContent();
        }

        //DELETE: api/categories/{categoryId} => Delete Categories
        [HttpDelete("{categoryId:guid}")]
        public IActionResult DeleteCategoryById(Guid categoryId)
        {
            var foundCategory = categories.FirstOrDefault(Category =>
                Category.CategoryId == categoryId
            );
            if (foundCategory == null)
            {
                return NotFound("Invalid categories");
            }
            categories.Remove(foundCategory);
            return Ok(new { message = "Category deleted successfully" });
        }
    }
}
