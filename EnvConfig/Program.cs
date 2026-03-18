var builder = WebApplication.CreateBuilder(args);

// ── Configuration pipeline ──────────────────────────────────────────
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables()
    .AddCommandLine(args);

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

// ── 🔍 Practice Step 3: Debug output ───────────────────────────────
Console.WriteLine($"Environment: {builder.Environment.EnvironmentName}");
Console.WriteLine($"Notifications: {builder.Configuration["TaskManagement:EnableNotifications"]}");
Console.WriteLine($"API Key exists: {!string.IsNullOrEmpty(builder.Configuration["ExternalApi:ApiKey"])}");

// ── Services ────────────────────────────────────────────────────────
builder.Services.AddOpenApi();

var app = builder.Build();

// ── Middleware pipeline ─────────────────────────────────────────────
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.MapGet("/weatherforecast", () =>
{
    var summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild",
        "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    return Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast(
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        )).ToArray();
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

// dotnet user-secrets init
// dotnet user-secrets set "ExternalApi:ApiKey" "dev-api-key-12345"
// dotnet user-secrets set "ConnectionString:ExternalService" "http://dev-api.taskservice.com"  ✅ fixed typo
// dotnet run --environment Development

// dotnet user-secrets init
// dotnet user-secrets set "ExternalApi:ApiKey" "dev-api-key-12345"
// dotnet user-secrets set "ConnectionString:ExternalService" "http://dev-api.taskservice.com"

// dotnet run --environment Development
// ```
// Expected console output:
// ```
// Environment: Development
// Notifications: True
// API Key exists: True        ← comes from User Secrets

// dotnet run --environment Production
// ```
// Expected console output:
// ```
// Environment: Production
// Notifications: False
// API Key exists: False       ← User Secrets not loaded in Production