using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using TaskLab;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<TaskDbContext>(options =>
    options.UseSqlite("Data Source=tasks.db"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.MapGet("/api/tasks", async (TaskDbContext db) =>
{
    return await db.Tasks.ToListAsync();
})
.WithName("GetTasks");

app.MapGet("/api/tasks/{id}", async (int id, TaskDbContext db) =>
{
    var task = await db.Tasks.FindAsync(id);
    if (task == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(task);
})
.WithName("GetTaskById");

app.MapPost("/api/tasks", async (TaskLab.Task task, TaskDbContext db) =>
{
    db.Tasks.Add(task);
    await db.SaveChangesAsync();
    return Results.Created($"/api/tasks/{task.Id}", task);
})
.WithName("CreateTask");

app.MapPut("/api/tasks/{id}", async (int id, TaskLab.Task inputTask, TaskDbContext db) =>
{
    var existingTask = await db.Tasks.FindAsync(id);
    if (existingTask == null)
    {
        return Results.NotFound();
    }
    existingTask.Title = inputTask.Title;
    existingTask.Description = inputTask.Description;
    existingTask.IsCompleted = inputTask.IsCompleted;

    await db.SaveChangesAsync();
    //return Results.Ok(existingTask);
    return Results.NoContent();
})
.WithName("UpdateTask");

app.MapDelete("/api/tasks/{id}", async (int id, TaskDbContext db) =>
{
    var task = await db.Tasks.FindAsync(id);
    if (task == null)
    {
        return Results.NotFound();
    }
    db.Tasks.Remove(task);
    await db.SaveChangesAsync();
    return Results.NoContent();
})
.WithName("DeleteTask");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

// dotnet add package Microsoft.EntityFrameworkCore.sqlite
// dotnet add package Microsoft.EntityFrameworkCore.Design

// dotnet ef migrations add InitialCreate
// dotnet ef database update


// dotnet add package Microsoft.EntityFrameworkCore
// dotnet add package Swashbuckle.AspNetCore
// dotnet add package Microsoft.AspNetCore.OpenApi

// for debugging
// dotnet watch run

// dotnet run --launch-profile Production