using Microsoft.AspNetCore.Http; // STEP 6: Required for HttpContext.Session extensions
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using stateManagement.Models;

namespace stateManagement.Controllers;

public class TaskController : Controller
{
    // STEP 7: Define session keys as constants — avoids typos and naming conflicts
    private const string SessionKeyViewMode = "Prefs_ViewMode";
    private const string SessionKeySortOrder = "Prefs_SortOrder";
    private const string SessionKeyShowCompleted = "Prefs_ShowCompleted";

    public IActionResult Index()
    {
        // STEP 8: Read preferences from session so the View can apply them immediately
        // ?? is the null-coalescing operator — gives a default if nothing is stored yet
        ViewBag.ViewMode = HttpContext.Session.GetString(SessionKeyViewMode) ?? "list";
        ViewBag.SortOrder = HttpContext.Session.GetString(SessionKeySortOrder) ?? "date";

        // STEP 9: Session only stores strings — convert "true"/"false" back to bool
        var showStr = HttpContext.Session.GetString(SessionKeyShowCompleted);
        ViewBag.ShowCompleted = showStr == null ? true : showStr == "true";

        return View();
    }

    [HttpPost]
    public IActionResult SavePreferences(string viewMode, bool showCompleted, string sortOrder)
    {
        // STEP 10: Store each preference as a string in session using our constant keys
        HttpContext.Session.SetString(SessionKeyViewMode, viewMode ?? "list");
        HttpContext.Session.SetString(SessionKeySortOrder, sortOrder ?? "date");

        // STEP 11: bool can't go into session directly — convert it to "true" or "false"
        HttpContext.Session.SetString(SessionKeyShowCompleted, showCompleted ? "true" : "false");

        // STEP 12: Save a TempData message so the user sees confirmation after redirect
        TempData["SuccessMessage"] = "Your preferences have been saved!";

        return RedirectToAction("Index");
    }

    public IActionResult GetPreferences()
    {
        // STEP 13: Read each value from session — fall back to sensible defaults with ??
        var viewMode = HttpContext.Session.GetString(SessionKeyViewMode) ?? "list";
        var sortOrder = HttpContext.Session.GetString(SessionKeySortOrder) ?? "date";
        var showStr = HttpContext.Session.GetString(SessionKeyShowCompleted);

        // STEP 14: Parse the stored string back into a real bool (default true for new users)
        var showCompleted = showStr == null ? true : showStr == "true";

        // STEP 15: Return the preferences as JSON — easy to consume from views or JS
        var preferences = new
        {
            ViewMode = viewMode,
            ShowCompleted = showCompleted,
            SortOrder = sortOrder
        };

        return Json(preferences);
    }
}