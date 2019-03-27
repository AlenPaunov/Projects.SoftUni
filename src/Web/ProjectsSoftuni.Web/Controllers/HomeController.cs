namespace ProjectsSoftuni.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using ProjectsSoftuni.Services;

    public class HomeController : BaseController
    {
        private readonly IProjectService projectService;

        public HomeController(IProjectService projectService)
        {
            this.projectService = projectService;
        }

        public IActionResult Home()
        {
            return this.View();
        }

        public async Task<IActionResult> Index(string sortOrder)
        {
            if (this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value == null)
            {
                return this.RedirectToAction(nameof(this.Home));
            }

            this.ViewData["NameSortParm"] = sortOrder == "name" ? "name_desc" : "name";
            this.ViewData["OwnerSortParm"] = sortOrder == "owner" ? "owner_desc" : "owner";
            this.ViewData["StatusSortParm"] = sortOrder == "status" ? "status_desc" : "status";

            var projects = await this.projectService.GetAllProjects();

            switch (sortOrder)
            {
                case "name_desc":
                    projects.Projects = projects.Projects.OrderByDescending(p => p.Name).ToArray();
                    break;
                case "name":
                    projects.Projects = projects.Projects.OrderBy(p => p.Name).ToList();
                    break;
                case "owner_desc":
                    projects.Projects = projects.Projects.OrderByDescending(p => p.Owner).ToList();
                    break;
                case "owner":
                    projects.Projects = projects.Projects.OrderBy(p => p.Owner).ToList();
                    break;
                case "status_desc":
                    projects.Projects = projects.Projects.OrderByDescending(p => p.Status).ToList();
                    break;
                case "status":
                    projects.Projects = projects.Projects.OrderBy(p => p.Status).ToList();
                    break;
            }

            return this.View(projects);
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => this.View();
    }
}
