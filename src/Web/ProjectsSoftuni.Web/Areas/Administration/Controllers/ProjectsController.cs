using ProjectsSoftuni.Services.Models.InputModels;
using ProjectsSoftuni.Web.Models;

namespace ProjectsSoftuni.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using ProjectsSoftuni.Common;
    using ProjectsSoftuni.Services.Contracts;
    using ProjectsSoftuni.Services.Models.Projects;
    using ProjectsSoftuni.Web.Areas.Administration.ViewModels.Applications;
    using ProjectsSoftuni.Web.Areas.Administration.ViewModels.Projects;
    using System.Linq;
    using System.Threading.Tasks;

    public class ProjectsController : AdministrationController
    {
        private const string ProjectStatusesStr = "ProjectStatuses";
        private const string ApplicationsStr = "Applications";
        private const string ProjectIdStr = "ProjectId";

        private readonly IProjectService projectService;
        private readonly IProjectStatusSevice projectStatusService;
        private readonly IApplicationService applicationService;
        private readonly ISpecificationService specificationService;

        public ProjectsController(
            IProjectService projectService,
            IProjectStatusSevice projectStatusService,
            IApplicationService applicationService,
            ISpecificationService specificationService)
        {
            this.projectService = projectService;
            this.projectStatusService = projectStatusService;
            this.applicationService = applicationService;
            this.specificationService = specificationService;
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
            var project = await this.projectService.GetProjectDetailsByIdAsync<ProjectDetailsViewModel>(id);
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

        public async Task<IActionResult> ProjectsWithApplicationStatusWaiting(string sortOrder, string searchString, string currentFilter, int? pageIndex)
        {
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

            var projects = await this.projectService.GetAllByApplicationStatusNameAsync<ProjectIndexViewModel>(GlobalConstants.WaitingApplicationStatus);

            if (projects == null)
            {
                var controllerName = ControllerHelper.RemoveControllerFromStr(nameof(HomeController));
                return this.RedirectToAction(nameof(HomeController.Index), controllerName);
            }

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

        public IActionResult UploadSpecification(string id)
        {
            this.ViewData[ProjectIdStr] = id;
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadSpecification(UploadSpecificationsInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var isCreated = await this.specificationService.UploadSpecificationAsync(model);

            if (!isCreated)
            {
                var controllerName = ControllerHelper.RemoveControllerFromStr(nameof(HomeController));
                return this.RedirectToAction(nameof(HomeController.Index), controllerName);
            }

            return this.RedirectToAction(nameof(this.Details), new { id = model.ProjectId });
        }
    }
}
