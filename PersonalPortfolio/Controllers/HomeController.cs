using Microsoft.AspNetCore.Mvc;
using PersonalPortfolio.Models;

namespace PersonalPortfolio.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // Pass featured projects to the landing page
            ViewBag.FeaturedProjects = GetProjects().Where(p => p.IsFeatured).ToList();
            ViewBag.Skills = GetSkills();
            return View();
        }

        public IActionResult About()
        {
            ViewBag.Skills = GetSkills();
            return View();
        }

        public IActionResult Projects()
        {
            ViewBag.Projects = GetProjects();
            return View();
        }

        public IActionResult Contact()
        {
            return View(new ContactMessage());
        }

        // ─── Private Data Methods ────────────────────────────────────────────

        private static List<Project> GetProjects()
        {
            return new List<Project>
            {
                new Project
                {
                    Id = 1,
                    Title = "E-Commerce Website",
                    Description = "Full-stack e-commerce platform with user authentication, " +
                                  "shopping cart, product search, and Stripe payment integration.",
                    TechStack = "ASP.NET Core, Entity Framework Core, Bootstrap, Stripe API",
                    GitHubUrl = "https://github.com/yourusername/ecommerce-project",
                    LiveUrl = "https://demo-ecommerce.example.com",
                    ImageUrl = "/images/projects/ecommerce.png",
                    CompletedDate = DateTime.Now.AddMonths(-2),
                    IsFeatured = true
                },
                new Project
                {
                    Id = 2,
                    Title = "Task Management App",
                    Description = "A Kanban-style task manager with drag-and-drop boards, " +
                                  "team collaboration, deadlines, and real-time updates via SignalR.",
                    TechStack = "Blazor Server, SignalR, SQL Server, Tailwind CSS",
                    GitHubUrl = "https://github.com/yourusername/task-manager",
                    LiveUrl = "",
                    ImageUrl = "/images/projects/taskmanager.png",
                    CompletedDate = DateTime.Now.AddMonths(-5),
                    IsFeatured = true
                },
                new Project
                {
                    Id = 3,
                    Title = "Weather Dashboard",
                    Description = "Real-time weather dashboard that consumes the OpenWeatherMap API, " +
                                  "showing 7-day forecasts, maps, and historical charts.",
                    TechStack = "ASP.NET Core MVC, Chart.js, OpenWeatherMap API, Bootstrap",
                    GitHubUrl = "https://github.com/yourusername/weather-dashboard",
                    LiveUrl = "https://weather.example.com",
                    ImageUrl = "/images/projects/weather.png",
                    CompletedDate = DateTime.Now.AddMonths(-8),
                    IsFeatured = false
                },
                new Project
                {
                    Id = 4,
                    Title = "Personal Finance Tracker",
                    Description = "Budget tracker with expense categorisation, monthly reports, " +
                                  "CSV import/export, and visual spending breakdowns.",
                    TechStack = "ASP.NET Core, SQLite, Chart.js, JavaScript",
                    GitHubUrl = "https://github.com/yourusername/finance-tracker",
                    LiveUrl = "",
                    ImageUrl = "/images/projects/finance.png",
                    CompletedDate = DateTime.Now.AddMonths(-11),
                    IsFeatured = false
                }
            };
        }

        // ✅ GetSkills() implemented
        private static List<Skill> GetSkills()
        {
            return new List<Skill>
            {
                // Backend
                new Skill { Id = 1, Name = "C#",            Category = "Backend",  ProficiencyLevel = 5, IconClass = "devicon-csharp-plain" },
                new Skill { Id = 2, Name = "ASP.NET Core",  Category = "Backend",  ProficiencyLevel = 5, IconClass = "devicon-dotnetcore-plain" },
                new Skill { Id = 3, Name = "Entity Framework", Category = "Backend", ProficiencyLevel = 4, IconClass = "devicon-microsoftsqlserver-plain" },
                new Skill { Id = 4, Name = "REST APIs",     Category = "Backend",  ProficiencyLevel = 4, IconClass = "fas fa-plug" },

                // Frontend
                new Skill { Id = 5, Name = "Blazor",        Category = "Frontend", ProficiencyLevel = 4, IconClass = "devicon-dotnetcore-plain" },
                new Skill { Id = 6, Name = "JavaScript",    Category = "Frontend", ProficiencyLevel = 4, IconClass = "devicon-javascript-plain" },
                new Skill { Id = 7, Name = "Bootstrap",     Category = "Frontend", ProficiencyLevel = 5, IconClass = "devicon-bootstrap-plain" },
                new Skill { Id = 8, Name = "HTML & CSS",    Category = "Frontend", ProficiencyLevel = 5, IconClass = "devicon-html5-plain" },

                // Database
                new Skill { Id = 9,  Name = "SQL Server",  Category = "Database", ProficiencyLevel = 4, IconClass = "devicon-microsoftsqlserver-plain" },
                new Skill { Id = 10, Name = "SQLite",       Category = "Database", ProficiencyLevel = 3, IconClass = "devicon-sqlite-plain" },

                // DevOps
                new Skill { Id = 11, Name = "Git & GitHub", Category = "DevOps",  ProficiencyLevel = 4, IconClass = "devicon-github-original" },
                new Skill { Id = 12, Name = "Azure",        Category = "DevOps",  ProficiencyLevel = 3, IconClass = "devicon-azure-plain" },
            };
        }
    }
}