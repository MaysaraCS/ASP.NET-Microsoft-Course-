var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// STEP 1: Register session services in the dependency injection container
// This must be done before builder.Build()
builder.Services.AddSession(options =>
{
    // STEP 2: Set how long the session lives without activity (here: 30 minutes)
    options.IdleTimeout = TimeSpan.FromMinutes(30);

    // STEP 3: HttpOnly = true means JS cannot access the session cookie — more secure
    options.Cookie.HttpOnly = true;

    // STEP 4: IsEssential = true means session works even if user hasn't accepted cookies
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

// STEP 5: Add UseSession() to the middleware pipeline — must come BEFORE MapControllerRoute
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Task}/{action=Index}/{id?}");

app.Run();