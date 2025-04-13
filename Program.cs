var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseHttpsRedirection();

List<Category> categories = new List<Category>();

// Define routes
app.MapGet("/", () => "API Working");

app.MapGet("/api/categories", () => Results.Ok(categories));

app.MapPost("/api/categories", () =>
{
     var newCategory = new Category
     {
          CategoryId = Guid.Parse("d2719f0e-95d7-4f52-a4cc-292b34ef8b3a"),
          Name = "Book",
          Description = "Lorem ipsum",
          CreatedAt = DateTime.UtcNow,
     };
     categories.Add(newCategory);
     return Results.Created($"/api/categories/{newCategory.CategoryId}", newCategory);
});

app.MapDelete("/api/categories", () =>
{
     var foundCategory = categories.FirstOrDefault(Category => Category.CategoryId == Guid.Parse("d2719f0e-95d7-4f52-a4cc-292b34ef8b3a"));
     if (foundCategory == null)
     {
          return Results.NotFound("No categories found");
     }
     categories.Remove(foundCategory);
     return Results.NoContent();
});

app.MapPut("/api/categories", () =>
{
     var foundCategory = categories.FirstOrDefault(Category => Category.CategoryId == Guid.Parse("d2719f0e-95d7-4f52-a4cc-292b34ef8b3a"));
     if (foundCategory == null)
     {
          return Results.NotFound("No categories found");
     }
     foundCategory.Name = "Smart Phone";
     foundCategory.Description = "Update description";
     return Results.NoContent();
});
app.Run();

public record Category
{
     public Guid CategoryId { get; set; }
     public string? Name { get; set; }
     public string? Description { get; set; }
     public DateTime CreatedAt { get; set; }
};



