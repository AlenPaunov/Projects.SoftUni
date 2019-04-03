namespace ProjectsSoftuni.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using ProjectsSoftuni.Common;
    using ProjectsSoftuni.Services.Contracts;
    using ProjectsSoftuni.Services.Models.Projects;
    using ProjectsSoftuni.Web.Areas.Administration.ViewModels.Applications;
    using ProjectsSoftuni.Web.Areas.Administration.ViewModels.Projects;

    public class ProjectsController : AdministrationController
    {
        private const string ProjectStatusesStr = "ProjectStatuses";
        private const string ApplicationsStr = "Applications";

        private readonly IProjectService projectService;
        private readonly IProjectStatusSevice projectStatusService;
        private readonly IApplicationService applicationService;

        public ProjectsController(IProjectService projectService, IProjectStatusSevice projectStatusService, IApplicationService applicationService)
        {
            this.projectService = projectService;
            this.projectStatusService = projectStatusService;
            this.applicationService = applicationService;
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

            var controllerName = ControllerHelper.RemoveControllerFromStr(nameof(HomeController));
            return this.RedirectToAction(nameof(HomeController.Index), controllerName);
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            var projectViewModel = this.projectService.GetProjectEditViewModel(id);
            var projectStatuses = this.projectStatusService.GetAllProjectStatuses();

            if (projectViewModel == null)
            {
                var controllerName = ControllerHelper.RemoveControllerFromStr(nameof(HomeController));
                return this.RedirectToAction(nameof(HomeController.Index), controllerName);
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
            var controllerName = ControllerHelper.RemoveControllerFromStr(nameof(HomeController));

            if (projectId == null)
            {
                return this.RedirectToAction(nameof(HomeController.Index), controllerName);
            }

            return this.RedirectToAction(nameof(HomeController.Index), controllerName);
        }

        public async Task<IActionResult> Details(string id)
        {
            var project = this.projectService.GetProjectDetailsById(id);
            var applications = await this.applicationService.GetAllByProjectId<ApplicationViewModel>(id);

            if (project == null)
            {
                var controllerName = ControllerHelper.RemoveControllerFromStr(nameof(HomeController));
                return this.RedirectToAction(nameof(HomeController.Index), controllerName);
            }

            if (applications != null)
            {
                var applicationsViewModel = new ApplicationsViewModel() { Applications = applications };
                this.ViewData[ApplicationsStr] = applicationsViewModel;
            }

            return this.View(project);
        }
    }
}
