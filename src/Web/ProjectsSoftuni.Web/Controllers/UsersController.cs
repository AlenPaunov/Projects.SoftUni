namespace ProjectsSoftuni.Web.Controllers
{
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using ProjectsSoftuni.Common;
    using ProjectsSoftuni.Services.Contracts;
    using ProjectsSoftuni.Web.Models;
    using ProjectsSoftuni.Web.ViewModels.Users;

    public class UsersController : BaseController
    {
        private const string ProjectStatusesStr = "ProjectStatuses";

        private readonly IUserService userService;
        private readonly IProjectStatusSevice projectStatusService;
        private readonly IProjectService projectService;

        public UsersController(IUserService userService, IProjectStatusSevice projectStatusService, IProjectService projectService)
        {
            this.userService = userService;
            this.projectStatusService = projectStatusService;
            this.projectService = projectService;
        }

        public async Task<IActionResult> MyProjects(string sortOrder, string searchString, string currentFilter, int? pageIndex)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                var controllerName = ControllerHelper.RemoveControllerFromStr(nameof(HomeController));
                return this.RedirectToAction(nameof(HomeController.Home), controllerName);
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

            var projects = await this.projectService.GetProjectsByUserIdAsync<ProjectListViewModel>(userId);

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                projects = projects
                     .Where(p => p.Name.ToLower().Contains(searchString.ToLower())
                         || p.Name.ToLower().Contains(searchString.ToLower()))
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
            return this.View(PaginatedList<ProjectListViewModel>.Create(projects, pageIndex ?? 1, pageSize));
        }
    }
}