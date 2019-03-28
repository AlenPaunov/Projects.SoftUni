using ProjectsSoftuni.Common;

namespace ProjectsSoftuni.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using ProjectsSoftuni.Data.Common.Repositories;
    using ProjectsSoftuni.Data.Models;
    using ProjectsSoftuni.Services.Mapping;

    public class ApplicationService : IApplicationService
    {
        private const int InvalidApplicationStatusId = -1;

        private readonly IRepository<Application> applicationsRepository;
        private readonly IRepository<ApplicationStatus> applicationStatusesRepository;
        private readonly IRepository<Project> projectsRepository;

        public ApplicationService(
            IRepository<Application> applicationsRepository,
            IRepository<ApplicationStatus> applicationStatusesRepository,
            IRepository<Project> projectsRepository)
        {
            this.applicationsRepository = applicationsRepository;
            this.applicationStatusesRepository = applicationStatusesRepository;
            this.projectsRepository = projectsRepository;
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

        //public async Task ApproveApplication(string projectId, string userId)
        //{
        //    var application = await this.applicationsRepository
        //        .All()
        //        .SingleOrDefaultAsync(a => a.ProjectId == projectId && a.UserId == userId);

        //    var applicationStatusId = this.GetApplicationStatusIdByName(GlobalConstants.AprovedApplicationStatus);

        //    if (applicationStatusId == InvalidApplicationStatusId)
        //    {
        //        return null;
        //    }

        //    application.ApplicationStatusId = applicationStatusId;


        //}

        private Project GetProjectById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return null;
            }

            var project = this.projectsRepository.All().SingleOrDefault(p => p.Id == id);

            return project;
        }

        private int GetApplicationStatusIdByName(string name)
        {
            int applicationStatusId = -1;

            if (string.IsNullOrWhiteSpace(name))
            {
                return applicationStatusId;
            }

            var applicationStatus = this.applicationStatusesRepository
                .AllAsNoTracking()
                .SingleOrDefaultAsync(s => s.Name == name);

            if (applicationStatus == null)
            {
                return applicationStatusId;
            }

            applicationStatusId = applicationStatus.Id;
            return applicationStatusId;
        }
    }
}
