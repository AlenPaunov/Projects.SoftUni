using ProjectsSoftuni.Common;

namespace ProjectsSoftuni.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Services;

    [Authorize]
    public class ProjectsController : BaseController
    {
        private const string ApplyProjectEnabledStr = "ApplyProjectEnabled";
        private const string IsRejectedProjectStr = "IsRejectedProject";

        private readonly IProjectService projectService;
        private readonly IUserService userService;

        public ProjectsController(IProjectService projectService, IUserService userService)
        {
            this.projectService = projectService;
            this.userService = userService;
        }

        public IActionResult Details(string id)
        {
            var project = this.projectService.GetProjectDetailsById(id);

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
    }
}
