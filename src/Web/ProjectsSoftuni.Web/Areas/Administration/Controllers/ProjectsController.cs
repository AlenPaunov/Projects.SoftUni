namespace ProjectsSoftuni.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using ProjectsSoftuni.Services;
    using ProjectsSoftuni.Services.Models;
    using ProjectsSoftuni.Web.Controllers;

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
        public async Task<IActionResult> Create(ProjectIndexViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}