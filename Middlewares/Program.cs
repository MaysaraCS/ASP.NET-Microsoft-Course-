var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();

// app.Use(async (context, next) =>
// {
//     var startTime = DateTime.UtcNow;
//     await next();
//     // Do something before the next middleware
//     Console.WriteLine("Before next middleware");

//     var endTime = DateTime.UtcNow;
//     double elapsedTime = (endTime - startTime).TotalMilliseconds;

//     Console.WriteLine(
//         $"{{DateTime.UtcNow}} - Request to {context.Request.Path} {context.Request.Method} took {elapsedTime} ms"
//     );
// });
app.UseRouting();
app.UseMiddleware<RequestLoggingMiddleware>();
app.MapControllers();
app.Run();

