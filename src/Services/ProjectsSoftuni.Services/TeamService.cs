namespace ProjectsSoftuni.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using HtmlAgilityPack;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.EntityFrameworkCore;
    using ProjectsSoftuni.Common;
    using ProjectsSoftuni.Data.Common.Repositories;
    using ProjectsSoftuni.Data.Models;
    using ProjectsSoftuni.Services.Contracts;
    using ProjectsSoftuni.Services.Mapping;
    using ProjectsSoftuni.Services.Models.Users;

    public class TeamService : ITeamService
    {
        private readonly IRepository<Team> teamRepository;
        private readonly ITeamUserStatusService teamUserStatusService;
        private readonly IUserService userService;
        private readonly IEmailSender emailSender;

        public TeamService(
            IRepository<Team> teamRepository,
            ITeamUserStatusService teamUserStatusService,
            IUserService userService,
            IEmailSender emailSender)
        {
            this.teamRepository = teamRepository;
            this.teamUserStatusService = teamUserStatusService;
            this.userService = userService;
            this.emailSender = emailSender;
        }

        public async Task<string> CreteTeamAsync(string teamName, string projectId, string userId)
        {
            var teamUserStatusId = this.teamUserStatusService.GetIdByName(GlobalConstants.TeamUserStatusTeamLead).Result;

            var teamUser = new TeamUser() { UserId = userId, TeamUserStatusId = teamUserStatusId };

            var team = new Team()
            {
                Name = teamName,
                ProjectId = projectId,
            };

            team.Members.Add(teamUser);

            await this.teamRepository.AddAsync(team);
            await this.teamRepository.SaveChangesAsync();

            return team.Id;
        }

        public async Task<TModel> GetByProjectIdAsync<TModel>(string projectId, string userId)
            where TModel : class
        {
            if (string.IsNullOrWhiteSpace(projectId) && string.IsNullOrWhiteSpace(userId))
            {
                return null;
            }

            var project = await this.teamRepository
                .AllAsNoTracking()
                .Where(t => t.ProjectId == projectId && t.Members.Any(m => m.UserId == userId))
                .To<TModel>()
                .FirstOrDefaultAsync();

            return project;
        }

        public async Task<ICollection<TModel>> GetAllAsync<TModel>()
        {
            var teams = await this.teamRepository
                .AllAsNoTracking()
                .To<TModel>()
                .ToListAsync();

            return teams;
        }

        //public async Task<bool> SendApprovalMailAsync(string memberStr, string teamId, string invitationUserId)
        //{
        //    var team = await this.teamRepository
        //        .All()
        //        .SingleOrDefaultAsync(t => t.Id == teamId);

        //    if (team == null)
        //    {
        //        return false;
        //    }

        //    var member = await this.userService.GetByUsernameOrEmailAsync<UserViewModel>(memberStr);
        //    var user = await this.userService.GetByIdAsync<UserViewModel>(invitationUserId);

        //    if (member == null || user == null)
        //    {
        //        return false;
        //    }

        //    var emailContent = this.GenerateEmailContent(member, user, team);
        //    var subject = string.Format(
        //        EmailSendConstants.InvitationEmailSubject,
        //        user.Username,
        //        team.Project.Name);

        //    await this.emailSender.SendEmailAsync(
        //        member.Email,
        //        subject,
        //        emailContent);

        //    return true;
        //}

        private string GenerateEmailContent(UserViewModel member, UserViewModel invitationUser, Team team)
        {
            var invitationTemplatePath = EmailSendConstants.InvitationEmailHtmlPath;
            var doc = new HtmlDocument();
            doc.Load(invitationTemplatePath);

            var projectDetailsLink = string.Format(EmailSendConstants.ProjectDetailsLink, team.ProjectId);

            var content = doc.Text;

            content = content.Replace(EmailSendConstants.InviteUserImagePlaceholder, null)
                             .Replace(EmailSendConstants.InviteUserUsernamePlaceholder, invitationUser.Username)
                             .Replace(EmailSendConstants.MemberImagePlaceholder, null)
                             .Replace(EmailSendConstants.MemberUsernamePlaceholder, member.Username)
                             .Replace(EmailSendConstants.InviteUserUsernamePlaceholder, invitationUser.Username)
                             .Replace(EmailSendConstants.ProjectDetailsLinkPlaceholder, projectDetailsLink)
                             .Replace(EmailSendConstants.ProjectNamePlaceholder, team.Project.Name)
                             .Replace(EmailSendConstants.InvitationLinkPlaceholder, null);


            //var content = string.Format(
            //    doc.Text,
            //    null,
            //    invitationUser.Username,
            //    null,
            //    member.Username,
            //    invitationUser.Username,
            //    projectDetailsLink,
            //    team.Project.Name,
            //    null
            //);

            return content;
        }
    }
}
