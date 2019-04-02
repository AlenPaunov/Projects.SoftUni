using ProjectsSoftuni.Common;
using ProjectsSoftuni.Services.Contracts;
using ProjectsSoftuni.Web.ViewModels.Teams;
using System.Security.Claims;
using System.Threading.Tasks;

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
        public async Task<IActionResult> CreateTeam(CreateTeamInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction(
                    nameof(HomeController.Index),
                    ControllerHelper.RemoveControllerFromStr(nameof(HomeController)));
            }

            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var teamId = await this.teamService.CreteTeam(model.Name, model.ProjectId, userId);

            return this.RedirectToAction(
                nameof(ProjectsController.Details),
                ControllerHelper.RemoveControllerFromStr(nameof(ProjectsController)),
                new { id = model.ProjectId });
        }
    }
}