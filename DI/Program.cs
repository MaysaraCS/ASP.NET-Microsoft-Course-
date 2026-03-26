var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSingleton<IMyService, MyService>();
// builder.Services.AddScoped<IMyService, MyService>();
// builder.Services.AddTransient<IMyService, MyService>();

var app = builder.Build();

app.Use(async (context, next) =>
{
    var myInterface = context.RequestServices.GetRequiredService<IMyService>();
    myInterface.LogCreation("First Middleware.");
    await next.Invoke();
});
app.Use(async (context, next) =>
{
    var myInterface = context.RequestServices.GetRequiredService<IMyService>();
    myInterface.LogCreation("Second Middleware.");
    await next.Invoke();
});
app.MapGet("/", (IMyService myInterface) =>
{
    myInterface.LogCreation("Hello from the root endpoint!");
    return Results.Ok("Check the console for the log message.");
});

app.Run();



