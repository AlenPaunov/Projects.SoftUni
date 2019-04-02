using ProjectsSoftuni.Services.Contracts;

namespace ProjectsSoftuni.Web.Controllers
{
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using ProjectsSoftuni.Services;
    using ProjectsSoftuni.Services.Models.Projects;
    using ProjectsSoftuni.Web.Models;

    public class HomeController : BaseController
    {
        private const string ProjectStatusesStr = "ProjectStatuses";

        private readonly IProjectService projectService;
        private readonly IProjectStatusSevice projectStatusService;

        public HomeController(IProjectService projectService, IProjectStatusSevice projectStatusService)
        {
            this.projectService = projectService;
            this.projectStatusService = projectStatusService;
        }

        public IActionResult Home()
        {
            return this.View();
        }

        public async Task<IActionResult> Index(string sortOrder, string searchString, string currentFilter, int? pageIndex)
        {
            if (this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value == null)
            {
                return this.RedirectToAction(nameof(this.Home));
            }

            this.ViewData["NameSortParm"] = sortOrder == "name" ? "name_desc" : "name";
            this.ViewData["OwnerSortParm"] = sortOrder == "owner" ? "owner_desc" : "owner";
            this.ViewData["StatusSortParm"] = sortOrder == "status" ? "status_desc" : "status";
            this.ViewData["CurrentFilter"] = searchString;
            this.ViewData["CurrentSort"] = sortOrder;

            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            this.ViewData["CurrentFilter"] = searchString;

            var projects = this.projectService.GetAllProjects();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                projects.Projects = projects.Projects
                     .Where(p => p.Name.ToLower().Contains(searchString.ToLower())
                         || p.Owner.ToLower().Contains(searchString.ToLower()));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    projects.Projects = projects.Projects.OrderByDescending(p => p.Name);
                    break;
                case "name":
                    projects.Projects = projects.Projects.OrderBy(p => p.Name);
                    break;
                case "owner_desc":
                    projects.Projects = projects.Projects.OrderByDescending(p => p.Owner);
                    break;
                case "owner":
                    projects.Projects = projects.Projects.OrderBy(p => p.Owner);
                    break;
                case "status_desc":
                    projects.Projects = projects.Projects.OrderByDescending(p => p.Status);
                    break;
                case "status":
                    projects.Projects = projects.Projects.OrderBy(p => p.Status);
                    break;
            }

            var projectStatuses = this.projectStatusService.GetAllProjectStatuses();
            this.ViewData[ProjectStatusesStr] = projectStatuses.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.Name,
            });

            int pageSize = 5;
            return this.View(await PaginatedList<ProjectIndexViewModel>.CreateAsync(projects.Projects, pageIndex ?? 1, pageSize));
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => this.View();
    }
}
