using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using JqueryLab.Models;

namespace JqueryLab.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }
    [HttpGet]
    public IActionResult GetUserData()
    {
        var userData = new
        {
            Name = "John Doe",
            Email = "john.doe@example.com",
            LastLogin = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        };
        return Json(userData);
    }

    public IActionResult Index()
    {
        ViewBag.UserName = "John Doe";
        ViewBag.UserSettings = new { Theme = "dark", Language = "en" };
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
