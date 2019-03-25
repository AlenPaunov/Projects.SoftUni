namespace ProjectsSoftuni.Web.Controllers
{

    using Microsoft.AspNetCore.Mvc;
    using ProjectsSoftuni.Services;
    using ProjectsSoftuni.Services.Models.Projects;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public class ProjectsController : BaseController
    {
        private readonly IProjectService projectService;

        public ProjectsController(IProjectService projectService)
        {
            this.projectService = projectService;
        }

        public IActionResult Details(string id)
        {
            var project = this.projectService.GetProjectDetailsById(id);

            if (project == null)
            {
                return this.RedirectToAction(nameof(HomeController.Index), nameof(HomeController));
            }

            return this.View(project);
        }

        public async Task<IActionResult> ApplyForProject(string projectId)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            await this.projectService.ApplyForProjectAsync(projectId, userId);

            return this.RedirectToAction(nameof(this.Details), new { id = projectId });
        }
    }
}