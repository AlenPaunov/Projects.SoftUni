namespace ProjectsSoftuni.Web.Controllers
{

    using Microsoft.AspNetCore.Mvc;
    using ProjectsSoftuni.Services;

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
    }
}