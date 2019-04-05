namespace ProjectsSoftuni.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using ProjectsSoftuni.Common;
    using ProjectsSoftuni.Services.Contracts;
    using ProjectsSoftuni.Services.Models.Projects;
    using ProjectsSoftuni.Web.ViewModels.Projects;

    [Authorize]
    public class ProjectsController : BaseController
    {
        private const string ApplyProjectEnabledStr = "ApplyProjectEnabled";
        private const string IsRejectedProjectStr = "IsRejectedProject";

        private readonly IProjectService projectService;
        private readonly IUserService userService;
        private readonly ITeamService teamService;

        public ProjectsController(IProjectService projectService, IUserService userService, ITeamService teamService)
        {
            this.projectService = projectService;
            this.userService = userService;
            this.teamService = teamService;
        }

        public async Task<IActionResult> Details(string id)
        {
            var project = await this.projectService.GetProjectDetailsByIdAsync<ProjectDetailsViewModel>(id);

            if (project == null)
            {
                string homeControllerName = ControllerHelper.RemoveControllerFromStr(nameof(HomeController));
                return this.RedirectToAction(nameof(HomeController.Index), homeControllerName);
            }

            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var applyProjectEnabled = this.userService.ApplicationEnabled(userId);
            var isRejectedProject = this.userService.IsRejectedForTheProject(userId, id);

            this.ViewData[ApplyProjectEnabledStr] = applyProjectEnabled;
            this.ViewData[IsRejectedProjectStr] = isRejectedProject;

            return this.View(project);
        }

        public async Task<IActionResult> MyProjectDetails(string id)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var project = await this.teamService.GetByProjectIdAsync<MyProjectDetailsViewModel>(id, userId);

            if (project == null)
            {
                string homeControllerName = ControllerHelper.RemoveControllerFromStr(nameof(HomeController));
                return this.RedirectToAction(nameof(HomeController.Index), homeControllerName);
            }


            return this.View(project);
        }
    }
}
