namespace ProjectsSoftuni.Web.Controllers
{
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using ProjectsSoftuni.Services.Contracts;
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

            this.ViewData["NameSortParam"] = sortOrder == "name" ? "name_desc" : "name";
            this.ViewData["OwnerSortParam"] = sortOrder == "owner" ? "owner_desc" : "owner";
            this.ViewData["StatusSortParam"] = sortOrder == "status" ? "status_desc" : "status";
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

            var projects = await this.projectService.GetAllAsync<ProjectIndexViewModel>();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                projects = projects
                     .Where(p => p.Name.ToLower().Contains(searchString.ToLower())
                         || p.Owner.ToLower().Contains(searchString.ToLower()))
                     .ToList();
            }

            switch (sortOrder)
            {
                case "name_desc":
                    projects = projects.OrderByDescending(p => p.Name).ToList();
                    break;
                case "name":
                    projects = projects.OrderBy(p => p.Name).ToList();
                    break;
                case "owner_desc":
                    projects = projects.OrderByDescending(p => p.Owner).ToList();
                    break;
                case "owner":
                    projects = projects.OrderBy(p => p.Owner).ToList();
                    break;
                case "status_desc":
                    projects = projects.OrderByDescending(p => p.StatusName).ToList();
                    break;
                case "status":
                    projects = projects.OrderBy(p => p.StatusName).ToList();
                    break;
            }

            var projectStatuses = this.projectStatusService.GetAllProjectStatuses();
            this.ViewData[ProjectStatusesStr] = projectStatuses.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.Name,
            });

            int pageSize = 5;
            return this.View(PaginatedList<ProjectIndexViewModel>.Create(projects, pageIndex ?? 1, pageSize));
        }

        //public IActionResult Privacy()
        //{
        //    return this.View();
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => this.View();
    }
}
