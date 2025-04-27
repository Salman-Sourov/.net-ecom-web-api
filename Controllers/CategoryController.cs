using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce_Web_API.DTOs;
using Ecommerce_Web_API.Models;
using Microsoft.AspNetCore.Http.HttpResults;
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
            var categoryList = categories
                .Select(c => new CategoryReadDto
                {
                    CategoryId = c.CategoryId,
                    Name = c.Name,
                    Description = c.Description,
                    CreatedAt = c.CreatedAt,
                })
                .ToList();

            return Ok(
                ApiResponse<List<CategoryReadDto>>.SuccessResponse(
                    categoryList,
                    200,
                    "Categories returned successfully"
                )
            );
        }

        //POST: api/categories => Create Categories
        [HttpPost]
        public IActionResult CreateCategories([FromBody] CategoryCreateDto categoryData)
        {
            var newCategory = new Category
            {
                CategoryId = Guid.NewGuid(),
                Name = categoryData.Name,
                Description = categoryData.Description,
                CreatedAt = DateTime.UtcNow,
            };
            categories.Add(newCategory);

            var categoryReadDto = new CategoryReadDto
            {
                CategoryId = Guid.NewGuid(),
                Name = newCategory.Name,
                Description = newCategory.Description,
                CreatedAt = newCategory.CreatedAt,
            };

            return Created(
                $"/api/categories/{newCategory.CategoryId}",
                ApiResponse<CategoryReadDto>.SuccessResponse(
                    categoryReadDto,
                    201,
                    "Category created successfully"
                )
            );
        }

        //PUT: api/categories => Update Categories
        [HttpPut("{categoryId:guid}")]
        public IActionResult UpdateCategoryByID(
            [FromRoute] Guid categoryId,
            [FromBody] CategoryUpdateDto categoryData
        )
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

            return Ok(
                ApiResponse<object>.SuccessResponse(null, 204, "Category updated successfully")
            );
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
            return Ok(
                ApiResponse<object>.SuccessResponse(null, 204, "Category deleted successfully")
            );
        }
    }
}
