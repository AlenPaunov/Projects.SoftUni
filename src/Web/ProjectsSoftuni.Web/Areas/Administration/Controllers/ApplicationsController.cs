namespace ProjectsSoftuni.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using ProjectsSoftuni.Common;
    using ProjectsSoftuni.Services.Contracts;

    public class ApplicationsController : AdministrationController
    {
        private readonly IApplicationService applicationService;

        public ApplicationsController(IApplicationService applicationService)
        {
            this.applicationService = applicationService;
        }

        public async Task<IActionResult> Approve(string projectId, string teamId)
        {
            var isApprovedTeam = await this.applicationService.ApproveApplicationAsync(projectId, teamId);

            var controllerName = ControllerHelper.RemoveControllerFromStr(nameof(ProjectsController));
            return this.RedirectToAction(nameof(ProjectsController.Details), controllerName, new { id = projectId });
        }

        public async Task<IActionResult> Reject(string projectId, string teamId)
        {
            var isRejectedTeam = await this.applicationService.RejectApplicationAsync(projectId, teamId);

            var controllerName = ControllerHelper.RemoveControllerFromStr(nameof(ProjectsController));
            return this.RedirectToAction(nameof(ProjectsController.Details), controllerName, new { id = projectId });
        }
    }
}