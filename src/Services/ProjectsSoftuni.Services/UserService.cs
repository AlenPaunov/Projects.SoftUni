namespace ProjectsSoftuni.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using ProjectsSoftuni.Common;
    using ProjectsSoftuni.Data.Common.Repositories;
    using ProjectsSoftuni.Data.Models;
    using ProjectsSoftuni.Services.Contracts;
    using ProjectsSoftuni.Services.Mapping;

    public class UserService : IUserService
    {
        private const int MaxAllowedProject = 3;

        private readonly IRepository<ProjectsSoftuniUser> userRepository;

        public UserService(IRepository<ProjectsSoftuniUser> userRepository)
        {
            this.userRepository = userRepository;
        }

        public bool ApplicationEnabled(string userId)
        {
            var user = this.GetUserById(userId).Result;
            var userTeams = user.Teams.Select(t => t.Team).ToList();

            if (userTeams.Any(ut => ut.Application.ApplicationStatus.Name == GlobalConstants.WaitingApplicationStatus))
            {
                return false;
            }

            var userNotFinishedProjects = userTeams.Where(ut => ut.Project.Status.Name != GlobalConstants.FinishedProjectStatus);

            return userNotFinishedProjects.Count() < MaxAllowedProject;
        }

        public bool IsRejectedForTheProject(string userId, string projectId)
        {
            Validator.ThrowIfStringIsNullOrEmpty(projectId, "ProjectId can't be null. UserService");

            var user = this.GetUserById(userId);
            var userTeamForProject = user?.Result
                                          .Teams
                                          .SingleOrDefault(t => t.Team.ProjectId == projectId);

            var isRejected = userTeamForProject?
                                 .Team
                                 .Application
                                 .ApplicationStatus
                                 .Name == GlobalConstants.RejectedApplicationStatus;

            if (isRejected)
            {
                return true;
            }

            return userTeamForProject != null;
        }

        private async Task<ProjectsSoftuniUser> GetUserById(string userId)
        {
            Validator.ThrowIfStringIsNullOrEmpty(userId, "UserId can't be null. UserService");

            var user = await this.userRepository
                .All()
                .SingleOrDefaultAsync(u => u.Id == userId);

            return user;
        }
    }
}
