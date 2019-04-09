namespace ProjectsSoftuni.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using ProjectsSoftuni.Common;
    using ProjectsSoftuni.Services.Contracts;
    using ProjectsSoftuni.Web.ViewModels.Teams;

    [Authorize]
    public class TeamsController : BaseController
    {
        private const string ProjectId = "ProjectId";
        private const string TeamId = "TeamId";

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
                return this.View(model);
            }

            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var teamId = await this.teamService.CreteTeam(model.Name, model.ProjectId, userId);

            return this.RedirectToAction(
                nameof(ProjectsController.Details),
                ControllerHelper.RemoveControllerFromStr(nameof(ProjectsController)),
                new { id = model.ProjectId });
        }

        //public IActionResult AddMember(string teamId, string projectId)
        //{
        //    this.ViewData[TeamId] = teamId;
        //    this.ViewData[ProjectId] = projectId;
        //    return this.View();
        //}

        //[HttpPost]
        //public async Task<IActionResult> AddMember(AddMemberViewModel model)
        //{
        //    if (!this.ModelState.IsValid)
        //    {
        //        return this.View(model);
        //    }

        //    var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        //    var isSend = await this.teamService.SendApprovalMailAsync(model.Member, model.TeamId, userId);

        //    var controllerName = ControllerHelper.RemoveControllerFromStr(nameof(ProjectsController));
        //    return this.RedirectToAction(nameof(ProjectsController.MyProjectDetails), controllerName, new { id = model.ProjectId });
        //}
    }
}