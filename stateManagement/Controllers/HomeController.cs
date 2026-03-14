using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using stateManagement.Models;

namespace stateManagement.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
    public IActionResult ProcessForm(string userInput)
    {
        if (!string.IsNullOrEmpty(userInput))
        {
            // Store the user input in TempData to pass it to the Result view
            TempData["SuccessMessage"] = $"submitted successfully: {userInput}";
            TempData["MessageType"] = "success"; // Store the message type in TempData
        }
        else
        {
            TempData["ErrorMessage"] = "Please enter some input.";
            TempData["MessageType"] = "error"; // Store the message type in TempData
        }
        return RedirectToAction("Index");
    }

    public IActionResult SavePreference(string theme, string language)
    {
        HttpContext.Session.SetString("ThemePreference", theme);
        HttpContext.Session.SetString("LanguagePreference", language);

        TempData["SuccessMessage"] = "Preferences saved successfully!";
        return RedirectToAction("Preferences");
    }
    public IActionResult Preferences()
    {
        ViewBag.Current = HttpContext.Session.GetString("ThemePreference") ?? "light";
        ViewBag.Language = HttpContext.Session.GetString("LanguagePreference") ?? "en";
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
