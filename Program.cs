using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.Configure<ApiBehaviorOptions>(Options =>
{
    Options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context
            .ModelState.Where(e => e.Value != null && e.Value.Errors.Count > 0)
            .Select(e => new
            {
                Field = e.Key.Contains('.') ? e.Key.Split('.').Last() : e.Key,
                Errors = e.Value != null
                    ? e.Value.Errors.Select(x => x.ErrorMessage).ToArray()
                    : new string[0],
            })
            .ToList();

        return new BadRequestObjectResult(new { Message = "Validation failed", Errors = errors });
    };
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => "API Working");

app.Run();
