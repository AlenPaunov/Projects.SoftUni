using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectsSoftuni.Common;
using ProjectsSoftuni.Services;
using ProjectsSoftuni.Web.ViewModels.Teams;

namespace ProjectsSoftuni.Web.Controllers
{
    public class ApplicationsController : BaseController
    {
        private readonly IApplicationService applicationService;

        public ApplicationsController(IApplicationService applicationService)
        {
            this.applicationService = applicationService;
        }

        [HttpPost]
        public async Task<IActionResult> ApplyingForProject(CreateTeamInputModel model)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var isApplied = await this.applicationService.ApplyTeamForProjectAsync(model.Name, model.ProjectId, userId);

            if (!isApplied)
            {
                return this.RedirectToAction(nameof(HomeController.Index), ControllerHelper.RemoveControllerFromStr(nameof(HomeController)));
            }

            return this.RedirectToAction(
                nameof(ProjectsController.Details),
                ControllerHelper.RemoveControllerFromStr(nameof(ProjectsController)),
                new { id = model.ProjectId });
        }
    }
}