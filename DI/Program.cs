var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddSingleton<IMyInterface, MyService>();
// builder.Services.AddScoped<IMyInterface, MyService>();
builder.Services.AddTransient<IMyInterface, MyService>();

var app = builder.Build();

app.Use(async (context, next) =>
{
    var myInterface = context.RequestServices.GetRequiredService<IMyInterface>();
    myInterface.LogCreation("First Middleware.");
    await next.Invoke();
});
app.Use(async (context, next) =>
{
    var myInterface = context.RequestServices.GetRequiredService<IMyInterface>();
    myInterface.LogCreation("Second Middleware.");
    await next.Invoke();
});
app.MapGet("/", (IMyInterface myInterface) =>
{
    myInterface.LogCreation("Hello from the root endpoint!");
    return Results.Ok("Check the console for the log message.");
});

app.Run();

public interface IMyInterface
{
    void LogCreation(string message);
}
public class MyService : IMyInterface
{
    private readonly int _serviceId;
    public MyService()
    {
        _serviceId = new Random().Next(1000, 9999);
    }
    public void LogCreation(string message)
    {
        Console.WriteLine($"Service ID: {_serviceId} - {message}");
    }
}
