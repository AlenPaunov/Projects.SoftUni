using System.Security.Claims;
using ProjectsSoftuni.Common;
using ProjectsSoftuni.Services;
using ProjectsSoftuni.Web.ViewModels.Teams;

namespace ProjectsSoftuni.Web.Controllers
{

    using Microsoft.AspNetCore.Mvc;

    public class TeamsController : BaseController
    {
        private const string ProjectId = "ProjectId";

        private readonly ITeamService teamService;

        public TeamsController(ITeamService teamService)
        {
            this.teamService = teamService;
        }

        public IActionResult CreateTeam(string projectId)
        {
            this.ViewData[ProjectId] = projectId;
            return this.View();
        }


        [HttpPost]
        public IActionResult CreateTeam(CreateTeamInputModel model)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var teamId = this.teamService.CreteTeam(model.Name, model.ProjectId, userId);

            return this.RedirectToAction(
                nameof(ProjectsController.Details),
                ControllerHelper.RemoveControllerFromStr(nameof(ProjectsController)),
                new { id = model.ProjectId });
        }

        //public async Task<IActionResult> CreateTeam(string projectId)
        //{
        //    var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        //    var applyEnabled = await this.projectService.ApplyForProjectAsync(projectId, userId);

        //    return this.RedirectToAction(nameof(this.Details), new { id = projectId });
        //}
    }
}