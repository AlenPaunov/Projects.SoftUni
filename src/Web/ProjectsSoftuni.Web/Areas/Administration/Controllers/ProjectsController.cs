namespace ProjectsSoftuni.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using ProjectsSoftuni.Services;
    using ProjectsSoftuni.Services.Models.Projects;
    using ProjectsSoftuni.Web.Areas.Administration.ViewModels.Projects;

    public class ProjectsController : AdministrationController
    {
        private const string ProjectStatusesStr = "ProjectStatuses";

        private readonly IProjectService projectService;
        private readonly IProjectStatusSevice projectStatusService;

        public ProjectsController(IProjectService projectService, IProjectStatusSevice projectStatusService)
        {
            this.projectService = projectService;
            this.projectStatusService = projectStatusService;
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

        [HttpGet]
        public IActionResult Edit(string id)
        {
            var projectViewModel = this.projectService.GetProjectEditViewModel(id);
            var projectStatuses = this.projectStatusService.GetAllProjectStatuses();

            if (projectViewModel == null)
            {
                return this.RedirectToAction(nameof(this.Index));
            }

            this.ViewData[ProjectStatusesStr] = projectStatuses.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.Name,
            });

            return this.View(projectViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProjectEditViewModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            var projectId = await this.projectService.Edit(inputModel);

            if (projectId == null)
            {
                return this.RedirectToAction(nameof(this.Index));
            }

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
