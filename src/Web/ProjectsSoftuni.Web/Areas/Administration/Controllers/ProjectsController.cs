namespace ProjectsSoftuni.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using ProjectsSoftuni.Services;
    using ProjectsSoftuni.Web.Areas.Administration.ViewModels.Projects;

    public class ProjectsController : AdministrationController
    {
        private readonly IProjectService projectService;

        public ProjectsController(IProjectService projectService)
        {
            this.projectService = projectService;
        }

        public IActionResult Index()
        {
            var projects = this.projectService.GetProjectsWithWaitingApplicationStatus();

            return this.View(projects);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProjectInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var projectId = await this.projectService
                .CreateAsync(
                    input.Name,
                    input.Description,
                    input.Owner,
                    input.DueDate,
                    input.GitHubLink,
                    input.DeployLink,
                    input.Budget);

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}