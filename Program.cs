using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseHttpsRedirection();

List<Category> categories = new List<Category>();

// Define routes
app.MapGet("/", () => "API Working");

app.MapGet("/api/categories", ([FromQuery] string searchValue = "") =>
{
     // Console.WriteLine($"{searchValue}");
     if (!string.IsNullOrEmpty(searchValue))
     {
          var searchedCategories = categories.Where(c => !string.IsNullOrEmpty(c.Name) && c.Name.Contains(searchValue, StringComparison.OrdinalIgnoreCase)).ToList();
          return Results.Ok(searchedCategories);
     }
     return Results.Ok(categories);
});

app.MapPost("/api/categories", (Category categoryData) =>
{
     if (string.IsNullOrEmpty(categoryData.Name))
     {
          return Results.BadRequest("Category Name required");
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
          return Results.Created($"/api/categories/{newCategory.CategoryId}", newCategory);
     }

     else{
          return Results.BadRequest("Minimum name value 3");
     }

});

app.MapDelete("/api/categories/{categoryId:guid}", (Guid categoryId) =>
{
     var foundCategory = categories.FirstOrDefault(Category => Category.CategoryId == categoryId);
     if (foundCategory == null)
     {
          return Results.NotFound("Invalid categories");
     }
     categories.Remove(foundCategory);
     return Results.Ok(new { message = "Category deleted successfully" });
});

app.MapPut("/api/categories/{categoryId:guid}", (Guid categoryId, Category categoryData) =>
{
     if (categoryData == null)
     {
          return Results.BadRequest("Missing Category");
     }
     var foundCategory = categories.FirstOrDefault(Category => Category.CategoryId == categoryId);
     if (foundCategory == null)
     {
          return Results.BadRequest("Categories ID not found");
     }
     if (!string.IsNullOrWhiteSpace(categoryData.Name))
     {
          if (categoryData.Name.Length >= 2)
          {
               foundCategory.Name = categoryData.Name;
          }
          else
          {
               return Results.BadRequest("Add more character");
          }
     }
     if (!string.IsNullOrEmpty(categoryData.Description))
     {
          foundCategory.Description = categoryData.Description;
     }
     // Simulate delete logic
     Console.WriteLine($"Category with ID {categoryData.CategoryId} updated.");

     return Results.Ok(new { message = "Updated successfully" });
     // return Results.NoContent();
});
app.Run();

public record Category
{
     public Guid CategoryId { get; set; }
     public string? Name { get; set; }
     public string Description { get; set; } = string.Empty;
     public DateTime CreatedAt { get; set; }
};



