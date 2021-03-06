﻿namespace ProjectsSoftuni.Services
{
    using Microsoft.EntityFrameworkCore;
    using ProjectsSoftuni.Common;
    using ProjectsSoftuni.Data.Common.Repositories;
    using ProjectsSoftuni.Data.Models;
    using ProjectsSoftuni.Services.Contracts;
    using ProjectsSoftuni.Services.Mapping;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ApplicationService : IApplicationService
    {
        private const int InvalidApplicationStatusId = -1;

        private readonly IRepository<Application> applicationsRepository;
        private readonly IRepository<Project> projectsRepository;
        private readonly IRepository<ProjectsSoftuniUser> userRepository;
        private readonly IRepository<TeamUser> teamUserRepository;
        private readonly IRepository<Team> teamRepository;
        private readonly IUserService userService;
        private readonly ITeamUserStatusService teamUserStatusService;
        private readonly IApplicationStatusService applicationStatusService;

        public ApplicationService(
            IRepository<Application> applicationsRepository,
            IRepository<Project> projectsRepository,
            IRepository<ProjectsSoftuniUser> userRepository,
            IUserService userService,
            ITeamUserStatusService teamUserStatusService,
            IRepository<Team> teamRepository,
            IRepository<TeamUser> teamUserRepository,
            IApplicationStatusService applicationStatusService)
        {
            this.applicationsRepository = applicationsRepository;
            this.projectsRepository = projectsRepository;
            this.userRepository = userRepository;
            this.userService = userService;
            this.teamUserStatusService = teamUserStatusService;
            this.teamRepository = teamRepository;
            this.teamUserRepository = teamUserRepository;
            this.applicationStatusService = applicationStatusService;
        }

        public async Task<ICollection<TModel>> GetAllByProjectId<TModel>(string projectId)
        {
            if (string.IsNullOrWhiteSpace(projectId))
            {
                return null;
            }

            var applications = await this.applicationsRepository
                .AllAsNoTracking()
                .Where(a => a.ProjectId == projectId)
                .To<TModel>()
                .ToListAsync();

            return applications;
        }

        public async Task<bool> ApproveApplicationAsync(string projectId, string teamId)
        {
            if (projectId == null || teamId == null)
            {
                return false;
            }

            var application = await this.applicationsRepository
                .All()
                .SingleOrDefaultAsync(a => a.ProjectId == projectId && a.TeamId == teamId);

            if (application == null)
            {
                return false;
            }

            var applicationStatusId = await this.applicationStatusService.GetIdByNameAsync(GlobalConstants.ApprovedApplicationStatus);

            if (applicationStatusId == InvalidApplicationStatusId)
            {
                return false;
            }

            application.ApplicationStatusId = applicationStatusId;

            await this.applicationsRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RejectApplicationAsync(string projectId, string teamId)
        {
            if (projectId == null || teamId == null)
            {
                return false;
            }

            var application = await this.applicationsRepository
                .All()
                .SingleOrDefaultAsync(a => a.ProjectId == projectId && a.TeamId == teamId);

            if (application == null)
            {
                return false;
            }

            var applicationStatusId = await this.applicationStatusService.GetIdByNameAsync(GlobalConstants.RejectedApplicationStatus);

            if (applicationStatusId == InvalidApplicationStatusId)
            {
                return false;
            }

            application.ApplicationStatusId = applicationStatusId;

            await this.applicationsRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ApplyTeamForProjectAsync(string teamName, string projectId, string userId)
        {
            if (string.IsNullOrWhiteSpace(projectId) && string.IsNullOrWhiteSpace(userId))
            {
                return false;
            }

            var project = this.projectsRepository.All().SingleOrDefault(p => p.Id == projectId);
            var user = this.userRepository.All().SingleOrDefault(p => p.Id == userId);

            if (project == null || user == null)
            {
                return false;
            }

            if (project.Status.Name != GlobalConstants.OpenProjectStatus)
            {
                return false;
            }

            if (!this.userService.ApplicationEnabled(userId))
            {
                return false;
            }

            var applicationStatusId = await this.applicationStatusService.GetIdByNameAsync(GlobalConstants.WaitingApplicationStatus);

            var team = new Team() { Name = teamName, ProjectId = projectId };
            await this.teamRepository.AddAsync(team);

            var teamUserStatusId = this.teamUserStatusService.GetIdByName(GlobalConstants.TeamUserStatusTeamLead).Result;
            var teamUser = new TeamUser() { TeamId = team.Id, UserId = userId, TeamUserStatusId = teamUserStatusId };
            await this.teamUserRepository.AddAsync(teamUser);

            var application = new Application()
            {
                ProjectId = projectId,
                TeamId = team.Id,
                ApplicationStatusId = applicationStatusId,
            };

            await this.applicationsRepository.AddAsync(application);

            await this.teamRepository.SaveChangesAsync();
            await this.teamUserRepository.SaveChangesAsync();
            await this.applicationsRepository.SaveChangesAsync();

            return true;
        }

        private Project GetProjectById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return null;
            }

            var project = this.projectsRepository.All().SingleOrDefault(p => p.Id == id);

            return project;
        }
    }
}
